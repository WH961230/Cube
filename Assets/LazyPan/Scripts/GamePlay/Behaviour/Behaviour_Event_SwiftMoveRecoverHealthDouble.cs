using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_SwiftMoveRecoverHealthDouble : Behaviour {
        public Behaviour_Event_SwiftMoveRecoverHealthDouble(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.HEALTH, LabelStr.RECOVER, LabelStr.RATIO), out FloatData _ratio);
            _ratio.Float *= 2;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}