// Type: DwarfFortressConfig.PhoebusZipContainer
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace OnlineFortress.TextureTest {
    public class PhoebusZipContainer : PhoebusFileContainer, IDisposable {
        private Hashtable ZipEntries = new Hashtable();
        private FileStream fs;
        private ZipFile zf;
        private ReadOnlyCollection<string> Names;

        public PhoebusZipContainer(string ZipFileName) {
            try {
                this.fs = File.OpenRead(ZipFileName);
                this.zf = new ZipFile(this.fs);
                List<string> list = new List<string>();
                foreach (ZipEntry zipEntry in this.zf) {
                    if (zipEntry.IsFile) {
                        list.Add(PhoebusZipContainer.NormalizeDirectorySeparatorChars(zipEntry.Name));
                        this.ZipEntries.Add((object)PhoebusZipContainer.NormalizeDirectorySeparatorChars(zipEntry.Name).ToLower(), (object)zipEntry);
                    }
                }
                this.Names = list.AsReadOnly();
            } catch {
                if (this.zf != null) {
                    this.zf.IsStreamOwner = true;
                    this.zf.Close();
                } else if (this.fs != null)
                    this.fs.Dispose();
                throw;
            }
        }

        ~PhoebusZipContainer() {
            this.Dispose();
        }

        public ReadOnlyCollection<string> GetFiles() {
            return this.Names;
        }

        public Stream GetFileStream(string FileName) {
            FileName = FileName.ToLower();
            if (!this.ZipEntries.ContainsKey((object)FileName))
                return (Stream)null;
            ZipEntry entry = (ZipEntry)this.ZipEntries[(object)FileName];
            try {
                return this.zf.GetInputStream(entry);
            } catch {
                return (Stream)null;
            }
            return null;
        }

        public long GetFileSize(string FileName) {
            FileName = FileName.ToLower();
            if (!this.ZipEntries.ContainsKey((object)FileName))
                return -1L;
            ZipEntry zipEntry = (ZipEntry)this.ZipEntries[(object)FileName];
            try {
                return zipEntry.Size;
            } catch {
                return -1L;
            }
        }

        public void Dispose() {
            if (this.zf == null)
                return;
            this.zf.Close();
            this.fs.Dispose();
            this.zf = (ZipFile)null;
        }

        private static string NormalizeDirectorySeparatorChars(string PathName) {
            return PathName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
