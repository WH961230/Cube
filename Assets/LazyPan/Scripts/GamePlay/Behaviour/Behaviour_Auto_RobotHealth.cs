using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_RobotHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        public Behaviour_Auto_RobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();
            
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            _healthData.Float = _maxHealthData.Float;

            MessageRegister.Instance.Reg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
        }

        private void BeDamaged(int entityId, float damageValue) {
            if (entity.ID == entityId) {
                if (_healthData.Float != 0) {
                    if (_healthData.Float > 0) {
                        _healthData.Float -= damageValue;
                    }

                    if (_healthData.Float <= 0) {
                        _healthData.Float = 0;
                        Dead();
                    }
                } 
            }
        }

        private void Dead() {
            Obj.Instance.UnLoadEntity(entity);
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
        }
    }
}