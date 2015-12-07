USE [aspnet-WebApplication5-20150927053851]
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Steven Lavoie
-- Create date: 12-2-15
-- Description:	Return a note
-- =============================================
CREATE PROCEDURE [dbo].[CreateStockTransaction]
	@UserId nvarchar(128), 
	@StockName nvarchar(4), 
	@Quantity int,
	@TransactionPrice decimal,
	@HasSold bit
AS
BEGIN
	SET NOCOUNT ON;
	--0 is a buy, 1 is a sell on HasSold bit
    INSERT INTO dbo.StockTransactions (UserID, StockName, Quantity, TransactionPrice, HasSold)
    VALUES (@UserId, @StockName, @Quantity, @TransactionPrice, @HasSold)
END
GO