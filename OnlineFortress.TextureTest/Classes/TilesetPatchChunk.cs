using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace OnlineFortress.TextureTest {
    public class TilesetPatchChunk : IDisposable {
        private Bitmap bitmap;
        private BitmapData bitmapData;
        private Bitmap filter;
        private BitmapData filterData;
        private string command;
        private Rectangle srcArea;
        private Rectangle dstArea;

        public TilesetPatchChunk(ConfigPatchInfo configPatchInfo, Bitmap fileBitmap, Bitmap fileFilter) {
            if (fileBitmap == null)
                throw new Exception();
            if (!configPatchInfo.FitsInside(fileBitmap, false) || !configPatchInfo.FitsInside(fileFilter, true))
                throw new Exception();
            this.command = configPatchInfo.command.ToLower();
            this.srcArea = new Rectangle(0, 0, configPatchInfo.srcArea.Width, configPatchInfo.srcArea.Height);
            this.dstArea = configPatchInfo.dstArea;
            try {
                this.bitmap = fileBitmap.Clone(configPatchInfo.srcArea, PixelData.format);
                this.bitmapData = this.lockBitmap(this.bitmap);
                if (fileFilter == null) {
                    this.filter = (Bitmap)null;
                    this.filterData = (BitmapData)null;
                } else {
                    this.filter = fileFilter.Clone(configPatchInfo.srcArea, PixelData.format);
                    this.filterData = this.lockBitmap(this.filter);
                }
            } catch {
                this.Dispose();
                throw;
            }
        }

        public void Apply(BitmapData tileset, BitmapData background) {
            if (this.command == "ground" || this.command == "replace") {
                if (background != null)
                    this.drawClearWithBackground(tileset, background);
                else
                    this.drawClear(tileset);
            } else if (this.command == "clear")
                this.drawClear(tileset);
            else if (this.command == "overlay")
                this.drawOverlay(tileset);
            else if (this.filter != null) {
                if (background != null)
                    this.drawFilterWithBackground(tileset, background);
                else
                    this.drawFilter(tileset);
            } else
                this.drawOverlay(tileset);
        }

        protected void drawOverlay(BitmapData dstData) {
            PixelData.drawOverlay(this.srcArea, this.bitmapData, this.dstArea, dstData);
        }

        protected void drawClear(BitmapData dstData) {
            PixelData.drawClear(this.srcArea, this.bitmapData, this.dstArea, dstData);
        }

        protected void drawClearWithBackground(BitmapData dstData, BitmapData bgData) {
            PixelData.drawClearWithBackground(this.srcArea, this.bitmapData, this.dstArea, dstData, bgData);
        }

        protected void drawFilter(BitmapData dstData) {
            PixelData.drawFilter(this.srcArea, this.bitmapData, this.filterData, this.dstArea, dstData);
        }

        protected void drawFilterWithBackground(BitmapData dstData, BitmapData bgData) {
            PixelData.drawFilterWithBackground(this.srcArea, this.bitmapData, this.filterData, this.dstArea, dstData, bgData);
        }

        protected BitmapData lockBitmap(Bitmap bitmap) {
            if (bitmap == null)
                return (BitmapData)null;
            try {
                return bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelData.format);
            } catch {
            }
            return (BitmapData)null;
        }

        public void Dispose() {
            if (this.bitmap != null) {
                if (this.bitmapData != null) {
                    try {
                        this.bitmap.UnlockBits(this.bitmapData);
                    } catch {
                    }
                }
                this.bitmapData = (BitmapData)null;
                this.bitmap.Dispose();
                this.bitmap = (Bitmap)null;
            }
            if (this.filter == null)
                return;
            if (this.filterData != null) {
                try {
                    this.filter.UnlockBits(this.filterData);
                } catch {
                }
            }
            this.filterData = (BitmapData)null;
            this.filter.Dispose();
            this.filter = (Bitmap)null;
        }
    }
}
