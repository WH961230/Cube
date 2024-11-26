using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_GiantChanter : Behaviour {
        public Behaviour_Auto_GiantChanter(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //设置全局蚂蚁参数
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.RECOVER, LabelStr.RATIO),
                out FloatData _globalRecoverData);
            _globalRecoverData.Float = 0.1f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}