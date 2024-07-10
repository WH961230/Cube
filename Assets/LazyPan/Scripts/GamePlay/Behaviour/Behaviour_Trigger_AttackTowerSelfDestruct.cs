using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_AttackTowerSelfDestruct : Behaviour {
        private FloatData _damage;
        private Transform _body;

        public Behaviour_Trigger_AttackTowerSelfDestruct(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out _damage);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnAttackTower);
        }

        private void OnAttackTower(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity triggerEntity)) {
                if (triggerEntity.ObjConfig.Type == "Tower") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamagePlayer, _damage.Float);
                    SelfDestruct();
                }
            }
        }

        private void SelfDestruct() {
            Obj.Instance.UnLoadEntity(entity);
        }
        
        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnAttackTower);
        }
    }
}