﻿using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_RobotHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        private Image _healthImg;
        public Behaviour_Auto_RobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            _healthImg = Cond.Instance.Get<Image>(entity, LabelStr.HEALTH);
            
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            _healthData.Float = _maxHealthData.Float;

            MessageRegister.Instance.Reg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (_healthImg != null) {
                _healthImg.fillAmount = _healthData.Float / _maxHealthData.Float;
            }
        }

        private void BeDamaged(int entityId, float damageValue) {
            if (entity.ID == entityId) {
                if (_healthData.Float != 0) {
                    if (_healthData.Float > 0) {
                        _healthData.Float -= damageValue;
                    }

                    if (_healthData.Float <= 0) {
                        _healthData.Float = 0;
                        Dead();
                    }
                } 
            }
        }

        private void Dead() {
            Entity drop = Obj.Instance.LoadEntity("Obj_PickUp_ExperiencePoint");
            drop.SetBeginLocationInfo(new LocationInformationData() {
                Position = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position
            });
            Obj.Instance.UnLoadEntity(entity);
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
        }
    }
}