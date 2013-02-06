// Type: DwarfFortressConfig.Raws
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System.IO;
using System.Text.RegularExpressions;

namespace OnlineFortress.TextureTest {
    public abstract class Raws {
        protected static char slash = Path.DirectorySeparatorChar;
        private static Regex regexVersionEntry = new Regex("\\[Version:([^:\\]\\[]*):([^:\\]\\[]*)\\]", RegexOptions.IgnoreCase);
        protected const string unknown = "Unknown";
        public string Name;
        public string Version;
        public string Location;

        static Raws() {
        }

        public string VersionFilename() {
            return this.Location + (object)Raws.slash + "version.txt";
        }

        public void GetVersionFromFile() {
            try {
                using (StreamReader streamReader = new StreamReader(this.VersionFilename())) {
                    if (this.GetVersionFromString(streamReader.ReadToEnd()))
                        return;
                }
            } catch {
            }
            this.Name = "Unknown";
            this.Version = "";
        }

        public bool GetVersionFromString(string text) {
            if (text != null) {
                try {
                    Match match = Raws.regexVersionEntry.Match(text);
                    if (match.Success) {
                        if (match.Groups.Count >= 3) {
                            this.Name = match.Groups[1].Value;
                            this.Version = match.Groups[2].Value;
                            return true;
                        }
                    }
                } catch {
                }
            }
            return false;
        }
    }
}
