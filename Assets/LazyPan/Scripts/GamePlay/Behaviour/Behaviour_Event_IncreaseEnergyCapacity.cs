using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseEnergyCapacity : Behaviour {
        public Behaviour_Event_IncreaseEnergyCapacity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            BehaviourData.Get(LabelStr.TARGET, out StringData targetEntitySign);
            EntityRegister.TryGetEntityBySign(targetEntitySign.String, out Entity targetEntity);
            Cond.Instance.GetData(targetEntity, LabelStr.Assemble(Label.ENERGY, LabelStr.MAX), out FloatData _energyMaxData);
            float energyBefore = _energyMaxData.Float;
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, Label.ENERGY, LabelStr.MAX), out FloatData _increaseEnergyMaxData);
            _energyMaxData.Float += _increaseEnergyMaxData.Float;
            Debug.LogFormat("增加容量{0}: 之前{1} 之后{2}", _increaseEnergyMaxData.Float, energyBefore, _energyMaxData.Float);
        }

        public override void Clear() {
            base.Clear();
        }
    }
}