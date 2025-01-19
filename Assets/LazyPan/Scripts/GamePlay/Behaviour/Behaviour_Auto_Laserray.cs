using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Laserray : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _body;//身体
        private Transform _muzzle;//枪口
        private Transform _bulletFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private FloatData _fireRateInterval;//射击速率 射击时间间隔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
        private IntData _fireCount;
        private BoolData _effectAttackData;
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private FloatData _towerEnergy;
        private List<GameObject> _bullets = new List<GameObject>();

        private float fireRateIntervalDeploy;
        private StringData bulletData;
        private LineRenderer _fireRangeLineRenderer;//范围图片

        public Behaviour_Auto_Laserray(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //冲锋枪根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _bulletFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.FOOT));
            //子弹标识
            Cond.Instance.GetData(entity, LabelStr.BULLET, out bulletData);
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //枪口
            _muzzle = Cond.Instance.Get<Transform>(entity, LabelStr.MUZZLE);
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL),
                out _fireRateInterval);
            //获取射击伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE),
                out _fireDamage);
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE),
                out _fireRange);
            //效果攻击
            Cond.Instance.GetData(base.entity, LabelStr.Assemble(LabelStr.EFFECT, LabelStr.ATTACK),
                out _effectAttackData);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            //范围
            _fireRangeLineRenderer = Cond.Instance.Get<LineRenderer>(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE));
            MyMathUtil.ClearCircleRenderer(_fireRangeLineRenderer);
            //射击个数
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.COUNT), out _fireCount);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            fireRateIntervalDeploy = _fireRateInterval.Float;
            _bullets.Clear();
        }

        public override void DelayedExecute() {
            
        }

        private GameObject GetBullet() {
            return Cond.Instance.Get<GameObject>(entity, bulletData.String);
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
            if (EntityRegister.TryGetEntitiesWithinDistance("机器人", _foot.position,
                    _fireRange.Float, out List<Entity> entities)) {
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
                    fireRateIntervalDeploy = _fireRateInterval.Float;
                    ShotLaser();
                }
            }
        }

        private void ShotLaser() {
            //找到目前的敌人
            int instanceLaserNum = _fireCount.Int;
            //获取所有机器人
            bool findRobotEntity = EntityRegister.TryGetEntitiesByType("机器人", out List<Entity> robotEntities);
            if (findRobotEntity) {
                int entityNum = robotEntities.Count;
                //机器人数量大于等于开火数量
                if (entityNum >= instanceLaserNum) {
                    int[] robotIndex = MathUtil.Instance.GetRandNoRepeatIndex(entityNum, instanceLaserNum);
                    foreach (var tmpIndex in robotIndex) {
                        CreateLaser(robotEntities[tmpIndex]);
                    }
                }
            }
        }

        private void CreateLaser(Entity robotEntity) {
            GameObject bulletGameObject =
                Object.Instantiate(GetBullet(), _muzzle.position, Quaternion.identity, _bulletFoot);
            bulletGameObject.SetActive(true);
            bulletGameObject.transform.position = _muzzle.position;
            Vector3 direction = (Cond.Instance.Get<Transform>(robotEntity, LabelStr.BODY).position -
                                 bulletGameObject.transform.position).normalized;
            direction.y = 0;
            _body.forward = direction;
            bulletGameObject.transform.forward = _body.forward;
            
            Comp comp = bulletGameObject.GetComponent<Comp>();

            Cond.Instance.Get<LineRenderer>(comp, LabelStr.LINE).widthMultiplier *= 4;

            comp.OnTriggerEnterEvent.RemoveAllListeners();
            comp.OnTriggerEnterEvent.AddListener(OnTriggerEvent);
            ClockUtil.Instance.AlarmAfter(0.2f, () => {
                if (bulletGameObject != null) {
                    _bullets.Remove(bulletGameObject);
                    Object.Destroy(bulletGameObject);
                    bulletGameObject = null;
                }
            });
        }

        private void OnTriggerEvent(Collider collider) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "机器人") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
                    //效果攻击
                    if (_effectAttackData.Bool) {
                        float rand = Random.Range(-1f, 1f);
                        if (rand > 0) {
                            MessageRegister.Instance.Dis(MessageCode.MsgBurnEntity, bodyEntity.ID);
                        } else {
                            MessageRegister.Instance.Dis(MessageCode.MsgFrostEntity, bodyEntity.ID);
                        }
                    }
                }
            }
        }

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