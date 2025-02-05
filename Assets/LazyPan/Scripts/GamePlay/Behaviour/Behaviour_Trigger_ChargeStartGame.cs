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
        private Image _energyRangeImage;
        private StringData _chargeSoundData;
        private GameObject soundGo;

        public Behaviour_Trigger_ChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flowSceneA);

            Cond.Instance.GetData(entity, Label.ENERGY, out _energyData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.MAX, out _energyMaxData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.SPEED, out _energySpeedData);
            Cond.Instance.GetData(entity, Label.ENERGY + Label.ING, out _isChargingEnergyData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.CHARGE, LabelStr.SOUND), out _chargeSoundData);

            _energyImage = Cond.Instance.Get<Image>(entity, Label.ENERGY);
            _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
            
            _energyRangeImage = Cond.Instance.Get<Image>(entity, Label.ENERGY + LabelStr.RANGE);

            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(ChargeIn);
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerExitEvent.AddListener(ChargeOut);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            if (_isChargingEnergyData.Bool) {
                _energyData.Float += _energySpeedData.Float * Time.deltaTime;
                if (_energyData.Float >= _energyMaxData.Float) {
                    Sound.Instance.SoundRecycle(soundGo);
                    Next();
                    return;
                }

                if (_energyImage.gameObject.activeSelf) {
                    _energyImage.fillAmount = _energyData.Float / _energyMaxData.Float;
                }
            } else {
                _energyImage.gameObject.SetActive(false);
                _energyData.Float = 0;
            }
            
            _energyRangeImage.gameObject.SetActive(_isChargingEnergyData.Bool);
        }

        private void Next() {
            _flowSceneA.Next("SceneB");
        }

        private void ChargeIn(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    _isChargingEnergyData.Bool = true;
                    _energyImage.gameObject.SetActive(true);
                    soundGo = Sound.Instance.SoundPlay(_chargeSoundData.String, Vector3.zero, true, -1);
                    Cond.Instance.GetEvent(entity, "开始充能")?.Invoke();
                }
            }
        }

        private void ChargeOut(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(),
                    out Entity playerEntity)) {
                if (playerEntity.Type == "Player") {
                    _isChargingEnergyData.Bool = false;
                    _energyImage.gameObject.SetActive(false);
                    _energyData.Float = 0;
                    Sound.Instance.SoundRecycle(soundGo);
                    Cond.Instance.GetEvent(entity, "结束充能")?.Invoke();
                }
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