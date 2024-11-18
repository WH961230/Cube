using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LazyPan {
    public class Behaviour_Auto_GlobalTrackingOfTower : Behaviour {
        private FloatData _movementSpeedData;
        private BoolData _frost;
        private FloatData _frostRatio;
        private NavMeshAgent _navMeshAgent;
        private Entity _towerEntity;
        private Transform _towerBody;
        
        public Behaviour_Auto_GlobalTrackingOfTower(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            _navMeshAgent = Cond.Instance.Get<NavMeshAgent>(entity, LabelStr.NAVMESHAGENT);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out _movementSpeedData);
            Cond.Instance.GetData(entity, LabelStr.FROST, out _frost);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FROST, LabelStr.RATIO), out _frostRatio);
            if(EntityRegister.TryGetEntitiesByType("Tower", out List<Entity> towerEntityList)) {
                _towerEntity = towerEntityList[0];
            }
            _towerBody = Cond.Instance.Get<Transform>(_towerEntity, LabelStr.BODY);
            _navMeshAgent.SetDestination(_towerBody.position);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            _navMeshAgent.speed = _movementSpeedData.Float * (_frost.Bool ? _frostRatio.Float : 1);
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}