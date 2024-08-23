using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_GrenadeIncreaseBoomRange : Behaviour {
        public Behaviour_Auto_GrenadeIncreaseBoomRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //爆炸范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.RANGE), out FloatData _BoomRange);
            _BoomRange.Float *= 1.5f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}