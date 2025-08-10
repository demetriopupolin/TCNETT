
-- Cria um novo login com usuário e senha (ajuste conforme sua necessidade)
CREATE LOGIN fiapuser WITH PASSWORD = 'SenhaForte123!';
GO

-- Usa o banco
USE fiapcloudgames;
GO

-- Cria usuário vinculado ao login dentro do banco
CREATE USER fiapuser FOR LOGIN fiapuser;
GO

-- Dá permissões de leitura e escrita
ALTER ROLE db_datareader ADD MEMBER fiapuser;
ALTER ROLE db_datawriter ADD MEMBER fiapuser;

SELECT name FROM sys.sql_logins WHERE name = 'fiapuser';
