-- Database Creation
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TicketManagementDB')
BEGIN
    CREATE DATABASE TicketManagementDB;
END
GO

USE TicketManagementDB;
GO

-- Table Creation
-- IssueTypes Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'IssueTypes')
BEGIN
    CREATE TABLE IssueTypes
    (
        IssueTypeId INT IDENTITY(1,1) PRIMARY KEY,
        TypeName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(255) NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END
GO

-- CustomerTickets Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CustomerTickets')
BEGIN
    CREATE TABLE CustomerTickets
    (
        TicketId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        MobileNumber NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        IssueTypeId INT NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        Priority NVARCHAR(10) NOT NULL, -- Low, Medium, High
        Status NVARCHAR(20) NOT NULL DEFAULT 'Open', -- Open, In Progress, Resolved, Closed
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        CONSTRAINT FK_CustomerTickets_IssueTypes FOREIGN KEY (IssueTypeId) REFERENCES IssueTypes(IssueTypeId)
    );
END
GO

-- Seed Data for IssueTypes
IF NOT EXISTS (SELECT TOP 1 * FROM IssueTypes)
BEGIN
    INSERT INTO IssueTypes (TypeName, Description, IsActive)
    VALUES 
        ('Hardware Issue', 'Problems related to physical devices or components', 1),
        ('Software Issue', 'Problems related to software or applications', 1),
        ('Network Issue', 'Problems related to connectivity or network devices', 1),
        ('Account Issue', 'Problems related to user accounts or authentication', 1),
        ('Billing Issue', 'Problems related to billing or payments', 1),
        ('General Inquiry', 'General questions or information requests', 1);
END
GO

-- Stored Procedures

-- Get All Issue Types
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'spGetAllIssueTypes')
    DROP PROCEDURE spGetAllIssueTypes;
GO

CREATE PROCEDURE spGetAllIssueTypes
AS
BEGIN
    SELECT 
        IssueTypeId,
        TypeName,
        Description,
        IsActive
    FROM 
        IssueTypes
    WHERE 
        IsActive = 1
    ORDER BY 
        TypeName;
END
GO

-- Create Ticket
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'spCreateTicket')
    DROP PROCEDURE spCreateTicket;
GO

CREATE PROCEDURE spCreateTicket
    @FullName NVARCHAR(100),
    @MobileNumber NVARCHAR(20),
    @Email NVARCHAR(100),
    @IssueTypeId INT,
    @Description NVARCHAR(MAX),
    @Priority NVARCHAR(10)
AS
BEGIN
    INSERT INTO CustomerTickets 
        (FullName, MobileNumber, Email, IssueTypeId, Description, Priority, Status, CreatedDate)
    VALUES 
        (@FullName, @MobileNumber, @Email, @IssueTypeId, @Description, @Priority, 'Open', GETDATE());
    
    SELECT SCOPE_IDENTITY() AS TicketId;
END
GO

-- Get Ticket By Id
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'spGetTicketById')
    DROP PROCEDURE spGetTicketById;
GO

CREATE PROCEDURE spGetTicketById
    @TicketId INT
AS
BEGIN
    SELECT 
        t.TicketId,
        t.FullName,
        t.MobileNumber,
        t.Email,
        t.IssueTypeId,
        i.TypeName AS IssueTypeName,
        t.Description,
        t.Priority,
        t.Status,
        t.CreatedDate,
        t.ModifiedDate
    FROM 
        CustomerTickets t
    INNER JOIN 
        IssueTypes i ON t.IssueTypeId = i.IssueTypeId
    WHERE 
        t.TicketId = @TicketId;
END
GO

-- Update Ticket
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'spUpdateTicket')
    DROP PROCEDURE spUpdateTicket;
GO

CREATE PROCEDURE spUpdateTicket
    @TicketId INT,
    @FullName NVARCHAR(100),
    @MobileNumber NVARCHAR(20),
    @Email NVARCHAR(100),
    @IssueTypeId INT,
    @Description NVARCHAR(MAX),
    @Priority NVARCHAR(10),
    @Status NVARCHAR(20)
AS
BEGIN
    UPDATE CustomerTickets 
    SET 
        FullName = @FullName,
        MobileNumber = @MobileNumber,
        Email = @Email,
        IssueTypeId = @IssueTypeId,
        Description = @Description,
        Priority = @Priority,
        Status = @Status,
        ModifiedDate = GETDATE()
    WHERE 
        TicketId = @TicketId;
    
    SELECT @TicketId AS TicketId;
END
GO

-- Get All Tickets with optional filtering
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'spGetAllTickets')
    DROP PROCEDURE spGetAllTickets;
GO

CREATE PROCEDURE spGetAllTickets
    @IssueTypeId INT = NULL,
    @Priority NVARCHAR(10) = NULL
AS
BEGIN
    SELECT 
        t.TicketId,
        t.FullName,
        t.MobileNumber,
        t.Email,
        t.IssueTypeId,
        i.TypeName AS IssueTypeName,
        t.Description,
        t.Priority,
        t.Status,
        t.CreatedDate,
        t.ModifiedDate
    FROM 
        CustomerTickets t
    INNER JOIN 
        IssueTypes i ON t.IssueTypeId = i.IssueTypeId
    WHERE 
        (@IssueTypeId IS NULL OR t.IssueTypeId = @IssueTypeId)
        AND (@Priority IS NULL OR t.Priority = @Priority)
    ORDER BY 
        t.CreatedDate DESC;
END
GO 