using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SuperBurning : Behaviour {
        public Behaviour_Event_SuperBurning(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.RATIO), out FloatData _boomRatio);
            _boomRatio.Float = 0.15f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}