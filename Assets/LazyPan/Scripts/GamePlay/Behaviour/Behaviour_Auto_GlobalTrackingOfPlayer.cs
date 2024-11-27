using UnityEngine;
using UnityEngine.AI;


namespace LazyPan {
    public class Behaviour_Auto_GlobalTrackingOfPlayer : Behaviour {
        private FloatData _movementSpeedData;
        private FloatData _speedConversionRatio;
        private FloatData _globalSpeedConversionRatio;
        private BoolData _frost;
        private BoolData _frozen;
        private FloatData _frostRatio;
        private FloatData _healthData;
        private NavMeshAgent _navMeshAgent;
        private Entity _playerEntity;
        private Transform _playerBody;
        private Vector3 _playerBodyPos;
        
        private float _updateInterval = 0.1f;  // 更新间隔
        private float _timeSinceLastUpdate = 0f;
        public Behaviour_Auto_GlobalTrackingOfPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _navMeshAgent = Cond.Instance.Get<NavMeshAgent>(entity, LabelStr.NAVMESHAGENT);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out _movementSpeedData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SPEED, LabelStr.CONVERSION, LabelStr.RATIO),
                out _speedConversionRatio);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.SPEED, LabelStr.CONVERSION, LabelStr.RATIO),
                out _globalSpeedConversionRatio);
            Cond.Instance.GetData(entity, LabelStr.FROST, out _frost);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FROST, LabelStr.RATIO), out _frostRatio);
            _playerEntity = Cond.Instance.GetPlayerEntity();
            _playerBody = Cond.Instance.Get<Transform>(_playerEntity, LabelStr.BODY);
            _navMeshAgent.SetDestination(_playerBody.position);
            Cond.Instance.GetData(entity, LabelStr.FROZEN, out _frozen);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            float movementSpeed = _movementSpeedData.Float;
            if (_globalSpeedConversionRatio != null && _globalSpeedConversionRatio.Float > 0) {
                movementSpeed = _globalSpeedConversionRatio.Float * _healthData.Float;
            } else if (_speedConversionRatio != null) {
                movementSpeed = _speedConversionRatio.Float * _healthData.Float;
            }
            _navMeshAgent.speed = movementSpeed * (_frost.Bool ? _frostRatio.Float : 1);
            _navMeshAgent.speed = _frozen.Bool ? 0 : _navMeshAgent.speed;

            _timeSinceLastUpdate += Time.deltaTime;
            if (_timeSinceLastUpdate >= _updateInterval) {
                if (_playerBodyPos != _playerBody.position && _navMeshAgent.isOnNavMesh) {
                    _navMeshAgent.SetDestination(_playerBody.position);
                    _playerBodyPos = _playerBody.position; // 更新上次位置
                }
                _timeSinceLastUpdate = 0f;  // 重置计时器
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}