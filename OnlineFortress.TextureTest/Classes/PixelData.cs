// Type: DwarfFortressConfig.PixelData
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System.Drawing;
using System.Drawing.Imaging;

namespace OnlineFortress.TextureTest {
    public struct PixelData {
        public static PixelFormat format = PixelFormat.Format32bppArgb;
        public byte blue;
        public byte green;
        public byte red;
        public byte alpha;

        static PixelData() {
        }

        public PixelData(byte red, byte green, byte blue) {
            this.blue = blue;
            this.green = green;
            this.red = red;
            this.alpha = byte.MaxValue;
        }

        public static unsafe void drawOverlay(Rectangle srcArea, BitmapData srcData, Rectangle dstArea, BitmapData dstData) {
            if (srcData == null)
                return;
            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;
            if (dstData == null)
                return;
            int stride2 = dstData.Stride;
            int num2 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride2;
            int width = srcArea.Width;
            int height = srcArea.Height;
            for (int index1 = 0; index1 < height; ++index1) {
                int num3 = num1 + index1 * stride1;
                int num4 = num2 + index1 * stride2;
                for (int index2 = 0; index2 < width; ++index2) {
                    *(PixelData*)num4 = PixelData.overlayPixelData(*(PixelData*)num3, *(PixelData*)num4);
                    num3 += 4;
                    num4 += 4;
                }
            }
        }

        public static unsafe void drawClear(Rectangle srcArea, BitmapData srcData, Rectangle dstArea, BitmapData dstData) {
            if (srcData == null)
                return;
            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;
            if (dstData == null)
                return;
            int stride2 = dstData.Stride;
            int num2 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride2;
            int width = srcArea.Width;
            int height = srcArea.Height;
            for (int index1 = 0; index1 < height; ++index1) {
                int num3 = num1 + index1 * stride1;
                int num4 = num2 + index1 * stride2;
                for (int index2 = 0; index2 < width; ++index2) {
                    *(PixelData*)num4 = *(PixelData*)num3;
                    num3 += 4;
                    num4 += 4;
                }
            }
        }

        public static unsafe void drawClearWithBackground(Rectangle srcArea, BitmapData srcData, Rectangle dstArea, BitmapData dstData, BitmapData bgData) {
            if (srcData == null)
                return;
            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;
            if (dstData == null)
                return;
            int stride2 = dstData.Stride;
            int num2 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride2;
            if (bgData == null)
                return;
            int stride3 = bgData.Stride;
            int num3 = (int)bgData.Scan0 + dstArea.X * 4 + dstArea.Y * stride3;
            int width = srcArea.Width;
            int height = srcArea.Height;
            for (int index1 = 0; index1 < height; ++index1) {
                int num4 = num1 + index1 * stride1;
                int num5 = num2 + index1 * stride2;
                int num6 = num3 + index1 * stride3;
                for (int index2 = 0; index2 < width; ++index2) {
                    *(PixelData*)num5 = PixelData.overlayPixelData(*(PixelData*)num4, *(PixelData*)num6);
                    num4 += 4;
                    num5 += 4;
                    num6 += 4;
                }
            }
        }

        public static unsafe void drawFilter(Rectangle srcArea, BitmapData srcData, BitmapData filterData, Rectangle dstArea, BitmapData dstData) {
            if (srcData == null)
                return;
            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;
            if (filterData == null)
                return;
            int stride2 = filterData.Stride;
            int num2 = (int)filterData.Scan0 + srcArea.X * 4 + srcArea.Y * stride2;
            if (dstData == null)
                return;
            int stride3 = dstData.Stride;
            int num3 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride3;
            int width = srcArea.Width;
            int height = srcArea.Height;
            for (int index1 = 0; index1 < height; ++index1) {
                int num4 = num1 + index1 * stride1;
                int num5 = num2 + index1 * stride2;
                int num6 = num3 + index1 * stride3;
                for (int index2 = 0; index2 < width; ++index2) {
                    PixelData over = *(PixelData*)num4;
                    switch (*(uint*)num5) {
                        case 4278190080U:
                        case uint.MaxValue:
                            *(PixelData*)num6 = over;
                            break;
                        default:
                            *(PixelData*)num6 = PixelData.overlayPixelData(over, *(PixelData*)num6);
                            break;
                    }
                    num4 += 4;
                    num5 += 4;
                    num6 += 4;
                }
            }
        }

