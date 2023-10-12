CREATE PROCEDURE [dbo].[spOrders_GetById]
	@Id int

AS

begin
	
	set nocount on; /*turn of records affected counting */

	SELECT [Id], [OrderName], [OrderDate], [FoodId], [Quantity], [Total]	/* all fields */
	FROM dbo.[Order]														/* order is a keyword too, so is used [] */
	WHERE Id = @Id;															/* use ID param to bring up specific matching record */

end
