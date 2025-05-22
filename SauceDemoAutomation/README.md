# SauceDemo UI Automation Framework

## Overview

This project provides a UI automation framework for testing the [SauceDemo](https://www.saucedemo.com/) website. It is built using:

*   **C#**: Programming language.
*   **Playwright**: For browser automation.
*   **xUnit**: As the test runner.
*   **Allure**: For generating test reports.

The framework follows the Page Object Model (POM) design pattern for maintainable and scalable test scripts.

## Prerequisites

1.  **.NET SDK**: Version 6.0 or newer. You can download it from [here](https://dotnet.microsoft.com/download).
2.  **(Optional) Allure Commandline**: Required for generating Allure reports locally.
    *   Installation instructions can be found [here](https://docs.qameta.io/allure/#_installing_a_commandline).
    *   A common way to install it is using npm: `npm install -g allure-commandline --save-dev`
    *   Alternatively, you can download the binary and add it to your system's PATH.

## Setup

1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    ```
2.  **Navigate to the project directory:**
    ```bash
    cd SauceDemoAutomation
    ```
3.  **Restore NuGet packages:**
    ```bash
    dotnet restore SauceDemoAutomation.sln
    ```

## Running Tests

1.  **Run tests using the .NET CLI:**
    ```bash
    dotnet test SauceDemoAutomation.sln
    ```
    (Alternatively, you can run tests targeting the project file: `dotnet test SauceDemoAutomation/SauceDemoAutomation.csproj`)

2.  After the tests run, Allure results will be generated in the `SauceDemoAutomation/bin/Debug/net6.0/allure-results` directory (relative to the project file, or a similar path depending on your build configuration). The `allureConfig.json` is set to output to `allure-results` relative to where the tests are run.

## Generating Allure Reports Locally

1.  **Ensure Allure Commandline is installed** and accessible in your system's PATH.
2.  **Navigate to the project's root directory** (`SauceDemoAutomation` or the directory containing `allure-results`). If your `allure-results` are inside the test project's output directory (e.g., `SauceDemoAutomation/bin/Debug/net6.0/allure-results`), navigate there or specify the path to `allure-results`. For this project, assuming `allure-results` is configured to be at the root of `SauceDemoAutomation.csproj` execution:
    ```bash
    # Ensure you are in the directory where 'allure-results' is located.
    # This is typically SauceDemoAutomation/SauceDemoAutomation/bin/Debug/net6.0/
    # Or, if you've configured allureConfig.json to output to a different relative path, adjust accordingly.
    # For simplicity, let's assume allure-results is in the .csproj directory after tests.
    cd SauceDemoAutomation/bin/Debug/net6.0 
    allure generate --clean allure-results -o allure-report 
    ```
    *If `allure-results` is configured at the solution level (e.g., `SauceDemoAutomation/allure-results`), then run from `SauceDemoAutomation` directory:*
    ```bash
    cd SauceDemoAutomation 
    allure generate --clean allure-results -o allure-report
    ```
3.  **Open the generated report:**
    ```bash
    # From the same directory where you ran 'allure generate'
    allure open allure-report
    ```

## Page Object Model (POM)

This framework utilizes the Page Object Model design pattern to create a clear separation between test code and page-specific code.

*   All Page Objects are located in the `SauceDemoAutomation/PageObjects` directory.
*   **`LoginPage.cs`**: Contains locators and methods for interacting with the SauceDemo login page.
*   **`InventoryPage.cs`**: Contains locators and methods for interacting with the SauceDemo inventory/products page.

## TeamCity Integration Guide

This guide helps you set up a CI/CD pipeline in TeamCity to build, test, and generate Allure reports for this project.

### VCS Root Setup

1.  **URL**: Point to your Git repository (e.g., `https://github.com/your-username/SauceDemoAutomation.git`).
2.  **Default Branch**: Typically `main` or `master`.
3.  **Authentication**: Configure as needed (e.g., SSH key, username/password).

### Build Steps

1.  **Restore NuGet Packages**
    *   **Runner type**: .NET
    *   **Command**: `restore`
    *   **Solution file path**: `SauceDemoAutomation.sln`

2.  **Build Solution**
    *   **Runner type**: .NET
    *   **Command**: `build`
    *   **Solution file path**: `SauceDemoAutomation.sln`
    *   **Configuration**: `Release` (or `Debug` as needed)

3.  **Run xUnit Tests**
    *   **Runner type**: .NET
    *   **Command**: `test`
    *   **Project path**: `SauceDemoAutomation/SauceDemoAutomation.csproj`
    *   **Arguments**: (Optional) Add arguments if needed, e.g., logger configurations.
    *   *Allure results will be generated in the `allure-results` directory (typically within `SauceDemoAutomation/bin/<Configuration>/net6.0/allure-results` relative to the project file).*

4.  **(Optional) Install Allure Commandline on Agent**
    *   It's highly recommended to have Allure Commandline pre-installed on your TeamCity build agents or use a Docker image with Allure pre-installed. If not, you can add a step to install it.
    *   **Runner type**: Command Line
    *   **Step name**: Install Allure Commandline
    *   **Script**:
        ```bash
        # Example for installing Allure commandline (ensure it's appropriate for agent OS)
        # Ensure this script is idempotent or checks for existing installation.

        # Using npm (if Node.js is available on agent):
        echo "Attempting to install Allure Commandline using npm..."
        npm install -g allure-commandline --save-dev
        # Add npm global bin to PATH if not already there - this depends on agent setup
        # export PATH=$(npm root -g):$PATH 

        # Or, download and unpack (Linux example):
        # ALLURE_VERSION="2.27.0" # Use a recent stable version
        # if [ ! -d "/opt/allure-${ALLURE_VERSION}" ]; then
        #  echo "Downloading Allure ${ALLURE_VERSION}..."
        #  curl -o allure-${ALLURE_VERSION}.zip -L https://github.com/allure-framework/allure2/releases/download/${ALLURE_VERSION}/allure-${ALLURE_VERSION}.zip
        #  sudo mkdir -p /opt/allure-framework
        #  sudo unzip allure-${ALLURE_VERSION}.zip -d /opt/allure-framework/
        #  sudo ln -sfn /opt/allure-framework/allure-${ALLURE_VERSION} /opt/allure
        #  echo "Allure installed in /opt/allure"
        # else
        #  echo "Allure ${ALLURE_VERSION} already installed."
        # fi
        # export PATH=$PATH:/opt/allure/bin

        # Verify Allure installation
        allure --version
        ```
    *   **Note**: The agent needs appropriate permissions to install software. Using `sudo` might require passwordless sudo configuration for the build agent user, which has security implications.

5.  **Generate Allure Report**
    *   **Runner type**: Command Line
    *   **Step name**: Generate Allure Report
    *   **Working directory**: `SauceDemoAutomation/SauceDemoAutomation/bin/<Configuration>/net6.0/` (or the directory where `allure-results` is located after tests).
        *   *You might need to use TeamCity parameters to make `<Configuration>` dynamic, e.g., `%system.configuration%` if defined.*
        *   *Alternatively, if your allureConfig.json is set to output `allure-results` to a fixed path relative to the checkout directory (e.g., `checkoutDir/allure-results`), then set the working directory to your checkout directory.*
    *   **Script**:
        ```bash
        # Ensure you are in the directory containing 'allure-results'
        # The path to allure-results might need adjustment based on actual output.
        # If allure-results is in the current working directory:
        allure generate --clean allure-results -o allure-report-tc
        # If allure-results is in a subdirectory (e.g. if working dir is checkout root):
        # allure generate --clean SauceDemoAutomation/bin/Release/net6.0/allure-results -o allure-report-tc
        ```
    *   **Important**: The output directory for the TeamCity Allure report is `allure-report-tc` here to avoid conflicts if you also generate local reports named `allure-report`.

### Publishing Artifacts

1.  **Artifact paths**:
    *   `SauceDemoAutomation/SauceDemoAutomation/bin/<Configuration>/net6.0/allure-report-tc => allure-report.zip`
    *   This will take the `allure-report-tc` folder generated in the previous step (relative to the checkout directory if the working directory was set appropriately) and publish it as `allure-report.zip`.
    *   Adjust the path based on your actual build agent checkout directory and the working directory of the Allure generation step.
    *   Alternatively, to publish as a directory (if TeamCity Allure plugin supports it directly or for easier browsing): `SauceDemoAutomation/SauceDemoAutomation/bin/<Configuration>/net6.0/allure-report-tc`

### Allure Report Tab in TeamCity

1.  **Install Plugin**:
    *   Go to **Administration -> Plugins** in TeamCity.
    *   Install the "Allure Framework" plugin if not already installed. (Often available via **Browse plugins repository**).
2.  **Configure Report Tab**:
    *   Go to your **Project Settings** (or Build Configuration Settings).
    *   Find the "Report Tabs" section (may be called "Allure" or require adding a new build report tab).
    *   **Add New Report Tab**:
        *   **Tab Title**: `Allure Report` (or any preferred title).
        *   **Start Page**: `allure-report.zip!/index.html` (if you published a zip) or `allure-report-tc/index.html` (if you published a directory and it's served directly).
        *   The exact path might depend on how TeamCity serves artifacts and how the plugin expects them. Refer to the Allure TeamCity plugin documentation for precise details. The plugin might also auto-detect the report if artifacts are named conventionally (e.g., `allure-results` or `allure-report`).

This provides a comprehensive guide for users and CI/CD integration.
