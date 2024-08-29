using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ImmunityToFrost : Behaviour {
        public Behaviour_Event_ImmunityToFrost(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.IGNORE, LabelStr.FROST), out BoolData data);
            data.Bool = true;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}