using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Inertia : Behaviour {
        public Behaviour_Event_Inertia(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.CREATE, LabelStr.ROBOT, LabelStr.DELAY, LabelStr.TIME),
                out FloatData delayTime);
            delayTime.Float = 1;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}