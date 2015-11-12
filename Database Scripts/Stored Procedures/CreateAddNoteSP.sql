USE [aspnet-WebApplication5-20150927053851]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrew Popovich
-- Create date: 11-8-2015
-- Description:	Add Stock Note
-- =============================================
CREATE PROCEDURE [dbo].[AddStockNote]
	@UserId nvarchar(128), 
	@StockName nvarchar(4), 
	@StockNote nvarchar(300)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO dbo.StockNotes (UserID, StockName, StockNote)
    VALUES (@UserId, @StockName, @StockNote);
END
