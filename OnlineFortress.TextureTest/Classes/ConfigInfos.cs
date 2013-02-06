// Type: DwarfFortressConfig.ConfigInfos
// Assembly: DwarfFortressConfig, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\DF-Phoebus\PhoebusTilesetAssembler.exe

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OnlineFortress.TextureTest {
    public class ConfigInfos {
        private static Regex regexDimEntry = new Regex("\\[Dim(:([^:\\]\\[]*))+\\]", RegexOptions.IgnoreCase);
        private static Regex regexTileEntry = new Regex("\\[Tile(:([^:\\]\\[]*))+\\]", RegexOptions.IgnoreCase);
        private static ConfigInfos ConfigWallInfos = (ConfigInfos)null;
        private static ConfigInfos ConfigRoughWallInfos = (ConfigInfos)null;
        private static ConfigInfos ConfigDefaultInfos = (ConfigInfos)null;
        public TilesetSize size;
        public ICollection<ConfigPatchInfo> infos;

        static ConfigInfos() {
        }

        public ConfigInfos(List<ConfigPatchInfo> infos, TilesetSize finalSize) {
            this.size = new TilesetSize(finalSize);
            this.infos = (ICollection<ConfigPatchInfo>)infos.AsReadOnly();
        }

        public ConfigInfos(string config, TilesetSize finalSize) {
            this.extractSize(config, finalSize);
            this.extraPatchInfos(config, finalSize);
        }

        public ConfigInfos(string config, TilesetSize finalSize, ConfigInfos defaultInfos) {
            this.extractSize(config, defaultInfos.size);
            this.extraPatchInfos(config, finalSize);
            if (this.infos.Count != 0)
                return;
            this.infos = defaultInfos.infos;
        }

        private void extractSize(string config, TilesetSize defaultSize) {
            if (config != null) {
                Match match = ConfigInfos.regexDimEntry.Match(config);
                if (match.Success && match.Groups.Count >= 3) {
                    CaptureCollection captures = match.Groups[2].Captures;
                    try {
                        if (captures.Count == 2) {
                            this.size = new TilesetSize(Convert.ToInt32(captures[0].Value), Convert.ToInt32(captures[1].Value), defaultSize);
                            return;
                        } else if (captures.Count == 4) {
                            this.size = new TilesetSize(Convert.ToInt32(captures[0].Value), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), Convert.ToInt32(captures[3].Value));
                            return;
                        }
                    } catch {
                    }
                }
            }
            this.size = new TilesetSize(defaultSize);
        }

        public void extraPatchInfos(string config, TilesetSize finalSize) {
            List<ConfigPatchInfo> list = new List<ConfigPatchInfo>();
            if (config != null) {
                try {
                    MatchCollection matchCollection = ConfigInfos.regexTileEntry.Matches(config);
                    for (int index = 0; index < matchCollection.Count; ++index) {
                        Match match = matchCollection[index];
                        if (match.Success && match.Groups.Count >= 3) {
                            CaptureCollection captures = match.Groups[2].Captures;
                            if (captures.Count >= 3) {
                                if (captures.Count == 3) {
                                    try {
                                        list.Add(new ConfigPatchInfo(captures[0].Value.ToLower(), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), 1, 1, Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), this.size, finalSize));
                                    } catch {
                                    }
                                } else if (captures.Count == 5) {
                                    try {
                                        list.Add(new ConfigPatchInfo(captures[0].Value.ToLower(), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), Convert.ToInt32(captures[3].Value), Convert.ToInt32(captures[4].Value), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), this.size, finalSize));
                                    } catch {
                                    }
                                } else if (captures.Count == 6) {
                                    if (!(captures[3].Value.ToLower() != "to")) {
                                        try {
                                            list.Add(new ConfigPatchInfo(captures[0].Value.ToLower(), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), 1, 1, Convert.ToInt32(captures[4].Value), Convert.ToInt32(captures[5].Value), this.size, finalSize));
                                        } catch {
                                        }
                                    }
                                } else if (captures.Count == 8) {
                                    if (!(captures[5].Value.ToLower() != "to")) {
                                        try {
                                            list.Add(new ConfigPatchInfo(captures[0].Value.ToLower(), Convert.ToInt32(captures[1].Value), Convert.ToInt32(captures[2].Value), Convert.ToInt32(captures[3].Value), Convert.ToInt32(captures[4].Value), Convert.ToInt32(captures[6].Value), Convert.ToInt32(captures[7].Value), this.size, finalSize));
                                        } catch {
                                        }
                                    }
                                }
                            }
                        }
                    }
                } catch {
                }
            }
            this.infos = (ICollection<ConfigPatchInfo>)list.AsReadOnly();
        }

        public static ConfigInfos getWallInfos(TilesetSize finalSize) {
            if (ConfigInfos.ConfigWallInfos == null)
                ConfigInfos.ConfigWallInfos = new ConfigInfos(new List<ConfigPatchInfo>()
        {
          new ConfigPatchInfo("filter", 7, 11, 8, 2, 7, 11, finalSize, finalSize),
          new ConfigPatchInfo("filter", 3, 13, 4, 1, 3, 13, finalSize, finalSize)
        }, finalSize);
            return ConfigInfos.ConfigWallInfos;
        }

        public static ConfigInfos getRoughWallInfos(TilesetSize finalSize) {
            if (ConfigInfos.ConfigRoughWallInfos == null)
                ConfigInfos.ConfigRoughWallInfos = new ConfigInfos(new List<ConfigPatchInfo>()
        {
          new ConfigPatchInfo("ground", 3, 8, 4, 1, 3, 8, finalSize, finalSize),
          new ConfigPatchInfo("ground", 8, 8, 4, 1, 8, 8, finalSize, finalSize),
          new ConfigPatchInfo("ground", 12, 9, 2, 1, 12, 9, finalSize, finalSize),
          new ConfigPatchInfo("ground", 9, 10, 4, 1, 9, 10, finalSize, finalSize)
        }, finalSize);
            return ConfigInfos.ConfigRoughWallInfos;
        }

        public static ConfigInfos getDefaultInfos(TilesetSize finalSize) {
            if (ConfigInfos.ConfigDefaultInfos == null)
                ConfigInfos.ConfigDefaultInfos = new ConfigInfos(new List<ConfigPatchInfo>()
        {
          new ConfigPatchInfo("filter", 0, 0, 16, 16, 0, 0, finalSize, finalSize)
        }, finalSize);
            return ConfigInfos.ConfigDefaultInfos;
        }
    }
}
