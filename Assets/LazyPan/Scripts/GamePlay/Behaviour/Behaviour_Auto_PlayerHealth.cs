using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_PlayerHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private Slider _healthBar;//血条
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private FloatData _healthRecoverSpeed;//血量恢复速度
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        public Behaviour_Auto_PlayerHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            Comp _health = Cond.Instance.Get<Comp>(_ui, "Health");
            _healthBar = Cond.Instance.Get<Slider>(_health, "Slider");

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.HEALTH, LabelStr.SPEED), out _healthRecoverSpeed);
            _healthData.Float = _maxHealthData.Float;
            
            Cond.Instance.GetData(entity, Label.ENERGY + Label.ING, out _chargyingEnergyData);
            
            InputRegister.Instance.Load(InputCode.R, context => {
                if (context.performed) {
                    MsgBeDamaged(10);
                }
            });

            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.INVINCIBLE, Label.ING), out _invincibleData);
            
            MessageRegister.Instance.Reg<float>(MessageCode.MsgDamagePlayer, MsgBeDamaged);
            MessageRegister.Instance.Reg<float>(MessageCode.MsgRecoverHealth, MsgBeRecovered);
            Game.instance.OnLateUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            _healthBar.value = _healthData.Float / _maxHealthData.Float;
            if (_chargyingEnergyData.Bool) {
                BeRecoveredHealth(_healthRecoverSpeed.Float * Time.deltaTime);
            }
        }

        private void MsgBeRecovered(float recoverValue) {
            BeRecoveredHealth(recoverValue);
        }

        private void BeRecoveredHealth(float recoverValue) {
            if (_healthData.Float < _maxHealthData.Float) {
                _healthData.Float += recoverValue;
            } else {
                _healthData.Float = _maxHealthData.Float;
            }
        }

        private void MsgBeDamaged(float damageValue) {
            if (_invincibleData.Bool) {
                return;
            }

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
            MessageRegister.Instance.UnReg<float>(MessageCode.MsgRecoverHealth, MsgBeRecovered);
            MessageRegister.Instance.UnReg<float>(MessageCode.MsgDamagePlayer, MsgBeDamaged);
        }
    }
}