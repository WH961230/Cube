using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RobotReborn : Behaviour {
        public Behaviour_Auto_RobotReborn(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.REBORN, LabelStr.RATIO),
                out FloatData _rebornRatio);
            _rebornRatio.Float = 0.33f;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}