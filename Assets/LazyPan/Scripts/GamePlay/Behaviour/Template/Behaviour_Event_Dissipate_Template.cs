using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Dissipate_Template : Behaviour {
        public Behaviour_Event_Dissipate_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}