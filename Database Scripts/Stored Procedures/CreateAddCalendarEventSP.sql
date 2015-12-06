USE [aspnet-WebApplication5-20150927053851]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrew Popovich
-- Create date: 12-2-15
-- Description:	Adds a calendar event for a user
-- =============================================
CREATE PROCEDURE [dbo].[AddCalendarEvent]
	@UserId nvarchar(128),
	@EventId int,
	@EventName nvarchar(100),
	@EventDescription nvarchar(300),
	@EventStart datetime,
	@EventEnd datetime
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[CalendarEvents] ( UserID, EventID, EventName, EventDescription, EventStartTime, EventEndTime)
    VALUES ( @UserId, @EventId, @EventName, @EventDescription, @EventStart, @EventEnd); 
END
