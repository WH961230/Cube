using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RobotAntibody : Behaviour {
        public Behaviour_Auto_RobotAntibody(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.ANTIBODY, out BoolData antibodyBool);
            antibodyBool.Bool = true;
        }

        public override void DelayedExecute() {
        }


        public override void Clear() {
            base.Clear();
        }
    }
}