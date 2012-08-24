
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

        var tileSet;
        var tileSetWidth = 256;

        var tileSize = 16;
        var height = 50;
        var width = 80;

        $(function () {
            //var imageDimensions = { width: 18, height: 18 };
            //var spriteSheet = new SpriteSheet('images/coins.png', imageDimensions);
            //var firstImage = spriteSheet.get(0);

            tileSet = document.getElementById("tileSet");
//            tileSet.src = "images/ironhand_diagonal.png";

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
                //console.log(screen);

                for (y in screen) {
                    var yCord = round(y * tileSize);
                    for (x in screen[y]) {
                        var xCord = round(x * tileSize);
                        ctxBack.clearRect(xCord, yCord, tileSize, tileSize);
                        ctxBack.fillStyle = colorNumerToHex(screen[y][x].BackColor);
                        ctxBack.fillRect(xCord, yCord, tileSize, tileSize);

                        ctxFront.clearRect(xCord, yCord, tileSize, tileSize);

                        var sprite = getSprite(screen[y][x].tempChar);
                        if (sprite != null) {
                            //ctxFront.drawImage(tileSet, xCord, yCord);
                            ctxFront.drawImage(tileSet, sprite.x, sprite.y, tileSize, tileSize, xCord, yCord, tileSize, tileSize);
                        }

                    }
                }

            }

            $.connection.hub.start();

            // Setup keybinding
            var xTriggered = 0;
            $("#inp").keypress(function (event) {
                //if (event.which != 0) {
                //allow for f5 to function !? 
                event.preventDefault();
                //}
                console.log(event.which + " || " + event.keyCode);
                gameHub.sendKey(event.which, event.keyCode, event.altKey, event.ctrlKey, event.shiftKey);
            });

        });

        function connectionReady() {
            alert("Done calling first hub serverside-function");
        }

        function DrawFullScreen() {
            var screen = gameHub.drawFullScreen();
        }
        function floor(dbl) {
            return ~~dbl;
        }
        function round(dbl) {
            return (0.5 + dbl) | 0;
        }

        // Gets an sprites location on the sprite
        function getSprite(index) {
           if (index > 256) {
              switch (index) {
                 case 9552:
                    return getSprite(205);
              }
               console.log(index);
                return null; //Chars beyond this fall of the tileset figure this out later
            }
            var tileY = floor(index / (tileSetWidth / tileSize));
            var tileX = floor(index % (tileSetWidth / tileSize));
            return { "x": tileX * tileSize, "y": tileY * tileSize };
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
        * {
            margin: 0;
            padding: 0;
        }
        .clear {
            clear: both;
        }
        body {
            background-color: #000;
        }
    </style>
</head>
<body>
    <div>
        <input type="button" onclick="DrawFullScreen();" title="update" value="update" /><input type="text" id="inp" /><br />
        <div id="gamegrid">
            <canvas id="gamefieldBack" width="1440px" height="1280px" style="position: absolute; z-index: 10;"></canvas>
            <canvas id="gamefieldFront" width="1440px" height="12804px" style="position: absolute; z-index: 9;" ></canvas>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <img id="tileSet" src="images/Phoebus_16x16_Diagonal.png" style="display: none;" />
        
    </div>
</body>
</html>
