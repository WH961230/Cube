﻿using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RobotStrengthen : Behaviour {
        public Behaviour_Auto_RobotStrengthen(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            //BUFF数据
            BehaviourData.Get(LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out FloatData _movementSpeedData);
            BehaviourData.Get(LabelStr.DAMAGE, out FloatData _damageData);
            BehaviourData.Get(LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData _maxHealthData);

            //修改速度
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out FloatData _currentMovementSpeedData);
            _currentMovementSpeedData.Float *= 1 + _movementSpeedData.Float;

            //修改伤害
            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out FloatData _currentDamageData);
            _currentDamageData.Float *= 1 + _damageData.Float;

            //修改血量
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData _robotMaxHealthData);
            _robotMaxHealthData.Float *= 1 + _maxHealthData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}