using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_StrafesatelliteBurnRobot : Behaviour {
        public Behaviour_Auto_StrafesatelliteBurnRobot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //灼烧
            Cond.Instance.GetData(entity, LabelStr.BURN, out BoolData _burn);
            _burn.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}