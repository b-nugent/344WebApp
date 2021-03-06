CREATE TABLE [aspnet-WebApplication5-20150927053851].dbo.StockTransactions (
  TransactionID INTEGER IDENTITY(1,1) PRIMARY KEY,
  UserID NVARCHAR(128)FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.AspNetUsers ON DELETE CASCADE NOT NULL,
  StockName NVARCHAR(4) NOT NULL,
  Quantity INTEGER NOT NULL,
  TransactionPrice DECIMAL NOT NULL,
  HasSold BIT NOT NULL
);

CREATE TABLE [aspnet-WebApplication5-20150927053851].dbo.StockNotes (
  NoteID INTEGER IDENTITY(1,1) PRIMARY KEY,
  UserID NVARCHAR(128)FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.AspNetUsers ON DELETE CASCADE NOT NULL,
  StockName NVARCHAR(4) NOT NULL,
  StockNote NVARCHAR(300) NOT NULL
);

CREATE TABLE [aspnet-WebApplication5-20150927053851].dbo.CalendarEvents (
  EventID INTEGER IDENTITY(1,1) PRIMARY KEY,
  UserID NVARCHAR(128)FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.AspNetUsers ON DELETE CASCADE NOT NULL,
  EventName NVARCHAR(100) NOT NULL,
  EventStartTime DATETIME NOT NULL,
  EventEndTime DATETIME NOT NULL,
  EventDescription NVARCHAR(300) NOT NULL
);

CREATE TABLE [aspnet-WebApplication5-20150927053851].dbo.ChatHistory (
  MessageID INTEGER IDENTITY(1,1) PRIMARY KEY,
  UserID NVARCHAR(128)FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.AspNetUsers ON DELETE CASCADE NOT NULL,
  MessageContent NVARCHAR(300) NOT NULL,
  TimeReceived DATETIME NOT NULL
);

CREATE TABLE [aspnet-WebApplication5-20150927053851].dbo.UserChatHistory (
  ReceivedUserID NVARCHAR(128)FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.AspNetUsers NOT NULL,
  MessageID INTEGER FOREIGN KEY REFERENCES [aspnet-WebApplication5-20150927053851].dbo.ChatHistory(MessageID) ON DELETE CASCADE
);