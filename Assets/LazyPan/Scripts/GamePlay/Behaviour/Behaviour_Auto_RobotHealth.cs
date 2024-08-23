using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_RobotHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private FloatData _beheadHealthRatio;
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        private BoolData _burning;
        private BoolData _frosting;
        private FloatData _autoAbsorbExperienceRatio;//自动吸收经验值概率
        private FloatData _burnTime;
        private FloatData _burnAttack;
        private FloatData _frostTime;
        private FloatData _frostSlowRatio;
        private StringData _beHitSound;
        private Image _healthImg;
        private float burnDeploy;
        private float frostDeploy;
        public Behaviour_Auto_RobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            _healthImg = Cond.Instance.Get<Image>(entity, LabelStr.HEALTH);
            
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            _healthData.Float = _maxHealthData.Float;

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BE, LabelStr.HIT, LabelStr.SOUND),
                out _beHitSound);
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.BEHEAD, LabelStr.HEALTH, LabelStr.RATIO), out _beheadHealthRatio);
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(),
                LabelStr.Assemble(LabelStr.AUTO, LabelStr.ABSORB, LabelStr.EXPERIENCE, LabelStr.RATIO),
                out _autoAbsorbExperienceRatio);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BURN, Label.ING), out _burning);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BURN, Label.ING), out _frosting);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BURN, LabelStr.TIME),
                out _burnTime);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BURN, LabelStr.ATTACK),
                out _burnAttack);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.FROST, LabelStr.TIME),
                out _frostTime);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.FROST, LabelStr.SLOW, LabelStr.RATIO),
                out _frostSlowRatio);
            burnDeploy = _burnTime.Float;
            frostDeploy = _frostTime.Float;
            MessageRegister.Instance.Reg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgBurnRobot, Burn);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgFrostRobot, Frost);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            if (_healthImg != null) {
                _healthImg.fillAmount = _healthData.Float / _maxHealthData.Float;
            }

            OnBurnAndFrost();
        }
        
        //燃烧 燃烧时间完成后再次检测
        private void Burn(int entityId) {
            //燃烧几秒后 判断是否燃烧区域中
        }

        //冰霜
        private void Frost(int entityId) {
            //冰霜减速几秒 判断是否在减速区域中
        }

        private void OnBurnAndFrost() {
            if (_burning.Bool) {
                if (burnDeploy > 0) {
                    burnDeploy -= Time.deltaTime;
                    BeDamaged(entity.ID, _burnAttack.Float);
                } else {
                    burnDeploy = 0;
                }
            }

            if (_frosting.Bool) {
                if (frostDeploy > 0) {
                    frostDeploy -= Time.deltaTime;
                    //_frostSlowRatio.Float 减速系数 设置全局
                } else {
                    frostDeploy = 0;
                }
            }
        }

        private void BeDamaged(int entityId, float damageValue) {
            if (entity.ID == entityId) {
                if (_healthData.Float != 0) {
                    if (_healthData.Float > 0) {
                        //斩杀最小生命值低于系数的怪物
                        if (_healthData.Float < _maxHealthData.Float * _beheadHealthRatio.Float) {
                            damageValue = 9999;
                        }
                        _healthData.Float -= damageValue;
                        Sound.Instance.SoundPlay(_beHitSound.String, Vector3.zero, false, 2);
                    }

                    if (_healthData.Float <= 0) {
                        _healthData.Float = 0;
                        Dead(damageValue != Mathf.Infinity);
                    }
                } 
            }
        }

        private void Dead(bool isDrop) {
            if (isDrop) {
                DeathDropExperience();
            }
            MessageRegister.Instance.Dis(MessageCode.MsgRobotDead, entity.ID);
        }

        private void DeathDropExperience() {
            //掉落概率 1 - 100
            int randNum = Random.Range(0, 100);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.DROP, LabelStr.RATIO),
                out IntData intData);
            if (randNum <= intData.Int) {
                Entity drop = Obj.Instance.LoadEntity("A11_物体_可拾取_经验值");
                drop.SetBeginLocationInfo(new LocationInformationData() {
                    Position = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position
                });

                //有概率直接吸收
                float randAbsorb = Random.Range(0, 1);
                if (randAbsorb > 0 && randAbsorb < _autoAbsorbExperienceRatio.Float) {
                    if (BehaviourRegister.GetBehaviour(drop.ID,
                            out Behaviour_Trigger_PickUpExperiencePoint behaviour)) {
                        behaviour.Pick(Cond.Instance.GetPlayerEntity());
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgBurnRobot, Burn);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgFrostRobot, Frost);
        }
    }
}