USE [aspnet-WebApplication5-20150927053851]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Adam Prout
-- Create date: 12-2-15
-- Description:	Adds a chat message to the history
-- =============================================
CREATE PROCEDURE [dbo].[AddChatMessage]
	@UserId nvarchar(128),
	@MessageContent nvarchar(300),
	@TimeReceived datetime

AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[ChatHistory] (UserID, MessageContent, TimeReceived)
    VALUES (@UserId, @MessageContent, @TimeReceived); 
END
GO