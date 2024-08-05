using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_IncreaseEnergyCapacity : Behaviour {
        public Behaviour_Auto_IncreaseEnergyCapacity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            if (BehaviourData.Get(LabelStr.Assemble(LabelStr.ENERGY, LabelStr.MAX), out FloatData _increaseCapacityData)) {
                if (Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ENERGY, LabelStr.MAX), out FloatData _capacityData)) {
                    _capacityData.Float += _increaseCapacityData.Float;
                }
            }
        }

        public override void Clear() {
            base.Clear();
        }
    }
}