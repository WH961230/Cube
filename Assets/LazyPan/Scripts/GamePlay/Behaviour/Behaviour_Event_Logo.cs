using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_Logo : Behaviour {
        private Flow_SceneA flow;
        private Comp ui;
        private FloatData _logoTimeData;
        private Clock _clock;
        public Behaviour_Event_Logo(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out flow);
            ui = flow.GetUI();
            Comp logoComp = Cond.Instance.Get<Comp>(ui, LabelStr.LOGO);
            if (GlobalData.Instance.IsPlayLogo) {
                Cond.Instance.Get<GameObject>(ui, "Light").SetActive(true);
                //播放过Logo不再播放
                logoComp.gameObject.SetActive(false);
                flow.StartGame();
            } else {
                //第一次播放Logo
                GlobalData.Instance.IsPlayLogo = true;
                Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.LOGO, LabelStr.TIME), out _logoTimeData);
                _clock = ClockUtil.Instance.AlarmAfter(_logoTimeData.Float, () => {
                    //详情页
                    Comp detailComp = Cond.Instance.Get<Comp>(ui, LabelStr.DETAIL);
                    detailComp.gameObject.SetActive(true);
                    Button sureButton = Cond.Instance.Get<Button>(detailComp, LabelStr.SURE);
                    ButtonRegister.RemoveAllListener(sureButton);
                    ButtonRegister.AddListener(sureButton, Sure, detailComp.gameObject);
                    
                    Button qqButton = Cond.Instance.Get<Button>(detailComp, "QQ");
                    ButtonRegister.RemoveAllListener(qqButton);
                    ButtonRegister.AddListener(qqButton, () => {
                        Application.OpenURL(
                            "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=MkuQ5RmlgapDBRERuHUgbrUXt6__Yv46&authKey=9d%2BfxG%2BsSDd%2BvglcDmUCkWDdalaR16eJGg7qfgZE4yznSq1AMC4%2BQpDt8LqvhvWm&noverify=0&group_code=1016281195");
                    });

                    Button focusButton = Cond.Instance.Get<Button>(detailComp, "Focus");
                    ButtonRegister.RemoveAllListener(focusButton);
                    ButtonRegister.AddListener(focusButton, () => {
                        Application.OpenURL("https://space.bilibili.com/29326484?spm_id_from=333.1387.0.0");
                    });
                    
                    logoComp.gameObject.SetActive(false);
                    flow.StartGame();
                });
            }
        }

        private void Sure(GameObject go) {
            go.SetActive(false);
            Cond.Instance.Get<GameObject>(ui, "Light").SetActive(true);
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            ClockUtil.Instance.Stop(_clock);
        }
    }
}