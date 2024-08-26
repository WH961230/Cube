using Cinemachine;
using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ProtectIncreaseTowerViewRange : Behaviour {
        private BoolData _energying;
        private CinemachineVirtualCamera virtualCamera;
        public Behaviour_Event_ProtectIncreaseTowerViewRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ENERGY, Label.ING), out _energying);
            virtualCamera = Cond.Instance.Get<Transform>(Cond.Instance.GetCameraEntity(), Label.CAMERA).GetComponent<CinemachineVirtualCamera>();
            Game.instance.OnUpdateEvent.AddListener(OnViewUpdate);
        }

        private void OnViewUpdate() {
            if (_energying.Bool) {
                virtualCamera.m_Lens.OrthographicSize = 20;
            } else {
                virtualCamera.m_Lens.OrthographicSize = 12;
            }
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnViewUpdate);
        }
    }
}