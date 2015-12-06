DROP TABLE [aspnet-WebApplication5-20150927053851].dbo.UserChatHistory;
DROP TABLE [aspnet-WebApplication5-20150927053851].dbo.ChatHistory;
DROP TABLE [aspnet-WebApplication5-20150927053851].dbo.StockNotes;
DROP TABLE [aspnet-WebApplication5-20150927053851].dbo.StockTransactions;
DROP TABLE [aspnet-WebApplication5-20150927053851].dbo.CalendarEvents;

USE [aspnet-WebApplication5-20150927053851]
GO
DROP PROC dbo.AddCalendarEvent;
DROP PROC dbo.AddReceivedMessage;
DROP PROC dbo.AddStockNote;
DROP PROC dbo.AddChatMessage;
DROP PROC dbo.GetCalendarEvents;
DROP PROC dbo.GetChatMessages;
DROP PROC dbo.CreateStockTransaction;
DROP PROC dbo.DeleteCalendarEvent;
DROP PROC dbo.GetStockTransaction;
DROP PROC dbo.GetStockNote;