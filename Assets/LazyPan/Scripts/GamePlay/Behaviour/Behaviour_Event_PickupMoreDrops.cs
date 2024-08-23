using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_PickupMoreDrops : Behaviour {
        public Behaviour_Event_PickupMoreDrops(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.DROP, LabelStr.RATIO),
                out IntData intData);
            intData.Int = Mathf.FloorToInt(intData.Int * 1.5f);
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}