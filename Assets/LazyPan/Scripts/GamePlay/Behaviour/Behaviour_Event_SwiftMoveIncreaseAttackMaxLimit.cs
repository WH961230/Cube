using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SwiftMoveIncreaseAttackMaxLimit : Behaviour {
        public Behaviour_Event_SwiftMoveIncreaseAttackMaxLimit(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.MAX, LabelStr.RATIO), out FloatData _attackMaxRatio);
            _attackMaxRatio.Float *= 2;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}