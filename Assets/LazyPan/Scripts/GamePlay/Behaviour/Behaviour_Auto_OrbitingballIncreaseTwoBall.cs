using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_OrbitingballIncreaseTwoBall : Behaviour {
        public Behaviour_Auto_OrbitingballIncreaseTwoBall(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //增加环绕数量
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.COUNT), out IntData _surroundCount);
            _surroundCount.Int += 2; 
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}