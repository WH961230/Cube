using System;
using System.Collections.Generic;

namespace LazyPan {
    public class RobotWaveData : Data {
        public List<WaveData> Waves = new List<WaveData>();
        public override bool Get<T>(string sign, out T t) {
            if (typeof(T) == typeof(BoolData)) {
                foreach (BoolData data in Bools) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(IntData)) {
                foreach (IntData data in Ints) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(FloatData)) {
                foreach (FloatData data in Floats) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(StringData)) {
                foreach (StringData data in Strings) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(Vector3Data)) {
                foreach (Vector3Data data in Vector3s) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } if (typeof(T) == typeof(WaveData)) {
                foreach (WaveData data in Waves) {
                    if (data.Sign == sign) {
                        t = (T) Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            }

            t = default;
            return false;
        }
    }

    //波次
    [Serializable]
    public class WaveData {
        public string Sign;
        public string Description;
        public List<WaveInstanceData> WaveInstanceDefaultList;
        public List<WaveInstanceData> WaveInstanceList;
    }

    [Serializable]
    public class WaveInstanceData {
        public int InstanceNumber; //机器人创建个数
        public string InstanceRobotSign; //机器人标识
        public float InstanceDelayTime; //创建延时时间
    }
}