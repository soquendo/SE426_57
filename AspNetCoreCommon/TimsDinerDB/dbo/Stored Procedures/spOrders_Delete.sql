CREATE PROCEDURE [dbo].[spOrders_Delete]
	@Id int		/* incoming Id of order to delete */
AS

begin

	set nocount on;		/* turn off records affected counting */

	DELETE
	FROM dbo.[Order]	/* delete entire record based on matching ID do NOT forget the WHERE part.. why? */
	WHERE Id = @Id;

end
