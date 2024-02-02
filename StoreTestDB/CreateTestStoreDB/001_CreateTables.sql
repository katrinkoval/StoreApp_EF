

USE master

--ALTER DATABASE StoreTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE

IF (EXISTS (SELECT name 
FROM master.sys.databases 
WHERE ('[' + name + ']' = N'StoreTest'
OR name = N'StoreTest')))
BEGIN
	DROP DATABASE StoreTest
END

CREATE DATABASE StoreTest;
GO

USE StoreTest;
GO

CREATE TABLE [Consignments] (
	Number bigint NOT NULL UNIQUE,
	ConsignmentDate datetime NOT NULL,
	SupplierID bigint NOT NULL,
	RecipientID bigint NOT NULL,
  CONSTRAINT [PK_CONSIGMENTS] PRIMARY KEY CLUSTERED
  (
  [Number] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

CREATE TABLE [Individuals] (
	IPN bigint NOT NULL UNIQUE,
	Name nvarchar(20) NOT NULL,
	Surname nvarchar(20) NOT NULL,
  CONSTRAINT [PK_INDIVIDUALS] PRIMARY KEY CLUSTERED
  (
  [IPN] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO


CREATE TABLE [Products] (
	ID bigint NOT NULL,
	Name nvarchar(30) NOT NULL,
	UnitID tinyint NOT NULL,
	Price money NOT NULL,
  CONSTRAINT [PK_PRODUCTS] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Units] (
	ID tinyint NOT NULL,
	UnitType nchar(20) NOT NULL,
  CONSTRAINT [PK_UNITS] PRIMARY KEY CLUSTERED
  (
  [ID] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Orders] (
	ConsignmentNumber bigint NOT NULL,
	ProductID bigint NOT NULL,
	Amount float NOT NULL DEFAULT 1
)
GO
ALTER TABLE [Consignments] WITH CHECK ADD CONSTRAINT [Consignments_fk0] FOREIGN KEY ([SupplierID]) REFERENCES [Individuals]([IPN])
ON UPDATE CASCADE
GO
ALTER TABLE [Consignments] CHECK CONSTRAINT [Consignments_fk0]
GO
ALTER TABLE [Consignments] WITH CHECK ADD CONSTRAINT [Consignments_fk1] FOREIGN KEY ([RecipientID]) REFERENCES [Individuals]([IPN])
--ON UPDATE CASCADE
GO
ALTER TABLE [Consignments] CHECK CONSTRAINT [Consignments_fk1]
GO


ALTER TABLE [Products] WITH CHECK ADD CONSTRAINT [Products_fk0] FOREIGN KEY ([UnitID]) REFERENCES [Units]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Products] CHECK CONSTRAINT [Products_fk0]
GO


ALTER TABLE [Orders] WITH CHECK ADD CONSTRAINT [Orders_fk0] FOREIGN KEY ([ConsignmentNumber]) REFERENCES [Consignments]([Number])
ON UPDATE CASCADE
GO
ALTER TABLE [Orders] CHECK CONSTRAINT [Orders_fk0]
GO
ALTER TABLE [Orders] WITH CHECK ADD CONSTRAINT [Orders_fk1] FOREIGN KEY ([ProductID]) REFERENCES [Products]([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [Orders] CHECK CONSTRAINT [Orders_fk1]
GO
