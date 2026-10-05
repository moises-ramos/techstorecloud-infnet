-- =========================================================
-- TechStore Cloud — Script de Criação do Banco de Dados
-- Azure SQL Database
-- =========================================================

-- Criar a tabela de Produtos
-- (Este script deve ser executado no Azure SQL Database
--  caso as migrations do EF Core não sejam utilizadas)
-- =========================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produtos')
BEGIN
    CREATE TABLE [dbo].[Produtos] (
        [Id]                 INT             IDENTITY(1,1) NOT NULL,
        [Nome]               NVARCHAR(200)   NOT NULL,
        [Descricao]          NVARCHAR(1000)  NULL,
        [Categoria]          NVARCHAR(100)   NOT NULL,
        [Preco]              DECIMAL(18,2)   NOT NULL,
        [QuantidadeEstoque]  INT             NOT NULL,
        [DataCriacao]        DATETIME2       NOT NULL DEFAULT (GETUTCDATE()),
        [DataAtualizacao]    DATETIME2       NULL,
        [Ativo]              BIT             NOT NULL DEFAULT (1),

        CONSTRAINT [PK_Produtos] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Índice para consultas por Categoria
    CREATE NONCLUSTERED INDEX [IX_Produtos_Categoria]
        ON [dbo].[Produtos] ([Categoria]);

    -- Índice para filtrar produtos ativos
    CREATE NONCLUSTERED INDEX [IX_Produtos_Ativo]
        ON [dbo].[Produtos] ([Ativo]);

    PRINT 'Tabela [Produtos] criada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Tabela [Produtos] já existe.';
END
GO

-- =========================================================
-- Dados de exemplo (opcional)
-- =========================================================

INSERT INTO [dbo].[Produtos] ([Nome], [Descricao], [Categoria], [Preco], [QuantidadeEstoque])
VALUES
    (N'Licença Microsoft 365 Business', N'Assinatura anual do Microsoft 365 para empresas com Word, Excel, PowerPoint, Teams e OneDrive.', N'Software', 899.90, 500),
    (N'Azure DevOps Server 2022', N'Licença perpétua do Azure DevOps Server para CI/CD e gerenciamento de projetos on-premises.', N'Software', 4599.00, 50),
    (N'Surface Pro 9', N'Tablet 2-em-1 com processador Intel Core i7, 16GB RAM, 256GB SSD, tela 13 polegadas.', N'Hardware', 12499.00, 30),
    (N'Teclado Mecânico Gamer RGB', N'Teclado mecânico com switches Cherry MX Red, iluminação RGB e design ergonômico.', N'Acessórios', 459.90, 200),
    (N'Mouse Wireless Ergonômico', N'Mouse sem fio ergonômico com sensor de 4000 DPI e bateria recarregável.', N'Acessórios', 189.90, 350);
GO
