using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_PlayerMovement : Behaviour {
        private FloatData _movementSpeedData;//移动速度
        private Vector2 _inputMovementValue;
        private Vector3Data _moveDirectionData;//移动方向

        private BoolData _moveData;
        private BoolData _teleportData;
        private BoolData _knockbackData;
        private BoolData _invincibleData;

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
            Cond.Instance.GetData(entity, Label.Assemble(Label.MOVEMENT, Label.SPEED), out _movementSpeedData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.MOVEMENT, Label.ING), out _moveData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.TELEPORT, Label.ING), out _teleportData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.KNOCKBACK, Label.ING), out _knockbackData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.INVINCIBLE, Label.ING), out _invincibleData);
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.MOVEMENT, LabelStr.DIRECTION), out _moveDirectionData);

            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
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
            return Vector3.forward * _inputMovementValue.y + Vector3.right * _inputMovementValue.x;
        }

        private void OnUpdate() {
            Movement();
            Knockback();
        }

        private void Movement() {
            if (_characterController != null) {
                //传送中取消移动控制
                if (_teleportData.Bool || _knockbackData.Bool) {
                    _moveData.Bool = false;
                    return;
                }

                Vector3 moveDirection = GetMovementDirection();
                _characterController.Move(moveDirection * _movementSpeedData.Float * Time.deltaTime);
                if (_characterController.velocity.normalized != Vector3.zero) {
                    _moveDirectionData.Vector3 = _characterController.velocity.normalized;
                }

                _moveData.Bool = moveDirection != Vector3.zero;
            }
        }

        private void Knockback() {
            if (_characterController != null) {
                if (_invincibleData.Bool) {
                    _knockbackData.Bool = false;
                    return;
                }
                //击退中无法移动无法瞬移
                if (_knockbackSpeed > 0) {
                    _knockbackSpeed += _knockbackAcceleration * Time.deltaTime;
                    _characterController.Move(_knockbackDirection * _knockbackSpeed * Time.deltaTime);
                } else {
                    _knockbackSpeed = 0;
                }

                _knockbackData.Bool = _knockbackSpeed != 0;
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