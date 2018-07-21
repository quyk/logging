USE [FloggingConsole.Models.CustomerDbContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateNewCustomer]
    @Name NVARCHAR(MAX),
    @TotalPerchases MONEY,
    @TotalReturns MONEY
AS
    INSERT INTO dbo.Customers
    VALUES (@Name, @TotalPerchases, @TotalReturns)
