

//briefly show login error message
function LoginOnFailure(result) {
    $("#YourNickname").val("");
    $("#Error").text(result.responseText);
    setTimeout("$('#Error').empty();", 2000);
}

//called every 5 seconds
function Refresh() {
    var href = "/Chat/Index?user=" + encodeURIComponent($("#YourNickname").text());

    //call the Index method of the controller
    $("a#ActionLink").attr("href", href);
    $("a#ActionLink")[0].click();
    setTimeout("Refresh();", 5000);
}

//Briefly show the error returned by the server
function ChatOnFailure(result) {
    $("#Error").text(result.responseText);
    setTimeout("$('#Error').empty();", 2000);
}

//Executed when a successful communication with the server is finished
function ChatOnSuccess(result) {
    ScrollChat();
    ShowLastRefresh();
}

//scroll the chat window to the bottom
function ScrollChat() {
    var wtf = $('#ChatHistory');
    var height = wtf[0].scrollHeight;
    wtf.scrollTop(height);
}

//show the last time the chat state was fetched from the server
function ShowLastRefresh() {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    $("#LastRefresh").text("Last Refresh - " + time);
}

