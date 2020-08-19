CREATE TABLE [Config].[Modules] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Code]         NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Name]         NVARCHAR (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [NameCultured] NVARCHAR (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [Description]  NVARCHAR (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IsDeleted]     BIT            NOT NULL,
    [CreationDateTimeUtc]  DATETIME       NULL,
    [LastModificationDateTimeUtc] DATETIME       NULL,
    [CreatedByUserId]    NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [LastModifiedByUserId]   NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DeletionDateTimeUtc]     DATETIME NULL,
    [DeletedByUserId] NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL, 
    CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

