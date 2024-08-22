using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGunKnockbackRobot : Behaviour {
        public Behaviour_Auto_SubmachineGunKnockbackRobot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //是否可以击退敌人
            Cond.Instance.GetData(entity, LabelStr.KNOCKBACK, out BoolData _knockback);
            _knockback.Bool = true;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}