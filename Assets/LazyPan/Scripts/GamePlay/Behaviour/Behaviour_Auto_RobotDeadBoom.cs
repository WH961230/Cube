using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RobotDeadBoom : Behaviour {
        public Behaviour_Auto_RobotDeadBoom(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DEAD, LabelStr.BOOM), out BoolData deadBoom);
            deadBoom.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}