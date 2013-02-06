// Type: DwarfFortressConfig.ConfigPatchInfo
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Drawing;

namespace OnlineFortress.TextureTest {
    public struct ConfigPatchInfo {
        public string command;
        public Rectangle srcArea;
        public Rectangle dstArea;

        public ConfigPatchInfo(string command, int srcX, int srcY, int Width, int Height, int dstX, int dstY, TilesetSize srcSize, TilesetSize dstSize) {
            if (srcX < 0 || srcX + Width > srcSize.tileset.Width || (srcY < 0 || srcY + Height > srcSize.tileset.Height) || (dstX < 0 || dstX + Width > dstSize.tileset.Width || (dstY < 0 || dstY + Height > dstSize.tileset.Height)))
                throw new Exception();
            this.command = command.ToLower();
            this.srcArea = new Rectangle(srcX * srcSize.tile.Width, srcY * srcSize.tile.Height, Width * srcSize.tile.Width, Height * srcSize.tile.Height);
            this.dstArea = new Rectangle(dstX * dstSize.tile.Width, dstY * dstSize.tile.Height, Width * dstSize.tile.Width, Height * dstSize.tile.Height);
        }

        public bool FitsInside(Bitmap bitmap, bool NullBitmapReturnValue = false) {
            if (bitmap == null)
                return NullBitmapReturnValue;
            if (bitmap.Width >= this.srcArea.X + this.srcArea.Width)
                return bitmap.Height >= this.srcArea.Y + this.srcArea.Height;
            else
                return false;
        }
    }
}
