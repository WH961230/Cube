using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RingAttackFreezingArea : Behaviour {
        public Behaviour_Auto_RingAttackFreezingArea(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.RATIO),
                out FloatData _boomRatio);
            _boomRatio.Float = 0.5f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}