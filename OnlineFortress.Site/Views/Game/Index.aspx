
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
        var tileSetWidth = 256;

        var tileSize = 16;
        var height = 50;
        var width = 80;

        var bTileCacheLoaded = false;
        var alphamask;
        var tilecache = new Array(16);

        $(function () {
            tileSet = document.getElementById("tileSet");

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
                if (!bTileCacheLoaded) {
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
            setTimeout('loadColor(0)', 100);

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
                    //[BLUE_R:46][BLUE_G:88][BLUE_B:255]
                    //return "#0000BB";
                    return "#2E58FF";
                case 2:
                    //[GREEN_R:70][GREEN_G:170][GREEN_B:56]
                    return "#46AA38";
                    //return "#00BB00";
                case 3:
                    //[CYAN_R:56][CYAN_G:136][CYAN_B:170]
                    return "#3888AA";
                    //return "#00BBBB";
                case 4:
                    //[RED_R:170]
                    return "#AA0000";
                    //return "#BB0000";
                case 5:
                    //[MAGENTA_R:170][MAGENTA_G:56][MAGENTA_B:136]
                    return "#AA3888";
                    //return "#BB00BB";
                case 6:
                    //[BROWN_R:170][BROWN_G:85][BROWN_B:28]
                    return "#AA551C";
                    //return "#BBBB00";
                case 7:
                    //[LGRAY_R:187][LGRAY_G:177][LGRAY_B:167]
                    return "#BBB1A7";
                    //return "#BBBBBB";
                case 8:
                    //[DGRAY_R:135][DGRAY_G:125][DGRAY_B:115]
                    return "#877D73";
                    //return "#555555";
                case 9:
                    //[LBLUE_R:96][LBLUE_G:128][LBLUE_B:255]
                    return "#6080E1";
                    //return "#5555FF";
                case 10:
                    //[LGREEN_R:105][LGREEN_G:255][LGREEN_B:84]
                    return "#69FF54";
                    //return "#55FF55";
                case 11:
                    //[LCYAN_R:84][LCYAN_G:212][LCYAN_B:255]
                    return "#54D4FF";
                    //return "#55FFFF";
                case 12:
                    //[LRED_R:255]
                    return "#FF0000";
                    //return "#FF5555";
                case 13:
                    //[LMAGENTA_R:255][LMAGENTA_G:84][LMAGENTA_B:212]
                    return "#FF54D4";
                    //return "#FF55FF";
                case 14:
                    //[YELLOW_R:255][YELLOW_G:204]
                    return "#FFCC00";
                    //return "#FFFF55";
                case 15:
                    //[WHITE_R:255][WHITE_G:250][WHITE_B:245]
                    return "#FFFAF5";
                    //return "#FFFFFF";
            }
            return "FF0099"; // Error color magicpink
        }

        function loadColor(color) {
            var canvasBack = document.getElementById("gamefieldBack");
            var canvasFront = document.getElementById("gamefieldFront");
            var ctxBack = canvasBack.getContext("2d");
            var ctxFront = canvasFront.getContext("2d");

            console.log('loadColor(' + color + ')');
            if (alphamask == null) {
                console.log("Loading alphamask");
                alphamask = toRgbaFromAlphaChannel(tileSet)
            }

            ctxFront.clearRect(0, 0, alphamask.width, alphamask.height);

            ctxFront.fillStyle = colorNumerToHex(color);
            ctxFront.fillRect(0, 0, alphamask.width, alphamask.height);
            ctxFront.globalCompositeOperation = 'xor';
            ctxFront.drawImage(alphamask, 0, 0);

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
            } else if (color === 16) {
                bTileCacheLoaded = true;
                $.connection.hub.start();
            }

        }



        var toRgbaFromAlphaChannel = function (alphaChannelImage) {
            var width = alphaChannelImage.width;
            var height = alphaChannelImage.height;

            return renderToCanvas(width, height, function (ctx) {
                var alpha = renderToCanvas(width, height, function (ctx) {
                    var id, data, i;
                    ctx.drawImage(alphaChannelImage, 0, 0);
                    id = ctx.getImageData(0, 0, width, height);
                    data = id.data;

                    //  loop trough each pixel
                    for (i = data.length - 1; i > 0; i -= 4) {
                        var R = data[i - 3];
                        var G = data[i - 2];
                        var B = data[i - 1];
                        var A = data[i];

                        if (true) {
                            // base foreground transpiracy on the color of grayness of everything that has a highalpha
                            var rgbMax = Math.max(R, G, B);
                            var rgbMin = Math.min(R, G, B);

                            // allow for a variance of 1 in the grayscale, since that happens on the tilesheets
                            if ((rgbMax - rgbMin) <= 1) {
                                data[i] = 255 - R;
                                data[i - 3] = 0;
                                data[i - 2] = 0;
                                data[i - 1] = 0;
                            } else {
                                // remove nongrayscale pixels from the transpiracy sheet
                                data[i] = 255;
                                data[i - 3] = 0;
                                data[i - 2] = 0;
                                data[i - 1] = 0;
                            }
                        }
                    }
                    ctx.clearRect(0, 0, width, height);
                    ctx.putImageData(id, 0, 0);
                });

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
