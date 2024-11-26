using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_AttackTowerSelfDestruct : Behaviour {
        private FloatData _damage;
        private FloatData _damageConversionRatio;
        private FloatData _globalDamageConversionRatio;
        private FloatData _healthData;
        private Transform _body;

        public Behaviour_Trigger_AttackTowerSelfDestruct(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.CONVERSION, LabelStr.RATIO),
                out _damageConversionRatio);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.CONVERSION, LabelStr.RATIO),
                out _globalDamageConversionRatio);
            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out _damage);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnAttackTower);
        }
        
        public override void DelayedExecute() {
            
        }

        private void OnAttackTower(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity triggerEntity)) {
                if (triggerEntity.ObjConfig.Type == "Tower") {
                    float damage = _damage.Float;
                    if (_globalDamageConversionRatio != null && _globalDamageConversionRatio.Float > 0) {
                        damage = _globalDamageConversionRatio.Float * _healthData.Float;
                    } else if (_damageConversionRatio != null) {
                        damage = _damageConversionRatio.Float * _healthData.Float;
                    }
                    MessageRegister.Instance.Dis(MessageCode.MsgDamagePlayer, damage);
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, entity.ID, Mathf.Infinity);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnAttackTower);
        }
    }
}