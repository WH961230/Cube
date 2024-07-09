using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public class Behaviour_Auto_SubmachineGun : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _bulletFoot;//子弹根节点
        private Transform _body;//身体
        private Transform _muzzle;//枪口
        private Transform _towerFoot;//塔
        private FloatData _fireRateInterval;//射击速率 射击时间间隔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体

        private float fireRateIntervalDeploy;
        private GameObject bulletTemplate;
        public Behaviour_Auto_SubmachineGun(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Debug.Log("冲锋枪");
            //冲锋枪根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _bulletFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.FOOT));
            //子弹模板
            bulletTemplate = Cond.Instance.Get<GameObject>(entity, LabelStr.BULLET);
            //冲锋枪
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
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            fireRateIntervalDeploy = _fireRateInterval.Float;
        }

        private void OnLateUpdate() {
            if (_foot != null && _towerFoot != null) {
                _foot.position = _towerFoot.position;
                if (_targetInRangeRobotEntity != null) {
                    Vector3 aimDir = Cond.Instance.Get<Transform>(_targetInRangeRobotEntity, LabelStr.BODY).position -
                                     _foot.position;
                    aimDir.y = 0;
                    _foot.forward = aimDir;
                }
            }
        }

        private void OnUpdate() {
            if (EntityRegister.TryGetEntitiesWithinDistance("Robot", _foot.position,
                _fireRange.Float, out List<Entity> entities)) {
                _targetInRangeRobotEntity = entities[Random.Range(0, entities.Count)];
            } else {
                _targetInRangeRobotEntity = null;
            }

            if (_targetInRangeRobotEntity != null) {
                if (fireRateIntervalDeploy > 0) {
                    fireRateIntervalDeploy -= Time.deltaTime;
                } else {
                    fireRateIntervalDeploy = _fireRateInterval.Float;
                    GameObject instanceBullet = Object.Instantiate(bulletTemplate, _bulletFoot);
                    //位置
                    instanceBullet.transform.position = _muzzle.position;
                    //方向
                    Vector3 aimDir = Cond.Instance.Get<Transform>(_targetInRangeRobotEntity, LabelStr.BODY).position - _foot.position;
                    aimDir.y = 0;
                    instanceBullet.transform.forward = aimDir.normalized;

                    Comp comp = instanceBullet.GetComponent<Comp>();
                    comp.OnParticleCollisionEvent.AddListener(OnParticleCollisionEvent);
                    instanceBullet.SetActive(true);
                }
            }
        }

        private void OnParticleCollisionEvent(GameObject arg0, GameObject fxGo) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.GetInstanceID(), out Entity entity)) {
                if (entity.ObjConfig.Type == "Robot") {
                    Debug.Log("获取机器人");
                    fxGo.SetActive(false);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Game.instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}