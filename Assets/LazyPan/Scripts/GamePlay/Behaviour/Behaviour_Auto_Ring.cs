using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_Ring : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _ballFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private FloatData _fireDamage;//射击伤害
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private List<GameObject> _balls = new List<GameObject>();

        private FloatData _waveSpeed;
        private IntData _surroundCount;
        private GameObject _ball1;
        private GameObject _ball2;
        private GameObject _ball3;
        private float waveDeploy;
        private int surroundCount;
        private bool isPlay;

        public Behaviour_Auto_Ring(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
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
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.SPEED), out _waveSpeed);
            //获取环绕数量
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.COUNT), out _surroundCount);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            _balls.Clear();
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf;
            //RefreshBall(active);
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
            RingWave();
        }

        private void RingWave() {
            if (_ballFoot != null) {
                if (isPlay) {
                    _ballFoot.localScale *= _waveSpeed.Float;
                    if (_ballFoot.localScale == Vector3.one * 5) {
                        Color c = _ballFoot.GetComponent<Image>().color;
                        c.a = 0;
                        _ballFoot.GetComponent<Image>().color = c;
                        isPlay = false;
                        waveDeploy = 3;
                    }
                } else {
                    if (waveDeploy > 0) {
                        waveDeploy -= Time.deltaTime;
                    } else {
                        isPlay = true;
                    }
                }
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