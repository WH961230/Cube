using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_GiantMutant : Behaviour {
        public Behaviour_Auto_GiantMutant(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.ANTIBODY, out BoolData antibodyBool);
            antibodyBool.Bool = true;
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}