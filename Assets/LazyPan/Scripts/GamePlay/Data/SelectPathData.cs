using System;
using System.Collections.Generic;

namespace LazyPan {
    public class SelectPathData : Data {
        public List<PathData> Paths = new List<PathData>();
        public override bool Get<T>(string sign, out T t) {
            base.Get(sign, out t);
            if (typeof(T) == typeof(PathData)) {
                foreach (PathData data in Paths) {
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

    //选择数据
    [Serializable]
    public class PathData {
        public string Sign;
        public string BehaviourName;
        public string Description;
        public string NextSign;
    }
}