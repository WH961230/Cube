using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame : Behaviour {
        private Flow_SceneA _flowSceneA;
        private bool _isChargingEnergy;
        private float _energy;
        private float _energyMax;
        private float _energyChargeSpeed;
        private Image _energyImage;

        public Behaviour_Trigger_ChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowSceneA);

            _energy = 0;
            _energyMax = 10;
            _energyChargeSpeed = 5;
            _energyImage = Cond.Instance.Get<Image>(entity, Cube.Label.ENERGY);
            _energyImage.fillAmount = _energy / _energyMax;

            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (_isChargingEnergy) {
                _energy += _energyChargeSpeed * Time.deltaTime;
                if (_energy >= _energyMax) {
                    _flowSceneA.Next("SceneB");
                    return;
                }

                if (_energyImage.gameObject.activeSelf) {
                    _energyImage.fillAmount = _energy / _energyMax;
                }
            } else {
                _energyImage.gameObject.SetActive(false);
                _energy = 0;
            }
        }

        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    _isChargingEnergy = true;
                    _energyImage.gameObject.SetActive(true);
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    _isChargingEnergy = false;
                    _energyImage.gameObject.SetActive(false);
                    _energy = 0;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.RemoveListener(ChargeOut);
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}