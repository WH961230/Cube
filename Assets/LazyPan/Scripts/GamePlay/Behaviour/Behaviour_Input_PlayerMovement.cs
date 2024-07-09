using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_PlayerMovement : Behaviour {
        private FloatData _movementSpeedData;//移动速度
        private Vector3 _movementDirection;//移动方向
        private Vector2 _inputMovementValue;

        private Vector3 _knockbackDirection;
        private FloatData _knockbackSpeedSetting;
        private FloatData _knockbackAccelerationSetting;

        private float _knockbackSpeed;
        private float _knockbackAcceleration;

        private CharacterController _characterController;
        public Behaviour_Input_PlayerMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            InputRegister.Instance.Load(InputRegister.Motion, InputMotionEvent);
            MessageRegister.Instance.Reg<Vector3>(MessageCode.MsgKnockbackPlayer, KnockbackPlayer);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.KNOCKBACK, LabelStr.SPEED), out _knockbackSpeedSetting);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.KNOCKBACK, LabelStr.ACCELERATION), out _knockbackAccelerationSetting);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void KnockbackPlayer(Vector3 direction) {
            _knockbackDirection = direction;
            _knockbackSpeed = _knockbackSpeedSetting.Float;
            _knockbackAcceleration = _knockbackAccelerationSetting.Float;
        }

        private void InputMotionEvent(InputAction.CallbackContext obj) {
            _inputMovementValue = obj.ReadValue<Vector2>();
        }

        private Vector3 GetMovementDirection() {
            _movementDirection = Vector3.forward * _inputMovementValue.y + Vector3.right * _inputMovementValue.x;
            return _movementDirection;
        }

        private void OnUpdate() {
            Movement();
            Knockback();
        }

        private void Movement() {
            if (_characterController != null) {
                Vector3 moveDirection = GetMovementDirection();
                Cond.Instance.GetData(entity, Label.Assemble(Label.MOVEMENT, Label.SPEED), out _movementSpeedData);
                _characterController.Move(moveDirection * _movementSpeedData.Float * Time.deltaTime);
            }
        }

        private void Knockback() {
            if (_characterController != null) {
                if (_knockbackSpeedSetting != null) {
                    if (_knockbackSpeed > 0) {
                        _knockbackSpeed += _knockbackAcceleration * Time.deltaTime;
                        _characterController.Move(_knockbackDirection * _knockbackSpeed * Time.deltaTime);
                    } else {
                        _knockbackSpeed = 0;
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<Vector3>(MessageCode.MsgKnockbackPlayer, KnockbackPlayer);
            InputRegister.Instance.UnLoad(InputRegister.Motion, InputMotionEvent);
        }
    }
}