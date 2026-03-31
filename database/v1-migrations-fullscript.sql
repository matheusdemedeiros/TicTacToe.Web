IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523021732_Initial-migration'
)
BEGIN
    CREATE TABLE [TicPlayers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [NickName] nvarchar(150) NOT NULL,
        [Symbol] nvarchar(1) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        CONSTRAINT [PK_TicPlayers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250523021732_Initial-migration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250523021732_Initial-migration', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE TABLE [TicMatches] (
        [Id] uniqueidentifier NOT NULL,
        [State] int NOT NULL,
        [PlayMode] int NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [Board] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TicMatches] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE TABLE [TicMatchPlayers] (
        [MatchId] uniqueidentifier NOT NULL,
        [PlayerId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TicMatchPlayers] PRIMARY KEY ([MatchId], [PlayerId]),
        CONSTRAINT [FK_TicMatchPlayers_TicMatches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [TicMatches] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TicMatchPlayers_TicPlayers_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [TicPlayers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE TABLE [TicScores] (
        [Id] uniqueidentifier NOT NULL,
        [WinningSymbol] nvarchar(1) NULL,
        [WinningPlayerId] uniqueidentifier NULL,
        [Tie] bit NOT NULL,
        [MatchId] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        CONSTRAINT [PK_TicScores] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TicScores_TicMatches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [TicMatches] ([Id]),
        CONSTRAINT [FK_TicScores_TicPlayers_WinningPlayerId] FOREIGN KEY ([WinningPlayerId]) REFERENCES [TicPlayers] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE INDEX [IX_TicMatchPlayers_PlayerId] ON [TicMatchPlayers] ([PlayerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TicScores_MatchId] ON [TicScores] ([MatchId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    CREATE INDEX [IX_TicScores_WinningPlayerId] ON [TicScores] ([WinningPlayerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250527010050_Adiciciona as matches no contexto'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250527010050_Adiciciona as matches no contexto', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530022916_Adiciona o conceito do jogador atual'
)
BEGIN
    ALTER TABLE [TicMatches] ADD [CurrentPlayerId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530022916_Adiciona o conceito do jogador atual'
)
BEGIN
    CREATE INDEX [IX_TicMatches_CurrentPlayerId] ON [TicMatches] ([CurrentPlayerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530022916_Adiciona o conceito do jogador atual'
)
BEGIN
    ALTER TABLE [TicMatches] ADD CONSTRAINT [FK_TicMatches_TicPlayers_CurrentPlayerId] FOREIGN KEY ([CurrentPlayerId]) REFERENCES [TicPlayers] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530022916_Adiciona o conceito do jogador atual'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530022916_Adiciona o conceito do jogador atual', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530023754_Ajusta a coluna currentPlayerId'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TicMatches]') AND [c].[name] = N'CurrentPlayerId');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [TicMatches] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [TicMatches] ALTER COLUMN [CurrentPlayerId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530023754_Ajusta a coluna currentPlayerId'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530023754_Ajusta a coluna currentPlayerId', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530052413_Ajusta a coluna do board'
)
BEGIN
    EXEC sp_rename N'[TicMatches].[Board]', N'BoardJson', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530052413_Ajusta a coluna do board'
)
BEGIN
    ALTER TABLE [TicMatches] ADD [WinningSimbol] nvarchar(1) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530052413_Ajusta a coluna do board'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530052413_Ajusta a coluna do board', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530173826_Torna o winning symbol nulo'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TicMatches]') AND [c].[name] = N'WinningSimbol');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [TicMatches] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [TicMatches] ALTER COLUMN [WinningSimbol] nvarchar(1) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530173826_Torna o winning symbol nulo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530173826_Torna o winning symbol nulo', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TicMatches]') AND [c].[name] = N'BoardJson');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [TicMatches] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [TicMatches] DROP COLUMN [BoardJson];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TicMatches]') AND [c].[name] = N'WinningSimbol');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [TicMatches] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [TicMatches] DROP COLUMN [WinningSimbol];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    ALTER TABLE [TicMatches] ADD [TicBoardId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    CREATE TABLE [TicBoards] (
        [Id] uniqueidentifier NOT NULL,
        [WinningSimbol] nvarchar(1) NULL,
        [Board] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        CONSTRAINT [PK_TicBoards] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TicMatches_TicBoardId] ON [TicMatches] ([TicBoardId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    ALTER TABLE [TicMatches] ADD CONSTRAINT [FK_TicMatches_TicBoards_TicBoardId] FOREIGN KEY ([TicBoardId]) REFERENCES [TicBoards] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530181421_cria a tabela do board'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530181421_cria a tabela do board', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530201124_torna o symbolo nulable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530201124_torna o symbolo nulable', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530201309_torna o symbolo nulable 2'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TicPlayers]') AND [c].[name] = N'Symbol');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [TicPlayers] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [TicPlayers] ALTER COLUMN [Symbol] nvarchar(1) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530201309_torna o symbolo nulable 2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530201309_torna o symbolo nulable 2', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328185416_AddShortCodeToTicMatch'
)
BEGIN
    ALTER TABLE [TicMatches] ADD [ShortCode] nvarchar(6) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328185416_AddShortCodeToTicMatch'
)
BEGIN

                    UPDATE TicMatches
                    SET ShortCode = LEFT(REPLACE(NEWID(), '-', ''), 6)
                    WHERE ShortCode = '';
                
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328185416_AddShortCodeToTicMatch'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TicMatches_ShortCode] ON [TicMatches] ([ShortCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260328185416_AddShortCodeToTicMatch'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260328185416_AddShortCodeToTicMatch', N'9.0.5');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329035721_AddComputerDifficultyToTicMatch'
)
BEGIN
    ALTER TABLE [TicMatches] ADD [ComputerDifficulty] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329035721_AddComputerDifficultyToTicMatch'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329035721_AddComputerDifficultyToTicMatch', N'9.0.5');
END;

COMMIT;
GO

