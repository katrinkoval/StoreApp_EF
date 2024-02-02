USE StoreTest

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[UpdateConsignment]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].UpdateConsignment
END
GO

CREATE PROCEDURE UpdateConsignment
	@Number BIGINT,
	@SupplierID BIGINT = NULL,
	@RecipientID BIGINT = NULL,
	@ConsigmentDate DATETIME = NULL,
	@SupplierName NVARCHAR(20) = NULL,
	@SupplierSurname NVARCHAR(20) = NULL,
	@RecipientName NVARCHAR(20) = NULL,
	@RecipientSurname NVARCHAR(20) = NULL,
	@ErrorCode BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF (@Number IS NULL) OR	NOT EXISTS 
	(
		SELECT Number 
		FROM Consignments
		WHERE Number = @Number
	)
	BEGIN
		SET @ErrorCode = 1
		RETURN
	END

	IF((@SupplierID IS NULL AND (@SupplierName IS NULL OR @SupplierSurname IS NULL)) 
		OR (@RecipientID IS NULL AND (@RecipientName IS NULL OR @RecipientSurname IS NULL)))
	BEGIN
		SET @ErrorCode = 2
		RETURN
	END	
		
		
	IF(@SupplierID IS NULL)
	BEGIN
		SELECT @SupplierID = IPN
		FROM Individuals
		WHERE Name = @SupplierName AND Surname = @SupplierSurname
	END

	IF(@SupplierID IS NULL)
	BEGIN
		SET @ErrorCode = 3
		RETURN
	END

	IF(@RecipientID IS NULL)
	BEGIN
		SELECT @RecipientID = IPN
		FROM Individuals
		WHERE Name = @RecipientName AND Surname = @RecipientSurname
	END

	IF(@RecipientID IS NULL)
	BEGIN
		SET @ErrorCode = 4
		RETURN
	END


	IF (@SupplierID IS NOT NULL) and NOT EXISTS 
	(
		SELECT * 
		FROM Individuals
		WHERE IPN = @SupplierID
	) 
	BEGIN
		SET @ErrorCode = 5
		RETURN
	END

	IF (@RecipientID IS NOT NULL) and NOT EXISTS 
	(
		SELECT * 
		FROM Individuals
		WHERE IPN = @RecipientID
	) 
	BEGIN
		SET @ErrorCode = 6
		RETURN
	END
			
	-------------------------
	--VALIDATE @ConsigmentDate

	IF @ConsigmentDate IS NULL
	BEGIN
		SET @ConsigmentDate = GETDATE();
	END
	UPDATE [dbo].[Consignments]	
	SET SupplierID = ISNULL(@SupplierID, SupplierID),
		RecipientID = ISNULL(@RecipientID, RecipientID),
		ConsignmentDate = ISNULL(@ConsigmentDate, ConsignmentDate)
	WHERE Number= @Number
	
	SET @ErrorCode = 0

END
GO
