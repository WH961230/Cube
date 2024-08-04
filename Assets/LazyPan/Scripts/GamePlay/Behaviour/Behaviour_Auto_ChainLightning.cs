using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ChainLightning : Behaviour {
        private FloatData _detectDistance;//检测距离
        private IntData _numberOfCombos;//连击次数
        private FloatData _attackDamage;//伤害基础值
        private FloatData _attackAttenuationRatio;//伤害递减系数
        private FloatData _attackInterval;//攻击间隔

        private float attackIntervalDeploy;//攻击间隔雇佣

        public Behaviour_Auto_ChainLightning(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
        }

        private void OnUpdate() {
            if (attackIntervalDeploy > 0) {
                attackIntervalDeploy -= Time.deltaTime;
            } else {
                OnAttack();
            }
        }

        private void OnAttack() {
            Debug.Log("闪电链攻击");
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}