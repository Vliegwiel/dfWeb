// Type: DwarfFortressConfig.TilesetAssembler
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace OnlineFortress.TextureTest {
    public class TilesetAssembler : UserControl {
        private static string OutputDirectory = "Data\\Art\\";
        private static string OutputFile = "Phoebus_16x16.png";
        private static TilesetSize finalSize = new TilesetSize(16, 16, 16, 16);
        private static bool drawScreen = true;
        private static bool reloading = false;
        private static bool update = false;
        private static Color Black = Color.FromArgb(0, 0, 0);
        private static Color DarkMagenta = Color.FromArgb(128, 0, 128);
        private static Color DarkGrey = Color.FromArgb(128, 128, 128);
        private static Color White = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
        private List<string> zipPaths = new List<string>();
        private List<string> basePaths = new List<string>();
        private List<TilesetList> BackgroundListBoxes = new List<TilesetList>();
        private List<TilesetList> TilesetListBoxes = new List<TilesetList>();
        private Bitmap tileset = TilesetAssembler.newEmptyBitmap();
        private Bitmap screen = TilesetAssembler.newEmptyBitmap();
        private IContainer components;
        private FlowLayoutPanel TilesetListPanel;
        private PictureBox TilesetView;
        private Panel ListPanel;
        private Label GroundLabel;
        private ListBox GroundListBox;
        private Label GroundModLabel;
        private Label BaseLabel;
        private ListBox BaseListBox;
        private Label FontLabel;
        private ListBox FontListBox;
        private Label WallLabel;
        private ListBox WallListBox;
        private Label StoneLabel;
        private ListBox StoneListBox;
        private Label SymbolLabel;
        private ListBox SymbolListBox;
        private Label CustomLabel;
        private Button SaveTilesetButton;
        private Label ElfSoulLabel;
        private CheckedListBox GroundModListBox;
        private CheckedListBox CustomListBox;
        private FlowLayoutPanel flowLayoutPanel1;

        public int bgcolor = 0;

        static TilesetAssembler() {
        }

        public TilesetAssembler() {
            this.InitializeComponent();
            this.zipPaths.Add(@"D:\DF-Phoebus\data\config\TilesetAssemblerData.zip");
            this.basePaths.Add(@"D:\DF-Phoebus\data\config\TilesetAssembler");

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (folderPath != "")
                this.basePaths.Add(folderPath + "\\DwarfFortress\\TilesetAssembler");

            this.BackgroundListBoxes.Add(new TilesetList(this.GroundListBox, "Ground"));
            this.BackgroundListBoxes.Add(new TilesetList(this.StoneListBox, "Rough"));
            this.BackgroundListBoxes.Add(new TilesetList(this.GroundModListBox, "GroundMod"));
            this.TilesetListBoxes.Add(new TilesetList(this.BaseListBox, "Base"));
            this.TilesetListBoxes.Add(new TilesetList(this.FontListBox, "Font"));
            this.TilesetListBoxes.Add(new TilesetList(this.WallListBox, "Wall"));
            this.TilesetListBoxes.Add(new TilesetList(this.SymbolListBox, "Symbol"));
            this.TilesetListBoxes.Add(new TilesetList(this.CustomListBox, "Custom"));
            new ToolTip().SetToolTip((Control)this.TilesetView, "Right-Click: Toggle between tileset and preview.\nLeft-Click: Change background color.");
        }

        private void TilesetAssembler_Load(object sender, EventArgs e) {
            this.LoadFiles();
            this.GenerateTileset();
            this.TilesetView.Refresh();
        }

        public void Reload() {
            this.SuspendLayout();
            this.LoadFiles();
            this.GenerateTileset();
            this.ResumeLayout();
            this.TilesetView.Refresh();
        }

        private void LoadFiles() {
            ICollection<PhoebusFileContainer> FileContainers = (ICollection<PhoebusFileContainer>)new List<PhoebusFileContainer>();
            TilesetAssembler.reloading = true;
            foreach (string ZipFileName in this.zipPaths) {
                FileContainers.Add((PhoebusFileContainer)new PhoebusZipContainer(ZipFileName));
            }
            //foreach (string PathName in this.basePaths) {
            //    FileContainers.Add((PhoebusFileContainer)new PhoebusPathContainer(PathName));
            //}
            foreach (TilesetList tilesetList in this.BackgroundListBoxes) {
                try {
                    tilesetList.LoadFileContainers(FileContainers);
                } catch {
                }
            }
            foreach (TilesetList tilesetList in this.TilesetListBoxes) {
                try {
                    tilesetList.LoadFileContainers(FileContainers);
                } catch {
                }
            }
            foreach (PhoebusFileContainer phoebusFileContainer in (IEnumerable<PhoebusFileContainer>)FileContainers) {
                try {
                    phoebusFileContainer.Dispose();
                } catch {
                }
            }
            TilesetAssembler.reloading = false;
        }

        private void TilesetListBox_Changed(object sender, EventArgs e) {
            if (TilesetAssembler.reloading || TilesetAssembler.update)
                return;
            TilesetAssembler.update = true;
            this.TilesetView.Invalidate();
        }

        private void TilesetCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e) {
            if (!TilesetAssembler.reloading) {
                TilesetAssembler.reloading = true;
                CheckedListBox checkedListBox = (CheckedListBox)sender;
                if (e.Index != -1 && e.NewValue == CheckState.Checked) {
                    string str = ((string)checkedListBox.Items[e.Index]).Split(new char[1] {' '})[0];
                    for (int index = checkedListBox.CheckedIndices.Count - 1; index >= 0; --index) {
                        if (checkedListBox.CheckedIndices[index] != checkedListBox.SelectedIndex) {
                            if (str == ((string)checkedListBox.Items[checkedListBox.CheckedIndices[index]]).Split(new char[1] {' '})[0])
                                checkedListBox.SetItemChecked(checkedListBox.CheckedIndices[index], false);
                        }
                    }
                }
                TilesetAssembler.reloading = false;
            }
            if (TilesetAssembler.reloading || TilesetAssembler.update)
                return;
            TilesetAssembler.update = true;
            this.TilesetView.Invalidate();
        }

        private void TilesetListBox_ClearSelected(object sender, MouseEventArgs e) {
            if (TilesetAssembler.reloading)
                return;
            ((ListControl)sender).SelectedIndex = -1;
        }

        private void GenerateTileset() {
            Bitmap bitmap1 = TilesetAssembler.newEmptyBitmap();
            BitmapData bitmapData1 = bitmap1.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadWrite, bitmap1.PixelFormat);
            foreach (TilesetList tilesetList in this.BackgroundListBoxes)
                tilesetList.Apply(bitmapData1, (BitmapData)null);
            bitmap1.UnlockBits(bitmapData1);
            BitmapData bitmapData2 = (BitmapData)null;
            Bitmap bitmap2 = new Bitmap((Image)bitmap1);
            BitmapData bitmapData3 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
            BitmapData bitmapData4 = bitmap1.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadOnly, bitmap1.PixelFormat);
            foreach (TilesetList tilesetList in this.TilesetListBoxes)
                tilesetList.Apply(bitmapData3, bitmapData4);
            bitmap1.UnlockBits(bitmapData4);
            bitmapData2 = (BitmapData)null;
            bitmap2.UnlockBits(bitmapData3);
            Bitmap bitmap3 = this.tileset;
            this.tileset = bitmap2;
            bitmap1.Dispose();
            bitmap3.Dispose();
            BitmapData bitmapData5 = this.tileset.LockBits(new Rectangle(0, 0, this.tileset.Width, this.tileset.Height), ImageLockMode.ReadWrite, this.tileset.PixelFormat);
            Bitmap bitmap4 = ScreenGenerator.generateScreen(bitmapData5, bgcolor);
            this.tileset.UnlockBits(bitmapData5);
            Bitmap bitmap5 = this.screen;
            this.screen = bitmap4;
            bitmap5.Dispose();
        }

        private void TilesetView_Click(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                TilesetAssembler.drawScreen = !TilesetAssembler.drawScreen;
                this.TilesetView.Invalidate();
            } else if (this.TilesetView.BackColor == TilesetAssembler.DarkMagenta)
                this.TilesetView.BackColor = TilesetAssembler.Black;
            else if (this.TilesetView.BackColor == TilesetAssembler.Black)
                this.TilesetView.BackColor = TilesetAssembler.White;
            else if (this.TilesetView.BackColor == TilesetAssembler.White)
                this.TilesetView.BackColor = TilesetAssembler.DarkGrey;
            else
                this.TilesetView.BackColor = TilesetAssembler.DarkMagenta;

            bgcolor++;
            if (bgcolor == 16) bgcolor = 0;
            this.GenerateTileset();
        }

        private void TilesetView_Resize(object sender, EventArgs e) {
            this.TilesetView.Refresh();
        }

        private void TilesetView_Paint(object sender, PaintEventArgs e) {
            if (TilesetAssembler.update) {
                TilesetAssembler.update = false;
                this.GenerateTileset();
            }
            if (TilesetAssembler.drawScreen) {
                float num1 = Math.Min((float)this.TilesetView.Width / (float)this.screen.Width, (float)this.TilesetView.Height / (float)this.screen.Height);
                int width;
                int height;
                if ((double)num1 >= 1.0) {
                    int num2 = (int)num1;
                    width = this.screen.Width * num2;
                    height = this.screen.Height * num2;
                    e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                } else {
                    width = (int)((double)num1 * (double)this.screen.Width);
                    height = (int)((double)num1 * (double)this.screen.Height);
                    e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
                }
                int x = (this.TilesetView.Width - width) / 2;
                int y = (this.TilesetView.Height - height) / 2;
                e.Graphics.DrawImage((Image)this.screen, x, y, width, height);
            } else {
                int num = Math.Min(this.TilesetView.Width, this.TilesetView.Height);
                if (num >= 256) {
                    num -= num % 256;
                    e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                } else
                    e.Graphics.InterpolationMode = InterpolationMode.Bicubic;
                int x = (this.TilesetView.Width - num) / 2;
                int y = (this.TilesetView.Height - num) / 2;
                e.Graphics.DrawImage((Image)this.tileset, x, y, num, num);
            }
        }

        private void SaveTilesetButton_Click(object sender, EventArgs e) {
            if (this.SaveToPNG(TilesetAssembler.OutputDirectory + TilesetAssembler.OutputFile)) {
                int num1 = (int)MessageBox.Show("\"" + TilesetAssembler.OutputDirectory + TilesetAssembler.OutputFile + "\" has been saved.");
            } else {
                int num2 = (int)MessageBox.Show("Phoebus' Tileset Assembler failed to create the tileset file.");
            }
        }

        private bool SaveToPNG(string file) {
            FileStream fileStream;
            try {
                fileStream = new FileStream(file, FileMode.OpenOrCreate);
            } catch {
                return false;
            }
            try {
                this.tileset.Save((Stream)fileStream, ImageFormat.Png);
            } catch {
                fileStream.Dispose();
                return false;
            }
            fileStream.Dispose();
            return true;
        }

        private static Bitmap newEmptyBitmap() {
            return new Bitmap(TilesetAssembler.finalSize.bitmap.Width, TilesetAssembler.finalSize.bitmap.Height, PixelData.format);
        }

        protected override void Dispose(bool disposing) {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.TilesetListPanel = new FlowLayoutPanel();
            this.BaseLabel = new Label();
            this.BaseListBox = new ListBox();
            this.FontLabel = new Label();
            this.FontListBox = new ListBox();
            this.WallLabel = new Label();
            this.WallListBox = new ListBox();
            this.SymbolLabel = new Label();
            this.SymbolListBox = new ListBox();
            this.StoneLabel = new Label();
            this.StoneListBox = new ListBox();
            this.GroundLabel = new Label();
            this.GroundListBox = new ListBox();
            this.GroundModLabel = new Label();
            this.GroundModListBox = new CheckedListBox();
            this.CustomLabel = new Label();
            this.CustomListBox = new CheckedListBox();
            this.SaveTilesetButton = new Button();
            this.ElfSoulLabel = new Label();
            this.TilesetView = new PictureBox();
            this.ListPanel = new Panel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.TilesetListPanel.SuspendLayout();
            //this.TilesetView.BeginInit();
            this.ListPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            this.TilesetListPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.TilesetListPanel.AutoSize = true;
            this.TilesetListPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.TilesetListPanel.Controls.Add((Control)this.BaseLabel);
            this.TilesetListPanel.Controls.Add((Control)this.BaseListBox);
            this.TilesetListPanel.Controls.Add((Control)this.FontLabel);
            this.TilesetListPanel.Controls.Add((Control)this.FontListBox);
            this.TilesetListPanel.Controls.Add((Control)this.WallLabel);
            this.TilesetListPanel.Controls.Add((Control)this.WallListBox);
            this.TilesetListPanel.Controls.Add((Control)this.SymbolLabel);
            this.TilesetListPanel.Controls.Add((Control)this.SymbolListBox);
            this.TilesetListPanel.Controls.Add((Control)this.StoneLabel);
            this.TilesetListPanel.Controls.Add((Control)this.StoneListBox);
            this.TilesetListPanel.Controls.Add((Control)this.GroundLabel);
            this.TilesetListPanel.Controls.Add((Control)this.GroundListBox);
            this.TilesetListPanel.FlowDirection = FlowDirection.TopDown;
            this.TilesetListPanel.Location = new Point(20, 3);
            this.TilesetListPanel.Margin = new Padding(20, 3, 10, 3);
            this.TilesetListPanel.Name = "TilesetListPanel";
            this.TilesetListPanel.RightToLeft = RightToLeft.No;
            this.TilesetListPanel.Size = new Size(140, 314);
            this.TilesetListPanel.TabIndex = 1;
            this.BaseLabel.Anchor = AnchorStyles.Top;
            this.BaseLabel.AutoSize = true;
            this.BaseLabel.Location = new Point(51, 10);
            this.BaseLabel.Margin = new Padding(3, 10, 3, 0);
            this.BaseLabel.Name = "BaseLabel";
            this.BaseLabel.Size = new Size(38, 13);
            this.BaseLabel.TabIndex = 28;
            this.BaseLabel.Text = "Tileset";
            this.BaseListBox.FormattingEnabled = true;
            this.BaseListBox.Location = new Point(0, 26);
            this.BaseListBox.Margin = new Padding(0, 3, 0, 3);
            this.BaseListBox.Name = "BaseListBox";
            this.BaseListBox.Size = new Size(140, 30);
            this.BaseListBox.TabIndex = 29;
            this.BaseListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.FontLabel.Anchor = AnchorStyles.Top;
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new Point(56, 59);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new Size(28, 13);
            this.FontLabel.TabIndex = 24;
            this.FontLabel.Text = "Font";
            this.FontListBox.FormattingEnabled = true;
            this.FontListBox.Location = new Point(0, 75);
            this.FontListBox.Margin = new Padding(0, 3, 0, 3);
            this.FontListBox.Name = "FontListBox";
            this.FontListBox.Size = new Size(140, 30);
            this.FontListBox.TabIndex = 25;
            this.FontListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.WallLabel.Anchor = AnchorStyles.Top;
            this.WallLabel.AutoSize = true;
            this.WallLabel.Location = new Point(53, 108);
            this.WallLabel.Name = "WallLabel";
            this.WallLabel.Size = new Size(33, 13);
            this.WallLabel.TabIndex = 18;
            this.WallLabel.Text = "Walls";
            this.WallListBox.FormattingEnabled = true;
            this.WallListBox.Location = new Point(0, 124);
            this.WallListBox.Margin = new Padding(0, 3, 0, 3);
            this.WallListBox.Name = "WallListBox";
            this.WallListBox.Size = new Size(140, 30);
            this.WallListBox.TabIndex = 19;
            this.WallListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.SymbolLabel.Anchor = AnchorStyles.Top;
            this.SymbolLabel.AutoSize = true;
            this.SymbolLabel.Location = new Point(31, 162);
            this.SymbolLabel.Margin = new Padding(3, 5, 3, 0);
            this.SymbolLabel.Name = "SymbolLabel";
            this.SymbolLabel.Size = new Size(77, 13);
            this.SymbolLabel.TabIndex = 22;
            this.SymbolLabel.Text = "Stone Symbols";
            this.SymbolListBox.FormattingEnabled = true;
            this.SymbolListBox.Location = new Point(0, 178);
            this.SymbolListBox.Margin = new Padding(0, 3, 0, 3);
            this.SymbolListBox.Name = "SymbolListBox";
            this.SymbolListBox.Size = new Size(140, 30);
            this.SymbolListBox.TabIndex = 23;
            this.SymbolListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.StoneLabel.Anchor = AnchorStyles.Top;
            this.StoneLabel.AutoSize = true;
            this.StoneLabel.Location = new Point(38, 211);
            this.StoneLabel.Name = "StoneLabel";
            this.StoneLabel.Size = new Size(64, 13);
            this.StoneLabel.TabIndex = 20;
            this.StoneLabel.Text = "Stone Walls";
            this.StoneListBox.FormattingEnabled = true;
            this.StoneListBox.Location = new Point(0, 227);
            this.StoneListBox.Margin = new Padding(0, 3, 0, 3);
            this.StoneListBox.Name = "StoneListBox";
            this.StoneListBox.Size = new Size(140, 30);
            this.StoneListBox.TabIndex = 21;
            this.StoneListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.GroundLabel.Anchor = AnchorStyles.Top;
            this.GroundLabel.AutoSize = true;
            this.GroundLabel.Location = new Point(49, 265);
            this.GroundLabel.Margin = new Padding(3, 5, 3, 0);
            this.GroundLabel.Name = "GroundLabel";
            this.GroundLabel.Size = new Size(42, 13);
            this.GroundLabel.TabIndex = 26;
            this.GroundLabel.Text = "Ground";
            this.GroundListBox.FormattingEnabled = true;
            this.GroundListBox.Location = new Point(0, 281);
            this.GroundListBox.Margin = new Padding(0, 3, 0, 3);
            this.GroundListBox.Name = "GroundListBox";
            this.GroundListBox.Size = new Size(140, 30);
            this.GroundListBox.TabIndex = 27;
            this.GroundListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.GroundModLabel.Anchor = AnchorStyles.Top;
            this.GroundModLabel.AutoSize = true;
            this.GroundModLabel.Location = new Point(34, 68);
            this.GroundModLabel.Margin = new Padding(3, 5, 3, 0);
            this.GroundModLabel.Name = "GroundModLabel";
            this.GroundModLabel.Size = new Size(71, 13);
            this.GroundModLabel.TabIndex = 16;
            this.GroundModLabel.Text = "Ground Mods";
            this.GroundModListBox.CheckOnClick = true;
            this.GroundModListBox.FormattingEnabled = true;
            this.GroundModListBox.Location = new Point(0, 84);
            this.GroundModListBox.Margin = new Padding(0, 3, 0, 3);
            this.GroundModListBox.Name = "GroundModListBox";
            this.GroundModListBox.Size = new Size(140, 34);
            this.GroundModListBox.TabIndex = 34;
            this.GroundModListBox.ThreeDCheckBoxes = true;
            this.GroundModListBox.ItemCheck += new ItemCheckEventHandler(this.TilesetCheckedListBox_ItemCheck);
            this.GroundModListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.GroundModListBox.MouseUp += new MouseEventHandler(this.TilesetListBox_ClearSelected);
            this.CustomLabel.Anchor = AnchorStyles.Top;
            this.CustomLabel.AutoSize = true;
            this.CustomLabel.Location = new Point(34, 10);
            this.CustomLabel.Margin = new Padding(3, 10, 3, 0);
            this.CustomLabel.Name = "CustomLabel";
            this.CustomLabel.Size = new Size(72, 13);
            this.CustomLabel.TabIndex = 32;
            this.CustomLabel.Text = "Customization";
            this.CustomListBox.CheckOnClick = true;
            this.CustomListBox.FormattingEnabled = true;
            this.CustomListBox.Location = new Point(0, 26);
            this.CustomListBox.Margin = new Padding(0, 3, 0, 3);
            this.CustomListBox.Name = "CustomListBox";
            this.CustomListBox.Size = new Size(140, 34);
            this.CustomListBox.TabIndex = 35;
            this.CustomListBox.ItemCheck += new ItemCheckEventHandler(this.TilesetCheckedListBox_ItemCheck);
            this.CustomListBox.SelectedIndexChanged += new EventHandler(this.TilesetListBox_Changed);
            this.CustomListBox.MouseUp += new MouseEventHandler(this.TilesetListBox_ClearSelected);
            this.SaveTilesetButton.Anchor = AnchorStyles.Top;
            this.SaveTilesetButton.Location = new Point(3, 131);
            this.SaveTilesetButton.Margin = new Padding(3, 10, 3, 3);
            this.SaveTilesetButton.Name = "SaveTilesetButton";
            this.SaveTilesetButton.Size = new Size(133, 23);
            this.SaveTilesetButton.TabIndex = 30;
            this.SaveTilesetButton.Text = "Save Tileset";
            this.SaveTilesetButton.UseVisualStyleBackColor = true;
            this.SaveTilesetButton.Click += new EventHandler(this.SaveTilesetButton_Click);
            this.ElfSoulLabel.AutoSize = true;
            this.ElfSoulLabel.ForeColor = SystemColors.ControlDark;
            this.ElfSoulLabel.Location = new Point(3, 157);
            this.ElfSoulLabel.Name = "ElfSoulLabel";
            this.ElfSoulLabel.Size = new Size(115, 13);
            this.ElfSoulLabel.TabIndex = 31;
            this.ElfSoulLabel.Text = "Powered with Elf Souls";
            this.TilesetView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.TilesetView.BackColor = SystemColors.ControlDark;
            this.TilesetView.Location = new Point(330, 10);
            this.TilesetView.Margin = new Padding(0);
            this.TilesetView.Name = "TilesetView";
            this.TilesetView.Size = new Size(478, 500);
            this.TilesetView.TabIndex = 1;
            this.TilesetView.TabStop = false;
            this.TilesetView.Paint += new PaintEventHandler(this.TilesetView_Paint);
            this.TilesetView.MouseClick += new MouseEventHandler(this.TilesetView_Click);
            this.TilesetView.MouseDoubleClick += new MouseEventHandler(this.TilesetView_Click);
            this.TilesetView.Resize += new EventHandler(this.TilesetView_Resize);
            this.ListPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.ListPanel.AutoScroll = true;
            this.ListPanel.Controls.Add((Control)this.TilesetListPanel);
            this.ListPanel.Controls.Add((Control)this.flowLayoutPanel1);
            this.ListPanel.Location = new Point(0, 0);
            this.ListPanel.Margin = new Padding(0);
            this.ListPanel.Name = "ListPanel";
            this.ListPanel.RightToLeft = RightToLeft.Yes;
            this.ListPanel.Size = new Size(330, 520);
            this.ListPanel.TabIndex = 0;
            this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add((Control)this.CustomLabel);
            this.flowLayoutPanel1.Controls.Add((Control)this.CustomListBox);
            this.flowLayoutPanel1.Controls.Add((Control)this.GroundModLabel);
            this.flowLayoutPanel1.Controls.Add((Control)this.GroundModListBox);
            this.flowLayoutPanel1.Controls.Add((Control)this.SaveTilesetButton);
            this.flowLayoutPanel1.Controls.Add((Control)this.ElfSoulLabel);
            this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new Point(170, 3);
            this.flowLayoutPanel1.Margin = new Padding(20, 3, 20, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = RightToLeft.No;
            this.flowLayoutPanel1.Size = new Size(140, 170);
            this.flowLayoutPanel1.TabIndex = 36;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScrollMargin = new Size(20, 20);
            this.Controls.Add((Control)this.TilesetView);
            this.Controls.Add((Control)this.ListPanel);
            this.Name = "TilesetAssembler";
            this.Size = new Size(818, 520);
            this.Load += new EventHandler(this.TilesetAssembler_Load);
            this.TilesetListPanel.ResumeLayout(false);
            this.TilesetListPanel.PerformLayout();
            //this.TilesetView.EndInit();
            this.ListPanel.ResumeLayout(false);
            this.ListPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
