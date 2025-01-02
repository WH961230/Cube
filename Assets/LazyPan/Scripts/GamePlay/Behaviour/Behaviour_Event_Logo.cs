using DG.Tweening;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_Logo : Behaviour {
        private FloatData _logoTimeData;
        private Clock _clock;
        public Behaviour_Event_Logo(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out Flow_SceneA flow);
            Comp ui = flow.GetUI();
            Comp logoComp = Cond.Instance.Get<Comp>(ui, LabelStr.LOGO);
            if (GlobalData.Instance.IsPlayLogo) {
                Cond.Instance.Get<GameObject>(ui, "Light").SetActive(true);
                //播放过Logo不再播放
                logoComp.gameObject.SetActive(false);
                flow.StartGame();
            } else {
                //第一次播放Logo
                GlobalData.Instance.IsPlayLogo = true;
                Cond.Instance.Get<GameObject>(ui, "FirstLight").SetActive(true);
                Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.LOGO, LabelStr.TIME), out _logoTimeData);
                _clock = ClockUtil.Instance.AlarmAfter(_logoTimeData.Float, () => {
                    logoComp.gameObject.SetActive(false);
                    flow.StartGame();
                });
            }
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            ClockUtil.Instance.Stop(_clock);
        }
    }
}