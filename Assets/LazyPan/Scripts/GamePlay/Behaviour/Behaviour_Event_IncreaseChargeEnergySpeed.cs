using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseChargeEnergySpeed : Behaviour {
        public Behaviour_Event_IncreaseChargeEnergySpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            BehaviourData.Get(LabelStr.TARGET, out StringData targetEntitySign);
            EntityRegister.TryGetEntityBySign(targetEntitySign.String, out Entity targetEntity);
            Cond.Instance.GetData(targetEntity, LabelStr.Assemble(Label.ENERGY, LabelStr.SPEED), out FloatData _energySpeedData);
            float energySpeedBefore = _energySpeedData.Float;
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, Label.ENERGY, LabelStr.SPEED), out FloatData _increaseEnergySpeedData);
            _energySpeedData.Float += _increaseEnergySpeedData.Float;
            Debug.LogFormat("增加充能速度{0}: 之前{1} 之后{2}", _increaseEnergySpeedData.Float, energySpeedBefore, _energySpeedData.Float);
        }

        public override void Clear() {
            base.Clear();
        }
    }
}