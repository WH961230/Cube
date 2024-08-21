using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Behead : Behaviour {
        public Behaviour_Event_Behead(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BEHEAD, LabelStr.HEALTH, LabelStr.RATIO), out FloatData _beheadHealthRatio);
            _beheadHealthRatio.Float = 0.05f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}