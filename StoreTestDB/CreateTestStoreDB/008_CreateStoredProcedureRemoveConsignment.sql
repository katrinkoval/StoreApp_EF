USE StoreTest

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[RemoveConsignment]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].RemoveConsignment
END
GO

CREATE PROCEDURE RemoveConsignment
	@Number BIGINT,
	@DateInterval TINYINT = 1,
	@ErrorStatus INT OUTPUT
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
	WHERE ConsignmentNumber = @Number

	DELETE FROM Consignments
	WHERE Number = @Number 

	SET @ErrorStatus = 0

END
GO