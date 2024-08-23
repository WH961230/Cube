using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_PickupIncreaseTimeLimitCanOverlayMoveSpeed : Behaviour {
        public Behaviour_Event_PickupIncreaseTimeLimitCanOverlayMoveSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.PICK, LabelStr.INCREASE, LabelStr.MOVE, LabelStr.SPEED),
                out FloatData PickIncreaseMoveSpeed);
            PickIncreaseMoveSpeed.Float = 0.01f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}