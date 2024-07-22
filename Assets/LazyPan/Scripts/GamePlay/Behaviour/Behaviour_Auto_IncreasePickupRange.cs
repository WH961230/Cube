using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_IncreasePickupRange : Behaviour {
        public Behaviour_Auto_IncreasePickupRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            BehaviourData.Get(LabelStr.RANGE, out FloatData _rangeData);

            Cond.Instance.GetData(entity, LabelStr.RANGE, out FloatData _currentRangeData);
            _currentRangeData.Float += _rangeData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}