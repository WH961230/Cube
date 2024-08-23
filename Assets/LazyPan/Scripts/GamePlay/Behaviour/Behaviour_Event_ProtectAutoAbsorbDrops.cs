using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ProtectAutoAbsorbDrops : Behaviour {
        public Behaviour_Event_ProtectAutoAbsorbDrops(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(),
                LabelStr.Assemble(LabelStr.AUTO, LabelStr.ABSORB, LabelStr.EXPERIENCE, LabelStr.RATIO),
                out FloatData _autoAbsorbExperienceRatio);
            _autoAbsorbExperienceRatio.Float = 0.1f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}