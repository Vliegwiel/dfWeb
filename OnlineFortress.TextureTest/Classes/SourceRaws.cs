// Type: DwarfFortressConfig.SourceRaws
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnlineFortress.TextureTest {
    public class SourceRaws : Raws {
        public SourceRaws(string directory) {
            this.Location = directory;
            if (!Directory.Exists(directory) || !File.Exists(this.VersionFilename()) || !File.Exists(this.ZipFilename()))
                throw new FileNotFoundException();
            this.GetVersionFromFile();
            if (this.Version == "Unknown")
                throw new InvalidDataException();
        }

        public static List<SourceRaws> ListSourceRaws(string path) {
            List<SourceRaws> list = new List<SourceRaws>();
            try {
                if (!Directory.Exists(path))
                    return list;
                foreach (string directory in (IEnumerable<string>)Enumerable.OrderBy<string, string>((IEnumerable<string>)Directory.GetDirectories(path), (Func<string, string>)(dir => dir))) {
                    try {
                        list.Add(new SourceRaws(directory));
                    } catch {
                    }
                }
            } catch {
            }
            return list;
        }

        public string ZipFilename() {
            return this.Location + (object)Raws.slash + "raws.zip";
        }

        public string PreviewFilename() {
            string path = this.Location + (object)Raws.slash + "preview.png";
            if (File.Exists(path))
                return path;
            else
                return (string)null;
        }
    }
}
