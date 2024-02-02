USE master

IF (EXISTS (SELECT name 
FROM master.sys.databases 
WHERE ('[' + name + ']' = N'StoreTest'
OR name = N'StoreTest')))
BEGIN
	DROP DATABASE StoreTest
END