// Type: DwarfFortressConfig.ScreenGenerator
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System.Drawing;
using System.Drawing.Imaging;

namespace OnlineFortress.TextureTest {
    public class ScreenGenerator {
        public static byte[, ,] tiles = new byte[16, 16, 3] {
            {
                {32, 8, 0},
                {37, 8, 0},
                {32, 8, 0},
                {131, 8, 7},
                {43, 15, 7},
                {43, 15, 7},
                {43, 15, 7},
                {43, 15, 7},
                {43, 8, 0},
                {186,8,0},
                {43,8,0},
                {96,8,0},
                {201,8,7},
                {133,8,7},
                {7,8,0},
                {133,8,7}
            },
            {
                {32,8,0},
                {32,8,0},
                {32,8,0},
                {132,7,7},
                {201,15,7},
                {203,15,7},
                {203,15,7},
                {187,15,7},
                {43,8,0},
                {186,8,0},
                {43,8,0},
                {44,8,0},
                {186,8,7},
                {133,8,7},
                {199,8,7},
                {133,8,7}
              },
              {
                {32,8,0},
                {32,8,0},
                {156,14,7},
                {133,15,7},
                {200,15,7},
                {202,15,7},
                {202,15,7},
                {188,15,7},
                {43,8,0},
                {186,8,0},
                {43,8,0},
                {96,8,0},
                {200,8,7},
                {205,8,7},
                {188,8,7},
                {133,8,7}
              },
              {
                {137,15,7},
                {134,15,6},
                {139,4,7},
                {46,4,0},
                {39,15,0},
                {44,15,0},
                {44,15,0},
                {46,8,0},
                {43,8,0},
                {186,8,0},
                {43,8,0},
                {46,8,0},
                {96,8,0},
                {7,8,0},
                {44,8,0},
                {133,7,7}
              },
              {
                {32,8,9},
                {250,2,0},
                {250,10,0},
                {31,4,0},
                {44,15,0},
                {201,15,0},
                {187,15,0},
                {96,8,0},
                {43,7,0},
                {186,7,0},
                {43,8,0},
                {39,8,0},
                {46,7,0},
                {44,8,0},
                {96,8,0},
                {133,7,7}
              },
              {
                {32,8,9},
                {254,10,0},
                {249,8,0},
                {31,10,0},
                {46,15,0},
                {200,15,0},
                {188,15,0},
                {46,15,0},
                {43,7,0},
                {199,7,0},
                {43,7,0},
                {96,7,0},
                {39,7,0},
                {7,7,0},
                {7,7,0},
                {133,7,7}
              },
              {
                {32,8,9},
                {250,8,0},
                {250,2,0},
                {31,2,0},
                {39,15,0},
                {96,15,0},
                {46,15,0},
                {96,15,0},
                {43,7,0},
                {43,7,0},
                {43,7,0},
                {39,7,0},
                {44,7,0},
                {96,7,0},
                {46,7,0},
                {96,7,0}
              },
              {
                {187,15,0},
                {43,15,0},
                {43,15,0},
                {43,15,7},
                {43,15,7},
                {43,15,7},
                {43,15,7},
                {43,15,7},
                {43,7,0},
                {43,7,0},
                {43,7,0},
                {43,7,7},
                {43,7,7},
                {43,7,7},
                {43,7,7},
                {43,7,7}
              },
              {
                {206,15,0},
                {187,15,0},
                {43,15,0},
                {43,15,7},
                {43,15,7},
                {199,15,7},
                {43,15,7},
                {43,15,7},
                {201,7,0},
                {203,7,0},
                {187,7,0},
                {43,7,7},
                {201,7,7},
                {203,7,7},
                {187,7,7},
                {43,7,7}
              },
              {
                {200,15,0},
                {206,15,0},
                {187,15,0},
                {43,15,7},
                {199,15,7},
                {206,15,7},
                {199,15,7},
                {43,15,7},
                {204,7,0},
                {206,7,0},
                {185,7,0},
                {43,7,7},
                {204,7,7},
                {206,7,7},
                {185,7,7},
                {43,7,7}
              },
              {
                {43,15,0},
                {200,15,0},
                {206,15,0},
                {187,15,0},
                {43,15,7},
                {199,15,7},
                {43,15,7},
                {43,15,7},
                {200,7,0},
                {202,7,0},
                {188,7,0},
                {43,7,7},
                {200,7,7},
                {202,7,7},
                {188,7,7},
                {43,7,7}
              },
              {
                {43,15,0},
                {43,15,0},
                {200,15,0},
                {188,15,0},
                {43,15,7},
                {43,15,7},
                {43,15,7},
                {43,15,7},
                {43,7,0},
                {43,7,0},
                {43,7,0},
                {43,7,7},
                {43,7,7},
                {43,7,7},
                {43,7,7},
                {43,7,7}
              },
              {
                {44,2,0},
                {46,2,0},
                {252,14,0},
                {39,10,0},
                {46,2,0},
                {34,10,0},
                {96,2,0},
                {147,10,6},
                {44,2,0},
                {46,2,0},
                {96,10,0},
                {6,10,0},
                {18,10,4},
                {44,10,0},
                {96,2,0},
                {247,1,0}
              },
              {
                {5,10,0},
                {18,10,2},
                {6,10,0},
                {96,2,0},
                {39,10,0},
                {96,2,0},
                {39,10,0},
                {46,2,0},
                {253,5,0},
                {44,10,0},
                {5,10,0},
                {252,14,0},
                {39,10,0},
                {96,2,0},
                {5,10,0},
                {126,1,0}
              },
              {
                {96,10,0},
                {6,10,0},
                {96,2,0},
                {147,10,6},
                {44,10,0},
                {96,10,0},
                {5,10,0},
                {96,10,0},
                {96,10,0},
                {39,2,0},
                {96,2,0},
                {39,2,0},
                {44,10,0},
                {18,10,9},
                {247,1,0},
                {126,1,0}
              },
              {
                {219,8,0},
                {96,11,0},
                {85,11,0},
                {114,11,0},
                {105,11,0},
                {115,11,0},
                {116,11,0},
                {39,11,0},
                {32,11,0},
                {100,11,0},
                {101,11,0},
                {114,11,0},
                {112,11,0},
                {115,11,0},
                {46,11,0},
                {219,8,0}
              }
            };
        public static PixelData[] colors = new PixelData[16] {
            new PixelData(0, 0, 0),
            new PixelData(46, 88, 255),
            new PixelData(79, 170, 56),
            new PixelData(56, 136, 170),
            new PixelData(170, 0, 0),
            new PixelData(170, 56, 136),
            new PixelData(170, 85, 28),
            new PixelData(187, 177, 167),
            new PixelData(135, 125, 115),
            new PixelData(96, 0, 255),
            new PixelData(118, 255, 84),
            new PixelData(84, 212, 255),
            new PixelData(255, 0, 0),
            new PixelData(255, 84, 212),
            new PixelData(255, 204, 0),
            new PixelData(255, 250, 245)
        };

