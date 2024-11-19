using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_LaserrayAttackChargeEnergyIncreaseAttack : Behaviour {
        public Behaviour_Auto_LaserrayAttackChargeEnergyIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.EFFECT, LabelStr.ATTACK), out BoolData attackData);
            attackData.Bool = true;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}