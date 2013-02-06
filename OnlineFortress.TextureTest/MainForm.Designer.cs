using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using OnlineFortress.TextureTest;

namespace OnlineFortress.TextureTest {
    partial class MainForm {
        private IContainer components = null;
        private TilesetAssembler tilesetAssembler1;

        protected override CreateParams CreateParams {
            get {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 33554432;
                return createParams;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            this.KeyPreview = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue != 116)
                return;
            e.Handled = true;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
            tilesetAssembler1 = new TilesetAssembler();
            this.SuspendLayout();
            this.tilesetAssembler1.AutoScrollMargin = new Size(20, 20);
            this.tilesetAssembler1.Dock = DockStyle.Fill;
            this.tilesetAssembler1.Location = new Point(0, 0);
            this.tilesetAssembler1.Name = "tilesetAssembler1";
            this.tilesetAssembler1.Size = new Size(882, 580);
            this.tilesetAssembler1.TabIndex = 0;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.ClientSize = new Size(882, 580);
            this.Controls.Add((Control)this.tilesetAssembler1);
            //this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Name = "MainForm";
            this.Text = "Phoebus' Dwarf Fortress Config Utility";
            this.Load += new EventHandler(this.MainForm_Load);
            this.KeyDown += new KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
        }
    }
}