using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_PlayerExperience : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private Slider _experienceBar;
        private FloatData _maxExperienceData;
        private FloatData _experienceData;
        public Behaviour_Auto_PlayerExperience(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();
            Comp _experience = Cond.Instance.Get<Comp>(_ui, "Experience");
            _experienceBar = Cond.Instance.Get<Slider>(_experience, "Slider");
            Cond.Instance.GetData(entity, LabelStr.EXPERIENCE, out _experienceData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.EXPERIENCE), out _maxExperienceData);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            _experienceBar.value = _experienceData.Float / _maxExperienceData.Float;
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}