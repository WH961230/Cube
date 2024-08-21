using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ProtectWhenEnergyIsFullIncreaseAttack : Behaviour {
        private FloatData _towerEnergy;
        private FloatData _towerEnergyMax;
        private FloatData _attackRatio;
        private float attackRatio;
        private bool isTrigger;
        public Behaviour_Event_ProtectWhenEnergyIsFullIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            EntityRegister.TryGetRandEntityByType("Tower", out Entity towerEntity);
            Cond.Instance.GetData(towerEntity, LabelStr.ENERGY, out _towerEnergy);
            Cond.Instance.GetData(towerEntity, LabelStr.Assemble(LabelStr.ENERGY, LabelStr.MAX), out _towerEnergyMax);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out _attackRatio);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
        }

        private void OnUpdate() {
            if (_towerEnergy.Float == _towerEnergyMax.Float) {
                if (!isTrigger) {
                    _attackRatio.Float += 0.2f;
                    isTrigger = true;
                }
            } else {
                if (isTrigger) {
                    _attackRatio.Float -= 0.2f;
                    isTrigger = false;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}