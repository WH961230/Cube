using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_LaserrayIncreaseARay : Behaviour {
        public Behaviour_Auto_LaserrayIncreaseARay(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.COUNT), out IntData _fireCount);
            _fireCount.Int++;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}