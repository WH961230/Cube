using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Event_PlayerThreeChooseOne : Behaviour {
        public Behaviour_Event_PlayerThreeChooseOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.MsgLevelUp, MsgPlayerThreeChooseOne);
            InputRegister.Instance.Load(InputRegister.Space, context => {
                MessageRegister.Instance.Dis(MessageCode.MsgLevelUp);
            });
            

        }

        private void MsgPlayerThreeChooseOne() {
            OpenPlayerThreeChooseOneUI();
        }

        private void OpenPlayerThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (!choose.gameObject.activeSelf) {
                
                //获取三个Buff
                List<string> objConfigs = ObjConfig.GetKeys();
                List<string> playBuffBehaviour = new List<string>();
                foreach (string key in objConfigs) {
                    ObjConfig config = ObjConfig.Get(key);
                    if (config.Type == "PlayerBuff") {
                        playBuffBehaviour.Add(config.SetUpBehaviourSign);
                    }
                }

                if (playBuffBehaviour.Count == 0) {
                    return;
                }
                
                choose.gameObject.SetActive(true);
                int resultCount = 1;
                int[] index = MathUtil.Instance.GetRandNoRepeatIndex(playBuffBehaviour.Count, resultCount);

                for (int i = 0; i < index.Length; i++) {
                    string behaviour = playBuffBehaviour[i];
                    
                    //注册按钮事件
                    Button button = Cond.Instance.Get<Button>(
                        Cond.Instance.Get<Comp>(choose, LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, i.ToString())),
                        LabelStr.BUTTON);
                    ButtonRegister.RemoveAllListener(button);
                    ButtonRegister.AddListener(button, () => {
                        Debug.Log("点击" + i.ToString());
                        EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
                        BehaviourRegister.RegisterBehaviour(_tower.ID, behaviour);
                    });
                }

                Button buttonB = Cond.Instance.Get<Button>(
                    Cond.Instance.Get<Comp>(choose, LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, LabelStr.B)),
                    LabelStr.BUTTON);
                ButtonRegister.RemoveAllListener(buttonB);
                ButtonRegister.AddListener(buttonB, () => {
                    Debug.Log("点击B");
                });
            
                Button buttonC = Cond.Instance.Get<Button>(
                    Cond.Instance.Get<Comp>(choose, LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, LabelStr.C)),
                    LabelStr.BUTTON);
                ButtonRegister.RemoveAllListener(buttonC);
                ButtonRegister.AddListener(buttonC, () => {
                    Debug.Log("点击C");
                });

                InputRegister.Instance.Load(InputRegister.ESCAPE, InputCloseUI);
            }
        }

        private void InputCloseUI(InputAction.CallbackContext obj) {
            if (obj.performed) {
                Debug.Log("关闭界面");
                ClosePlayerThreeChooseOneUI();
                InputRegister.Instance.UnLoad(InputRegister.ESCAPE, InputCloseUI);
            }
        }

        private void ClosePlayerThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            choose.gameObject.SetActive(false);
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.MsgLevelUp, MsgPlayerThreeChooseOne);
        }
    }
}