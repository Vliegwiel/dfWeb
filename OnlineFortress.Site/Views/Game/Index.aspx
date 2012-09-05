
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
        var debug;

        var tileSet;
        var white;
        var tileSetWidth = 256;

        var tileSize = 16;
        var height = 50;
        var width = 80;

        var tilecache = new Array(16);

        $(function () {
            tileSet = document.getElementById("tileSet");
            white = document.getElementById("white");

            $.connection.hub.logging = true;

            gameHub = $.connection.game;

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
                var canvasBack = document.getElementById("gamefieldBack");
                var canvasFront = document.getElementById("gamefieldFront");
                var ctxBack = canvasBack.getContext("2d");
                var ctxFront = canvasFront.getContext("2d");

                if (debug) {
                    console.log(screen);
                }
                if (tilecache.length < 16) {
                    return
                }
                for (y in screen) {
                    var yCord = round(y * tileSize);
                    for (x in screen[y]) {
                        var xCord = round(x * tileSize);
                        //ctxBack.clearRect(xCord, yCord, tileSize, tileSize);
                        ctxFront.fillStyle = colorNumerToHex(screen[y][x].BackColor);
                        ctxFront.fillRect(xCord, yCord, tileSize, tileSize);

                        ctxFront.clearRect(xCord, yCord, tileSize, tileSize);
                        var r = screen[y][x].ForeColor
                        var q = screen[y][x].tempChar;

                        ctxFront.putImageData(tilecache[r][q], xCord, yCord);
                    }
                }

            }
            console.log("test");
            $.connection.hub.start();

            // Setup keybinding
            $("#inp").keypress(function (event) {
                //if (event.which != 0) {
                //allow for f5 to function !? 
                event.preventDefault();
                //}
                console.log(event.which + " || " + event.keyCode);
                gameHub.sendKey(event.which, event.keyCode, event.altKey, event.ctrlKey, event.shiftKey);
            });
            console.log("test");
            setTimeout('loadColor(0)', 200);

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

        function loadColor(color) {
            var canvasBack = document.getElementById("gamefieldBack");
            var canvasFront = document.getElementById("gamefieldFront");
            var ctxBack = canvasBack.getContext("2d");
            var ctxFront = canvasFront.getContext("2d");

            console.log('loadColor(' + color + ')');
            var set = toRgbaFromAlphaChannel(white, tileSet)

            ctxFront.clearRect(0, 0, set.width, set.height);

            ctxFront.fillStyle = colorNumerToHex(color);
            ctxFront.fillRect(0, 0, set.width, set.height);
            //ctxFront.drawImage(white, 0, 0);
            ctxFront.globalCompositeOperation = 'xor';
            ctxFront.drawImage(set, 0, 0);

            tilecache[color] = new Array(256);
            var i = 0
            for (y = 0; y < 16; y++) {
                var yCord = round(y * tileSize);
                    
                for (x = 0; x < 16; x++) {
                    var xCord = round(x * tileSize);
                    tilecache[color][i++] = ctxFront.getImageData(xCord, yCord, 16, 16);
                } // For x
            } // For y

            if ((color < 16) && (color >= 0)) {
                setTimeout('loadColor(' + (color + 1) + ')', 1);
            } else if(color === 16) {
                $.connection.hub.start();
            }

        }



        var toRgbaFromAlphaChannel = function (rgbImage, alphaChannelImage) {
            var width = alphaChannelImage.width;
            var height = alphaChannelImage.height;

            return renderToCanvas(width, height, function (ctx) {
                var alpha = renderToCanvas(width, height, function (ctx) {
                    var id, data, i;
                    ctx.drawImage(alphaChannelImage, 0, 0);
                    id = ctx.getImageData(0, 0, width, height);
                    data = id.data;

                    for (i = data.length - 1; i > 0; i -= 4) {
                        var R = data[i - 3];
                        var G = data[i - 2];
                        var B = data[i - 1];
                        var A = data[i];

                        if (A === 255) {
                            // if no transpiracy check for grayness, grayscales get inverted for the xor
                            if ((R == G) && (G == B)) {
                                data[i] = 255 - data[i - 3];
                                data[i - 3] = 0;
                                data[i - 2] = 0;
                                data[i - 1] = 0;
                            }
                            data[i - 3] = 0;
                            data[i - 2] = 0;
                            data[i - 1] = 0;
                        } else {
                            // Pixel has an alpha value, invert that for the xor
                            data[i] = 255 - data[i];
                            data[i - 3] = 0;
                            data[i - 2] = 0;
                            data[i - 1] = 0;
                        }

                    }
                    ctx.clearRect(0, 0, width, height);
                    ctx.putImageData(id, 0, 0);
                });

                //ctx.globalCompositeOperation = 'source-over';
                //ctx.drawImage(rgbImage, 0, 0);
                //ctx.globalCompositeOperation = 'xor';
                ctx.drawImage(alpha, 0, 0);

            });
        };

        var renderToCanvas = function (width, height, renderFunction) {
            var buffer = document.getElementById("buffer");
            buffer.width = width;
            buffer.height = height;
            renderFunction(buffer.getContext('2d'));
            return buffer;
        };

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
            background: #000;
        }
    </style>
</head>
<body>
    <div>
        <input type="button" onclick="DrawFullScreen();" title="update" value="update" />
        <input type="text" id="inp" />
        <input type="button" onclick="debug = true;" title="debug" value="debug" /><br />
        
        <div id="gamegrid">
            <canvas id="gamefieldBack" width="1440px" height="1280px" style="position: absolute; z-index: 9; display:none;"></canvas>
            <canvas id="gamefieldFront" width="1440px" height="1280px" style="position: absolute; z-index: 10;" ></canvas>
        </div>
        <br />
        <br />
            <canvas id="buffer" width="1440px" height="1280px" style="position: absolute; z-index: 9; display:none;"></canvas>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <img id="tileSet" src="images/Phoebus_16x16_Diagonal.png" style="display:none;" />
    </div>
</body>
</html>
