using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RingAttackBurningArea : Behaviour {
        public Behaviour_Auto_RingAttackBurningArea(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FROZEN, LabelStr.RATIO),
                out FloatData _frozenRatio);
            _frozenRatio.Float = 0.5f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}