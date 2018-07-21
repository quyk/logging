CREATE TABLE [dbo].[Customersdd]
(
    [Id] INT IDENTITY(1, 1),
    [Name] NVARCHAR(10) NULL,
    [TotalPurchases] DECIMAL (18, 2) NOT NULL,
    [TotalReturns] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_dbo_Customers] PRIMARY KEY  CLUSTERED ([Id])  ON [PRIMARY] 
);
