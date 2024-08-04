using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreasesAllWeaponFireRate : Behaviour {
        public Behaviour_Event_IncreasesAllWeaponFireRate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}