// Type: DwarfFortressConfig.SavegameManager
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OnlineFortress.TextureTest {
    public class SavegameManager : UserControl {
        private List<Savegame> savegames = new List<Savegame>();
        private RawsList graphics;
        private RawsList objects;
        private IContainer components;
        private FlowLayoutPanel SelectionFlowPanel;
        private Label RawLabel;
        private ListBox ObjectsListBox;
        private Label GraphicsLabel;
        private ListBox GraphicsListBox;
        private Panel SelectionScrollPanel;
        private Panel SaveListScrollPanel;
        private FlowLayoutPanel SaveListFlowPanel;
        private FlowLayoutPanel SavegameBoundPanel;
        private FlowLayoutPanel SavegameInfoPanel;
        private FlowLayoutPanel SavegameCommandPanel;
        private Button button1;
        private Button button2;
        private Label SavegameNameLabel;
        private Label label1;
        private Button GraphicsAllButton;
        private Button ObjectsAllButton;
        private PictureBox GraphicsPictureBox;

        public SavegameManager() {
            this.InitializeComponent();
        }

        private void SavegameManager_Load(object sender, EventArgs e) {
            this.Load_Savegames();
            this.graphics = new RawsList("Graphics", "data\\config\\graphics", this.GraphicsListBox, this.GraphicsPictureBox, this.savegames);
            this.objects = new RawsList("Objects", "data\\config\\objects", this.ObjectsListBox, (PictureBox)null, this.savegames);
        }

        private void Load_Savegames() {
            try {
                Savegame savegame = new Savegame("New Worlds", ".", true);
                this.SaveListFlowPanel.Controls.Add((Control)savegame.GetPanel());
                this.savegames.Add(savegame);
            } catch {
            }
            try {
                foreach (DirectoryInfo directoryInfo in new DirectoryInfo("data\\save").GetDirectories()) {
                    if (!(directoryInfo.Name.ToLower() == "current")) {
                        try {
                            Savegame savegame = new Savegame(directoryInfo.Name, directoryInfo.FullName, false);
                            this.SaveListFlowPanel.Controls.Add((Control)savegame.GetPanel());
                            this.savegames.Add(savegame);
                        } catch {
                        }
                    }
                }
            } catch {
            }
        }

        private void GraphicsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.graphics == null)
                return;
            this.graphics.UpdateSelected();
        }

        private void ObjectsListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.objects == null)
                return;
            this.objects.UpdateSelected();
        }

        private void GraphicsAllButton_Click(object sender, EventArgs e) {
            this.graphics.InstallAll();
        }

        private void ObjectsAllButton_Click(object sender, EventArgs e) {
            this.objects.InstallAll();
        }

        protected override void Dispose(bool disposing) {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.SelectionFlowPanel = new FlowLayoutPanel();
            this.GraphicsLabel = new Label();
            this.GraphicsListBox = new ListBox();
            this.GraphicsAllButton = new Button();
            this.GraphicsPictureBox = new PictureBox();
            this.RawLabel = new Label();
            this.ObjectsListBox = new ListBox();
            this.ObjectsAllButton = new Button();
            this.SelectionScrollPanel = new Panel();
            this.SaveListScrollPanel = new Panel();
            this.SaveListFlowPanel = new FlowLayoutPanel();
            this.SavegameBoundPanel = new FlowLayoutPanel();
            this.SavegameInfoPanel = new FlowLayoutPanel();
            this.SavegameNameLabel = new Label();
            this.SavegameCommandPanel = new FlowLayoutPanel();
            this.button1 = new Button();
            this.label1 = new Label();
            this.button2 = new Button();
            this.SelectionFlowPanel.SuspendLayout();
            //this.GraphicsPictureBox. BeginInit();
            this.SelectionScrollPanel.SuspendLayout();
            this.SaveListScrollPanel.SuspendLayout();
            this.SaveListFlowPanel.SuspendLayout();
            this.SavegameBoundPanel.SuspendLayout();
            this.SavegameInfoPanel.SuspendLayout();
            this.SavegameCommandPanel.SuspendLayout();
            this.SuspendLayout();
            this.SelectionFlowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.SelectionFlowPanel.AutoSize = true;
            this.SelectionFlowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SelectionFlowPanel.Controls.Add((Control)this.GraphicsLabel);
            this.SelectionFlowPanel.Controls.Add((Control)this.GraphicsListBox);
            this.SelectionFlowPanel.Controls.Add((Control)this.GraphicsAllButton);
            this.SelectionFlowPanel.Controls.Add((Control)this.GraphicsPictureBox);
            this.SelectionFlowPanel.Controls.Add((Control)this.RawLabel);
            this.SelectionFlowPanel.Controls.Add((Control)this.ObjectsListBox);
            this.SelectionFlowPanel.Controls.Add((Control)this.ObjectsAllButton);
            this.SelectionFlowPanel.FlowDirection = FlowDirection.TopDown;
            this.SelectionFlowPanel.Location = new Point(22, 3);
            this.SelectionFlowPanel.Name = "SelectionFlowPanel";
            this.SelectionFlowPanel.RightToLeft = RightToLeft.No;
            this.SelectionFlowPanel.Size = new Size(146, 217);
            this.SelectionFlowPanel.TabIndex = 0;
            this.GraphicsLabel.Anchor = AnchorStyles.Top;
            this.GraphicsLabel.AutoSize = true;
            this.GraphicsLabel.Location = new Point(37, 20);
            this.GraphicsLabel.Margin = new Padding(3, 20, 3, 0);
            this.GraphicsLabel.Name = "GraphicsLabel";
            this.GraphicsLabel.Size = new Size(71, 13);
            this.GraphicsLabel.TabIndex = 3;
            this.GraphicsLabel.Text = "Unit Graphics";
            this.GraphicsListBox.FormattingEnabled = true;
            this.GraphicsListBox.Location = new Point(3, 36);
            this.GraphicsListBox.Margin = new Padding(3, 3, 3, 0);
            this.GraphicsListBox.Name = "GraphicsListBox";
            this.GraphicsListBox.Size = new Size(140, 43);
            this.GraphicsListBox.TabIndex = 1;
            this.GraphicsListBox.SelectedIndexChanged += new EventHandler(this.GraphicsListBox_SelectedIndexChanged);
            this.GraphicsAllButton.Anchor = AnchorStyles.Top;
            this.GraphicsAllButton.AutoSize = true;
            this.GraphicsAllButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GraphicsAllButton.Location = new Point(14, 79);
            this.GraphicsAllButton.Margin = new Padding(3, 0, 3, 3);
            this.GraphicsAllButton.Name = "GraphicsAllButton";
            this.GraphicsAllButton.Size = new Size(118, 23);
            this.GraphicsAllButton.TabIndex = 4;
            this.GraphicsAllButton.Text = "Apply To All Saves ->";
            this.GraphicsAllButton.UseVisualStyleBackColor = true;
            this.GraphicsAllButton.Click += new EventHandler(this.GraphicsAllButton_Click);
            this.GraphicsPictureBox.Anchor = AnchorStyles.Top;
            this.GraphicsPictureBox.Location = new Point(72, 108);
            this.GraphicsPictureBox.Name = "GraphicsPictureBox";
            this.GraphicsPictureBox.Size = new Size(1, 1);
            this.GraphicsPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            this.GraphicsPictureBox.TabIndex = 6;
            this.GraphicsPictureBox.TabStop = false;
            this.RawLabel.Anchor = AnchorStyles.Top;
            this.RawLabel.AutoSize = true;
            this.RawLabel.Location = new Point(39, 132);
            this.RawLabel.Margin = new Padding(3, 20, 3, 0);
            this.RawLabel.Name = "RawLabel";
            this.RawLabel.Size = new Size(68, 13);
            this.RawLabel.TabIndex = 2;
            this.RawLabel.Text = "Tileset Raws";
            this.ObjectsListBox.FormattingEnabled = true;
            this.ObjectsListBox.Location = new Point(3, 148);
            this.ObjectsListBox.Margin = new Padding(3, 3, 3, 0);
            this.ObjectsListBox.Name = "ObjectsListBox";
            this.ObjectsListBox.Size = new Size(140, 43);
            this.ObjectsListBox.TabIndex = 0;
            this.ObjectsListBox.SelectedIndexChanged += new EventHandler(this.ObjectsListBox_SelectedIndexChanged);
            this.ObjectsAllButton.Anchor = AnchorStyles.Top;
            this.ObjectsAllButton.AutoSize = true;
            this.ObjectsAllButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ObjectsAllButton.Location = new Point(14, 191);
            this.ObjectsAllButton.Margin = new Padding(3, 0, 3, 3);
            this.ObjectsAllButton.Name = "ObjectsAllButton";
            this.ObjectsAllButton.Size = new Size(118, 23);
            this.ObjectsAllButton.TabIndex = 5;
            this.ObjectsAllButton.Text = "Apply To All Saves ->";
            this.ObjectsAllButton.UseVisualStyleBackColor = true;
            this.ObjectsAllButton.Click += new EventHandler(this.ObjectsAllButton_Click);
            this.SelectionScrollPanel.AutoScroll = true;
            this.SelectionScrollPanel.Controls.Add((Control)this.SelectionFlowPanel);
            this.SelectionScrollPanel.Dock = DockStyle.Left;
            this.SelectionScrollPanel.Location = new Point(0, 0);
            this.SelectionScrollPanel.Name = "SelectionScrollPanel";
            this.SelectionScrollPanel.RightToLeft = RightToLeft.Yes;
            this.SelectionScrollPanel.Size = new Size(171, 510);
            this.SelectionScrollPanel.TabIndex = 1;
            this.SaveListScrollPanel.AutoScroll = true;
            this.SaveListScrollPanel.Controls.Add((Control)this.SaveListFlowPanel);
            this.SaveListScrollPanel.Dock = DockStyle.Fill;
            this.SaveListScrollPanel.Location = new Point(171, 0);
            this.SaveListScrollPanel.Name = "SaveListScrollPanel";
            this.SaveListScrollPanel.RightToLeft = RightToLeft.Yes;
            this.SaveListScrollPanel.Size = new Size(525, 510);
            this.SaveListScrollPanel.TabIndex = 2;
            this.SaveListFlowPanel.AutoSize = true;
            this.SaveListFlowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SaveListFlowPanel.Controls.Add((Control)this.SavegameBoundPanel);
            this.SaveListFlowPanel.FlowDirection = FlowDirection.TopDown;
            this.SaveListFlowPanel.Location = new Point(6, 3);
            this.SaveListFlowPanel.Name = "SaveListFlowPanel";
            this.SaveListFlowPanel.RightToLeft = RightToLeft.No;
            this.SaveListFlowPanel.Size = new Size(376, 65);
            this.SaveListFlowPanel.TabIndex = 0;
            this.SavegameBoundPanel.AutoSize = true;
            this.SavegameBoundPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SavegameBoundPanel.BackColor = SystemColors.Control;
            this.SavegameBoundPanel.BorderStyle = BorderStyle.FixedSingle;
            this.SavegameBoundPanel.Controls.Add((Control)this.SavegameInfoPanel);
            this.SavegameBoundPanel.Controls.Add((Control)this.SavegameCommandPanel);
            this.SavegameBoundPanel.FlowDirection = FlowDirection.TopDown;
            this.SavegameBoundPanel.Location = new Point(0, 0);
            this.SavegameBoundPanel.Margin = new Padding(0);
            this.SavegameBoundPanel.Name = "SavegameBoundPanel";
            this.SavegameBoundPanel.Size = new Size(376, 65);
            this.SavegameBoundPanel.TabIndex = 0;
            this.SavegameBoundPanel.Visible = false;
            this.SavegameInfoPanel.AutoSize = true;
            this.SavegameInfoPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SavegameInfoPanel.Controls.Add((Control)this.SavegameNameLabel);
            this.SavegameInfoPanel.Location = new Point(0, 0);
            this.SavegameInfoPanel.Margin = new Padding(0, 0, 0, 3);
            this.SavegameInfoPanel.Name = "SavegameInfoPanel";
            this.SavegameInfoPanel.Size = new Size(213, 28);
            this.SavegameInfoPanel.TabIndex = 0;
            this.SavegameNameLabel.AutoSize = true;
            this.SavegameNameLabel.Font = new Font("Consolas", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.SavegameNameLabel.Location = new Point(3, 0);
            this.SavegameNameLabel.Name = "SavegameNameLabel";
            this.SavegameNameLabel.Size = new Size(207, 28);
            this.SavegameNameLabel.TabIndex = 0;
            this.SavegameNameLabel.Text = "World Generator";
            this.SavegameCommandPanel.AutoSize = true;
            this.SavegameCommandPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.SavegameCommandPanel.Controls.Add((Control)this.button1);
            this.SavegameCommandPanel.Controls.Add((Control)this.label1);
            this.SavegameCommandPanel.Controls.Add((Control)this.button2);
            this.SavegameCommandPanel.Location = new Point(0, 34);
            this.SavegameCommandPanel.Margin = new Padding(0, 3, 0, 0);
            this.SavegameCommandPanel.Name = "SavegameCommandPanel";
            this.SavegameCommandPanel.Size = new Size(374, 29);
            this.SavegameCommandPanel.TabIndex = 1;
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.button1.Location = new Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(98, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "-> Install Raws ->";
            this.button1.UseVisualStyleBackColor = true;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(107, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(145, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Raws: Phoebus v0.31.25v19";
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.button2.Location = new Point(258, 3);
            this.button2.Name = "button2";
            this.button2.Size = new Size(113, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "-> Install Graphics ->";
            this.button2.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add((Control)this.SaveListScrollPanel);
            this.Controls.Add((Control)this.SelectionScrollPanel);
            this.Name = "SavegameManager";
            this.Size = new Size(696, 510);
            this.Load += new EventHandler(this.SavegameManager_Load);
            this.SelectionFlowPanel.ResumeLayout(false);
            this.SelectionFlowPanel.PerformLayout();
            //this.GraphicsPictureBox.EndInit();
            this.SelectionScrollPanel.ResumeLayout(false);
            this.SelectionScrollPanel.PerformLayout();
            this.SaveListScrollPanel.ResumeLayout(false);
            this.SaveListScrollPanel.PerformLayout();
            this.SaveListFlowPanel.ResumeLayout(false);
            this.SaveListFlowPanel.PerformLayout();
            this.SavegameBoundPanel.ResumeLayout(false);
            this.SavegameBoundPanel.PerformLayout();
            this.SavegameInfoPanel.ResumeLayout(false);
            this.SavegameInfoPanel.PerformLayout();
            this.SavegameCommandPanel.ResumeLayout(false);
            this.SavegameCommandPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
