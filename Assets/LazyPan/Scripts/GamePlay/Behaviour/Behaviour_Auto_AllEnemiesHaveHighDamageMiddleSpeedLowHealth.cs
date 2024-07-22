using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_AllEnemiesHaveHighDamageMiddleSpeedLowHealth : Behaviour {
        public Behaviour_Auto_AllEnemiesHaveHighDamageMiddleSpeedLowHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            BehaviourData.Get(LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out FloatData _movementSpeedData);
            BehaviourData.Get(LabelStr.DAMAGE, out FloatData _damageData);
            BehaviourData.Get(LabelStr.HEALTH, out FloatData _healthData);

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out FloatData _currentMovementSpeedData);
            _currentMovementSpeedData.Float = _movementSpeedData.Float;

            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out FloatData _currentDamageData);
            _currentDamageData.Float = _damageData.Float;

            Cond.Instance.GetData(entity, LabelStr.HEALTH, out FloatData _currentHealthData);
            _currentHealthData.Float = _healthData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}