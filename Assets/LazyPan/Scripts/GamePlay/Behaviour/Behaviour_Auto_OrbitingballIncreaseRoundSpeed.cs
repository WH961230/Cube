using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_OrbitingballIncreaseRoundSpeed : Behaviour {
        public Behaviour_Auto_OrbitingballIncreaseRoundSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //获取环绕速度
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.SPEED), out FloatData _surroundSpeed);
            _surroundSpeed.Float *= 2;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}