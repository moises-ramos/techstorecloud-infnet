-- =========================================================
-- TechStore Cloud — Script de Criação do Banco de Dados
-- Azure SQL Database
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
