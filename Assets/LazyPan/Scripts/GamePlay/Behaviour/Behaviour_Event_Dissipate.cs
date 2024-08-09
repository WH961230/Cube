using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_Dissipate : Behaviour {
        public Behaviour_Event_Dissipate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}