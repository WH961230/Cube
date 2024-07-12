using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_RobotThreeChooseOne : Behaviour {
        private List<Entity> _buffs = new List<Entity>();
        public Behaviour_Event_RobotThreeChooseOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InitBuffs();
            MessageRegister.Instance.Reg(MessageCode.MsgRobotUp, MsgRobotThreeChooseOne);
            
            #region Test

            InputRegister.Instance.Load(InputCode.E, context => {
                if (context.performed) {
                    MessageRegister.Instance.Dis(MessageCode.MsgRobotUp);
                }
            });

            #endregion
        }

        //初始化Buff
        private void InitBuffs() {
            _buffs.Clear();
            List<string> objConfigs = ObjConfig.GetKeys();
            string[] types = new[] { "RobotBuff", "WaveBuff" };
            foreach (string keyStr in objConfigs) {
                string[] keys = keyStr.Split("|");
                if (!Flo.Instance.CurFlowSign.Contains(keys[0])) {
                    continue;
                }
                string key = keys[1];
                ObjConfig config = ObjConfig.Get(key);
                foreach (var type in types) {
                    if (config.Type == type) {
                        Entity buffInstanceEntity = Obj.Instance.LoadEntity(config.Sign);
                        _buffs.Add(buffInstanceEntity);
                        break;
                    }
                }
            }
        }

        //获取类型集合的所有实体
        private List<Entity> GetBuff(string[] types) {
            List<Entity> retEntities = new List<Entity>();
            //遍历所有的buff
            foreach (var buffEntity in _buffs) {
                foreach (var type in types) {
                    //获取当前的类型
                    if (buffEntity.ObjConfig.Type == type) {
                        Type dataType = buffEntity.Data.GetType();
                        if (dataType == typeof(RobotWaveData)) {
                            retEntities.Add(buffEntity);
                        } else {
                            if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                                if (!usedBoolData.Bool) {
                                    retEntities.Add(buffEntity);
                                }
                            }
                        }
                    }
                }
            }

            return retEntities;
        }

        //清理Buff
        private void ClearBuffs() {
            foreach (var buffEntity in _buffs) {
                Obj.Instance.UnLoadEntity(buffEntity);
            }
            _buffs.Clear();
        }

        //传入消息开始三选一
        private void MsgRobotThreeChooseOne() {
            OpenRobotThreeChooseOneUI();
        }

        //打开机器人三选一界面
        private void OpenRobotThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (!choose.gameObject.activeSelf) {

                List<Entity> robotBuffEntities = GetBuff(new[]{"RobotBuff", "WaveBuff"});
                if (robotBuffEntities.Count == 0) {
                    return;
                }

                Time.timeScale = 0;
                choose.gameObject.SetActive(true);
                int resultCount = 3;
                int[] index = MathUtil.Instance.GetRandNoRepeatIndex(robotBuffEntities.Count, resultCount);

                for (int i = 0; i < index.Length; i++) {
                    Entity buffEntity = robotBuffEntities[i];
                    Comp item = Cond.Instance.Get<Comp>(choose,
                        LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, i.ToString()));
                    //注册图片
                    Image image = Cond.Instance.Get<Image>(item, LabelStr.ICON);
                    Cond.Instance.GetData(buffEntity, LabelStr.ICON, out StringData spritePathData);
                    image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spritePathData.String);

                    //注册说明
                    TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                    Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                    info.text = infoData.String;

                    //注册按钮事件
                    Button button = Cond.Instance.Get<Button>(item, LabelStr.BUTTON);
                    ButtonRegister.RemoveAllListener(button);
                    if (buffEntity.ObjConfig.Type == "RobotBuff") {
                        ButtonRegister.AddListener(button, RegisterBehaviour, buffEntity);
                    } else if (buffEntity.ObjConfig.Type == "WaveBuff") {
                        ButtonRegister.AddListener(button, RegisterWave, buffEntity);
                    }
                }

                InputRegister.Instance.Load(InputRegister.ESCAPE, InputCloseUI);
            }
        }

        private void RegisterBehaviour(Entity buffEntity) {
            if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData behaviourSignStringData)) {
                if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                    //有目标 针对单独目标
                    if (Cond.Instance.GetData(buffEntity, LabelStr.TARGET, out StringData targetSignStringData)) {
                        if (EntityRegister.TryGetEntityBySign(targetSignStringData.String, out Entity targetEntity)) {
                            BehaviourRegister.RegisterBehaviour(targetEntity.ID, behaviourSignStringData.String);
                            usedBoolData.Bool = true;
                            CloseRobotThreeChooseOneUI();
                        }
                    } else {
                        //无目标针对所有敌人 在敌人的生成位置 放置待注册Buff
                        if (EntityRegister.TryGetEntityBySign("Obj_Creator_RobotCreator",
                                out Entity createRobotEntity)) {
                            if (BehaviourRegister.GetBehaviour(createRobotEntity.ID,
                                    out Behaviour_Event_CreateRandomPositionRobot beh)) {
                                beh.AddSetUpBehaviourSign(behaviourSignStringData.String);
                                usedBoolData.Bool = true;
                                CloseRobotThreeChooseOneUI();
                            }
                        }
                    }
                }
            }
        }

        //波次BUFF可以重复选择
        private void RegisterWave(Entity buffEntity) {
            if (Cond.Instance.GetData<RobotWaveData, WaveData>(buffEntity, LabelStr.WAVE, out WaveData waveData)) {
                //无目标针对所有敌人 在敌人的生成位置 放置待注册Buff
                if (EntityRegister.TryGetEntityBySign("Obj_Creator_RobotCreator",
                        out Entity createRobotEntity)) {
                    if (BehaviourRegister.GetBehaviour(createRobotEntity.ID,
                            out Behaviour_Event_CreateRandomPositionRobot beh)) {
                        beh.AddWaveInstanceData(waveData.WaveInstanceDefaultList[0]);
                        CloseRobotThreeChooseOneUI();
                    }
                }
            }
        }

        //按键关闭UI
        private void InputCloseUI(InputAction.CallbackContext obj) {
            if (obj.started) {
                CloseRobotThreeChooseOneUI();
            }
        }

        //关闭角色三选一界面
        private void CloseRobotThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            choose.gameObject.SetActive(false);

            InputRegister.Instance.UnLoad(InputRegister.ESCAPE, InputCloseUI);
            Time.timeScale = 1;
        }

        public override void Clear() {
            base.Clear();
            CloseRobotThreeChooseOneUI();
            ClearBuffs();
            MessageRegister.Instance.UnReg(MessageCode.MsgRobotUp, MsgRobotThreeChooseOne);
        }
    }
}