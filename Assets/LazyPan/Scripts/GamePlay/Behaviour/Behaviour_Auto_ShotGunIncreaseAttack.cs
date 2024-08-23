using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ShotGunIncreaseAttack : Behaviour {
        public Behaviour_Auto_ShotGunIncreaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //获取射击伤害
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.FIRE, LabelStr.DAMAGE),
                out FloatData _fireDamage);
            _fireDamage.Float *= 1.5f;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}