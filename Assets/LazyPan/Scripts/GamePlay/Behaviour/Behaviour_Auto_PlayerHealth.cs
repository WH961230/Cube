using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_PlayerHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private Slider _healthBar;
        private FloatData _maxHealthData;
        private FloatData _healthData;
        public Behaviour_Auto_PlayerHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            Comp _health = Cond.Instance.Get<Comp>(_ui, "Health");
            _healthBar = Cond.Instance.Get<Slider>(_health, "Slider");

            Cond.Instance.GetData(entity, "MaxHealth", out _maxHealthData);
            Cond.Instance.GetData(entity, "Health", out _healthData);
            _healthData.Float = _maxHealthData.Float;

            Game.instance.OnLateUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            _healthBar.value = _healthData.Float / _maxHealthData.Float;
            if (Input.GetKeyDown(KeyCode.Space)) {
                _healthData.Float -= 5;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnLateUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}