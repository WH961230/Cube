using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_KnockbackPlayer : Behaviour {
        private FloatData _damage;
        private Transform _body;
        public Behaviour_Auto_KnockbackPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out _damage);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnKnockback);
        }

        private void OnKnockback(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.ObjConfig.Type == "Player") {
                    Vector3 direction = (Cond.Instance.Get<Transform>(playerEntity, LabelStr.BODY).position - _body.position).normalized;
                    direction.y = 0;
                    MessageRegister.Instance.Dis(MessageCode.MsgKnockbackPlayer, direction);
                    MessageRegister.Instance.Dis(MessageCode.MsgDamagePlayer, _damage.Float);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnKnockback);
        }
    }
}