        static ScreenGenerator() {
        }

        public static Bitmap generateScreen(BitmapData srcData, int backcolor) {
            int widthInTiles = ScreenGenerator.tiles.GetLength(0);
            int heightInTiles = ScreenGenerator.tiles.GetLength(1);

            Rectangle srcArea = new Rectangle(0, 0, 16, 16);
            Rectangle dstArea = new Rectangle(0, 0, 16, 16);

            Bitmap bitmap = new Bitmap(heightInTiles * 16, widthInTiles * 16, PixelData.format);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            for (int row = 0; row < widthInTiles; ++row) {
                for (int cell = 0; cell < heightInTiles; ++cell) {
                    int num = (int)ScreenGenerator.tiles[row, cell, 0];
                    int frColor = (int)ScreenGenerator.tiles[row, cell, 1];
                    int bgColor = (int)ScreenGenerator.tiles[row, cell, 2];

                    if (num <= 255 && frColor <= 15 && bgColor <= 15) {
                        srcArea.X = (num & 15) << 4;
                        srcArea.Y = num & 240;

                        dstArea.X = cell << 4;
                        dstArea.Y = row << 4;

                        if (backcolor > 0) {
                            PixelData.drawTileWithColor(srcArea, srcData, dstArea, bitmapData, ScreenGenerator.colors[frColor], ScreenGenerator.colors[backcolor]);
                        }else {
                            PixelData.drawTileWithColor(srcArea, srcData, dstArea, bitmapData, ScreenGenerator.colors[frColor], ScreenGenerator.colors[bgColor]);
                        }
                    }
                }
            }
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}
