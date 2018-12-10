<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentDashboard.aspx.cs" Inherits="RecruitmentQnA.AgentChat.AgentDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="./botchat.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <style>
        /* The navigation bar */
        .navbar {
            overflow: hidden;
            background-color: #333;
            position: fixed; /* Set the navbar to fixed position */
            top: 0; /* Position the navbar at the top of the page */
            width: 100%; /* Full width */
        }

            /* Links inside the navbar */
            .navbar a {
                float: left;
                display: block;
                color: #f2f2f2;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
            }

                /* Change background on mouse-over */
                .navbar a:hover {
                    background: #ddd;
                    color: black;
                }

        /* Main content */
        .main {
            margin-top: 30px; /* Add a top margin to avoid content overlay */
        }
    </style>
</head>

<body>
    <form runat="server">
        <div class="main">
            <div class="container">
                <div class="navbar">
                    <a href="logout.aspx" onclick="connectAgent()">Logout</a>
                </div>
                <asp:HiddenField ID="hdnUserId" runat="server" />
                <asp:HiddenField ID="hdnDirectLineKey" runat="server" />
                <h2 style="font-family: Segoe UI;" id="heading">Agent Chat</h2>
                <div class="row">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <div id="BotChatGoesHere" style="float: left"></div>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="row">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <div>
                            <button class="btn btn-primary" id="connect" onclick="connectAgent()">Connect</button>
                            <button class="btn btn-primary" onclick="stopConversation()">Stop Conversation with User</button>
                        </div>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
            </div>
        </div>
    </form>
    <style>
        .wc-chatview-panel {
            width: 320px;
            height: 500px;
            position: relative;
        }

        .h2 {
            font-family: Segoe UI;
        }
    </style>
    <script src="./botchat.js"></script>
    <script>
        var params = BotChat.queryParams(location.search);
        var connected = false;
        var userid = document.getElementById('hdnUserId');
        var secret = document.getElementById('hdnDirectLineKey');

        var user = {
            id: userid.value || 'userid',
            name: userid.value || 'username'
        };

        var bot = {
            id: params['botid'] || 'botid',
            name: params["botname"] || 'botname'
        };

        window['botchatDebug'] = params['debug'] && params['debug'] === "true";

        var botConnection = new BotChat.DirectLine({
            secret: secret.value,
            token: params['t'],
            domain: params['domain'],
            webSocket: params['webSocket'] && params['webSocket'] === "true"
        });

        BotChat.App({
            botConnection: botConnection,
            user: user,
            bot: bot
        }, document.getElementById("BotChatGoesHere"));
        
        const connectAgent = () => {            
            var name;
            if (!connected)
                name = "connect"
            else
                name = "disconnect"
            botConnection
                .postActivity({ type: "event", value: "", from: user, name: name })
                .subscribe(connectionSuccess);
        };
        const connectionSuccess = (id) => {
            if (id === "retry")
                return;
            console.log("success");
            connected = !connected;
            if (connected) {
                document.getElementById("connect").innerHTML = "Disconnect";
                document.getElementById("heading").innerHTML = "You are now connected and are available for chat.";
            }
            else {
                document.getElementById("connect").innerHTML = "Connect";
                document.getElementById("heading").innerHTML = "You have been disconnected. Press connect to make yourself available.";
            }
        };

        const stopConversation = () => {
            botConnection
                .postActivity({ type: "event", value: "", from: user, name: "stopConversation" })
                .subscribe(id => console.log("success"));
        };

        </script>
</body>

</html>
