using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LazyPan {
    public class Behaviour_Auto_SubmachineGun : Behaviour {
        private float fireRateIntervalDeploy;
        private Transform _foot;//冲锋枪
        private Transform _body;//身体
        private Transform _muzzle;//枪口
        private Transform _bulletFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private FloatData _fireRateInterval;//射击速率 射击时间间隔
        private FloatData _globalAttackSpeedRatio;//全局射击速度
        private FloatData _fireDamage;//射击伤害
        private FloatData _globalAttackRatio;//全局射击伤害
        private FloatData _fireRange;//射击范围
        private FloatData _towerEnergy;//塔能量
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private List<GameObject> _bullets = new List<GameObject>();
        private GameObject bulletTemplate;
        private LineRenderer _fireRangeLineRenderer;//范围图片
        
        public Behaviour_Auto_SubmachineGun(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //冲锋枪根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _bulletFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.FOOT));
            //子弹模板
            bulletTemplate = Cond.Instance.Get<GameObject>(entity, LabelStr.BULLET);
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //枪口
            _muzzle = Cond.Instance.Get<Transform>(entity, LabelStr.MUZZLE);
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            //全局攻击伤害系数
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out _globalAttackRatio);
            //全局射击速率
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.SPEED, LabelStr.RATIO), out _globalAttackSpeedRatio);
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL), out _fireRateInterval);
            //获取射击伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE), out _fireDamage);
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out _fireRange);
            _fireRangeLineRenderer = Cond.Instance.Get<LineRenderer>(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE));
            MyMathUtil.ClearCircleRenderer(_fireRangeLineRenderer);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);

            fireRateIntervalDeploy = _fireRateInterval.Float * (1 / _globalAttackSpeedRatio.Float);
            _bullets.Clear();
        }

        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf && _towerEnergy.Float > 0;
            _fireRangeLineRenderer.gameObject.SetActive(active);
            if (active) {
                MyMathUtil.CircleLineRenderer(_fireRangeLineRenderer, _foot.position, _fireRange.Float, 200);
            } else {
                MyMathUtil.ClearCircleRenderer(_fireRangeLineRenderer);
            }

            return active;
        }

        private void OnLateUpdate() {
            if (!IsActive()) {
                return;
            }

            SetToTowerPoint();
        }

        private void SetToTowerPoint() {
            if (_foot != null && _towerFoot != null) {
                _foot.position = _towerFoot.position;
            }
        }

        private void OnUpdate() {
            if (!IsActive()) {
                return;
            }
            GetWithinDistanceEntity();
            ShotBulletToEnemy();
        }

        private void GetWithinDistanceEntity() {
            if (EntityRegister.TryGetEntitiesWithinDistance("机器人", _foot.position, _fireRange.Float, out List<Entity> entities)) {
                _targetInRangeRobotEntity = entities[Random.Range(0, entities.Count)];
            } else {
                _targetInRangeRobotEntity = null;
            }
        }

        private void ShotBulletToEnemy() {
            if (_targetInRangeRobotEntity != null) {
                if (fireRateIntervalDeploy > 0) {
                    fireRateIntervalDeploy -= Time.deltaTime;
                } else {
                    fireRateIntervalDeploy = _fireRateInterval.Float * (1 / _globalAttackSpeedRatio.Float);

                    GameObject instanceBullet = FireParticleSystemBullet();
                    _bullets.Add(instanceBullet);
                    Comp comp = instanceBullet.GetComponent<Comp>();
                    comp.OnParticleCollisionEvent.RemoveAllListeners();
                    comp.OnParticleCollisionEvent.AddListener(OnParticleCollisionEvent);
                }
            }
        }

        private GameObject FireParticleSystemBullet() {
            Transform targetRobot = Cond.Instance.Get<Transform>(_targetInRangeRobotEntity, LabelStr.BODY);

            //创建子弹
            GameObject bulletGameObject =
                Object.Instantiate(bulletTemplate, _muzzle.position, Quaternion.identity, _bulletFoot);
            bulletGameObject.SetActive(true);

            ParticleSystem bulletInstance = bulletGameObject.GetComponent<ParticleSystem>();

            //计算预瞄敌人
            NavMeshAgent targetRobotNavMeshAgent = Cond.Instance.Get<NavMeshAgent>(_targetInRangeRobotEntity, LabelStr.NAVMESHAGENT);
            if (ComputeDirection(targetRobot.position, _foot.position, targetRobotNavMeshAgent.velocity, bulletInstance.startSpeed, out Vector3 result)) {
                result.y = 0;
                _body.forward = result;
            } else {
                _body.forward = (targetRobot.position - _foot.position).normalized;
            }

            bulletGameObject.transform.position = _muzzle.position;
            bulletGameObject.transform.forward = _body.forward;
            return bulletGameObject;
        }

        private void OnParticleCollisionEvent(GameObject arg0, GameObject fxGo) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "机器人") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float * _globalAttackRatio.Float);
                    fxGo.SetActive(false);
                }
            }
        }

        #region Math

        private bool ComputeDirection(Vector3 targetDir, Vector3 bulletStartPoint, Vector3 vA, float speed, out Vector3 result) {
            var aTob = bulletStartPoint - targetDir;
            var dc = aTob.magnitude;
            var alpha = Vector3.Angle(aTob, vA) * Mathf.Deg2Rad;
            var sA = vA.magnitude;
            var r = sA / speed;
            if (MyMathUtil.ComputeRoot((1 - r * r), 2 * dc * r * Mathf.Cos(alpha), -dc * dc, out float root1, out float root2) == 0) {
                result = default;
                return false;
            }

            var dA = Mathf.Max(root1, root2);
            var t = dA / speed;
            var c = targetDir + t * vA;
            result = (c - bulletStartPoint).normalized;
            return true;
        }

        #endregion

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Game.instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
            foreach (var bullet in _bullets) {
                GameObject.Destroy(bullet);
            }
        }
    }
}