using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_ChainLightningIncreaseACatapult : Behaviour {
        public Behaviour_Auto_ChainLightningIncreaseACatapult(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //连击次数
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.COMBO, LabelStr.COUNT), out IntData _numberOfCombos);
            _numberOfCombos.Int++;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}