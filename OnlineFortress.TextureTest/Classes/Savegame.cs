// Type: DwarfFortressConfig.Savegame
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace OnlineFortress.TextureTest {
    public class Savegame {
        protected static char slash = Path.DirectorySeparatorChar;
        private FlowLayoutPanel SavegamePanel = new FlowLayoutPanel();
        private Label SaveNameLabel = new Label();
        private FlowLayoutPanel GraphicsPanel = new FlowLayoutPanel();
        private Button GraphicsButton = new Button();
        private Label GraphicsLabel = new Label();
        private FlowLayoutPanel ObjectsPanel = new FlowLayoutPanel();
        private Button ObjectsButton = new Button();
        private Label ObjectsLabel = new Label();
        private string Name;
        private string SavePath;
        private bool Main;
        private SavegameRaws Graphics;
        private SourceRaws GraphicsSource;
        private SavegameRaws Objects;
        private SourceRaws ObjectsSource;

        static Savegame() {
        }

        public Savegame(string SavegameName, string SavegamePath, bool Main) {
            this.Name = SavegameName;
            this.SavePath = SavegamePath;
            this.Main = Main;
            this.GeneratePanel();
            this.LoadCurrentGraphics();
            this.LoadCurrentObjects();
        }

        public void LoadCurrentGraphics() {
            this.Graphics = new SavegameRaws(this.SavePath + (object)Savegame.slash + "raw" + (string)(object)Savegame.slash + "graphics");
            this.GraphicsLabel.Text = "Unit Graphics: " + this.Graphics.Name + " " + this.Graphics.Version;
        }

        public void LoadCurrentObjects() {
            this.Objects = new SavegameRaws(this.SavePath + (object)Savegame.slash + "raw" + (string)(object)Savegame.slash + "objects");
            this.ObjectsLabel.Text = "Tileset Raws: " + this.Objects.Name + " " + this.Objects.Version;
        }

        public Panel GetPanel() {
            return (Panel)this.SavegamePanel;
        }

        public void SetSelected(string Category, SourceRaws selected) {
            if (Category.ToLower() == "graphics") {
                this.SetSelectedGraphics(selected);
            } else {
                if (!(Category.ToLower() == "objects"))
                    return;
                this.SetSelectedObjects(selected);
            }
        }

        public void SetSelectedGraphics(SourceRaws selected) {
            this.GraphicsSource = selected;
            if (selected != null) {
                this.GraphicsButton.Text = Savegame.GetRelativePrefix(this.GraphicsSource, this.Graphics) + " " + selected.Name + " Unit Graphics";
                this.GraphicsButton.Enabled = true;
            } else {
                this.GraphicsButton.Text = "Select some Unit Graphics";
                this.GraphicsButton.Enabled = false;
            }
        }

        public void SetSelectedObjects(SourceRaws selected) {
            this.ObjectsSource = selected;
            if (selected != null) {
                this.ObjectsButton.Text = Savegame.GetRelativePrefix(this.ObjectsSource, this.Objects) + " " + selected.Name + " Tileset Raws";
                this.ObjectsButton.Enabled = true;
            } else {
                this.ObjectsButton.Text = "Select some Tileset Raws";
                this.ObjectsButton.Enabled = false;
            }
        }

        private static string GetRelativePrefix(SourceRaws sourceRaws, SavegameRaws savegameRaws) {
            if (!(sourceRaws.Name == savegameRaws.Name))
                return "Install";
            int num = string.Compare(sourceRaws.Version, savegameRaws.Version, true);
            if (num > 0)
                return "Upgrade";
            return num < 0 ? "Retrograde" : "Re-install";
        }

        private void Graphics_Install(object sender, EventArgs e) {
            this.Graphics.Install(this.GraphicsSource, true);
            this.LoadCurrentGraphics();
            this.SetSelectedGraphics(this.GraphicsSource);
        }

        private void Objects_Install(object sender, EventArgs e) {
            this.Objects.Install(this.ObjectsSource, false);
            this.LoadCurrentObjects();
            this.SetSelectedObjects(this.ObjectsSource);
        }

        public void Install(string Category, SourceRaws raws) {
            if (Category.ToLower() == "graphics") {
                this.Graphics.Install(raws, true);
                this.LoadCurrentGraphics();
                this.SetSelectedGraphics(raws);
            } else {
                if (!(Category.ToLower() == "objects"))
                    return;
                this.Objects.Install(raws, false);
                this.LoadCurrentObjects();
                this.SetSelectedObjects(raws);
            }
        }

        private void GeneratePanel() {
            this.SavegamePanel.AutoSize = true;
            this.SavegamePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SavegamePanel.FlowDirection = FlowDirection.TopDown;
            this.SavegamePanel.Margin = new Padding(0, 3, 0, 3);
            this.SavegamePanel.Padding = new Padding(3);
            this.SaveNameLabel.AutoSize = true;
            this.SaveNameLabel.Font = new Font("Consolas", this.Main ? 15f : 13f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.SaveNameLabel.Text = this.Main ? this.Name : "World: " + this.Name;
            this.SaveNameLabel.Margin = new Padding(0);
            this.GraphicsPanel.AutoSize = true;
            this.GraphicsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GraphicsPanel.Margin = new Padding(0);
            this.GraphicsButton.AutoSize = true;
            this.GraphicsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GraphicsButton.UseVisualStyleBackColor = true;
            this.GraphicsButton.Margin = new Padding(0);
            this.GraphicsButton.Click += new EventHandler(this.Graphics_Install);
            this.SetSelectedGraphics((SourceRaws)null);
            this.GraphicsLabel.AutoSize = true;
            this.GraphicsLabel.Margin = new Padding(0);
            this.GraphicsLabel.Anchor = AnchorStyles.Left;
            this.ObjectsPanel.AutoSize = true;
            this.ObjectsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ObjectsPanel.Margin = new Padding(0);
            this.ObjectsButton.AutoSize = true;
            this.ObjectsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ObjectsButton.UseVisualStyleBackColor = true;
            this.ObjectsButton.Margin = new Padding(0);
            this.ObjectsButton.Click += new EventHandler(this.Objects_Install);
            this.SetSelectedObjects((SourceRaws)null);
            this.ObjectsLabel.AutoSize = true;
            this.ObjectsLabel.Margin = new Padding(0);
            this.ObjectsLabel.Anchor = AnchorStyles.Left;
            this.SavegamePanel.Controls.Add((Control)this.SaveNameLabel);
            this.GraphicsPanel.Controls.Add((Control)this.GraphicsButton);
            this.GraphicsPanel.Controls.Add((Control)this.GraphicsLabel);
            this.SavegamePanel.Controls.Add((Control)this.GraphicsPanel);
            this.ObjectsPanel.Controls.Add((Control)this.ObjectsButton);
            this.ObjectsPanel.Controls.Add((Control)this.ObjectsLabel);
            this.SavegamePanel.Controls.Add((Control)this.ObjectsPanel);
        }
    }
}
