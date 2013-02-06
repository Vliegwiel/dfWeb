// Type: DwarfFortressConfig.PhoebusPathContainer
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace OnlineFortress.TextureTest {
    public class PhoebusPathContainer : PhoebusFileContainer, IDisposable {
        private Hashtable FileNames = new Hashtable();
        private ReadOnlyCollection<string> Names;

        public PhoebusPathContainer(string PathName) {
            if (PathName == null)
                throw new ArgumentNullException();
            PathName = PhoebusPathContainer.NormalizePathName(PathName);
            string str1 = PathName.ToLower() + (object)Path.DirectorySeparatorChar;
            if (!Directory.Exists(PathName))
                throw new DirectoryNotFoundException();
            IEnumerable<string> enumerable = Directory.EnumerateFiles(PathName, "*.*", SearchOption.AllDirectories);
            List<string> list = new List<string>();
            foreach (string PathName1 in enumerable) {
                string str2 = PhoebusPathContainer.NormalizeDirectorySeparatorChars(PathName1);
                if (!(str2.Substring(0, str1.Length).ToLower() != str1)) {
                    string str3 = str2.Substring(str1.Length);
                    list.Add(str3);
                    this.FileNames.Add((object)str3.ToLower(), (object)PathName1);
                }
            }
            this.Names = list.AsReadOnly();
        }

        public ReadOnlyCollection<string> GetFiles() {
            return this.Names;
        }

        public Stream GetFileStream(string FileName) {
            FileName = FileName.ToLower();
            if (!this.FileNames.ContainsKey((object)FileName))
                return (Stream)null;
            string path = (string)this.FileNames[(object)FileName];
            try {
                return (Stream)new FileStream(path, FileMode.Open, FileAccess.Read);
            } catch {
                return (Stream)null;
            }
        }

        public long GetFileSize(string FileName) {
            FileName = FileName.ToLower();
            if (!this.FileNames.ContainsKey((object)FileName))
                return -1L;
            string fileName = (string)this.FileNames[(object)FileName];
            try {
                return new FileInfo(fileName).Length;
            } catch {
                return -1L;
            }
        }

        public void Dispose() {
        }

        private static string NormalizePathName(string PathName) {
            return PhoebusPathContainer.NormalizeDirectorySeparatorChars(PathName).TrimEnd(new char[1]
      {
        Path.DirectorySeparatorChar
      });
        }

        private static string NormalizeDirectorySeparatorChars(string PathName) {
            return PathName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
