using UnityEngine;
using UnityEngine.AI;


namespace LazyPan {
    public class Behaviour_Trigger_RobotKnockbackPlayer : Behaviour {
        private FloatData _damage;
        private Transform _body;
        private NavMeshAgent _nav;
        private Vector3 _knockbackDirection;
        
        private BoolData _knockbackData;
        private FloatData _knockbackSpeedSetting;
        private FloatData _knockbackAccelerationSetting;
        
        private float _knockbackSpeed;
        private float _knockbackAcceleration;
        public Behaviour_Trigger_RobotKnockbackPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            Cond.Instance.GetData(entity, LabelStr.DAMAGE, out _damage);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnKnockback);
            _nav = Cond.Instance.Get<NavMeshAgent>(entity, LabelStr.NAVMESHAGENT);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.KNOCKBACK, Label.ING), out _knockbackData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.KNOCKBACK, LabelStr.SPEED), out _knockbackSpeedSetting);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.KNOCKBACK, LabelStr.ACCELERATION), out _knockbackAccelerationSetting);
            MessageRegister.Instance.Reg<int, Vector3>(MessageCode.MsgKnockbackRobot, MsgKnockbackRobot);
            Game.instance.OnUpdateEvent.AddListener(OnKnockbackUpdate);
        }

        public override void DelayedExecute() {
            
        }

        private void OnKnockback(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.ObjConfig.Type == "Player") {
                    Cond.Instance.GetData(playerEntity, Label.Assemble(LabelStr.TELEPORT, Label.ING), out BoolData _teleportData);
                    //击退中取消瞬移
                    if (_teleportData.Bool) {
                        return;
                    }

                    Vector3 direction = (Cond.Instance.Get<Transform>(playerEntity, LabelStr.BODY).position - _body.position).normalized;
                    direction.y = 0;
                    MessageRegister.Instance.Dis(MessageCode.MsgKnockbackPlayer, direction);
                    MessageRegister.Instance.Dis(MessageCode.MsgDamagePlayer, _damage.Float);
                }
            }
        }

        private void MsgKnockbackRobot(int robotID, Vector3 direction) {
            if (robotID == entity.ID) {
                Debug.Log("击退敌人");
                _knockbackDirection = direction;
                _knockbackSpeed = _knockbackSpeedSetting.Float;
                _knockbackAcceleration = _knockbackAccelerationSetting.Float;
            }
        }

        private void OnKnockbackUpdate() {
            if (_nav != null) {
                if (_knockbackSpeed > 0) {
                    _knockbackSpeed += _knockbackAcceleration * Time.deltaTime;
                    _nav.Move(_knockbackDirection * _knockbackSpeed * Time.deltaTime);
                } else {
                    _knockbackSpeed = 0;
                }

                _knockbackData.Bool = _knockbackSpeed != 0;
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnKnockback);
            MessageRegister.Instance.UnReg<int, Vector3>(MessageCode.MsgKnockbackRobot, MsgKnockbackRobot);
            Game.instance.OnUpdateEvent.RemoveListener(OnKnockbackUpdate);
        }
    }
}