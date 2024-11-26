using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_AbsorbsDamageToHealthMax : Behaviour {
        private FloatData _healthMax;
        public Behaviour_Auto_AbsorbsDamageToHealthMax(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.HEALTH, LabelStr.MAX), out _healthMax);
            MessageRegister.Instance.Reg<int, float>(MessageCode.MsgAbsorbsDamageToHealthMax, MsgAbsorbsDamageToHealthMax);
        }

        private void MsgAbsorbsDamageToHealthMax(int entityId, float damage) {
            if (entityId == entity.ID) {
                _healthMax.Float += damage;
            }
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<int, float>(MessageCode.MsgAbsorbsDamageToHealthMax, MsgAbsorbsDamageToHealthMax);
        }
    }
}