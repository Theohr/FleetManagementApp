# Fleet Management

## Project Details
  * .NET Core Web App (MVC)
  * Entity Framework Core
  * FontAwesome

## Requirements to Run
  * .NET 8.0
  * Visual Studio 2022
  * SQL Server (Local DB)

## How to Build and Run the Project
  1. Clone the repository to your preferred directory
  2. Open the solution in Visual Studio 2022
  3. Open Package Manager Console and in Default Project dropdown select 5. Data Layer\FleetManagementServiceCore.Infrastructure and run the below Commands
       * Add-Migration InitialCreate (Not needed since migration is already added)
       * Update-Database
  4. Might need to change the Database Connection String in both files (see screenshot) to point at your Local DB for the app to run and execute functionality
       ![image](https://github.com/user-attachments/assets/fc2abc2f-fdaf-4938-896c-6e181984f004)
  5. Build the Solution
  6. Set as default the project 1. Presentation Layer\FleetManagementServiceCore
  7. Run the project

## Short video tutorial of the App's functionality
  https://www.youtube.com/watch?v=a81sxuBZrVA
