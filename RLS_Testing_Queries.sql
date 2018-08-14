-- connect as tenant 1
EXEC sp_set_session_context @key=N'TenantId', @value='6CB8DE43-2043-4415-B267-7FFFA2EB5AC0'
GO
SELECT * FROM dbo.Product
GO
 
-- connect as tenant 2
EXEC sp_set_session_context @key=N'TenantId', @value='25EA09EF-E24E-494B-911F-F63CE9ED8458'
GO
SELECT * FROM dbo.Product
GO

select * from dbo.Tenant

select * from Product