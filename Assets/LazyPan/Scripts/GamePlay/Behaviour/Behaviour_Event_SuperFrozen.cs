using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SuperFrozen : Behaviour {
        public Behaviour_Event_SuperFrozen(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FROZEN, LabelStr.RATIO), out FloatData _frozenRatio);
            _frozenRatio.Float = 0.15f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}