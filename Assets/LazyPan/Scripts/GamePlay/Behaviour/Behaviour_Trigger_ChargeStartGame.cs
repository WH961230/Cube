using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame : Behaviour {
        private Flow_SceneA _flowSceneA;
        private BoolData _isChargingEnergyData;
        private FloatData _energyData;
        private FloatData _energyMaxData;
        private FloatData _energySpeedData;
        private Image _energyImage;

        public Behaviour_Trigger_ChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowSceneA);

            entity.Comp.GetComponent<CubeData>().GetData(Cube.Label.ENERGY, out _energyData);
            entity.Comp.GetComponent<CubeData>().GetData(Cube.Label.ENERGY + Cube.Label.MAX, out _energyMaxData);
            entity.Comp.GetComponent<CubeData>().GetData(Cube.Label.ENERGY + Cube.Label.SPEED, out _energySpeedData);
            entity.Comp.GetComponent<CubeData>().GetData(Cube.Label.ENERGY + Cube.Label.ING, out _isChargingEnergyData);

            _energyImage = Cond.Instance.Get<Image>(entity, Cube.Label.ENERGY);
            _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;

            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (_isChargingEnergyData.Bool) {
                _energyData.Float += _energySpeedData.Float * Time.deltaTime;
                if (_energyData.Float >= _energyMaxData.Float) {
                    _flowSceneA.Next("SceneB");
                    return;
                }

                if (_energyImage.gameObject.activeSelf) {
                    _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
                }
            } else {
                _energyImage.gameObject.SetActive(false);
                _energyData.Float = 0;
            }
        }

        private void ChargeIn(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    _isChargingEnergyData.Bool = true;
                    _energyImage.gameObject.SetActive(true);
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.EntityData.BaseRuntimeData.Type == "Player") {
                    _isChargingEnergyData.Bool = false;
                    _energyImage.gameObject.SetActive(false);
                    _energyData.Float = 0;
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