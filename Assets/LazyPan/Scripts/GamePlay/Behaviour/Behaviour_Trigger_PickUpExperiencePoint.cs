using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_PickUpExperiencePoint : Behaviour {
        private BoolData _movePlayer;
        private FloatData _moveSpeed;
        private FloatData _getMoveSpeed;
        private FloatData _pickDis;
        private FloatData _getPickDis;
        private FloatData _pickMoveSpeed;
        private FloatData _pickIncreaseMoveSpeed;
        private FloatData _destroyDelayTime;
        private Transform _body;
        private float moveSpeedDeploy;
        public Behaviour_Trigger_PickUpExperiencePoint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnTriggerEnter);
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            //是否开始移动到玩家
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), Label.Assemble(LabelStr.EXPERIENCE, LabelStr.MOVE, LabelStr.PLAYER), out _movePlayer);
            //移动速度
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.MOVE, LabelStr.SPEED), out _moveSpeed);
            //获取移动速度
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.GET, LabelStr.MOVE, LabelStr.SPEED), out _getMoveSpeed);
            //拾取距离
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), Label.Assemble(LabelStr.PICK, LabelStr.DISTANCE), out _pickDis);
            //获取距离
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.GET, LabelStr.PICK, LabelStr.DISTANCE), out _getPickDis);
            //拾取增加速度总和
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.PICK, LabelStr.MOVE, LabelStr.SPEED),
                out _pickMoveSpeed);
            //拾取每次增加的速度
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.PICK, LabelStr.INCREASE, LabelStr.MOVE, LabelStr.SPEED),
                out _pickIncreaseMoveSpeed);
            //消失延时
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DESTROY, LabelStr.DELAY, LabelStr.TIME),
                out _destroyDelayTime);
            Game.instance.OnUpdateEvent.AddListener(OnMoveToPlayer);
            Game.instance.OnUpdateEvent.AddListener(OnMoveAddMoveSpeed);

            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.DESTROY, LabelStr.DELAY, LabelStr.INCREASE, LabelStr.TIME),
                out FloatData _destroyDelayIncreaseTime);
            
            ClockUtil.Instance.AlarmAfter(_destroyDelayTime.Float + _destroyDelayIncreaseTime.Float, () => {
                Obj.Instance.UnLoadEntity(entity);
            });
        }

        public override void DelayedExecute() {
            
        }

        private void OnTriggerEnter(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.ObjConfig.Type == "Player") {
                    Pick(playerEntity);
                }
            }
        }

        public void Pick(Entity playerEntity) {
            Cond.Instance.GetData(entity, LabelStr.EXPERIENCE, out FloatData addExpData);
            Cond.Instance.GetData(playerEntity, LabelStr.EXPERIENCE, out FloatData expData);
            Cond.Instance.GetData(playerEntity, LabelStr.Assemble(LabelStr.MAX, LabelStr.EXPERIENCE), out FloatData maxExpData);
            float afterExp = expData.Float + addExpData.Float;
            if (afterExp < maxExpData.Float) {
                expData.Float = afterExp;
            } else {
                expData.Float = afterExp - maxExpData.Float;
                //升级三选一
                MessageRegister.Instance.Dis(MessageCode.MsgPlayerLevelUp);
            }

            //拾取恢复血量
            Cond.Instance.GetData(playerEntity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData _healthMax);
            Cond.Instance.GetData(playerEntity, LabelStr.Assemble(LabelStr.PICK, LabelStr.RECOVER, LabelStr.RATIO), out FloatData _ratio);
            MessageRegister.Instance.Dis(MessageCode.MsgRecoverHealth, _healthMax.Float * _ratio.Float);

            //拾取增加移速
            _pickMoveSpeed.Float += _pickIncreaseMoveSpeed.Float;
            moveSpeedDeploy = 3;

            Obj.Instance.UnLoadEntity(entity);
        }

        private void OnMoveToPlayer() {
            if (_movePlayer.Bool) {
                Debug.Log("向玩家移动");
                Vector3 playerPos = Cond.Instance.Get<Transform>(Cond.Instance.GetPlayerEntity(), LabelStr.BODY).position;
                Vector3 moveDir = (playerPos - _body.position).normalized;
                moveDir.y = 0;

                if (Vector3.Distance(_body.position, playerPos) < _pickDis.Float) {
                    float distance = _getMoveSpeed.Float * Time.deltaTime;
                    _body.transform.position += moveDir * distance;
                    if (Vector3.Distance(_body.position, playerPos) < _getPickDis.Float) {
                        Pick(Cond.Instance.GetPlayerEntity());
                        _movePlayer.Bool = false;
                    }
                } else {
                    float distance = _moveSpeed.Float * Time.deltaTime;
                    _body.transform.position += moveDir * distance;
                }
            }
        }

        private void OnMoveAddMoveSpeed() {
            if (_pickMoveSpeed.Float != 1) {
                if (moveSpeedDeploy > 0) {
                    moveSpeedDeploy -= Time.deltaTime;
                } else {
                    _pickMoveSpeed.Float = 1;
                    moveSpeedDeploy = 0;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnTriggerEnter);
            Game.instance.OnUpdateEvent.RemoveListener(OnMoveToPlayer);
            Game.instance.OnUpdateEvent.RemoveListener(OnMoveAddMoveSpeed);
        }
    }
}