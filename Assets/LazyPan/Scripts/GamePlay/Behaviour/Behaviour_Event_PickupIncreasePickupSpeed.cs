using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_PickupIncreasePickupSpeed : Behaviour {
        public Behaviour_Event_PickupIncreasePickupSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.GET, LabelStr.MOVE, LabelStr.SPEED), out FloatData _getMoveSpeed);
            _getMoveSpeed.Float *= 1.5f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}