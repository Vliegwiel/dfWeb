using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OnlineFortress.TextureTest {
    public struct TilesetSize {
        private static Regex regexDimEntry = new Regex("\\[Dim(:([^:\\]\\[]*))+\\]", RegexOptions.IgnoreCase);
        public Size tileset;
        public Size tile;
        public Size bitmap;

        static TilesetSize() {
        }

        public TilesetSize(TilesetSize baseTilesetSize) {
            this.tileset = new Size(baseTilesetSize.tileset.Width, baseTilesetSize.tileset.Height);
            this.tile = new Size(baseTilesetSize.tile.Width, baseTilesetSize.tile.Height);
            this.bitmap = new Size(this.tile.Width * this.tileset.Width, this.tile.Height * this.tileset.Height);
        }

        public TilesetSize(int tilesetWidth, int tilesetHeight, TilesetSize baseTilesetSize) {
            this.tileset = new Size(tilesetWidth, tilesetHeight);
            this.tile = new Size(baseTilesetSize.tile.Width, baseTilesetSize.tile.Height);
            this.bitmap = new Size(this.tile.Width * this.tileset.Width, this.tile.Height * this.tileset.Height);
        }

        public TilesetSize(int tilesetWidth, int tilesetHeight, int tileWidth, int tileHeight) {
            this.tileset = new Size(tilesetWidth, tilesetHeight);
            this.tile = new Size(tileWidth, tileHeight);
            this.bitmap = new Size(this.tile.Width * this.tileset.Width, this.tile.Height * this.tileset.Height);
        }

        public TilesetSize(string text, TilesetSize baseTilesetSize) {
            if (text != null) {
                Match match = TilesetSize.regexDimEntry.Match(text);
                if (match.Success && match.Groups.Count >= 3) {
                    CaptureCollection captures = match.Groups[2].Captures;
                    if (captures.Count == 4) {
                        try {
                            this.tileset = new Size(Convert.ToInt32(captures[0].Value), Convert.ToInt32(captures[1].Value));
                            this.tile = new Size(Convert.ToInt32(captures[2].Value), Convert.ToInt32(captures[3].Value));
                            this.bitmap = new Size(this.tile.Width * this.tileset.Width, this.tile.Height * this.tileset.Height);
                            return;
                        } catch {
                        }
                    }
                }
            }
            this.tileset = new Size(baseTilesetSize.tile.Width, baseTilesetSize.tile.Height);
            this.tile = new Size(baseTilesetSize.tile.Width, baseTilesetSize.tile.Height);
            this.bitmap = new Size(this.tile.Width * this.tileset.Width, this.tile.Height * this.tileset.Height);
        }

        public bool FitsInside(Bitmap image) {
            if (image.Width >= this.bitmap.Width)
                return image.Height >= this.bitmap.Height;
            else
                return false;
        }

        public bool CompatibleWith(TilesetSize other) {
            if (this.tile.Width == other.tile.Width)
                return this.tile.Height == other.tile.Height;
            else
                return false;
        }

        public bool Contains(Rectangle area) {
            if (area.X >= 0 && area.X + area.Width <= this.bitmap.Width && area.Y >= 0)
                return area.Y + area.Height <= this.bitmap.Height;
            else
                return false;
        }
    }
}
