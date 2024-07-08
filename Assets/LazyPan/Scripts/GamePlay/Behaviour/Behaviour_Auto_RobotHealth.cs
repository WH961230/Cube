using Unity.VisualScripting;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_RobotHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private FloatData _maxHealthData;
        private FloatData _healthData;

        public Behaviour_Auto_RobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Comp triggerComp = Cond.Instance.Get<Comp>(entity, LabelStr.TRIGGER);
            triggerComp.OnTriggerEnterEvent.AddListener(OnTriggerEnter);
        }

        private void OnTriggerEnter(Collider arg0) {
            if (arg0.gameObject.layer == LayerMask.GetMask("Player")) {
                Debug.Log("玩家");
            }
        }

        private void BeDamaged(float damageValue) {
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

        private void Dead() {
            Obj.Instance.UnLoadEntity(entity);
        }

        public override void Clear() {
            base.Clear();
            Comp triggerComp = Cond.Instance.Get<Comp>(entity, LabelStr.TRIGGER);
            triggerComp.OnTriggerEnterEvent.RemoveListener(OnTriggerEnter);
        }
    }
}