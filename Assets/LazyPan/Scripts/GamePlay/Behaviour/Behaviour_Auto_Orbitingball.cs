using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Orbitingball : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _ballFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private FloatData _fireDamage;//射击伤害
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private List<GameObject> _balls = new List<GameObject>();
        private FloatData _surroundSpeed;
        private IntData _surroundCount;
        private FloatData _towerEnergy;//塔能量
        private GameObject _ball1;
        private GameObject _ball2;
        private GameObject _ball3;
        private int surroundCount;

        public Behaviour_Auto_Orbitingball(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _ballFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.BALL, LabelStr.FOOT));
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            //获取伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE), out _fireDamage);
            //获取环绕速度
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.SPEED), out _surroundSpeed);
            //获取环绕数量
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.COUNT), out _surroundCount);
            //塔能量
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            _balls.Clear();
        }
        
        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf && _towerEnergy.Float > 0;
            RefreshBall(active);
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
            BallRotate();
        }

        private void BallRotate() {
            if (_ballFoot != null) {
                _ballFoot.Rotate(Vector3.up * Time.deltaTime * _surroundSpeed.Float);
            }
        }

        private void RefreshBall(bool active) {
            if (active) {
                _ballFoot.gameObject.SetActive(true);
                if (surroundCount != _surroundCount.Int) {
                    int count = _surroundCount.Int;

                    while (count > 0) {
                        GameObject tmpBall = Cond.Instance.Get<GameObject>(entity, LabelStr.Assemble(LabelStr.BALL, count.ToString()));
                        tmpBall.SetActive(true);
                        
                        Comp comp = tmpBall.GetComponent<Comp>();
                        comp.OnTriggerEnterEvent.RemoveAllListeners();
                        comp.OnTriggerEnterEvent.AddListener(OnTriggerEnterEvent);
                        
                        count--;
                    }

                    surroundCount = _surroundCount.Int;
                }
            } else {
                _ballFoot.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnterEvent(Collider collider) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(collider.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "Robot") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
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