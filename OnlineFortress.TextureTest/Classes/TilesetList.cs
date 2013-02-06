using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OnlineFortress.TextureTest {
    internal class TilesetList {
        private List<string> nameList = new List<string>();
        private List<TilesetPatch> fileList = new List<TilesetPatch>();
        private CheckedListBox checkedListBox;
        private ListBox tilesetListBox;
        private string prefix;
        private ConfigInfos defaultConfigInfos;

        public TilesetList(CheckedListBox checkedListBox, string prefix) {
            this.checkedListBox = checkedListBox;
            this.InitializePrefix(prefix);
        }

        public TilesetList(ListBox tilesetListBox, string prefix) {
            this.tilesetListBox = tilesetListBox;
            this.InitializePrefix(prefix);
        }

        private void InitializePrefix(string prefix) {
            this.prefix = prefix.ToLower();
            if (prefix == "wall")
                this.defaultConfigInfos = ConfigInfos.getWallInfos(new TilesetSize(16, 16, 16, 16));
            else if (prefix == "rough" || prefix == "symbol")
                this.defaultConfigInfos = ConfigInfos.getRoughWallInfos(new TilesetSize(16, 16, 16, 16));
            else
                this.defaultConfigInfos = ConfigInfos.getDefaultInfos(new TilesetSize(16, 16, 16, 16));
        }

        public void LoadFileContainers(ICollection<PhoebusFileContainer> FileContainers) {
            ListBox listBox = this.tilesetListBox;
            if (this.checkedListBox != null)
                listBox = (ListBox)this.checkedListBox;
            if (listBox == null)
                return;
            this.ClearLists();
            foreach (PhoebusFileContainer Container in (IEnumerable<PhoebusFileContainer>)FileContainers) {
                try {
                    this.LoadFileContainer(Container);
                } catch {
                }
            }
            listBox.SuspendLayout();
            listBox.ClearSelected();
            listBox.Height = this.nameList.Count * listBox.ItemHeight + 4;
            listBox.DataSource = (object)this.nameList;
            if (this.tilesetListBox != null) {
                if (this.nameList.Count >= 1)
                    this.tilesetListBox.SelectedIndex = 0;
                else
                    this.tilesetListBox.SelectedIndex = -1;
            }
            if (this.checkedListBox != null)
                this.checkedListBox.SelectedIndex = -1;
            listBox.ResumeLayout();
        }

        private void ClearLists() {
            if (this.fileList != null) {
                foreach (TilesetPatch tilesetPatch in this.fileList) {
                    try {
                        tilesetPatch.Dispose();
                    } catch {
                    }
                }
            }
            this.nameList = new List<string>();
            this.fileList = new List<TilesetPatch>();
            if (this.tilesetListBox != null)
                this.tilesetListBox.ClearSelected();
            if (this.checkedListBox == null)
                return;
            this.checkedListBox.ClearSelected();
            foreach (int index in this.checkedListBox.CheckedIndices)
                this.checkedListBox.SetItemCheckState(index, CheckState.Unchecked);
        }

        private void LoadFileContainer(PhoebusFileContainer Container) {
            int length = this.prefix.Length;
            char oldChar = Path.DirectorySeparatorChar;
            int startIndex = this.prefix.Length + 1;
            foreach (string FileName in (IEnumerable<string>)Enumerable.OrderBy<string, string>((IEnumerable<string>)Container.GetFiles(), (Func<string, string>)(file => file))) {
                string str1 = FileName.ToLower();
                if (str1.StartsWith(this.prefix) && str1.EndsWith(".png") && ((int)FileName[length] == 95 || (int)FileName[length] == (int)oldChar)) {
                    string str2 = FileName.Substring(0, FileName.Length - 4);
                    if (str2.IndexOf('.') == -1 && Container.GetFileSize(FileName) > 0L) {
                        Bitmap bitmap = (Bitmap)null;
                        Bitmap filter = (Bitmap)null;
                        try {
                            bitmap = TilesetList.StreamToBitmap(Container, FileName);
                            if (bitmap != null) {
                                filter = TilesetList.StreamToBitmap(Container, str2 + ".filter.png");
                                this.fileList.Add(new TilesetPatch(TilesetList.StreamToString(Container, str2 + ".txt"), bitmap, filter, new TilesetSize(16, 16, 16, 16), this.defaultConfigInfos));
                                this.nameList.Add(str2.Substring(startIndex, str2.Length - startIndex).Replace('_', ' ').Replace(oldChar, ' ').Trim());
                            } else
                                continue;
                        } catch {
                        }
                        if (bitmap != null)
                            bitmap.Dispose();
                        if (filter != null)
                            filter.Dispose();
                    }
                }
            }
        }

        public TilesetList Apply(BitmapData tileset, BitmapData background) {
            if (this.tilesetListBox != null) {
                if (this.tilesetListBox.SelectionMode == SelectionMode.One) {
                    if (this.tilesetListBox.SelectedIndex >= 0 && this.tilesetListBox.SelectedIndex < this.fileList.Count)
                        this.fileList[this.tilesetListBox.SelectedIndex].Apply(tileset, background);
                } else {
                    for (int index = 0; index < this.tilesetListBox.SelectedIndices.Count; ++index)
                        this.fileList[this.tilesetListBox.SelectedIndices[index]].Apply(tileset, background);
                }
            } else {
                for (int index = 0; index < this.checkedListBox.CheckedIndices.Count; ++index)
                    this.fileList[this.checkedListBox.CheckedIndices[index]].Apply(tileset, background);
            }
            return this;
        }

        private static Bitmap StreamToBitmap(PhoebusFileContainer Container, string FileName) {
            Stream stream = (Stream)null;
            Bitmap bitmap = (Bitmap)null;
            BitmapData bitmapdata = (BitmapData)null;
            try {
                stream = Container.GetFileStream(FileName);
                if (stream == null)
                    return (Bitmap)null;
                bitmap = new Bitmap(stream);
                bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelData.format);
                bitmap.UnlockBits(bitmapdata);
                stream.Dispose();
            } catch {
                if (bitmapdata != null && bitmap != null)
                    bitmap.UnlockBits(bitmapdata);
                if (bitmap != null) {
                    bitmap.Dispose();
                    bitmap = (Bitmap)null;
                }
                if (stream != null)
                    stream.Dispose();
            }
            return bitmap;
        }

        private static string StreamToString(PhoebusFileContainer Container, string FileName) {
            Stream stream = (Stream)null;
            StreamReader streamReader = (StreamReader)null;
            string str = (string)null;
            try {
                stream = Container.GetFileStream(FileName);
                if (stream == null)
                    return (string)null;
                streamReader = new StreamReader(stream);
                str = streamReader.ReadToEnd();
                streamReader.Dispose();
                stream.Dispose();
            } catch {
                if (streamReader != null)
                    streamReader.Dispose();
                if (stream != null)
                    stream.Dispose();
            }
            return str;
        }
    }
}
