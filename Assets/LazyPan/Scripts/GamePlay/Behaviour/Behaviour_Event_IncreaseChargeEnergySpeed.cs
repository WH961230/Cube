using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseChargeEnergySpeed : Behaviour {
        public Behaviour_Event_IncreaseChargeEnergySpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(base.entity, LabelStr.Assemble(Label.ENERGY, LabelStr.SPEED), out FloatData _energySpeedData);
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, Label.ENERGY, LabelStr.SPEED), out FloatData _increaseEnergySpeedData);
            _energySpeedData.Float += _increaseEnergySpeedData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}