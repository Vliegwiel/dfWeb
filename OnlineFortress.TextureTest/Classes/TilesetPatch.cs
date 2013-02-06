using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace OnlineFortress.TextureTest {
    public class TilesetPatch : IDisposable {
        private List<TilesetPatchChunk> chunklist;

        public TilesetPatch(string config, Bitmap bitmap, Bitmap filter, TilesetSize finalSize, ConfigInfos defaultConfigInfos) {
            ConfigInfos configInfos = new ConfigInfos(config, finalSize, defaultConfigInfos);
            this.chunklist = new List<TilesetPatchChunk>();
            foreach (ConfigPatchInfo configPatchInfo in (IEnumerable<ConfigPatchInfo>)configInfos.infos) {
                try {
                    this.chunklist.Add(new TilesetPatchChunk(configPatchInfo, bitmap, filter));
                } catch {
                }
            }
        }

        public void Apply(BitmapData tileset, BitmapData background) {
            if (this.chunklist == null)
                return;
            foreach (TilesetPatchChunk tilesetPatchChunk in this.chunklist)
                tilesetPatchChunk.Apply(tileset, background);
        }

        public void Dispose() {
            if (this.chunklist == null)
                return;
            foreach (TilesetPatchChunk tilesetPatchChunk in this.chunklist)
                tilesetPatchChunk.Dispose();
            this.chunklist = (List<TilesetPatchChunk>)null;
        }
    }
}
