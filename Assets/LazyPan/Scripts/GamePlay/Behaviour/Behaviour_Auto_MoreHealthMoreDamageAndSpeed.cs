using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_MoreHealthMoreDamageAndSpeed : Behaviour {
        private FloatData _damageConversionRatio;
        private FloatData _speedConversionRatio;

        public Behaviour_Auto_MoreHealthMoreDamageAndSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.CONVERSION, LabelStr.RATIO),
                out _damageConversionRatio);
            _damageConversionRatio.Float = 0.5f;
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SPEED, LabelStr.CONVERSION, LabelStr.RATIO),
                out _speedConversionRatio);
            _speedConversionRatio.Float = 0.1f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}