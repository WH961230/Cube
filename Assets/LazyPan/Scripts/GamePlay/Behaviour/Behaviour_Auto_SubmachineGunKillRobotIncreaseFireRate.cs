using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGunKillRobotIncreaseFireRate : Behaviour {
        public Behaviour_Auto_SubmachineGunKillRobotIncreaseFireRate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.KILL, LabelStr.ROBOT, LabelStr.INCREASE, LabelStr.RATE, LabelStr.RATIO),
                out FloatData killRobotIncreaseRateRatio);
            killRobotIncreaseRateRatio.Float = 0.01f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}