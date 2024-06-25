using Cinemachine;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_CameraPlayerView : Behaviour {
        private Entity _playerEntity;
        private CinemachineVirtualCamera _virtualCamera;
        public Behaviour_Auto_CameraPlayerView(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _virtualCamera = Cond.Instance.Get<Transform>(entity, Label.CAMERA).GetComponent<CinemachineVirtualCamera>();
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (_playerEntity == null) {
                _playerEntity = Cond.Instance.GetPlayerEntity();
            } else {
                if (_virtualCamera.m_Follow == null) {
                    _virtualCamera.m_Follow = Cond.Instance.Get<Transform>(_playerEntity, Label.BODY);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}