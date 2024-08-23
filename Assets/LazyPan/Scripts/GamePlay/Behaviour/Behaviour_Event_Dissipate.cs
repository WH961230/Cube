using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Dissipate : Behaviour {
        public Behaviour_Event_Dissipate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.DESTROY, LabelStr.DELAY, LabelStr.INCREASE, LabelStr.TIME),
                out FloatData _destroyDelayIncreaseTime);
            _destroyDelayIncreaseTime.Float = 3;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}