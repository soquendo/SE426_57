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