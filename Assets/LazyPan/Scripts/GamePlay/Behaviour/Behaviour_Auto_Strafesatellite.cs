using System.Collections.Generic;
using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_Strafesatellite : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _body;//身体
        private Transform _towerFoot;//塔
        private IntData _level;
        private FloatData _fireRateInterval;//射击速率 射击时间间隔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
        private FloatData _fireRotateSpeed;//射击旋转速度
        private FloatData _towerEnergy;//塔能量
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private List<GameObject> _bullets = new List<GameObject>();

        private int _bulletInstanceIndex;
        private Transform _bulletParent;
        private Comp _bulletLevelFoot;
        private List<Transform> _bulletMuzzle = new List<Transform>();
        private bool _initBulletParent;
        private float fireRateIntervalDeploy;
        private GameObject _bulletTemplate;
        public Behaviour_Auto_Strafesatellite(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //冲锋枪根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹模板
            _bulletTemplate = Cond.Instance.Get<GameObject>(entity, LabelStr.BULLET);
            //子弹父物体 将子弹放于此处
            _bulletParent = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.PARENT));
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
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
            //等级
            Cond.Instance.GetData(entity, LabelStr.LEVEL, out _level);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            //旋转速度
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ROTATE, LabelStr.SPEED), out _fireRotateSpeed);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);

            fireRateIntervalDeploy = _fireRateInterval.Float;
            _bullets.Clear();
        }

        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf && _towerEnergy.Float > 0;
            if (!active) {
                _initBulletParent = false;
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

            BulletParentRotate();
            RoundShoot();
        }

        private void BulletParentRotate() {
            if (!_initBulletParent) {
                _bulletLevelFoot = Cond.Instance.Get<Comp>(entity,
                    LabelStr.Assemble(LabelStr.BULLET, LabelStr.FOOT, _level.Int.ToString()));

                _bulletMuzzle.Clear();
                foreach (var tmpMuzzle in _bulletLevelFoot.Transforms) {
                    if (tmpMuzzle.Sign.Equals("Muzzle")) {
                        _bulletMuzzle.Add(tmpMuzzle.Tran);
                    }
                }

                _bulletInstanceIndex = 0;
                _initBulletParent = true;
            }

            if (_bulletLevelFoot != null) {
                _bulletLevelFoot.transform.Rotate(Vector3.up, _fireRotateSpeed.Float * Time.deltaTime);
            }
        }

        private void RoundShoot() {
            if (_bulletMuzzle == null || _bulletMuzzle.Count == 0) {
                return;
            }

            if (fireRateIntervalDeploy > 0) {
                fireRateIntervalDeploy -= Time.deltaTime;
            } else {
                fireRateIntervalDeploy = _fireRateInterval.Float;

                if (_bulletInstanceIndex == _bulletMuzzle.Count) {
                    _bulletInstanceIndex = 0;
                }

                if (_bulletInstanceIndex < _bulletMuzzle.Count) {
                    Transform muzzle = _bulletMuzzle[_bulletInstanceIndex];
                    GameObject instanceBullet = FireParticleSystemBullet(muzzle);
                    _bullets.Add(instanceBullet);
                    Comp comp = instanceBullet.GetComponent<Comp>();
                    comp.OnParticleCollisionEvent.RemoveAllListeners();
                    comp.OnParticleCollisionEvent.AddListener(OnParticleCollisionEvent);
                    _bulletInstanceIndex++;
                }
            }
        }

        private GameObject FireParticleSystemBullet(Transform muzzle) {
            //创建子弹
            GameObject bulletGameObject = Object.Instantiate(_bulletTemplate, muzzle.position, Quaternion.identity, _bulletParent);
            bulletGameObject.SetActive(true);
            bulletGameObject.transform.position = muzzle.position;
            bulletGameObject.transform.forward = muzzle.forward;
            return bulletGameObject;
        }

        private void OnParticleCollisionEvent(GameObject arg0, GameObject fxGo) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "机器人") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
                    fxGo.SetActive(false);
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