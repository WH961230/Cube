using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_Ring : Behaviour {
        private Transform _foot;//冲锋枪
        private Transform _ringFoot;//子弹根节点
        private Transform _towerFoot;//塔
        private FloatData _fireDamage;//射击伤害
        private FloatData _fireRange;//射击范围
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
        
        private LineRenderer _fireRangeLineRenderer;//范围图片

        public Behaviour_Auto_Ring(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //根源
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            //子弹根节点
            _ringFoot = Cond.Instance.Get<Transform>(entity, LabelStr.Assemble(LabelStr.RING, LabelStr.FOOT));
            //获取塔 跟随塔的位置
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            
            //参数
            //获取伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE), out _fireDamage);
            //获取射击范围
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE), out _fireRange);

            //范围
            _fireRangeLineRenderer = Cond.Instance.Get<LineRenderer>(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.RANGE));
            MyMathUtil.ClearCircleRenderer(_fireRangeLineRenderer);
            
            _fireRangeLineRenderer.positionCount = numSegments + 1;
            _fireRangeLineRenderer.loop = true; // 使 LineRenderer 成为闭环圆环
            timer = 0f;
            
            //更新
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            
            _balls.Clear();
        }
        
        public override void DelayedExecute() {
            
        }

        private bool IsActive() {
            bool active = entity.Prefab.activeSelf;
            _fireRangeLineRenderer.gameObject.SetActive(active);
            if (active) {
                MyMathUtil.CircleLineRenderer(_fireRangeLineRenderer, _foot.position + new Vector3(0, 0, 1f), _fireRange.Float, 50);
                //RefreshBall(active);
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
            OnUpdateRingPulse();
        }

        private void OnTriggerEnterEvent(Collider collider) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(collider.GetInstanceID(), out Entity bodyEntity)) {
                if (bodyEntity.ObjConfig.Type == "Robot") {
                    MessageRegister.Instance.Dis(MessageCode.MsgDamageRobot, bodyEntity.ID, _fireDamage.Float);
                }
            }
        }

        #region 圆环

        public float startRadius = 0.1f; // 初始半径
        public float endRadius = 1.0f; // 最大半径
        public float pulseDuration = 2.0f; // 扩散时间
        public float delayBetweenPulses = 1.0f; // 延迟时间
        public int numSegments = 100; // 圆环的分段数
        private float timer;

        void OnUpdateRingPulse() {
            timer += Time.deltaTime;

            // 计算当前的扩散进度（0到1之间）
            float progress = Mathf.Clamp01(timer / pulseDuration);

            // 计算当前的半径和透明度
            float currentRadius = Mathf.Lerp(startRadius, _fireRange.Float, progress);
            float currentAlpha = Mathf.Lerp(1f, 0f, progress);

            // 更新圆环的位置和透明度
            UpdateCircle(currentRadius, currentAlpha);

            // 如果完成了一个扩散周期，重置计时器
            if (progress >= 1f) {
                timer = 0f;
                // 可选：等待一段时间再开始下一个扩散周期
                ClockUtil.Instance.AlarmAfter(delayBetweenPulses, () => {
                    // 重置计时器，开始下一个扩散周期
                    timer = 0f;
                });
            }
        }

        void UpdateCircle(float radius, float alpha) {
            float angleStep = 360f / numSegments;

            for (int i = 0; i <= numSegments; i++) {
                float angle = i * angleStep;
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                _fireRangeLineRenderer.SetPosition(i, new Vector3(x, 0f, z));
            }

            Color color = _fireRangeLineRenderer.startColor;
            color.a = alpha; // 设置透明度
            _fireRangeLineRenderer.startColor = color;
            _fireRangeLineRenderer.endColor = color;
        }

        #endregion

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            Game.instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}