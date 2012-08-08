﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index</title>
    <script type="text/javascript" src="../../Scripts/jquery-1.6.4.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.signalR-0.5.2.min.js"></script>
    <script type="text/javascript" src="signalr/hubs"></script>
    <script type="text/javascript" src="../../Scripts/benchmarker.js"></script>
    <script type="text/javascript">
        var ConnectionId = "";

        var height = 50;
        var width = 80;

        $(function () {
            $.connection.hub.logging = true;

            gameHub = $.connection.game;
            //myHub.someState = "SomeValue";

            $.connection.hub.error(function () {
                alert("An error occured");
            });

            gameHub.connectCallback = function (connectionId) {
                this.ConnectionId = connectionId
            }
            gameHub.addMessage = function (message) {
                alert(message);
                $('#messages').append('<li>' + message + '');
            };
            gameHub.ScreenUpdate = function (screen) {
                //BackColor                0
                //Character                "╔"
                //ForeColor                9
                var canvas = document.getElementById("gamefield");
                var ctx = canvas.getContext("2d");
                ctx.font = "16px mayday-no-highlight";
                ctx.textBaseline = "top";

                for (y in screen) {
                    for (x in screen[y]) {
                        ctx.clearRect(x * 16, y * 16, 16, 16);
                        ctx.fillStyle = "#000";
                        ctx.fillRect(x * 16, y * 16, 16, 16); 
                        ctx.fillStyle = "#CCC";
                        ctx.fillText(screen[y][x].Character, x * 16, y * 16);
                    }
                }

            }

            $.connection.hub.start();

            // Setup keybinding
            var xTriggered = 0;
            $("#gamefield").keypress(function (event) {
                //if (event.which != 0) {
                    // allow for f5 to function !? 
                    //event.preventDefault();
                // }
                console.log(event);
                gameHub.sendKey(event.which, event.altKey, event.ctrlKey, event.shiftKey);
            });

        });

        function connectionReady() {
            alert("Done calling first hub serverside-function");
        }

        function DrawFullScreen() {
            var screen = gameHub.drawFullScreen();
        }



    </script>
    <style type="text/css" media="all">
        *
        {
            margin: 0;
            padding: 0;
        }
        .clear
        {
            clear: both;
        }
        
        #gamegrid
        {
            font-family: mayday-no-highlight;
            font-size: 16pt;
        }
        
        .cell
        {
            float: left;
            height: 20px;
            width: 20px;
        }
        .bg0
        {
            background: #000;
        }
        .bg1
        {
            background: rgb(0, 0, 187);
        }
        .bg2
        {
            background: rgb(0, 187, 0);
        }
        .bg3
        {
            background: rgb(0, 187, 187);
        }
        .bg4
        {
            background: rgb(187, 0, 0);
        }
        .bg5
        {
            background: rgb(187, 0, 187);
        }
        .bg6
        {
            background: rgb(187, 187, 0);
        }
        .bg7
        {
            background: rgb(187, 187, 187);
        }
        .bg8
        {
            background: rgb(85, 85, 85);
        }
        .bg9
        {
            background: rgb(85, 85, 255);
        }
        .bg10
        {
            background: rgb(85, 255, 85);
        }
        .bg11
        {
            background: rgb(85, 255, 255);
        }
        .bg12
        {
            background: rgb(255, 85, 85);
        }
        .bg13
        {
            background: rgb(255, 85, 255);
        }
        .bg14
        {
            background: rgb(255, 255, 85);
        }
        .bg15
        {
            background: #FFF;
        }
        
        .col0
        {
            color: #000;
        }
        .col1
        {
            color: rgb(0, 0, 187);
        }
        .col2
        {
            color: rgb(0, 187, 0);
        }
        .col3
        {
            color: rgb(0, 187, 187);
        }
        .col4
        {
            color: rgb(187, 0, 0);
        }
        .col5
        {
            color: rgb(187, 0, 187);
        }
        .col6
        {
            color: rgb(187, 187, 0);
        }
        .col7
        {
            color: rgb(187, 187, 187);
        }
        .col8
        {
            color: rgb(85, 85, 85);
        }
        .col9
        {
            color: rgb(85, 85, 255);
        }
        .col10
        {
            color: rgb(85, 255, 85);
        }
        .col11 { color: rgb(85, 255, 255);}
        .col12 { color: rgb(255, 85, 85); }
        .col13 { color: rgb(255, 85, 255); }
        .col14 { color: rgb(255, 255, 85); }
        .col15 { color: #FFF; }
    </style>
</head>
<body>
    <div>
        <!--<textarea id="output" rows="20" cols="80"></textarea>-->
        <input type="button" onclick="DrawFullScreen();" title="update" value="update" /><br />
        <canvas id="gamefield" width="1280px" height="800px"></canvas>
        <br />
    </div>
</body>
</html>