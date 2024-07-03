using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGun : Behaviour {
        private Transform _foot;
        private Transform _towerFoot;
        public Behaviour_Auto_SubmachineGun(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Debug.Log("冲锋枪");
            _foot = Cond.Instance.Get<Transform>(entity, LabelStr.FOOT);
            EntityRegister.TryGetRandEntityByType("Tower", out Entity _tower);
            _towerFoot = Cond.Instance.Get<Transform>(_tower, LabelStr.FOOT);
            Game.instance.OnLateUpdateEvent.AddListener(OnLateUpdate);
        }

        private void OnLateUpdate() {
            if (_foot != null && _towerFoot != null) {
                _foot.position = _towerFoot.position;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnLateUpdateEvent.RemoveListener(OnLateUpdate);
        }
    }
}