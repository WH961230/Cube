using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseHealthSpeed : Behaviour {
        public Behaviour_Event_IncreaseHealthSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }
        
        public override void DelayedExecute() {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.HEALTH, LabelStr.SPEED), out FloatData _healthSpeedData);
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, LabelStr.HEALTH, LabelStr.SPEED), out FloatData _increaseHealthSpeedData);
            _healthSpeedData.Float += _increaseHealthSpeedData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}