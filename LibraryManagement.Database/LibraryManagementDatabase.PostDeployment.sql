/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Ensure Catalog has been built or else the searches will fail
IF (SELECT FULLTEXTCATALOGPROPERTY('FullTextCatalog', 'PopulateStatus') ) = 0 --Build is Idle
BEGIN
	ALTER FULLTEXT CATALOG FullTextCatalog
	REBUILD WITH ACCENT_SENSITIVITY = ON  
END