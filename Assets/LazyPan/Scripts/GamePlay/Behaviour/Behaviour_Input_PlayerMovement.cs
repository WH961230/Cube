using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_PlayerMovement : Behaviour {
        private float _movementSpeed;//移动速度
        private Vector3 _movementDirection;//移动方向
        private Vector2 _inputMovementValue;
        private CharacterController _characterController;
        public Behaviour_Input_PlayerMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            _movementSpeed = 5;
            InputRegister.Instance.Load(InputRegister.Motion, InputMotionEvent);
            Data.Instance.OnUpdateEvent.AddListener(OnMovementUpdate);
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
                _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnMovementUpdate);
            InputRegister.Instance.UnLoad(InputRegister.Motion, InputMotionEvent);
        }
    }
}