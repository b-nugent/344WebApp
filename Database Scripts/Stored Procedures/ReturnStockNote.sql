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
CREATE PROCEDURE [dbo].[GetStockNote]
	@UserId nvarchar(128), 
	@StockName nvarchar(4), 
	@StockNote nvarchar(300)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT StockNote
    FROM dbo.StockNotes 
    WHERE ((UserID = @UserId) AND (StockName = @StockName));
END
