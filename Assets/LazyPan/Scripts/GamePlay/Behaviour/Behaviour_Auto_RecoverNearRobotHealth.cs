using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_RecoverNearRobotHealth : Behaviour {
        private FloatData _recoverRatioData;
        private FloatData _recoverRangeData;
        private float deployTime;

        public Behaviour_Auto_RecoverNearRobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.RECOVER, LabelStr.RATIO), out _recoverRatioData);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.RECOVER, LabelStr.RANGE), out _recoverRangeData);
            Game.instance.OnUpdateEvent.AddListener(RecoverHealth);
        }

        private void RecoverHealth() {
            if (deployTime < 1) {
                deployTime += Time.deltaTime;
            } else {
                deployTime = 0;
                //激活触发器
                Vector3 robotBody = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position;
                Collider[] colliders = Physics.OverlapSphere(robotBody, _recoverRangeData.Float);
                foreach (var tmpCollider in colliders) {
                    if (EntityRegister.TryGetEntityByBodyPrefabID(tmpCollider.gameObject.GetInstanceID(), out Entity bodyEntity)) {
                        if (bodyEntity.ObjConfig.Type == "机器人") {
                            Cond.Instance.GetData(bodyEntity, LabelStr.HEALTH, out FloatData healthData);
                            MessageRegister.Instance.Dis(MessageCode.MsgRecoverHealth, bodyEntity.ID, healthData.Float * _recoverRatioData.Float);
                        }
                    }
                }
            }
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(RecoverHealth);
        }
    }
}