using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SniperrifleBulletPenetrate : Behaviour {
        public Behaviour_Auto_SniperrifleBulletPenetrate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.PENETRATE, out BoolData _penetrate);
            _penetrate.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}