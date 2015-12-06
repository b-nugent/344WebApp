USE [aspnet-WebApplication5-20150927053851]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brian Nugent
-- Create date: 12-6-15
-- Description:	Adds a calendar event for a user
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCalendarEvent]
	@UserId nvarchar(128),
	@EventId int
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [dbo].[CalendarEvents]
    WHERE (@UserId = UserID) and (@EventId = EventID)
END
GO