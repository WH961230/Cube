using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeActivation : Behaviour {
        private Flow_SceneB _flowSceneB;
        private BoolData _isChargingEnergyData;
        private FloatData _energyData;
        private FloatData _energyMaxData;
        private FloatData _energySpeedData;
        private GameObject _body;
        private Image _energyImage;
        public Behaviour_Trigger_ChargeActivation(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowSceneB);
            _body = Cond.Instance.Get<Transform>(base.entity, Label.BODY).gameObject;
            Cond.Instance.GetData(entity, Label.ENERGY, out _energyData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.MAX, out _energyMaxData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.SPEED, out _energySpeedData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.ING, out _isChargingEnergyData);
            
            _energyImage = Cond.Instance.Get<Image>(entity, Label.ENERGY);
            _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
            
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);

            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void ChargeIn(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    StartCharge();
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    CancelCharge();
                }
            }
        }

        private void StartCharge() {
            _isChargingEnergyData.Bool = true;
            _energyImage.gameObject.SetActive(true);
        }
        
        private void CancelCharge() {
            _isChargingEnergyData.Bool = false;
            _energyImage.gameObject.SetActive(false);
            _energyData.Float = 0;
        }

        private void OnUpdate() {
            if (_isChargingEnergyData.Bool) {
                _energyData.Float += _energySpeedData.Float * Time.deltaTime;
                if (_energyData.Float >= _energyMaxData.Float) {
                    CancelCharge();
                    MessageRegister.Instance.Dis(MessageCode.MsgLevelUp);
                    Obj.Instance.UnLoadEntity(entity);
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
        
        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.RemoveListener(ChargeOut);
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}