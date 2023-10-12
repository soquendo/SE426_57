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