USE StoreTest

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[AddOrder]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].AddOrder
END
GO

CREATE PROCEDURE AddOrder
	@ConsigmentNumber BIGINT,
	@ProductID BIGINT,
	@Amount FLOAT = 1.0,
	@ErrorCode BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF((@ConsigmentNumber IS NULL) OR NOT EXISTS 
		(
			SELECT Number
			FROM Consignments 
			WHERE Number = @ConsigmentNumber
		)) 
	BEGIN
		SET @ErrorCode = 1
		RETURN
	END

	IF ((@ProductID IS NULL) OR NOT EXISTS 
		(
			SELECT ID
			FROM Products 
			WHERE ID = @ProductID
		)) 
	BEGIN
		SET @ErrorCode = 2
		RETURN
	END

	IF (NOT EXISTS 
	(
		SELECT ConsignmentNumber 
		FROM Orders 
		WHERE ConsignmentNumber = @ConsigmentNumber AND ProductID = @ProductID 
	))
	BEGIN
		INSERT INTO Orders
				(ConsignmentNumber
				, ProductID
				, Amount)
		VALUES
		(@ConsigmentNumber, @ProductID, @Amount)
	END
	ELSE BEGIN
		UPDATE Orders
		SET Amount += @Amount
		WHERE ConsignmentNumber = @ConsigmentNumber AND ProductID = @ProductID
	END

	SET @ErrorCode = 0

END
GO

 
