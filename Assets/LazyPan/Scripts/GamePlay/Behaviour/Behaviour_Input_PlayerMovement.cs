using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_PlayerMovement : Behaviour {
        private FloatData _movementSpeedData;//移动速度
        private Vector3 _movementDirection;//移动方向
        private Vector2 _inputMovementValue;
        private CharacterController _characterController;
        public Behaviour_Input_PlayerMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            InputRegister.Instance.Load(InputRegister.Motion, InputMotionEvent);
            Game.instance.OnUpdateEvent.AddListener(OnMovementUpdate);
        }

        private void InputMotionEvent(InputAction.CallbackContext obj) {
            _inputMovementValue = obj.ReadValue<Vector2>();
        }

        private Vector3 GetMovementDirection() {
            _movementDirection = Vector3.forward * _inputMovementValue.y + Vector3.right * _inputMovementValue.x;
            return _movementDirection;
        }

        private void OnMovementUpdate() {
            if (_characterController != null) {
                Vector3 moveDirection = GetMovementDirection();
                 Cond.Instance.GetData(entity, Label.Assemble(Label.MOVEMENT, Label.SPEED), out _movementSpeedData);
                _characterController.Move(moveDirection * _movementSpeedData.Float * Time.deltaTime);
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnMovementUpdate);
            InputRegister.Instance.UnLoad(InputRegister.Motion, InputMotionEvent);
        }
    }
}