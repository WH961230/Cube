using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_Ring : Behaviour {
        private float waveDeploy;
        private int surroundCount;
        private bool isPlay;
        private Transform _foot;//冲锋枪
        private GameObject _ringBullet;//子弹
        private Material _ringBulletMat;
        private Transform _towerFoot;//塔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
        private Entity _targetInRangeRobotEntity;//目标范围内机器人实体
        private FloatData _towerEnergy;//塔能量
        private FloatData _waveSpeed;
        private IntData _surroundCount;
        private GameObject _ball1;
        private GameObject _ball2;
        private GameObject _ball3;
        private List<GameObject> _balls = new List<GameObject>();
        //圆环配置
        public FloatData _startRadius; // 初始半径
        public FloatData _pulseDuration; // 扩散时间
        public FloatData _delayBetweenPulses; // 延迟时间
        private float pulseTimer = -1;
        private float delayTimer = -1;

        private Comp _bulletTriggerComp;
        private LineRenderer _fireRangeLineRenderer;//范围渲染

        public Behaviour_Auto_Ring(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹
            _ringBullet = Cond.Instance.Get<GameObject>(entity, LabelStr.BULLET);
            //子弹触发
            _bulletTriggerComp = Cond.Instance.Get<Comp>(entity, LabelStr.Assemble(LabelStr.BULLET, LabelStr.TRIGGER));
            _bulletTriggerComp.OnTriggerEnterEvent.AddListener(OnTriggerEnterEvent);
            //材质球
            _ringBulletMat = _ringBullet.GetComponent<MeshRenderer>().material;
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            //塔能量
            Cond.Instance.GetData(_tower, LabelStr.ENERGY, out _towerEnergy);
            //参数
            //获取伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE), out _fireDamage);
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out _fireRange);
            //获取初始半径
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.START, LabelStr.RADIU), out _startRadius);
            //获取扩散时间
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.PULSE, LabelStr.DURATION), out _pulseDuration);
            //获取延迟时间
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DELAY, LabelStr.PULSE), out _delayBetweenPulses);
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            _balls.Clear();
        }
        
        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf && _towerEnergy.Float > 0;
            _ringBullet.SetActive(active);
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
            OnUpdateRingPulse();
        }

        private void OnTriggerEnterEvent(Collider collider) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(collider.gameObject.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "Robot") {
                    Debug.Log("圆环攻击敌人" + _fireDamage.Float);
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
                }
            }
        }

        #region 圆环

        void OnUpdateRingPulse() {
            //扩散
            if (pulseTimer > 0) {
                pulseTimer -= Time.deltaTime;

                // 计算当前的扩散进度（0到1之间）
                float progress = Mathf.Clamp01(1 - pulseTimer / _pulseDuration.Float);

                // 计算当前的半径和透明度
                float currentRadius = Mathf.Lerp(_startRadius.Float, _fireRange.Float, progress);
                float currentAlpha = Mathf.Lerp(1f, 0f, progress);

                // 更新圆环的位置和透明度
                Color color = _ringBulletMat.color;
                color.a = currentAlpha;
                _ringBulletMat.color = color;
                
                //圆环扩散
                _ringBullet.transform.localScale = Vector3.one * currentRadius * 2;
                //圆环碰撞体扩散
                _bulletTriggerComp.transform.localScale = Vector3.one * currentRadius * 2;
            } else {
                if (pulseTimer != 0) {
                    delayTimer = _delayBetweenPulses.Float;
                    pulseTimer = 0;
                    _ringBullet.transform.localScale = Vector3.one;
                    _bulletTriggerComp.transform.localScale = Vector3.one;
                    _bulletTriggerComp.gameObject.SetActive(false);
                    Color color = _ringBulletMat.color;
                    color.a = 1;
                    _ringBulletMat.color = color;
                }
            }

            //延迟
            if (delayTimer > 0) {
                delayTimer -= Time.deltaTime;
            } else {
                if (delayTimer != 0) {
                    pulseTimer = _pulseDuration.Float;
                    _bulletTriggerComp.gameObject.SetActive(true);
                    delayTimer = 0;
                }
            }
        }
        
        #endregion

        public override void Clear() {
            base.Clear();
            _bulletTriggerComp.OnTriggerEnterEvent.RemoveListener(OnTriggerEnterEvent);
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Game.instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}