using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ChainLightning : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _body;//身体
        private FloatData _detectDistance;//检测距离
        private IntData _numberOfCombos;//连击次数
        private FloatData _attackDamage;//伤害基础值
        private FloatData _attackAttenuationRatio;//伤害递减系数
        private FloatData _attackInterval;//攻击间隔

        private FloatData _towerEnergy;

        private float attackIntervalDeploy;//攻击间隔雇佣
        private FloatData _fireRateInterval;

        public Behaviour_Auto_ChainLightning(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out _fireRateInterval);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
        }
        
        private bool IsActive() {
            bool active = entity.Prefab.activeSelf && _towerEnergy.Float > 0;
            return active;
        }

        private void OnUpdate() {
            if (!IsActive()) {
                return;
            }
            if (attackIntervalDeploy > 0) {
                attackIntervalDeploy -= Time.deltaTime;
            } else {
                OnAttack();
                attackIntervalDeploy = _fireRateInterval.Float;
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