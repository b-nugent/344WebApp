USE [aspnet-WebApplication5-20150927053851]
GO
/****** Object:  StoredProcedure [dbo].[GetStockTransaction]    Script Date: 12/06/2015 02:41:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Steven Lavoie
-- Create date: 12-2-15
-- Description:	Return a stock transaction
-- =============================================
CREATE PROCEDURE [dbo].[GetUniqueStocks]
	@UserId nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;
	--0 is a buy, 1 is a sell on HasSold bit
	SELECT DISTINCT StockName
    FROM dbo.StockTransactions 
    WHERE ((UserID = @UserId));
END
