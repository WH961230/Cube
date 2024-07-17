using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Event_PlayerThreeChooseOne : Behaviour {
        private List<Entity> _buffs = new List<Entity>();
        public Behaviour_Event_PlayerThreeChooseOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            InitBuffs();
            MessageRegister.Instance.Reg(MessageCode.MsgPlayerLevelUp, MsgPlayerThreeChooseOne);

            #region Test

            InputRegister.Instance.Load(InputCode.Q, context => {
                if (context.performed) {
                    MessageRegister.Instance.Dis(MessageCode.MsgPlayerLevelUp);
                }
            });

            #endregion
        }

        //初始化Buff
        private void InitBuffs() {
            _buffs.Clear();
            List<string> objConfigs = ObjConfig.GetKeys();
            string[] types = new[] { "WeaponBuff", "PlayerBuff", "TowerBuff", "AssembledBuff" };
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
                //获取当前的类型
                if (buffEntity.ObjConfig.Type == "PlayerBuff" ||
                    buffEntity.ObjConfig.Type == "WeaponBuff" ||
                    buffEntity.ObjConfig.Type == "TowerBuff") {
                    if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                        if (!usedBoolData.Bool) {
                            retEntities.Add(buffEntity);
                        }
                    }
                } else if (buffEntity.ObjConfig.Type == "AssembledBuff") {
                    if (EntityRegister.TryGetRandEntityByType("Tower",
                        out Entity towerEntity)) {
                        if (BehaviourRegister.GetBehaviour(towerEntity.ID,
                            out Behaviour_Auto_TowerWeaponManagement beh)) {
                            if (beh.GetAssembledWeaponCount() < 4) {
                                if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                                    if (!usedBoolData.Bool) {
                                        retEntities.Add(buffEntity);
                                    }
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
        private void MsgPlayerThreeChooseOne() {
            OpenPlayerThreeChooseOneUI();
        }

        //打开角色三选一界面
        private void OpenPlayerThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (!choose.gameObject.activeSelf) {

                List<Entity> playBuffEntity = GetBuff(new[]{"WeaponBuff", "PlayerBuff", "TowerBuff", "AssembledBuff"});
                if (playBuffEntity.Count == 0) {
                    return;
                }

                Time.timeScale = 0;
                choose.gameObject.SetActive(true);
                int resultCount = 3;
                int[] index = MathUtil.Instance.GetRandNoRepeatIndex(playBuffEntity.Count, resultCount);

                if (index != null) {
                    for (int i = 0; i < index.Length; i++) {
                        Entity buffEntity = playBuffEntity[i];
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
                        if (buffEntity.ObjConfig.Type == "PlayerBuff" ||
                            buffEntity.ObjConfig.Type == "WeaponBuff" ||
                            buffEntity.ObjConfig.Type == "TowerBuff") {
                            ButtonRegister.AddListener(button, RegisterBehaviour, buffEntity);
                        } else if (buffEntity.ObjConfig.Type == "AssembledBuff") {
                            ButtonRegister.AddListener(button, RegisterAssembledWeapon, buffEntity);
                        }
                    }
                }
                InputRegister.Instance.Load(InputRegister.ESCAPE, InputCloseUI);
            }
        }

        private void RegisterBehaviour(Entity buffEntity) {
            //让目标单位注册该方法
            if (Cond.Instance.GetData(buffEntity, LabelStr.TARGET, out StringData targetSignStringData)) {
                if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData behaviourSignStringData)) {
                    if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                        if (EntityRegister.TryGetEntityBySign(targetSignStringData.String, out Entity targetEntity)) {
                            BehaviourRegister.RegisterBehaviour(targetEntity.ID, behaviourSignStringData.String);
                            usedBoolData.Bool = true;
                            ClosePlayerThreeChooseOneUI();
                        }
                    }
                }
            }
        }

        private void RegisterAssembledWeapon(Entity buffEntity) {
            if (Cond.Instance.GetData(buffEntity, LabelStr.SIGN, out StringData stringData)) {
                if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                    if (EntityRegister.TryGetRandEntityByType("Tower",
                        out Entity towerEntity)) {
                        if (BehaviourRegister.GetBehaviour(towerEntity.ID,
                            out Behaviour_Auto_TowerWeaponManagement beh)) {
                            beh.InitWeapon(stringData.String);
                            usedBoolData.Bool = true;
                            ClosePlayerThreeChooseOneUI();
                        }
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
            Time.timeScale = 1;
        }

        public override void Clear() {
            base.Clear();
            ClosePlayerThreeChooseOneUI();
            ClearBuffs();
            MessageRegister.Instance.UnReg(MessageCode.MsgPlayerLevelUp, MsgPlayerThreeChooseOne);
        }
    }
}