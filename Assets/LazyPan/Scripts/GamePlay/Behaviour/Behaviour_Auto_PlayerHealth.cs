using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_PlayerHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private float burnDeploy;
        private BoolData _burn;
        private FloatData _burnAttack;
        private Slider _healthBar;//血条
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private FloatData _healthRecoverSpeed;//血量恢复速度
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        private FloatData _damageReduceRatio;
        private BoolData _ignoreBurn;

        public Behaviour_Auto_PlayerHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            Comp _health = Cond.Instance.Get<Comp>(_ui, "Health");
            _healthBar = Cond.Instance.Get<Slider>(_health, "Slider");

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.HEALTH, LabelStr.SPEED), out _healthRecoverSpeed);
            _healthData.Float = _maxHealthData.Float;

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DAMAGE, LabelStr.REDUCE, LabelStr.RATIO),
                out _damageReduceRatio);

            Cond.Instance.GetData(entity, Label.ENERGY + Label.ING, out _chargyingEnergyData);

            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.INVINCIBLE, Label.ING), out _invincibleData);

            Cond.Instance.GetData(entity, LabelStr.BURN, out _burn);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BURN, LabelStr.ATTACK),
                out _burnAttack);

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.IGNORE, LabelStr.BURN), out _ignoreBurn);
            
            MessageRegister.Instance.Reg<float>(MessageCode.MsgDamagePlayer, MsgBeDamaged);
            MessageRegister.Instance.Reg<float>(MessageCode.MsgRecoverHealth, MsgBeRecovered);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgBurnEntity, BurnPlayer);
            Game.instance.OnLateUpdateEvent.AddListener(OnUpdate);
        }

        private void BurnPlayer(int entityID) {
            if (entity.ID == entityID) {
                //燃烧几秒后 判断是否燃烧区域中
                burnDeploy = 1;
                //燃烧特效
            }
        }

        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            _healthBar.value = _healthData.Float / _maxHealthData.Float;
            if (_chargyingEnergyData.Bool) {
                BeRecoveredHealth(_healthRecoverSpeed.Float * Time.deltaTime);
            }

            Burn();
        }

        private void Burn() {
            if (_ignoreBurn.Bool) {
                return;
            }

            if (burnDeploy > 0) {
                burnDeploy -= Time.deltaTime;
                MsgBeDamaged(_burnAttack.Float * Time.deltaTime);
                _burn.Bool = true;
            } else {
                _burn.Bool = false;
            }
        }

        private void MsgBeRecovered(float recoverValue) {
            BeRecoveredHealth(recoverValue);
            Debug.Log("恢复血量:" + recoverValue + " 当前血量:" + _healthData.Float);
        }

        private void BeRecoveredHealth(float recoverValue) {
            if (_healthData.Float == _maxHealthData.Float || recoverValue == 0) {
                return;
            }

            if (_healthData.Float < _maxHealthData.Float) {
                _healthData.Float += recoverValue;
            }

            if (_healthData.Float > _maxHealthData.Float) {
                _healthData.Float = _maxHealthData.Float;
            }
        }

        private void MsgBeDamaged(float damageValue) {
            if (_invincibleData.Bool) {
                return;
            }

            float damageRatio = _damageReduceRatio.Float;
            if (_healthData.Float != 0) {
                if (_healthData.Float > 0) {
                    _healthData.Float -= damageValue * damageRatio;
                }

                if (_healthData.Float <= 0) {
                    _healthData.Float = 0;
                    Next();
                }

                Debug.Log("玩家受到伤害:" + damageValue * damageRatio + " 当前血量:" + _healthData.Float);
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
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgBurnEntity, BurnPlayer);
        }
    }
}