using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SwiftMoveIncreaseAttack : Behaviour {
        private FloatData _attackRatio;
        private FloatData _attackMaxRatio;
        private BoolData _movementing;
        private float moveTime;
        private bool isMoving;
        private float addAttackDamageExtra;
        private float addAttackDamageExtraEach;

        public Behaviour_Event_SwiftMoveIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MOVEMENT, Label.ING), out _movementing);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out _attackRatio);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.MAX, LabelStr.RATIO), out _attackMaxRatio);
            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
            moveTime = 1;
        }

        public override void DelayedExecute() {
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
                    if (addAttackDamageExtra < _attackMaxRatio.Float) {
                        addAttackDamageExtraEach = 0.01f;
                        float preExtra = addAttackDamageExtra + addAttackDamageExtraEach;
                        if (preExtra > _attackMaxRatio.Float) {
                            addAttackDamageExtra = _attackMaxRatio.Float;
                            addAttackDamageExtraEach = _attackMaxRatio.Float - addAttackDamageExtra;
                        } else {
                            addAttackDamageExtra += addAttackDamageExtraEach;
                        }
                        _attackRatio.Float += addAttackDamageExtraEach;
                    }

                    moveTime = 1;
                }
            } else {
                moveTime = 0;
                _attackRatio.Float -= addAttackDamageExtra;
                addAttackDamageExtra = 0;
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}