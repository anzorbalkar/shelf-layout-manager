#!/bin/bash

# Script to install PostgreSQL, create a database, user, and grant privileges.

# --- Configuration ---
DB_NAME="txdb"
DB_USER="tx_admin"
DB_PASS="SCARA123!" # Note: Hardcoding passwords is not recommended for production.

# --- Helper Functions ---
echo_info() {
    echo "[INFO] $1"
}

echo_warn() {
    echo "[WARN] $1"
}

echo_error() {
    echo "[ERROR] $1" >&2
}

command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# --- OS Detection ---
OS_TYPE=""
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    OS_TYPE="linux"
elif [[ "$OSTYPE" == "darwin"* ]]; then
    OS_TYPE="macos"
elif [[ "$OSTYPE" == "cygwin" ]] || [[ "$OSTYPE" == "msys" ]] || [[ "$OSTYPE" == "win32" ]]; then # Git Bash, Cygwin, or other MinGW environments
    OS_TYPE="windows"
else
    echo_error "Unsupported operating system: $OSTYPE"
    exit 1
fi

echo_info "Detected Operating System: $OS_TYPE"

# --- PostgreSQL Installation ---
install_postgres() {
    if command_exists psql; then
        echo_info "PostgreSQL (psql client) appears to be already installed."
        PG_VERSION=$(psql --version 2>&1)
        echo_info "Detected version: $PG_VERSION"
        return 0
    fi

    echo_info "Attempting to install PostgreSQL..."
    case "$OS_TYPE" in
        linux)
            if command_exists apt-get; then
                echo_info "Using apt-get (Debian/Ubuntu)..."
                sudo apt-get update
                sudo apt-get install -y postgresql postgresql-contrib
                sudo systemctl start postgresql
                sudo systemctl enable postgresql
            elif command_exists yum; then
                echo_info "Using yum (Fedora/CentOS/RHEL)..."
                echo_warn "For Fedora/CentOS/RHEL, you might need to manually enable the PostgreSQL repository and install a specific version."
                echo_warn "This script will attempt a generic 'yum install postgresql postgresql-server'."
                sudo yum install -y postgresql postgresql-server postgresql-contrib
                if [ -d /usr/pgsql-*/bin ]; then
                    PG_BIN_DIR=$(find /usr/pgsql-* -name "bin" -type d | head -n 1)
                    if [ -n "$PG_BIN_DIR" ] && [ -x "$PG_BIN_DIR/postgresql-setup" ]; then
                         sudo "$PG_BIN_DIR/postgresql-setup" initdb
                    elif command_exists postgresql-setup; then
                         sudo postgresql-setup initdb
                    else
                        echo_warn "Could not automatically initialize PostgreSQL. Please do it manually."
                    fi
                elif command_exists initdb && [ "$(whoami)" == "postgres" ]; then # Simpler distros might just need initdb as postgres user
                    initdb -D /var/lib/pgsql/data # Or appropriate data directory
                fi
                sudo systemctl start postgresql || sudo service postgresql start
                sudo systemctl enable postgresql || sudo chkconfig postgresql on
            else
                echo_error "Neither apt-get nor yum found. Please install PostgreSQL manually."
                return 1
            fi
            ;;
        macos)
            if command_exists brew; then
                echo_info "Using Homebrew..."
                brew update
                brew install postgresql
                brew services start postgresql
            else
                echo_error "Homebrew not found. Please install Homebrew first, or install PostgreSQL manually."
                echo_info "See: https://brew.sh/ and https://www.postgresql.org/download/macosx/"
                return 1
            fi
            ;;
        windows)
            echo_warn "PostgreSQL installation on Windows via this Bash script requires manual steps or a pre-existing installation."
            echo_info "Option 1: If you have winget (Windows Package Manager) accessible from your Bash environment (e.g., some WSL setups):"
            echo_info "  You could try: winget install -e --id PostgreSQL.PostgreSQL"
            echo_info "Option 2: Download the installer from https://www.postgresql.org/download/windows/ and run it."
            echo_info "After installation, ensure 'psql' is in your PATH."
            if ! command_exists psql; then
                 echo_error "psql command not found. Please install PostgreSQL and add its bin directory to your PATH."
                 return 1
            fi
            ;;
        *)
            echo_error "Installation for $OS_TYPE not implemented in this script."
            return 1
            ;;
    esac

    if ! command_exists psql; then
        echo_error "PostgreSQL installation appears to have failed (psql command not found)."
        return 1
    else
        echo_info "PostgreSQL installation seems successful or was already present."
    fi
    return 0
}

