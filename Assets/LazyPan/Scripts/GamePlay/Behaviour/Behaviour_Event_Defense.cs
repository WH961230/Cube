using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Defense : Behaviour {
        public Behaviour_Event_Defense(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.REDUCE, LabelStr.RATIO), out FloatData _damageReduceRatio);
            _damageReduceRatio.Float = 0.75f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}