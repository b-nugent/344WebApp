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
-- Author:		Adam Prout
-- Create date: 12-4-2015
-- Description:	Select messages from databases
-- =============================================
CREATE PROCEDURE [dbo].[GetChatMessages]
	@UserID nvarchar(128)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT H.MessageContent, H.UserID
	FROM dbo.ChatHistory H, dbo.UserChatHistory U
	WHERE (U.ReceivedUserID = @UserID) AND (H.MessageID = U.MessageID);
END
GO