# --- Database Setup ---
setup_database() {
    echo_info "Starting database setup..."

    PSQL_CMD_PREFIX=""
    # On macOS with Homebrew, the current user is typically the admin.
    # On Linux, 'postgres' is the typical superuser for PostgreSQL.
    PG_ADMIN_DB="postgres" # Standard maintenance database to connect to for admin tasks

    if [[ "$OS_TYPE" == "linux" ]]; then
        # Check if we need to use sudo -u postgres
        if id "postgres" &>/dev/null && [ "$(whoami)" != "postgres" ]; then
            PSQL_CMD_PREFIX="sudo -u postgres"
        fi
    fi
    # For macOS (and potentially Windows if user is already postgres or has rights),
    # PSQL_CMD_PREFIX will be empty, meaning commands run as the current user.

    echo_info "Will attempt to run psql commands. You might be prompted for your sudo password or PostgreSQL user password."

    # Check if database exists
    if $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -lqt | cut -d \| -f 1 | grep -qw "$DB_NAME"; then
        echo_info "Database '$DB_NAME' already exists."
    else
        echo_info "Creating database '$DB_NAME'..."
        $PSQL_CMD_PREFIX createdb "$DB_NAME"
        if [ $? -ne 0 ]; then
            # Fallback to psql -c if createdb fails (e.g. not in path for sudo -u, or other reasons)
            # Explicitly connect to the admin database to create another database.
            $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -c "CREATE DATABASE \"$DB_NAME\";"
            if [ $? -ne 0 ]; then
                echo_error "Failed to create database '$DB_NAME'."
                return 1
            else
                echo_info "Database '$DB_NAME' created successfully."
            fi
        else
            echo_info "Database '$DB_NAME' created successfully."
        fi
    fi

    # Check if user exists
    # Connect to PG_ADMIN_DB to query pg_roles
    if $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -tAc "SELECT 1 FROM pg_roles WHERE rolname='$DB_USER'" | grep -q 1; then
        echo_info "User '$DB_USER' already exists."
        echo_info "Attempting to update password for user '$DB_USER'..."
        $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -c "ALTER USER \"$DB_USER\" WITH PASSWORD '$DB_PASS';"
        if [ $? -ne 0 ]; then
            echo_warn "Failed to update password for user '$DB_USER'. This might be okay if permissions are already set."
        else
            echo_info "Password updated for user '$DB_USER'."
        fi
    else
        echo_info "Creating user '$DB_USER' with password 'cheese'..."
        $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -c "CREATE USER \"$DB_USER\" WITH PASSWORD '$DB_PASS';"
        if [ $? -ne 0 ]; then
            echo_error "Failed to create user '$DB_USER'."
            return 1
        else
            echo_info "User '$DB_USER' created successfully."
        fi
    fi

    echo_info "Granting all privileges on database '$DB_NAME' to user '$DB_USER'..."
    # Connect to PG_ADMIN_DB to grant privileges ON another database
    $PSQL_CMD_PREFIX psql -d "$PG_ADMIN_DB" -c "GRANT ALL PRIVILEGES ON DATABASE \"$DB_NAME\" TO \"$DB_USER\";"
    if [ $? -ne 0 ]; then
        echo_error "Failed to grant privileges on database '$DB_NAME' to '$DB_USER'."
        # Note: For GRANT ALL ON DATABASE to be truly effective for creating schemas, etc.,
        # the user might need to be OWNER or have CREATEDB.
        # The `GRANT ALL PRIVILEGES ON DATABASE` gives CONNECT, TEMPORARY, CREATE (schemas) rights.
        return 1
    else
        echo_info "Privileges granted successfully on database '$DB_NAME'."
    fi

    echo_info "Granting privileges on schema public and future tables in '$DB_NAME' to '$DB_USER'..."
    # Now, connect to the newly created/verified DB_NAME to manage its internal objects
    $PSQL_CMD_PREFIX psql -d "$DB_NAME" -c "GRANT ALL ON SCHEMA public TO \"$DB_USER\";"
    if [ $? -ne 0 ]; then echo_warn "Failed to grant ALL ON SCHEMA public to $DB_USER."; fi
    $PSQL_CMD_PREFIX psql -d "$DB_NAME" -c "ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO \"$DB_USER\";"
    if [ $? -ne 0 ]; then echo_warn "Failed to set default table privileges for $DB_USER in schema public."; fi
    $PSQL_CMD_PREFIX psql -d "$DB_NAME" -c "ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO \"$DB_USER\";"
    if [ $? -ne 0 ]; then echo_warn "Failed to set default sequence privileges for $DB_USER in schema public."; fi
    $PSQL_CMD_PREFIX psql -d "$DB_NAME" -c "ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON FUNCTIONS TO \"$DB_USER\";"
    if [ $? -ne 0 ]; then echo_warn "Failed to set default function privileges for $DB_USER in schema public."; fi

    echo_info "Database setup completed."
    echo_info "--- Summary ---"
    echo_info "Database: $DB_NAME"
    echo_info "User:     $DB_USER"
    echo_info "Password: $DB_PASS (ensure this is changed if used in production)"
    echo_info "Host:     localhost (default)"
    echo_info "Port:     5432 (default)"
    echo_info "Connection string example: psql -U $DB_USER -d $DB_NAME -h localhost"

    return 0
}

# --- Main Execution ---
install_postgres
INSTALL_STATUS=$?

if [ $INSTALL_STATUS -ne 0 ]; then
    echo_error "PostgreSQL installation/check failed. Aborting database setup."
    exit 1
fi

# Brief pause to ensure PostgreSQL service is fully up, especially after a fresh install.
if [[ "$OS_TYPE" == "linux" ]] || [[ "$OS_TYPE" == "macos" ]]; then
    if [ $INSTALL_STATUS -eq 0 ] && ! psql -U $(whoami) -d postgres -c "SELECT 1;" > /dev/null 2>&1 ; then
      # Only pause if install command was run and server might be starting
      echo_info "Waiting a few seconds for PostgreSQL service to stabilize..."
      sleep 5
    fi
fi

setup_database
SETUP_STATUS=$?

if [ $SETUP_STATUS -ne 0 ]; then
    echo_error "Database setup failed."
    exit 1
fi

echo_info "Script finished."
exit 0