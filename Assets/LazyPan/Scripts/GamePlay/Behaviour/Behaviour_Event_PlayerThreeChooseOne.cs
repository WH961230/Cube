using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Event_PlayerThreeChooseOne : Behaviour {
        private StringData _startChooseSound;
        private Behaviour_Auto_TowerWeaponManagement weaponManagerBehaviour;
        private List<Entity> _buffs = new List<Entity>();
        List<Entity> retEntities = new List<Entity>();
        public Behaviour_Event_PlayerThreeChooseOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.START, LabelStr.CHOOSE, LabelStr.SOUND),
                out _startChooseSound);
            if (EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity)) {
                if (BehaviourRegister.GetBehaviour(towerEntity.ID, out weaponManagerBehaviour)) {
                }
            }

            InitBuffs();
            MessageRegister.Instance.Reg(MessageCode.MsgPlayerLevelUp, MsgPlayerThreeChooseOne);
        }

        public override void DelayedExecute() {
            
        }

        //初始化Buff
        private void InitBuffs() {
            _buffs.Clear();
            List<string> objConfigs = ObjConfig.GetKeys();
            // string[] types = new[] { "武器增益", "角色增益", "角色一次性增益", "属性增益" };
            string[] types = new[] { "武器增益" };
            foreach (string keyStr in objConfigs) {
                string[] keys = keyStr.Split("|");
                string[] flowCodes = Flo.Instance.CurFlowSign.Split("_");
                SceneConfig sceneConfig = SceneConfig.Get(flowCodes[1]);
                if (sceneConfig.Description != keys[0]) {
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

            if (EntityRegister.TryGetEntitiesByType("武器", out List<Entity> weapons)) {
                foreach (var tmpWeapon in weapons) {
                    _buffs.Add(tmpWeapon);
                }
            }
        }

        //获取类型集合的所有实体
        private List<Entity> GetBuff() {
            retEntities.Clear();
            //遍历所有的buff
            foreach (var buffEntity in _buffs) {
                //获取当前的类型
                if (buffEntity.ObjConfig.Type == "属性增益") {
                    //无限次数直接加
                    retEntities.Add(buffEntity);
                } else if (buffEntity.ObjConfig.Type == "角色一次性增益") {
                    //使用完后就无法再加
                    if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                        if (!usedBoolData.Bool) {
                            retEntities.Add(buffEntity);
                        }
                    }
                } else if (buffEntity.ObjConfig.Type == "角色增益") {
                    //当前路径不为空
                    if (Cond.Instance.GetData<SelectPathData, StringData>(buffEntity,
                            LabelStr.Assemble(LabelStr.CURRENT, LabelStr.PATH),
                            out StringData currentPathData)) {
                        if (!string.IsNullOrEmpty(currentPathData.String)) {
                            retEntities.Add(buffEntity);
                        }
                    }
                } else if (buffEntity.ObjConfig.Type == "武器增益") {
                    List<Entity> assembledWeapons = weaponManagerBehaviour.GetAssembledWeapons();
                    //当前目标武器玩家已装配
                    Cond.Instance.GetData<SelectPathData, StringData> (buffEntity, LabelStr.Assemble(LabelStr.TARGET, LabelStr.SIGN),
                        out StringData weaponSign);
                    //必须武器增益是已装备武器的
                    foreach (var tmpWeapon in assembledWeapons) {
                        if (tmpWeapon.Sign == weaponSign.String) {
                            //当前路径不为空
                            if (Cond.Instance.GetData<SelectPathData, StringData>(buffEntity,
                                    LabelStr.Assemble(LabelStr.CURRENT, LabelStr.PATH),
                                    out StringData currentPathData)) {
                                if (!string.IsNullOrEmpty(currentPathData.String)) {
                                    retEntities.Add(buffEntity);
                                }
                            }
                            break;
                        }
                    }
                } else if (buffEntity.ObjConfig.Type == "武器") {
                    List<Entity> assembledWeapons = weaponManagerBehaviour.GetAssembledWeapons();
                    //是否装配满4把武器 且 未使用
                    if (assembledWeapons.Count < 4) {
                        //未被使用的 buff
                        if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                            if (!usedBoolData.Bool) {
                                retEntities.Add(buffEntity);
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
        private void MsgPlayerThreeChooseOne() {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.PLAYER, LabelStr.LEVEL), out IntData playerLevel);
            playerLevel.Int++;
            MessageRegister.Instance.Dis(MessageCode.MsgLevelUp);
            OpenPlayerThreeChooseOneUI();
            Sound.Instance.SoundPlay(_startChooseSound.String, Vector3.zero, false, 2);
        }

        //打开角色三选一界面
        private void OpenPlayerThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            string debug = "开始玩家选择三选一：";

            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (!choose.gameObject.activeSelf) {
                List<Entity> playBuffEntity = GetBuff();
                if (playBuffEntity.Count == 0) {
                    return;
                }

                MessageRegister.Instance.Dis(MessageCode.MsgSetTimeScale, 0f);
                choose.gameObject.SetActive(true);
                int resultCount = 3;
                int[] index = MathUtil.Instance.GetRandNoRepeatIndex(playBuffEntity.Count, resultCount);
                if (index != null) {
                    for (int i = 0; i < index.Length; i++) {
                        int tmpIndex = index[i];
                        Entity buffEntity = playBuffEntity[tmpIndex];
                        Comp item = Cond.Instance.Get<Comp>(choose,
                            LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, i.ToString()));
                        //注册图片
                        // Image image = Cond.Instance.Get<Image>(item, LabelStr.ICON);
                        // Cond.Instance.GetData(buffEntity, LabelStr.ICON, out StringData spritePathData);
                        // image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spritePathData.String);

                        //注册按钮事件
                        Button button = Cond.Instance.Get<Button>(item, LabelStr.BUTTON);
                        ButtonRegister.RemoveAllListener(button);
                        if (buffEntity.ObjConfig.Type == "属性增益") {
                            //注册说明
                            TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                            Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                            info.text = infoData.String;
                            debug += buffEntity.ObjConfig.Type + " - " + infoData.String + "\n";
                            //按钮
                            ButtonRegister.AddListener(button, RegisterUnLimitBehaviour, buffEntity);
                        } else if (buffEntity.ObjConfig.Type == "角色一次性增益") {
                            //注册说明
                            TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                            Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                            info.text = infoData.String;
                            debug += buffEntity.ObjConfig.Type + " - " + infoData.String + "\n";
                            //按钮
                            ButtonRegister.AddListener(button, RegisterOnceBehaviour, buffEntity);
                        } else if (buffEntity.ObjConfig.Type == "角色增益") {
                            int randIndex = Random.Range(0, 2);
                            if (GetBuffPathData(randIndex, buffEntity, out PathData pathData)) {
                                TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                                info.text = pathData.Description;
                                debug += buffEntity.ObjConfig.Type + " - " + pathData.Description + "\n";
                            }
                            //按钮
                            ButtonRegister.AddListener(button, RegisterPathBehaviour, randIndex, buffEntity, Cond.Instance.GetPlayerEntity().ID);
                        } else if (buffEntity.ObjConfig.Type == "武器增益") {
                            int randIndex = Random.Range(0, 2);//随机
                            int entityID = -1;//装配实体
                            if (GetBuffPathData(randIndex, buffEntity, out PathData pathData)) {//获取buff路径数据
                                TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                                info.text = pathData.Description;
                                debug += buffEntity.ObjConfig.Type + " - " + pathData.Description + "\n";

                                List<Entity> assembledWeapons = weaponManagerBehaviour.GetAssembledWeapons();
                                //当前目标武器玩家已装配
                                Cond.Instance.GetData<SelectPathData, StringData> (buffEntity, LabelStr.Assemble(LabelStr.TARGET, LabelStr.SIGN),
                                    out StringData weaponSign);
                                //必须武器增益是已装备武器的
                                foreach (var tmpWeapon in assembledWeapons) {
                                    if (tmpWeapon.Sign == weaponSign.String) {
                                        //当前路径不为空
                                        entityID = tmpWeapon.ID;
                                        break;
                                    }
                                }
                            }

                            //按钮
                            ButtonRegister.AddListener(button, RegisterPathBehaviour, randIndex, buffEntity, entityID);
                        } else if (buffEntity.ObjConfig.Type == "武器") {
                            //注册说明
                            TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                            Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                            info.text = infoData.String;
                            debug += buffEntity.ObjConfig.Type + " - " + infoData.String + "\n";
                            //按钮
                            ButtonRegister.AddListener(button, RegisterAssembledWeapon, buffEntity);
                        }
                    }
                }
                InputRegister.Instance.Load(InputRegister.ESCAPE, InputCloseUI);
                Debug.Log(debug);
            }
        }

        private void RegisterUnLimitBehaviour(Entity buffEntity) {
            if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData behaviourSignStringData)) {
                BehaviourRegister.UnRegisterBehaviour(buffEntity.ID, behaviourSignStringData.String);
                BehaviourRegister.RegisterBehaviour(buffEntity.ID, behaviourSignStringData.String, out Behaviour outBehaviour);
                outBehaviour.DelayedExecute();
                ClosePlayerThreeChooseOneUI();
            }
        }

        private void RegisterOnceBehaviour(Entity buffEntity) {
            //让目标单位注册该方法
            if (Cond.Instance.GetData(buffEntity, LabelStr.Assemble(LabelStr.TARGET, LabelStr.TYPE), out StringData targetTypeStringData)) {
                if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData behaviourSignStringData)) {
                    if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                        if (EntityRegister.TryGetEntitiesByType(targetTypeStringData.String, out List<Entity> entities)) {
                            foreach (var tmpEntity in entities) {
                                //注册一次
                                BehaviourRegister.RegisterBehaviour(tmpEntity.ID, behaviourSignStringData.String, out Behaviour outBehaviour);
                                //一次性增益无法再次使用
                                usedBoolData.Bool = true;
                            }
                            ClosePlayerThreeChooseOneUI();
                        }
                    }
                }
            }
        }

        private bool GetBuffPathData(int randIndex, Entity buffEntity, out PathData pathData) {
            //让目标单位注册该方法
            if (Cond.Instance.GetData<SelectPathData, StringData>(buffEntity,
                    LabelStr.Assemble(LabelStr.CURRENT, LabelStr.PATH), out StringData currentPathData)) {
                string randPath = "";
                if (currentPathData.String.Contains("|")) {
                    string[] pathSigns = currentPathData.String.Split("|");
                    randPath = pathSigns[randIndex];
                } else {
                    randPath = currentPathData.String;
                }

                return Cond.Instance.GetData<SelectPathData, PathData>(buffEntity, randPath, out pathData);
            }

            pathData = default;
            return false;
        }

        private void RegisterPathBehaviour(int randIndex, Entity buffEntity, int targetID) {
            if (targetID == -1) {
                Debug.LogError("目标Buff错误 id = " + buffEntity.ID);
            }

            //让目标单位注册该方法
            if (Cond.Instance.GetData<SelectPathData, StringData> (buffEntity,
                    LabelStr.Assemble(LabelStr.CURRENT, LabelStr.PATH), out StringData currentPathData)) {
                if (GetBuffPathData(randIndex, buffEntity, out PathData targetPathData)) {
                    BehaviourRegister.RegisterBehaviour(targetID, targetPathData.BehaviourName, out Behaviour outBehaviour);
                    currentPathData.String = targetPathData.NextSign;
                    ClosePlayerThreeChooseOneUI();
                }
            }
        }

        private void RegisterAssembledWeapon(Entity buffEntity) {
            if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData stringData)) {
                if (EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity)) {
                    if (BehaviourRegister.GetBehaviour(towerEntity.ID, out Behaviour_Auto_TowerWeaponManagement beh)) {
                        beh.InitWeapon(stringData.String);
                        ClosePlayerThreeChooseOneUI();
                    }
                }
            }
        }

        //按键关闭UI
        private void InputCloseUI(InputAction.CallbackContext obj) {
            if (obj.started) {
                ClosePlayerThreeChooseOneUI();
            }
        }

        //关闭角色三选一界面
        private void ClosePlayerThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            choose.gameObject.SetActive(false);

            InputRegister.Instance.UnLoad(InputRegister.ESCAPE, InputCloseUI);
            MessageRegister.Instance.Dis(MessageCode.MsgSetTimeScale, 1f);
        }

        public override void Clear() {
            base.Clear();
            ClosePlayerThreeChooseOneUI();
            ClearBuffs();
            MessageRegister.Instance.UnReg(MessageCode.MsgPlayerLevelUp, MsgPlayerThreeChooseOne);
        }
    }
}