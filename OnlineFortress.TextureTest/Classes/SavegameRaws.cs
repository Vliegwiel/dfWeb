// Type: DwarfFortressConfig.SavegameRaws
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace OnlineFortress.TextureTest {
    public class SavegameRaws : Raws {
        public SavegameRaws(string directory) {
            this.Location = directory;
            this.GetVersionFromFile();
        }

        public bool Install(SourceRaws Source, bool clearDirectory = false) {
            try {
                if (File.Exists(Source.ZipFilename())) {
                    if (clearDirectory)
                        SavegameRaws.ClearDirectory(this.Location, false);
                    new FastZip().ExtractZip(Source.ZipFilename(), this.Location, FastZip.Overwrite.Always, (FastZip.ConfirmOverwriteDelegate)null, (string)null, (string)null, true);
                    File.Copy(Source.VersionFilename(), this.VersionFilename(), true);
                    this.Name = Source.Name;
                    this.Version = Source.Version;
                    return true;
                }
            } catch {
            }
            return false;
        }

        private static void ClearDirectory(string path, bool deleteDirectory = false) {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);
            foreach (string path1 in files) {
                try {
                    File.SetAttributes(path1, FileAttributes.Normal);
                    File.Delete(path1);
                } catch {
                }
            }
            foreach (string path1 in directories) {
                try {
                    SavegameRaws.ClearDirectory(path1, true);
                } catch {
                }
            }
            if (!deleteDirectory)
                return;
            try {
                Directory.Delete(path, false);
            } catch {
            }
        }
    }
}
