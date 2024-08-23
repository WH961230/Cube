using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_PickupGetPickupRecover : Behaviour {
        public Behaviour_Event_PickupGetPickupRecover(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.PICK, LabelStr.RECOVER, LabelStr.RATIO),
                out FloatData recoverRatio);
            recoverRatio.Float = 0.01f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}