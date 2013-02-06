// Type: DwarfFortressConfig.RawsList
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System.Collections.Generic;
using System.Windows.Forms;

namespace OnlineFortress.TextureTest {
    public class RawsList {
        private List<Savegame> savegames;
        private List<SourceRaws> rawslist;
        private List<string> names;
        private ListBox listbox;
        private PictureBox previewbox;
        private string Category;

        public RawsList(string category, string path, ListBox listbox, PictureBox previewbox, List<Savegame> savegames) {
            this.names = new List<string>();
            this.savegames = savegames;
            this.rawslist = SourceRaws.ListSourceRaws(path);
            this.Category = category;
            this.listbox = listbox;
            this.previewbox = previewbox;
            foreach (SourceRaws sourceRaws in this.rawslist)
                this.names.Add(sourceRaws.Name + " " + sourceRaws.Version);
            listbox.Height = this.names.Count * listbox.ItemHeight + 4;
            listbox.DataSource = (object)this.names;
            listbox.SelectedIndex = this.GetDefault();
            this.UpdateSelected();
        }

        public int GetDefault() {
            int num = -1;
            string strB = "";
            for (int index = 0; index < this.rawslist.Count; ++index) {
                SourceRaws sourceRaws = this.rawslist[index];
                if (sourceRaws.Name.ToLower() == "phoebus" && string.Compare(sourceRaws.Version, strB) > 0) {
                    strB = sourceRaws.Version;
                    num = index;
                }
            }
            if (num == -1 && this.names.Count > 0)
                num = 0;
            return num;
        }

        public void UpdateSelected() {
            SourceRaws selected = this.GetSelected();
            if (this.previewbox != null)
                this.previewbox.ImageLocation = selected == null ? (string)null : selected.PreviewFilename();
            foreach (Savegame savegame in this.savegames)
                savegame.SetSelected(this.Category, selected);
        }

        private SourceRaws GetSelected() {
            if (this.listbox.SelectedIndex < 0 || this.listbox.SelectedIndex >= this.rawslist.Count)
                return (SourceRaws)null;
            else
                return this.rawslist[this.listbox.SelectedIndex];
        }

        public void InstallAll() {
            SourceRaws selected = this.GetSelected();
            foreach (Savegame savegame in this.savegames)
                savegame.Install(this.Category, selected);
            this.UpdateSelected();
        }
    }
}
