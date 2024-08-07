﻿using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TowerChargeEnergy : Behaviour {
        private Flow_SceneB _flowSceneB;
        private BoolData _isChargingEnergyData;
        private FloatData _energyData;
        private FloatData _energyMaxData;
        private FloatData _energySpeedData;
        private FloatData _energySpeedDownData;
        private Image _energyImage;

        public Behaviour_Auto_TowerChargeEnergy(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowSceneB);

            Cond.Instance.GetData(entity, Label.ENERGY, out _energyData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, Label.MAX), out _energyMaxData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, Label.SPEED), out _energySpeedData);
            Cond.Instance.GetData(entity,  LabelStr.Assemble(Label.ENERGY, LabelStr.DOWN, Label.SPEED), out _energySpeedDownData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY ,Label.ING), out _isChargingEnergyData);

            _energyImage = Cond.Instance.Get<Image>(entity, Label.ENERGY);
            _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;

            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
        }

        private void ChargeIn(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    _isChargingEnergyData.Bool = true;
                    _energyImage.gameObject.SetActive(true);
                    Cond.Instance.GetData(playerEntity, Label.ENERGY + Label.ING, out BoolData playerIsChargingEnergyData);
                    playerIsChargingEnergyData.Bool = true;
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    _isChargingEnergyData.Bool = false;
                    Cond.Instance.GetData(playerEntity, Label.ENERGY + Label.ING, out BoolData playerIsChargingEnergyData);
                    playerIsChargingEnergyData.Bool = false;
                }
            }
        }

        private void OnUpdate() {
            if (_isChargingEnergyData.Bool) {
                _energyData.Float += _energySpeedData.Float * Time.deltaTime;
                if (_energyData.Float > _energyMaxData.Float) {
                    _energyData.Float = _energyMaxData.Float;
                }
            } else {
                _energyData.Float -= _energySpeedDownData.Float * Time.deltaTime;
                if (_energyData.Float < 0) {
                    _energyImage.gameObject.SetActive(false);
                    _energyData.Float = 0;
                }
            }

            if (_energyImage.gameObject.activeSelf) {
                _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.RemoveListener(ChargeOut);
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}