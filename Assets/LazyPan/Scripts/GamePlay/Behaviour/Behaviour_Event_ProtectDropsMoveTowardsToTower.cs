using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_ProtectDropsMoveTowardsToTower : Behaviour {
        private BoolData _chargeEnergy;
        private BoolData _movePlayer;
        public Behaviour_Event_ProtectDropsMoveTowardsToTower(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //是否开始移动到玩家
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(),
                Label.Assemble(LabelStr.EXPERIENCE, LabelStr.MOVE, LabelStr.PLAYER), out _movePlayer);
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), Label.Assemble(LabelStr.ENERGY, Label.ING),
                out _chargeEnergy);
            //是否开始移动到玩家
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
        }

        private void OnUpdate() {
            if (_chargeEnergy.Bool) {
                _movePlayer.Bool = true;
            } else {
                _movePlayer.Bool = false;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}