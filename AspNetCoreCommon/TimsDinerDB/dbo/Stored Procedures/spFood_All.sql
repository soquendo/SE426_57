CREATE PROCEDURE [dbo].[spFood_All]
	
AS
begin

	set nocount on; /* reduces amount of data returned by turning off the record count in the select command */
	SELECT [Id], [Title], [Description], [Price]
	FROM dbo.Food;

end