using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_OrbitingballFreezing : Behaviour {
        public Behaviour_Auto_OrbitingballFreezing(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //冰霜
            Cond.Instance.GetData(entity, LabelStr.FROST, out BoolData _frost);
            _frost.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}