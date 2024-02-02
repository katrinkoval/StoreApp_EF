USE StoreTest

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[RemoveOrder]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].RemoveOrder
END
GO

CREATE PROCEDURE RemoveOrder
	@Number BIGINT,
	@ProductID BIGINT,
	@DateInterval TINYINT = 1,
	@ErrorStatus BIGINT OUTPUT
AS
BEGIN
    IF(@Number IS NULL) OR NOT EXISTS
	(
		SELECT *
		FROM Consignments
		WHERE Number = @Number
	)
	BEGIN
		SET @ErrorStatus = 1
		RETURN
	END

	 IF(@ProductID IS NULL) OR NOT EXISTS
	(
		SELECT *
		FROM Products
		WHERE ID = @ProductID
	)
	BEGIN
		SET @ErrorStatus = 3
		RETURN
	END

	DECLARE @RemovedConsigmentDate DATETIME

	SELECT @RemovedConsigmentDate = ConsignmentDate
	FROM Consignments
	WHERE Number = @Number

	IF(@RemovedConsigmentDate > DATEADD(DAY, @DateInterval, GETDATE()))
	BEGIN
		SET @ErrorStatus = 2
		RETURN
	END

	DELETE FROM Orders
	WHERE ConsignmentNumber = @Number AND ProductID = @ProductID
	
	SET @ErrorStatus = 0

END
GO