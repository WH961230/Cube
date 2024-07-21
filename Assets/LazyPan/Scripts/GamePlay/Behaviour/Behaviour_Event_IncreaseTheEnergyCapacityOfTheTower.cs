using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseTheEnergyCapacityOfTheTower : Behaviour {
        public Behaviour_Event_IncreaseTheEnergyCapacityOfTheTower(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            Cond.Instance.GetData(base.entity, LabelStr.Assemble(Label.ENERGY, LabelStr.MAX), out FloatData _energyMaxData);
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, Label.ENERGY, LabelStr.MAX), out FloatData _increaseEnergyMaxData);
            _energyMaxData.Float += _increaseEnergyMaxData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}