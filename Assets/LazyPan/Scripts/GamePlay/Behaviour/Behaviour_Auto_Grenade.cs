using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Grenade : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _body;//身体
        private Transform _muzzle;//枪口
        private Transform _bulletFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private BoolData _burn;
        private BoolData _frost;
        private FloatData _fireRateInterval;//射击速率 射击时间间隔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
        private FloatData _BoomRange;//爆炸范围
        private FloatData _BoomDuringTime;//总持续时间
        private FloatData _towerEnergy;//塔能量
        private List<GameObject> _bullets = new List<GameObject>();

        private bool _isPrepareBoom;
        private bool _isBoom;
        private Vector3 _shotBoomDir;
        private Vector3 _shotBoomPoint;
        private FloatData _bulletMoveSpeed;
        private GameObject _bulletInstance;
        
        private float fireRateIntervalDeploy;
        private GameObject bulletTemplate;
        private LineRenderer _fireRangeLineRenderer;//范围图片

        private List<BurnAreaData> _burnAreaData = new List<BurnAreaData>();
        private List<FrostAreaData> _frostAreaData = new List<FrostAreaData>();

        public Behaviour_Auto_Grenade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //冲锋枪根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _bulletFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.FOOT));
            //子弹模板
            bulletTemplate = Cond.Instance.Get<GameObject>(entity, LabelStr.BULLET);
            //身体
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //燃烧
            Cond.Instance.GetData(entity, LabelStr.BURN, out _burn);
            //冰霜
            Cond.Instance.GetData(entity, LabelStr.FROST, out _frost);
            //枪口
            _muzzle = Cond.Instance.Get<Transform>(entity, LabelStr.MUZZLE);
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            //获取射击速率
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RATE, LabelStr.INTERVAL),
                out _fireRateInterval);
            //子弹移动速度
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.MOVE, LabelStr.SPEED),
                out _bulletMoveSpeed);
            //获取射击伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE),
                out _fireDamage);
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE),
                out _fireRange);
            //持续时间
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.DURATION, LabelStr.TIME), out _BoomDuringTime);
            //爆炸范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BOOM, LabelStr.RANGE), out _BoomRange);
            //范围
            _fireRangeLineRenderer = Cond.Instance.Get<LineRenderer>(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE));
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            fireRateIntervalDeploy = _fireRateInterval.Float;
            _bullets.Clear();
        }
        
        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            return entity.Prefab.activeSelf && _towerEnergy.Float > 0;
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

            float delta = Time.deltaTime;
            ShotGrenade();
            BoomCircleRange();
            BurnFrostAreaUpdate(delta);
        }

        private void ShotGrenade() {
            //射击频率
            if (fireRateIntervalDeploy > 0) {
                fireRateIntervalDeploy -= Time.deltaTime;
            } else {
                //第一阶段发射中
                if (_isPrepareBoom) {
                    if (!_isBoom) {
                        _bulletInstance.transform.position = Vector3.Lerp(_bulletInstance.transform.position,
                            _shotBoomPoint, Time.deltaTime * _bulletMoveSpeed.Float);

                        if (Vector3.Distance(_bulletInstance.transform.position, _shotBoomPoint) < 0.5f) {
                            _isBoom = true;
                        }
                    }
                } else {
                    //发射方向 有敌人朝向敌人 没敌人 朝向随机方向
                    _shotBoomDir = Random.onUnitSphere;
                    _shotBoomDir.y = 0;

                    //目标点范围显示
                    _shotBoomPoint = _shotBoomDir * _fireRange.Float;

                    //创建子弹
                    _bulletInstance = Object.Instantiate(bulletTemplate, _muzzle.position, Quaternion.Euler(_shotBoomDir), _bulletFoot);
                    _bulletInstance.SetActive(true);

                    _fireRangeLineRenderer.gameObject.SetActive(true);
                    _isPrepareBoom = true;
                }

                //第二阶段爆炸
                if (_isBoom) {
                    //激活触发器
                    Collider[] colliders = Physics.OverlapSphere(_shotBoomPoint, _BoomRange.Float);
                    foreach (var tmpCollider in colliders) {
                        OnTriggerEnter(tmpCollider);
                    }

                    //激活爆炸特效
                    //完全结束后延时
                    ClockUtil.Instance.AlarmAfter(0.1f, () => {
                        //销毁弹药
                        GameObject.Destroy(_bulletInstance);
                        _fireRangeLineRenderer.gameObject.SetActive(false);
                        _isBoom = false;
                        _isPrepareBoom = false;
                        //爆炸后开始延时
                        fireRateIntervalDeploy = _fireRateInterval.Float;
                    });

                    if (_burn.Bool) {
                        //燃烧区域
                        BurnArea(_shotBoomPoint);
                    }

                    if (_frost.Bool) {
                        //冰冻区域
                        FrostArea(_shotBoomPoint);
                    }
                }
            }
        }

        private void BurnArea(Vector3 point) {
            BurnAreaData instanceBurn = new BurnAreaData();
            instanceBurn.OnInit(_BoomDuringTime.Float, 1, point, _BoomRange.Float);
            _burnAreaData.Add(instanceBurn);
        }

        private void FrostArea(Vector3 point) {
            FrostAreaData instanceFrost = new FrostAreaData();
            instanceFrost.OnInit(_BoomDuringTime.Float, 1, point, _BoomRange.Float);
            _frostAreaData.Add(instanceFrost);
        }

        private void BurnFrostAreaUpdate(float delta) {
            for (int i = 0; i < _burnAreaData.Count; i++) {
                BurnAreaData tmpData = _burnAreaData[i];
                tmpData.OnUpdate(delta);
            }

            for (int i = _burnAreaData.Count; i >= 0 ; i--) {
                BurnAreaData tmpData = _burnAreaData[i];
                if (tmpData.BurnDeploy == -1) {
                    _burnAreaData.RemoveAt(i);
                }
            }

            for (int i = 0; i < _frostAreaData.Count; i++) {
                FrostAreaData tmpData = _frostAreaData[i];
                tmpData.OnUpdate(delta);
            }

            for (int i = _frostAreaData.Count; i >= 0 ; i--) {
                FrostAreaData tmpData = _frostAreaData[i];
                if (tmpData.FrostDeploy == -1) {
                    _frostAreaData.RemoveAt(i);
                }
            }
        }

        private void BoomCircleRange() {
            if (_fireRangeLineRenderer.gameObject.activeSelf) {
                MyMathUtil.CircleLineRenderer(_fireRangeLineRenderer, _shotBoomPoint, _BoomRange.Float, 30, 1);
            } else {
                MyMathUtil.ClearCircleRenderer(_fireRangeLineRenderer);
            }
        }

        private void OnTriggerEnter(Collider collider) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "机器人") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
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

    public class BurnAreaData {
        public float BurnDeploy;
        public float BurnIntervalDeploy;
        public float BurnInterval;//燃烧间隔
        public Vector3 Position;//爆炸位置
        public float BoomRange;//爆炸范围

        public void OnInit(float burnTime, float burnInterval, Vector3 point, float boomRange) {
            BurnInterval = burnInterval;
            Position = point;
            BoomRange = boomRange;
            BurnIntervalDeploy = BurnInterval;
            BurnDeploy = burnTime;
        }

        public void OnUpdate(float delta) {
            if (BurnDeploy > 0) {
                BurnDeploy -= delta;
                if (BurnIntervalDeploy < BurnInterval) {
                    BurnIntervalDeploy += delta;
                } else {
                    BurnEvent(Position, BoomRange);
                    BurnIntervalDeploy = 0;
                }
            } else {
                BurnDeploy = -1;
            }
        }

        private void BurnEvent(Vector3 point, float range) {
            string[] types = new[] { "机器人", "Player" };
            foreach (var tmpType in types) {
                //扫描范围内的敌人和玩家 造成燃烧效果
                if (EntityRegister.TryGetEntitiesWithinDistance(tmpType, point, range, out List<Entity> entities)) {
                    foreach (var tmpEntity in entities) {
                        MessageRegister.Instance.Dis(MessageCode.MsgBurnEntity, tmpEntity.ID);
                    }
                }
            }
        }
    }

    public class FrostAreaData {
        public float FrostDeploy;
        public float FrostIntervalDeploy;
        public float FrostInterval;//冰霜间隔
        public Vector3 Position;//爆炸位置
        public float BoomRange;//爆炸范围

        public void OnInit(float frostTime, float frostInterval, Vector3 point, float boomRange) {
            FrostInterval = frostInterval;
            Position = point;
            BoomRange = boomRange;
            FrostIntervalDeploy = frostInterval;
            FrostDeploy = frostTime;
        }

        public void OnUpdate(float delta) {
            if (FrostDeploy > 0) {
                FrostDeploy -= delta;
                if (FrostIntervalDeploy < FrostInterval) {
                    FrostIntervalDeploy += delta;
                } else {
                    FrostEvent(Position, BoomRange);
                    FrostIntervalDeploy = 0;
                }
            } else {
                FrostDeploy = -1;
            }
        }

        private void FrostEvent(Vector3 point, float range) {
            string[] types = new[] { "机器人", "Player" };
            foreach (var tmpType in types) {
                //扫描范围内的敌人和玩家 造成效果
                if (EntityRegister.TryGetEntitiesWithinDistance(tmpType, point, range, out List<Entity> entities)) {
                    foreach (var tmpEntity in entities) {
                        MessageRegister.Instance.Dis(MessageCode.MsgFrostEntity, tmpEntity.ID);
                    }
                }
            }
        }
    }
}