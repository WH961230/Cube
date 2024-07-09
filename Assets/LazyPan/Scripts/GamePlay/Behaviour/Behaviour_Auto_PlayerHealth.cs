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
            
            InputRegister.Instance.Load(InputCode.R, context => {
                if (context.performed) {
                    BeDamaged(10);
                }
            });

            MessageRegister.Instance.Reg<float>(MessageCode.MsgDamagePlayer, BeDamaged);
            Game.instance.OnLateUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            _healthBar.value = _healthData.Float / _maxHealthData.Float;
        }

        private void BeDamaged(float damageValue) {
            if (_healthData.Float != 0) {
                if (_healthData.Float > 0) {
                    _healthData.Float -= damageValue;
                }

                if (_healthData.Float <= 0) {
                    _healthData.Float = 0;
                    Next();
                }
            }
        }

        private void Next() {
            _flow.Settlement();
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnLateUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<float>(MessageCode.MsgDamagePlayer, BeDamaged);
        }
    }
}