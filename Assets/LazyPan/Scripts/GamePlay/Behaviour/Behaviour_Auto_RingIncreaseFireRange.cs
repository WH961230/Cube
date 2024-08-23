using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RingIncreaseFireRange : Behaviour {
        public Behaviour_Auto_RingIncreaseFireRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out FloatData _fireRange);
            _fireRange.Float *= 1.33f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}