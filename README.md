# Ticket Management System

A comprehensive ASP.NET Core MVC application for managing customer support tickets, using an N-Tier architecture with stored procedures.

## Architecture

This solution implements an N-Tier architecture with the following layers:

1. **Presentation Layer** - MVC Controllers and Views (Razor)
   - Handles user interaction and data presentation
   - Implements client-side validation using jQuery Validation
   - Contains responsive UI with Bootstrap 5

2. **Business Logic Layer (BLL)**
   - Contains validation logic, business rules, and services
   - Manages data transformation between presentation layer and DAL
   - Uses DTO to ViewModel mapping and vice versa

3. **Data Access Layer (DAL)**
   - Implements repository pattern for data access
   - Uses Microsoft.Data.SqlClient for database operations
   - Exclusively uses stored procedures for all database operations
   - Implements error handling and logging at this level

## Project Structure

- **TicketManagementSystem** - Main ASP.NET Core MVC project (Presentation Layer)
  - Controllers: Handle HTTP requests
  - Views: Razor views for UI
  - Models: View models specific to presentation

- **TicketManagementSystem.BLL** - Business Logic Layer
  - Models: ViewModels with validation attributes
  - Services: Business logic implementation

- **TicketManagementSystem.DAL** - Data Access Layer
  - Models: Data Transfer Objects (DTOs)
  - Repositories: Data access implementation
  - Database: Connection handling

- **SqlScript.sql** - SQL script for database creation and setup
  - Table definitions for CustomerTickets and IssueTypes
  - Stored procedures for all database operations
  - Seed data for initial values

## Features

- **Ticket Management**
  - Create new tickets with required information
  - List all tickets with filtering options
  - View ticket details
  - Edit existing tickets
  - Track creation and modification dates

- **Validation**
  - Client-side validation with jQuery Validate
  - Server-side validation using Data Annotations
  - Custom error messages and validation rules

- **Filtering**
  - Filter tickets by Issue Type
  - Filter tickets by Priority (Low, Medium, High)

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- SQL Server (Local or remote instance)
- Visual Studio 2022 or another compatible IDE

### Database Setup

1. Open SQL Server Management Studio (SSMS) or another SQL client
2. Connect to your SQL Server instance
3. Execute the `SqlScript.sql` file to create the database, tables, stored procedures, and seed data

### Application Configuration

1. Open the solution in Visual Studio
2. Modify the connection string in `appsettings.json` to point to your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=TicketManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Running the Application

1. Build the solution
2. Run the application
3. Navigate to the home page and use the navigation menu to access ticket functionality

## Notes and Assumptions

- **Error Handling**
  - Application implements structured error handling in all layers
  - Exceptions are caught and logged, with user-friendly messages displayed

- **Status Values**
  - Ticket statuses are predefined as: Open, In Progress, Resolved, Closed
  - Initial status is always set to "Open" when a ticket is created

- **Priority Levels**
  - Priority values are predefined as: Low, Medium, High
  - Each level has a different visual indicator (color-coded badges)

- **Security**
  - The application does not implement authentication/authorization
  - In a production environment, proper security measures should be implemented

- **Development Approach**
  - The application follows SOLID principles
  - Code focuses on maintainability and readability
  - No auto-scaffolding was used as per requirements 