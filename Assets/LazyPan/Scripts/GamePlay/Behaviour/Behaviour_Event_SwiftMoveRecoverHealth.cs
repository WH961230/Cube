using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SwiftMoveRecoverHealth : Behaviour {
        private FloatData _healthMax;
        private FloatData _ratio;
        private BoolData _movementing;
        private float moveTime;
        private bool isMoving;
        public Behaviour_Event_SwiftMoveRecoverHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, Label.ING), out _movementing);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _healthMax);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.HEALTH, LabelStr.RECOVER, LabelStr.RATIO), out _ratio);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            moveTime = 1;
        }

        private void OnUpdate() {
            if (_movementing.Bool) {
                if (!isMoving) {
                    isMoving = true;
                    moveTime = 1;
                }
            } else {
                isMoving = false;
            }

            if (isMoving) {
                if (moveTime > 0) {
                    moveTime -= Time.deltaTime;
                } else {
                    MessageRegister.Instance.Dis(MessageCode.MsgRecoverHealth, entity.ID, _healthMax.Float * _ratio.Float);
                    moveTime = 1;
                }
            } else {
                moveTime = 0;
            }
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}