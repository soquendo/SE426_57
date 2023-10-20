﻿/*
Deployment script for TimsDinerDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "TimsDinerDB"
:setvar DefaultFilePrefix "TimsDinerDB"
:setvar DefaultDataPath "C:\Users\008016420\AppData\Local\Microsoft\VisualStudio\SSDT\AspNetCoreCommon"
:setvar DefaultLogPath "C:\Users\008016420\AppData\Local\Microsoft\VisualStudio\SSDT\AspNetCoreCommon"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Creating Table [dbo].[Food]...';


GO
CREATE TABLE [dbo].[Food] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (250) NOT NULL,
    [Price]       FLOAT (53)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Order]...';


GO
CREATE TABLE [dbo].[Order] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [OrderName] NVARCHAR (50) NOT NULL,
    [OrderDate] DATETIME2 (7) NOT NULL,
    [FoodId]    INT           NOT NULL,
    [Quantity]  INT           NOT NULL,
    [Total]     FLOAT (53)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Procedure [dbo].[spFood_All]...';


GO
CREATE PROCEDURE [dbo].[spFood_All]
	
AS
begin

	set nocount on; /* reduces amount of data returned by turning off the record count in the select command */
	SELECT [Id], [Title], [Description], [Price]
	FROM dbo.Food;

end
GO
PRINT N'Creating Procedure [dbo].[spOrders_Delete]...';


GO
CREATE PROCEDURE [dbo].[spOrders_Delete]
	@Id int		/* incoming Id of order to delete */
AS

begin

	set nocount on;		/* turn off records affected counting */

	DELETE
	FROM dbo.[Order]	/* delete entire record based on matching ID do NOT forget the WHERE part.. why? */
	WHERE Id = @Id;

end
GO
PRINT N'Creating Procedure [dbo].[spOrders_GetById]...';


GO
CREATE PROCEDURE [dbo].[spOrders_GetById]
	@Id int

AS

begin
	
	set nocount on; /*turn of records affected counting */

	SELECT [Id], [OrderName], [OrderDate], [FoodId], [Quantity], [Total]	/* all fields */
	FROM dbo.[Order]														/* order is a keyword too, so is used [] */
	WHERE Id = @Id;															/* use ID param to bring up specific matching record */

end
GO
PRINT N'Creating Procedure [dbo].[spOrders_Insert]...';


GO
CREATE PROCEDURE [dbo].[spOrders_Insert]	/* name of function and all the incoming parameters (except for output) */
	@OrderName nvarchar(50),
	@OrderDate datetime2(7),
	@FoodId int,
	@Quantity int,
	@Total float,
	@Id int output						/* declaring this as output means we are creating a parameter we will fill and return as output */

AS

begin

	set nocount on;		/* turn off function that returns the # of records inserted */

	insert into dbo.[Order](OrderName, OrderDate, FoodId, Quantity, Total)	/* list of fields to fill in */
		values (@OrderName, @OrderDate, @FoodId, @Quantity, @Total);		/* list of parameters used to fill fields with values.. how can copy/paste help here?*/

	set @Id = SCOPE_IDENTITY();	/* set the @Id parameter to match the new Id value, so the program calling this stored proc can receive/know this id */

end
GO
PRINT N'Creating Procedure [dbo].[spOrders_UpdateName]...';


GO
CREATE PROCEDURE [dbo].[spOrders_UpdateName]
	@Id int,					/* incoming Id of order to update */
	@OrderName nvarchar(50)		/* new name to change the existing name.. notice this is the only thing we are changing, but we could do more */

AS

begin
	
	set nocount on; /*turn off records affected counting */

	UPDATE dbo.[Order]														
	SET OrderName = @OrderName		/* change name to new name based on matching ID; do NOT forget the WHERE part.. why? */
	WHERE Id = @Id;				

end
GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/* when DB is deployed or updated, this script runs, so we only want to run this */
if not exists (select * from dbo.Food)
begin
    /* if there is no data in Food table, then add these 3 records */
    insert into dbo.food(Title, [Description], Price)
    values
        ('Cheeseburger Meal', 'Cheeseburder, Fries, and a drink', 5.95),
        ('Chili Dog Meal', 'Chili Dog, Fries, and a drink', 4.95),
        ('Vegetarian Meal','Salad and a water', 1.95);
end
GO

GO
PRINT N'Update complete.';


GO
