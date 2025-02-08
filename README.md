# Fleet Management

## Project Details
  * .NET Core Web App (MVC)
  * Entity Framework Core
  * FontAwesome

## Requirements to Run
  * .NET 8.0 Framework
  * Visual Studio 2022
  * SQL Server (Local DB)

## Folder Structure 
  ### 1. Presentation Layer
  #### FleetManagementServiceCore
    - Consists of FleetManagementServiceCore project where the User can interact with the App.
  ### 2. Hosting Layer
  ### 3. Service Layer
  #### FleetManagementServiceCore.Service
    Service layer that makes the calls to the Database
  ### 4. Business Layer
  #### FleetManagementServiceCore.BusinessModels
    All required Models that connect the Backend with Frontend
  ### 5. Data Layer
  #### FleetManagementServiceCore.Infrastructure
    Class Library where the DbContext for Entity Framework is initiated and the Migrations for the Database take place.
    
  ![image](https://github.com/user-attachments/assets/695da158-3b7f-4e3d-a571-bbb86070b8ec)

## How to Build and Run the Project
  1. Clone the repository to your preferred directory
  2. Open the solution in Visual Studio 2022
  3. Open Package Manager Console and in Default Project dropdown select 5. Data Layer\FleetManagementServiceCore.Infrastructure and run the below Commands
       Add-Migration InitialCreate
       Update-Database
  4. Build the Solution
  5. Set as default the project 1. Presentation Layer\FleetManagementServiceCore
  6. Run the project

## Short video tutorial of the App
