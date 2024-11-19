using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_LaserrayReduceColdTimeAndRayWidth : Behaviour {
        public Behaviour_Auto_LaserrayReduceColdTimeAndRayWidth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //子弹
            Cond.Instance.GetData(entity, LabelStr.BULLET, out StringData bulletData);
            bulletData.String = "BulletNarrow";
            //冷却时间
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out FloatData intervalData);
            intervalData.Float /= 2;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}