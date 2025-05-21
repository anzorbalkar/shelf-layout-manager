#!/bin/bash

# Generic setup script that invokes PostgreSQL setup and then applies migration.

# --- Configuration ---
# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Define the names of the scripts to be invoked
POSTGRES_SETUP_SCRIPT_NAME="setup_postgres.sh"
MIGRATION_SCRIPT_NAME="apply_migration.sh"

POSTGRES_SETUP_SCRIPT_PATH="${SCRIPT_DIR}/${POSTGRES_SETUP_SCRIPT_NAME}"
MIGRATION_SCRIPT_PATH="${SCRIPT_DIR}/${MIGRATION_SCRIPT_NAME}"

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

# --- Main Execution ---
echo_info "Starting generic setup process..."
OVERALL_STATUS=0

# 1. Execute PostgreSQL Setup Script
echo_info "-----------------------------------------------------"
echo_info "Step 1: PostgreSQL Setup"
echo_info "-----------------------------------------------------"
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
            OVERALL_STATUS=1 # Mark overall failure
        fi
    else
        echo_error "${POSTGRES_SETUP_SCRIPT_NAME} found but is not executable."
        echo_error "Please run: chmod +x ${POSTGRES_SETUP_SCRIPT_PATH}"
        OVERALL_STATUS=1 # Mark overall failure
    fi
else
    echo_error "${POSTGRES_SETUP_SCRIPT_NAME} not found in the directory: ${SCRIPT_DIR}"
    echo_error "Please ensure it is present and correctly named."
    OVERALL_STATUS=1 # Mark overall failure
fi

# 2. Execute Migration Script (only if PostgreSQL setup was successful)
if [ ${OVERALL_STATUS} -eq 0 ]; then
    echo_info "-----------------------------------------------------"
    echo_info "Step 2: Apply Database Migration"
    echo_info "-----------------------------------------------------"
    if [ -f "${MIGRATION_SCRIPT_PATH}" ]; then
        if [ -x "${MIGRATION_SCRIPT_PATH}" ]; then
            echo_info "Found ${MIGRATION_SCRIPT_NAME}. Invoking it now..."
            # Execute the migration script
            # Pass along any arguments that were passed to this setup.sh script
            "${MIGRATION_SCRIPT_PATH}" "$@"
            EXECUTION_STATUS=$?

            if [ ${EXECUTION_STATUS} -eq 0 ]; then
                echo_info "${MIGRATION_SCRIPT_NAME} completed successfully."
            else
                echo_error "${MIGRATION_SCRIPT_NAME} failed with status ${EXECUTION_STATUS}."
                OVERALL_STATUS=1 # Mark overall failure
            fi
        else
            echo_error "${MIGRATION_SCRIPT_NAME} found but is not executable."
            echo_error "Please run: chmod +x ${MIGRATION_SCRIPT_PATH}"
            OVERALL_STATUS=1 # Mark overall failure
        fi
    else
        echo_warn "${MIGRATION_SCRIPT_NAME} not found in the directory: ${SCRIPT_DIR}"
        echo_warn "Skipping migration step."
        # Not necessarily an error if migration script is optional,
        # but you might want to change this to OVERALL_STATUS=1 if it's mandatory.
    fi
else
    echo_warn "Skipping migration due to previous errors in PostgreSQL setup."
fi
echo_info "-----------------------------------------------------"

if [ ${OVERALL_STATUS} -eq 0 ]; then
    echo_info "Generic setup process finished successfully."
else
    echo_error "Generic setup process encountered errors."
fi

exit ${OVERALL_STATUS}