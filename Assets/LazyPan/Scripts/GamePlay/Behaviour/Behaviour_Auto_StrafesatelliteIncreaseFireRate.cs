using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_StrafesatelliteIncreaseFireRate : Behaviour {
        public Behaviour_Auto_StrafesatelliteIncreaseFireRate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL),
                out FloatData _fireRateInterval);
            _fireRateInterval.Float *= 0.5f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}