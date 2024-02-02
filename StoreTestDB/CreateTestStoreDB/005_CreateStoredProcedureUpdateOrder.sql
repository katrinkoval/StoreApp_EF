USE StoreTest

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[UpdateOrder]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].UpdateOrder
END
GO
 

CREATE PROCEDURE UpdateOrder
	@Number BIGINT,
	@ProductID BIGINT,
	@ProductIDUpdated BIGINT = NULL,
	@Amount FLOAT = NULL,
	@ErrorCode BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF((@Number IS NULL) OR NOT EXISTS 
		(
			SELECT Number
			FROM Consignments 
			WHERE Number = @Number
		)) 
	BEGIN
		SET @ErrorCode = 1
		RETURN
	END
	
	IF (@ProductIDUpdated IS NOT NULL) AND NOT EXISTS 
	(
		SELECT ID 
		FROM Products
		WHERE ID = @ProductIDUpdated
	) 
	BEGIN
		SET @ErrorCode = 2
		RETURN
	END

	IF(@ProductID IS NULL)  OR NOT EXISTS 
	(
		SELECT ID 
		FROM Products
		WHERE ID = @ProductID
	) 
	BEGIN
		SET @ErrorCode = 3
		RETURN
	END

	IF (@Amount IS NULL) 
	BEGIN
		SET @Amount = 1
	END

	UPDATE Orders	
	SET ProductID = ISNULL(@ProductIDUpdated, ProductID),
		Amount = ISNULL(@Amount, Amount)
	WHERE ConsignmentNumber= @Number AND ProductID = @ProductID

	SET @ErrorCode = 0
END
GO
