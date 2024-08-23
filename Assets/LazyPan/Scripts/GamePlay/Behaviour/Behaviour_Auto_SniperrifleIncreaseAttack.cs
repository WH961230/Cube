using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SniperrifleIncreaseAttack : Behaviour {
        public Behaviour_Auto_SniperrifleIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE),
                out FloatData _fireDamage);
            _fireDamage.Float *= 2;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}