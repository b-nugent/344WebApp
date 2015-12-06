@echo off
set "CurrentFolder=%CD%\"
(   echo :r "%CurrentFolder%DropTablesScript.sql"
    echo :r "%CurrentFolder%CreateTablesScript.sql"
    echo :r "%CurrentFolder%\Stored Procedures\CreateAddCalendarEventSP.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateAddChatMessage.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateAddNoteSP.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateAddReceivedMessage.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateGetCalendarEventsSP.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateGetChatMessages.sql"
	echo :r "%CurrentFolder%\Stored Procedures\CreateStockTransaction.sql"
	echo :r "%CurrentFolder%\Stored Procedures\DeleteCalendarEvent.sql"
	echo :r "%CurrentFolder%\Stored Procedures\GetStockTransactions.sql"
	echo :r "%CurrentFolder%\Stored Procedures\ReturnStockNote.sql"
)>"%CurrentFolder%Main.sql"
sqlcmd.exe -E -S localhost\SQLEXPRESS -i "%CurrentFolder%Main.sql"
set "CurrentFolder="
pause