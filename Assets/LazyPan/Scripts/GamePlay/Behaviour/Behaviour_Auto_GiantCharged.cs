using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_GiantCharged : Behaviour {
        public Behaviour_Auto_GiantCharged(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.CONVERSION, LabelStr.RATIO),
                out FloatData _damageConversionRatio);
            _damageConversionRatio.Float = 0.5f;
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.SPEED, LabelStr.CONVERSION, LabelStr.RATIO),
                out FloatData _speedConversionRatio);
            _speedConversionRatio.Float = 0.1f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}