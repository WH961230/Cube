using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGunKillRobotIncreaseAttack : Behaviour {
        public Behaviour_Auto_SubmachineGunKillRobotIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.KILL, LabelStr.ROBOT, LabelStr.INCREASE, LabelStr.ATTACK, LabelStr.RATIO),
                out FloatData killRobotIncreaseAttackRatio);
            killRobotIncreaseAttackRatio.Float = 0.01f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}