<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
                var canvasBack = document.getElementById("gamefieldBack");
                var canvasFront = document.getElementById("gamefieldFront");
                var ctxBack = canvasBack.getContext("2d");
                var ctxFront = canvasFront.getContext("2d");
                ctxFront.font = "16px mayday-no-highlight";
                ctxFront.textBaseline = "top";

                for (y in screen) {
                    for (x in screen[y]) {
                        ctxBack.clearRect(x * 16, y * 16, 16, 16);
                        ctxBack.fillStyle = colorNumerToHex(screen[y][x].BackColor);
                        ctxBack.fillRect(x * 16, y * 16, 16, 16);   

                        ctxFront.clearRect(x * 16, y * 16, 16, 16);
                        ctxFront.fillStyle = colorNumerToHex(screen[y][x].ForeColor);
                        ctxFront.fillText(screen[y][x].Character, x * 16, y * 16);
                            
                    }
                }

            }

            $.connection.hub.start();

            // Setup keybinding
            var xTriggered = 0;
            $("#inp").keypress(function (event) {
                //if (event.which != 0) {
                // allow for f5 to function !? 
                //event.preventDefault();
                // }
                console.log(event.which +  " || " + event.keyCode);
                gameHub.sendKey(event.which, event.keyCode, event.altKey, event.ctrlKey, event.shiftKey);
            });

        });

        function connectionReady() {
            alert("Done calling first hub serverside-function");
        }

        function DrawFullScreen() {
            var screen = gameHub.drawFullScreen();
        }

        // Convert an consoleColor to an html color;
        function colorNumerToHex(color) {
        
            switch (color) {
                case 0:
                    return "#000000";
                case 1:
                    return "#0000BB";
                case 2:
                    return "#00BB00";
                case 3:
                    return "#00BBBB";
                case 4:
                    return "#BB0000";
                case 5:
                    return "#BB00BB";
                case 6:
                    return "#BBBB00";        
                case 7:
                    return "#BBBBBB";
                case 8:
                    return "#555555";
                case 9:
                    return "#5555FF";
                case 10:
                    return "#55FF55";
                case 11:
                    return "#55FFFF";
                case 12:
                    return "#FF5555";
                case 13:
                    return "#FF55FF";
                case 14:
                    return "#FFFF55";
                case 15:
                    return "#FFFFFF";
            }
            return "FF0099";
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
        <input type="button" onclick="DrawFullScreen();" title="update" value="update" /><input type="text" id="inp" /><br />
        <div id="gamegrid">
            <canvas id="gamefieldBack" width="1280px" height="800px" style="position: absolute; z-index: 9;"></canvas>
            <canvas id="gamefieldFront" width="1280px" height="800px" style="position: absolute; z-index: 10;" ></canvas>
        </div>
        
        <br />
    </div>
</body>
</html>
