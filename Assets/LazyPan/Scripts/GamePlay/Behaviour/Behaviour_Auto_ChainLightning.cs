using System.Collections.Generic;
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
        private FloatData _attackRange;//攻击范围
        
        private FloatData _globalAttackSpeedRatio;//全局射击速度
        private FloatData _globalAttackRatio;//全局射击伤害

        private FloatData _towerEnergy;
        private StringData _shootSoundData;//射击音效

        private float attackIntervalDeploy;//攻击间隔雇佣
        private FloatData _fireRateInterval;

        public Behaviour_Auto_ChainLightning(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out _fireRateInterval);
            //攻击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RANGE), out _attackRange);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            //连击次数
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.COMBO, LabelStr.COUNT), out _numberOfCombos);
            //伤害基础值
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ATTACK, LabelStr.DAMAGE), out _attackDamage);
            //全局攻击伤害系数
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out _globalAttackRatio);
            //全局射击速率
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.SPEED, LabelStr.RATIO), out _globalAttackSpeedRatio);

            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            
            //射击音效
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.SOUND), out _shootSoundData);
            
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);

            attackIntervalDeploy = _fireRateInterval.Float * (1 / _globalAttackSpeedRatio.Float);
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
                attackIntervalDeploy = _fireRateInterval.Float * (1 / _globalAttackSpeedRatio.Float);;
            }
        }

        private void OnAttack() {
            int comboCount = _numberOfCombos.Int;
            if (comboCount <= 0) {
                return;
            }
            Debug.Log("闪电链攻击");
            Vector3 fromPos = _body.position;
            List<Entity> findRobot = new List<Entity>();
            if (EntityRegister.TryGetEntitiesWithinDistance("机器人", fromPos, _attackRange.Float, out List<Entity> robots)) {
                int[] ind;
                if (robots.Count > _numberOfCombos.Int) {
                    ind = MathUtil.Instance.GetRandNoRepeatIndex(comboCount, robots.Count);
                } else {
                    ind = MathUtil.Instance.GetRandNoRepeatIndex(robots.Count, robots.Count);
                }

                for (int i = 0; i < ind.Length; i++) {
                    Debug.Log("闪电链攻击 " + robots[ind[i]].Prefab.name + " :" + (_attackDamage.Float * _globalAttackRatio.Float));
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, robots[ind[i]].ID, _attackDamage.Float * _globalAttackRatio.Float);
                }

                Sound.Instance.SoundPlay(_shootSoundData.String, Vector3.zero, false, 2);
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}