using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGunFiresFasterAndHasACloserRange : Behaviour {
        public Behaviour_Auto_SubmachineGunFiresFasterAndHasACloserRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out FloatData _fireRateInterval);
            _fireRateInterval.Float *= 0.5f;
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out FloatData _fireRange);
            _fireRange.Float *= 0.5f;
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
        }
    }
}