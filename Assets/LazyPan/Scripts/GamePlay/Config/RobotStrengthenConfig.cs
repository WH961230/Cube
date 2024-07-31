using System;
using System.Collections.Generic;

namespace LazyPan {
    public class RobotStrengthenConfig {
		public string Sign;
		public int Level;
		public float HealthPercentage;
		public float AttackPercentage;
		public float MovementSpeedPercentage;
		public string Text;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, RobotStrengthenConfig> dics = new Dictionary<string, RobotStrengthenConfig>();

        public RobotStrengthenConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Level = int.Parse(values[1]);
				HealthPercentage = float.Parse(values[2]);
				AttackPercentage = float.Parse(values[3]);
				MovementSpeedPercentage = float.Parse(values[4]);
				Text = values[5];

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("RobotStrengthenConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    RobotStrengthenConfig config = new RobotStrengthenConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static RobotStrengthenConfig Get(string sign) {
            if (dics.TryGetValue(sign, out RobotStrengthenConfig config)) {
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