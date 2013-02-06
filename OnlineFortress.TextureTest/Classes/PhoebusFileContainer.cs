// Type: DwarfFortressConfig.PhoebusFileContainer
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections.ObjectModel;
using System.IO;

namespace OnlineFortress.TextureTest {
    public interface PhoebusFileContainer : IDisposable {
        ReadOnlyCollection<string> GetFiles();

        Stream GetFileStream(string FileName);

        long GetFileSize(string FileName);
    }
}
