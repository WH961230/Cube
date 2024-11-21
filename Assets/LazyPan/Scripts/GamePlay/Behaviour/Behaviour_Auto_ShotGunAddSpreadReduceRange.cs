using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ShotGunAddSpreadReduceRange : Behaviour {
        public Behaviour_Auto_ShotGunAddSpreadReduceRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //减少射击间隔
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out FloatData _fireRateInterval);
            _fireRateInterval.Float /= 2f;
            //减少射程
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out FloatData _fireRange);
            _fireRange.Float /= 2f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}