        public static unsafe void drawFilterWithBackground(Rectangle srcArea, BitmapData srcData, BitmapData filterData, Rectangle dstArea, BitmapData dstData, BitmapData bgData) {
            if (srcData == null)
                return;
            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;
            if (filterData == null)
                return;
            int stride2 = filterData.Stride;
            int num2 = (int)filterData.Scan0 + srcArea.X * 4 + srcArea.Y * stride2;
            if (dstData == null)
                return;
            int stride3 = dstData.Stride;
            int num3 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride3;
            if (bgData == null)
                return;
            int stride4 = bgData.Stride;
            int num4 = (int)bgData.Scan0 + dstArea.X * 4 + dstArea.Y * stride4;
            int width = srcArea.Width;
            int height = srcArea.Height;
            for (int index1 = 0; index1 < height; ++index1) {
                int num5 = num1 + index1 * stride1;
                int num6 = num2 + index1 * stride2;
                int num7 = num3 + index1 * stride3;
                int num8 = num4 + index1 * stride4;
                for (int index2 = 0; index2 < width; ++index2) {
                    switch (*(uint*)num6) {
                        case 4278190080U:
                            *(PixelData*)num7 = *(PixelData*)num5;
                            break;
                        case uint.MaxValue:
                            *(PixelData*)num7 = PixelData.overlayPixelData(*(PixelData*)num5, *(PixelData*)num8);
                            break;
                        default:
                            *(PixelData*)num7 = PixelData.overlayPixelData(*(PixelData*)num5, *(PixelData*)num7);
                            break;
                    }
                    num5 += 4;
                    num6 += 4;
                    num7 += 4;
                    num8 += 4;
                }
            }
        }

        public static PixelData overlayPixelData(PixelData over, PixelData under) {
            if (over.alpha == 255 || under.alpha == 0)
                return over;
            if (over.alpha == 0)
                return under;

            int num1 = (int)over.alpha;
            int num2 = ((255 - num1) * (int)under.alpha + 127) / 255;
            int num3 = num1 + num2;
            int num4 = num3 / 2;
            PixelData pixelData;
            pixelData.blue = (byte)(((int)over.blue * num1 + (int)under.blue * num2 + num4) / num3);
            pixelData.green = (byte)(((int)over.green * num1 + (int)under.green * num2 + num4) / num3);
            pixelData.red = (byte)(((int)over.red * num1 + (int)under.red * num2 + num4) / num3);
            pixelData.alpha = (byte)num3;
            return pixelData;
        }

        public static unsafe void drawTileWithColor(Rectangle srcArea, BitmapData srcData, Rectangle dstArea, BitmapData dstData, PixelData fgColor, PixelData bgColor) {
            if (srcData == null || dstData == null)
                return;

            int stride1 = srcData.Stride;
            int num1 = (int)srcData.Scan0 + srcArea.X * 4 + srcArea.Y * stride1;

            int stride2 = dstData.Stride;
            int num2 = (int)dstData.Scan0 + dstArea.X * 4 + dstArea.Y * stride2;

            int width = srcArea.Width;
            int height = srcArea.Height;

            for (int yAxis = 0; yAxis < height; ++yAxis) {
                int num3 = num1 + yAxis * stride1;
                int num4 = num2 + yAxis * stride2;

                for (int xAxis = 0; xAxis < width; ++xAxis) {
                    PixelData over = *(PixelData*)num3;

                    over.red = (byte)(((int)over.red * (int)fgColor.red + 127) / 255);
                    over.green = (byte)(((int)over.green * (int)fgColor.green + 127) / 255);
                    over.blue = (byte)(((int)over.blue * (int)fgColor.blue + 127) / 255);

                    *(PixelData*)num4 = PixelData.overlayPixelData(over, bgColor);
                    num3 += 4;
                    num4 += 4;
                }
            }
        }
    }
}
