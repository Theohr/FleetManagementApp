# Fleet Management

## Project Details
  * .NET Core Web App (MVC)
  * Entity Framework Core
  * FontAwesome

## Requirements to Run
  * .NET 8.0
  * Visual Studio 2022
  * SQL Server (Local DB)

## Functionality (Validations added as well for each functionality)
  1. As a user I want to have the ability to create Vessels. (Done)
  2. As a user I want to have the ability to create containers and load them to the vessels. (Done)
  3. As a user I want to restrict the load of containers each vessel will have (Done)
  4. As a user I want to be able to make load transfers between the vessels. (Done)
  5. As a user I want to assign the vessels into different fleets. (Done)
  Bonus.  Use Microsoft SQL Server to save the state of the platform. (Done)

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

## Short video tutorial of the App
  https://www.youtube.com/watch?v=a81sxuBZrVA

## Folder Structure 
  ### 1. Presentation Layer
  #### FleetManagementServiceCore
    Consists of the main project where the User can interact and execute functionality.
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

