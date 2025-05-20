#!/bin/bash

# Generic setup script

# --- Configuration ---
# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Define the name of the PostgreSQL setup script
POSTGRES_SETUP_SCRIPT_NAME="setup_postgres.sh"
POSTGRES_SETUP_SCRIPT_PATH="${SCRIPT_DIR}/${POSTGRES_SETUP_SCRIPT_NAME}"

# --- Helper Functions ---
echo_info() {
    echo "[INFO] $1"
}

echo_error() {
    echo "[ERROR] $1" >&2
}

# --- Main Execution ---
echo_info "Starting generic setup process..."

# Check if the PostgreSQL setup script exists and is executable
if [ -f "${POSTGRES_SETUP_SCRIPT_PATH}" ]; then
    if [ -x "${POSTGRES_SETUP_SCRIPT_PATH}" ]; then
        echo_info "Found ${POSTGRES_SETUP_SCRIPT_NAME}. Invoking it now..."
        # Execute the PostgreSQL setup script
        # Pass along any arguments that were passed to this setup.sh script
        "${POSTGRES_SETUP_SCRIPT_PATH}" "$@"
        EXECUTION_STATUS=$?

        if [ ${EXECUTION_STATUS} -eq 0 ]; then
            echo_info "${POSTGRES_SETUP_SCRIPT_NAME} completed successfully."
        else
            echo_error "${POSTGRES_SETUP_SCRIPT_NAME} failed with status ${EXECUTION_STATUS}."
            exit ${EXECUTION_STATUS}
        fi
    else
        echo_error "${POSTGRES_SETUP_SCRIPT_NAME} found but is not executable."
        echo_error "Please run: chmod +x ${POSTGRES_SETUP_SCRIPT_PATH}"
        exit 1
    fi
else
    echo_error "${POSTGRES_SETUP_SCRIPT_NAME} not found in the directory: ${SCRIPT_DIR}"
    echo_error "Please ensure it is present and correctly named."
    exit 1
fi

echo_info "Generic setup process finished."
exit 0