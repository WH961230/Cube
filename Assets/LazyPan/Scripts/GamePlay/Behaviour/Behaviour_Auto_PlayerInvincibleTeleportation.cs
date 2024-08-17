using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Auto_PlayerInvincibleTeleportation : Behaviour {
        private Transform _body;

        private float _teleportAcceleration;//加速度
        private float _teleportSpeed;//冲刺初始速度

        private BoolData _teleportData;
        private BoolData _knockbackData;
        private BoolData _invincibleData;
        private FloatData _teleportSpeedData;
        private FloatData _teleportAccelerationData;
        private StringData _teleportSoundData;
        private Vector3Data _moveDirectionData;

        private CharacterController _characterController;

        private Vector3 lastVelocity;

        public Behaviour_Auto_PlayerInvincibleTeleportation(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            _characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);

            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            InputRegister.Instance.Load(InputRegister.Space, InputInvincibleTeleportation);

            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.TELEPORT, Label.ING), out _teleportData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.KNOCKBACK, Label.ING), out _knockbackData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.MOVEMENT, LabelStr.DIRECTION), out _moveDirectionData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.INVINCIBLE, Label.ING), out _invincibleData);

            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.TELEPORT, Label.SPEED), out _teleportSpeedData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.TELEPORT, LabelStr.ACCELERATION), out _teleportAccelerationData);

            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.TELEPORT, LabelStr.SOUND), out _teleportSoundData);
        }
        
        public override void DelayedExecute() {
            
        }

        private void InputInvincibleTeleportation(InputAction.CallbackContext obj) {
            if (obj.started) {
                if (!_teleportData.Bool) {
                    _teleportSpeed = _teleportSpeedData.Float;
                    _teleportAcceleration = _teleportAccelerationData.Float;
                    Sound.Instance.SoundPlay(_teleportSoundData.String, Vector3.zero, false, 2);
                }
            }
        }

        private void OnUpdate() {
            if (_characterController != null) {
                Vector3 moveDirection = _moveDirectionData.Vector3;
                if (moveDirection == Vector3.zero) {
                    moveDirection = _body.forward;
                }
                moveDirection.y = 0;

                if (_teleportSpeed > 0) {
                    _teleportSpeed += _teleportAcceleration * Time.deltaTime;
                    _characterController.Move(moveDirection * _teleportSpeed * Time.deltaTime);
                } else {
                    _teleportSpeed = 0;
                }

                _teleportData.Bool = _teleportSpeed != 0;
                _invincibleData.Bool = _teleportData.Bool;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            InputRegister.Instance.UnLoad(InputRegister.Space, InputInvincibleTeleportation);
        }
    }
}