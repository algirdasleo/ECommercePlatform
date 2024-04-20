# Ecommerce Platform Database Setup

This guide explains how to set up the databases required by the Ecommerce Platform using platform-specific scripts. Instructions are provided separately for Windows and macOS users.

Make sure that PostgreSQL command line tools are accessible from your system's command line.

## Configuration

**Database Credentials**:
- You will need to enter your PostgreSQL username and password when prompted by the script.

## Running the Setup Script

### Windows Users

    1. Right-click on the win_dbsetup.bat file and select "Run as administrator".
    2. When prompted, enter your PostgreSQL username and password.

### macOS Users

    1. Open Terminal.
    2. Navigate to the directory containing the mac_dbsetup.sh file.
    3. Run the following command:
        chmod +x mac_dbsetup.sh
    4. Run the following command:
        ./mac_dbsetup.sh
