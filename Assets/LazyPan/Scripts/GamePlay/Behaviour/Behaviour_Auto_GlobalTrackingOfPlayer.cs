using UnityEngine;
using UnityEngine.AI;


namespace LazyPan {
    public class Behaviour_Auto_GlobalTrackingOfPlayer : Behaviour {
        private FloatData _movementSpeedData;
        private BoolData _frost;
        private FloatData _frostRatio;
        private NavMeshAgent _navMeshAgent;
        private Entity _playerEntity;
        private Transform _playerBody;
        public Behaviour_Auto_GlobalTrackingOfPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _navMeshAgent = Cond.Instance.Get<NavMeshAgent>(entity, LabelStr.NAVMESHAGENT);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out _movementSpeedData);
            Cond.Instance.GetData(entity, LabelStr.FROST, out _frost);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FROST, LabelStr.RATIO), out _frostRatio);
            _playerEntity = Cond.Instance.GetPlayerEntity();
            _playerBody = Cond.Instance.Get<Transform>(_playerEntity, LabelStr.BODY);
            _navMeshAgent.SetDestination(_playerBody.position);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            _navMeshAgent.speed = _movementSpeedData.Float * (_frost.Bool ? _frostRatio.Float : 1);
            if (Vector3.Distance(_navMeshAgent.destination, _playerBody.position) > 0.1f) {
                _navMeshAgent.SetDestination(_playerBody.position);
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}