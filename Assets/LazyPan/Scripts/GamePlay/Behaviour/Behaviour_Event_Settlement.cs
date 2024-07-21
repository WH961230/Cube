using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_Settlement : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        public Behaviour_Event_Settlement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();
            Time.timeScale = 0;
            OpenSettlementUI();
        }
        
        public override void DelayedExecute() {
            
        }

        private void OpenSettlementUI() {
            Comp settlement = Cond.Instance.Get<Comp>(_ui, LabelStr.SETTLEMENT);
            if (!settlement.gameObject.activeSelf) {
                settlement.gameObject.SetActive(true);
                Button back = Cond.Instance.Get<Button>(settlement, LabelStr.BACK);
                ButtonRegister.RemoveAllListener(back);
                ButtonRegister.AddListener(back, Next);
            }
        }

        private void CloseSettlementUI() {
            Comp settlement = Cond.Instance.Get<Comp>(_ui, LabelStr.SETTLEMENT);
            settlement.gameObject.SetActive(false);
        }

        private void Next() {
            Time.timeScale = 1;
            CloseSettlementUI();
            _flow.Next("SceneA");
        }

        public override void Clear() {
            base.Clear();
            CloseSettlementUI();
        }
    }
}