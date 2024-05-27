# Agri-Energy Project Documentation

## Overview
The Agri-Energy Project is a web application designed to allow employees to create and manage farmer profiles. The application includes functionalities for user authentication, profile management, and product management. Data is persisted in a SQL Server database to ensure it is preserved between sessions.

## Setting Up the Development Environment

1. **Install .NET 8.0**
   Download and install the .NET 8.0 SDK from the official .NET download page [https://dotnet.microsoft.com/en-us/download]. This is necessary to build and run the project.

2. **Install Visual Studio**
   Download and install the latest version of Visual Studio from the Visual Studio download page [https://visualstudio.microsoft.com/downloads/].

3. **Open the Project**
   - Download the project ZIP file named 'ST10053561-Venkata Vikranth Jannatha-PROG7311_POE_Part 2.zip' and extract it.
   - Navigate to the extracted folder, where you will find the PROG_POE_2 directory.
   - Open Visual Studio and select File > Open > Project/Solution.
   - Navigate to the PROG_POE_2 directory and open the PROG_POE_2.sln solution file.

4. **Restore NuGet Packages**
   In Visual Studio:
   - Right-click on the solution in Solution Explorer.
   - Select "Restore NuGet Packages".

## Building and Running the Prototype

1. **Build the Project**
   In Visual Studio:
   - Select Build > Build Solution from the top menu.

2. **Run the Project**
   In Visual Studio:
   - Select Run button at the top and press run. The application will open in your default web browser.

## System Functionalities and User Roles

### User Authentication
Users can register and log in to the system. The system supports two roles: Employee and Farmer.

### Profile Management
- **Employee**: Can create, view, edit, and delete farmer profiles. Each profile includes information such as the farmer's name, location, and contact details.
- **Farmer**: Can view and edit their own profile. They cannot create new profiles or delete existing ones.

### Product Management
- **Employee**: Can add, view, edit, and delete products. Each product includes information such as the product's name, price, and description.

### Data Persistence
All data is stored in a SQL Server database, ensuring that it is preserved between sessions.

## User Roles

### Employee
- Create, view, edit, and delete farmer profiles.
- View the list of all farmer profiles.
- Manage products.

### Farmer
- View and edit their own profile.
- Cannot create new profiles or delete existing ones.
