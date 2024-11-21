using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ShotGunChangeShootWay : Behaviour {
        public Behaviour_Auto_ShotGunChangeShootWay(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.BINGO, out BoolData _bingo);
            _bingo.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}