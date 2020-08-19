CREATE TABLE [Config].[Configs] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Code]         NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [ModuleId]     INT            NOT NULL,
    [Key]          NVARCHAR (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Value]        NVARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Description]  NVARCHAR (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [IsDeleted]     BIT            NOT NULL,
    [CreationDateTimeUtc]  DATETIME       NULL,
    [LastModificationDateTimeUtc] DATETIME       NULL,
    [CreatedByUserId]    NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [LastModifiedByUserId]   NVARCHAR (50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DeletionDateTimeUtc] DATETIME NULL, 
    [DeletedByUserId] NVARCHAR(50)  COLLATE SQL_Latin1_General_CP1_CI_AS NULL, 
    CONSTRAINT [PK_Configs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

