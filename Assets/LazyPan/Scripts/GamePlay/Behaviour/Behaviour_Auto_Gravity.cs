using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_Gravity : Behaviour {
        private CharacterController _characterController;
        private float _gravitySpeed;
        public Behaviour_Auto_Gravity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            _gravitySpeed = 5;
            Game.instance.OnLateUpdateEvent.AddListener(OnGravityUpdate);
        }

        public override void DelayedExecute() {
            
        }

        private void OnGravityUpdate() {
            if (_characterController != null) {
                _characterController.Move(Vector3.down * _gravitySpeed * Time.fixedDeltaTime);
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnLateUpdateEvent.RemoveListener(OnGravityUpdate);
        }
    }
}