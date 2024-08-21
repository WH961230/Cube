using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ProtectIncreaseRecoverHealthSpeed : Behaviour {
        public Behaviour_Event_ProtectIncreaseRecoverHealthSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.HEALTH, LabelStr.SPEED),
                out FloatData healthSpeed);
            healthSpeed.Float *= 2;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}