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
USE [aspnet-WebApplication5-20150927053851]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Adam Prout
-- Create date: 12-4-2015
-- Description:	Add the received message to the database
-- =============================================
CREATE PROCEDURE [dbo].[AddReceivedMessage]
	-- Add the parameters for the stored procedure here
	@ReceivedUserId nvarchar(128),
	@MessageID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO [dbo].[UserChatHistory] (ReceivedUserID, MessageID)
   VALUES (@ReceivedUserId, @MessageID); 
END
GO
