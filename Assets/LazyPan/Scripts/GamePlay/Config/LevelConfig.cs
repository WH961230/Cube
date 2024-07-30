using System;
using System.Collections.Generic;

namespace LazyPan {
    public class LevelConfig {
		public string Sign;
		public int Wave;
		public string LevelType;
		public int DifficultyLowerLimit;
		public int DifficultyUpperLimit;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, LevelConfig> dics = new Dictionary<string, LevelConfig>();

        public LevelConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Wave = int.Parse(values[1]);
				LevelType = values[2];
				DifficultyLowerLimit = int.Parse(values[3]);
				DifficultyUpperLimit = int.Parse(values[4]);

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("LevelConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    LevelConfig config = new LevelConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static LevelConfig Get(string sign) {
            if (dics.TryGetValue(sign, out LevelConfig config)) {
                return config;
            }

            return null;
        }

        public static List<string> GetKeys() {
              if (!isInit) {
                   Init();
              }
              var keys = new List<string>();
              keys.AddRange(dics.Keys);
              return keys;
        }
    }
}