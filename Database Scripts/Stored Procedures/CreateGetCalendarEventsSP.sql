USE [aspnet-WebApplication5-20150927053851]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrew Popovich
-- Create date: 12-2-15
-- Description:	Return the events for a user
-- =============================================
CREATE PROCEDURE [dbo].[GetCalendarEvents]
	@UserId nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT EventID, EventName, EventStartTime, EventEndTime, EventDescription
    FROM dbo.CalendarEvents
    WHERE (UserID = @UserId);
END
GO