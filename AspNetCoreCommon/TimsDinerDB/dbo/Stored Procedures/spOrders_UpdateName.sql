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
