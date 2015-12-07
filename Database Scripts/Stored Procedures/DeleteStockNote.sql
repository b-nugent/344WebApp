USE [aspnet-WebApplication5-20150927053851]
GO
/****** Object:  StoredProcedure [dbo].[GetStockHistory]    Script Date: 12/06/2015 17:17:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Steven Lavoie
-- Create date: 12-2-15
-- Description:	Return a stock transaction
-- =============================================
CREATE PROCEDURE [dbo].[DeleteStockNote]
	@UserId nvarchar(128),
	@StockName nvarchar(4)
AS
BEGIN
	SET NOCOUNT ON;
	--0 is a buy, 1 is a sell on HasSold bit
	DELETE 
    FROM dbo.StockNotes
    WHERE ((UserID = @UserId) AND (StockName = @StockName));
END
