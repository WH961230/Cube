using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_GrenadeIncreaseAreaContinueTime : Behaviour {
        public Behaviour_Auto_GrenadeIncreaseAreaContinueTime(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //持续时间
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.DURATION, LabelStr.TIME), out FloatData _BoomDuringTime);
            _BoomDuringTime.Float *= 1.5f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}