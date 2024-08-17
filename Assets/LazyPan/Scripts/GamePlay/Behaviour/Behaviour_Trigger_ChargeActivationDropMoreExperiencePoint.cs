using UnityEngine;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeActivationDropMoreExperiencePoint : Behaviour {
        private Flow_SceneB _flowSceneB;
        private BoolData _isChargingEnergyData;
        private FloatData _energyData;
        private FloatData _energyMaxData;
        private FloatData _energyChargeSpeedData;
        private FloatData _energyDownSpeedData;
        private Image _energyImage;
        private StringData _chargeSoundData;
        private GameObject soundGo;
        public Behaviour_Trigger_ChargeActivationDropMoreExperiencePoint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, Label.ENERGY, out _energyData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, Label.MAX), out _energyMaxData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, LabelStr.CHARGE, Label.SPEED), out _energyChargeSpeedData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, LabelStr.DOWN, Label.SPEED), out _energyDownSpeedData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(Label.ENERGY, Label.ING), out _isChargingEnergyData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.CHARGE, LabelStr.SOUND), out _chargeSoundData);
            
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
                    StartCharge();
                    soundGo = Sound.Instance.SoundPlay(_chargeSoundData.String, Vector3.zero, true, -1);
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    _isChargingEnergyData.Bool = false;
                    Sound.Instance.SoundRecycle(soundGo);
                }
            }
        }

        private void StartCharge() {
            _isChargingEnergyData.Bool = true;
            _energyImage.gameObject.SetActive(true);
            Debug.Log("开始充能掉落大量经验值可激活物体");
        }

        private void OnUpdate() {
            if (_isChargingEnergyData.Bool) {
                _energyData.Float += _energyChargeSpeedData.Float * Time.deltaTime;
                if (_energyData.Float >= _energyMaxData.Float) {
                    Sound.Instance.SoundRecycle(soundGo);
                    Debug.Log("充能掉落大量经验值可激活物体完成");
                    DeathDropExperience();
                    Obj.Instance.UnLoadEntity(entity);
                    return;
                }
            } else {
                _energyData.Float -= _energyDownSpeedData.Float * Time.deltaTime;
                if (_energyData.Float < 0) {
                    _energyImage.gameObject.SetActive(false);
                    _energyData.Float = 0;
                }
            }

            if (_energyImage.gameObject.activeSelf) {
                _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
            }
        }

        private void DeathDropExperience() {
            int randNum = Random.Range(5, 10);
            while (randNum > 0) {
                Entity drop = Obj.Instance.LoadEntity("A11_物体_可拾取_经验值");
                drop.SetBeginLocationInfo(new LocationInformationData() {
                    Position = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position
                });
                randNum--;
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