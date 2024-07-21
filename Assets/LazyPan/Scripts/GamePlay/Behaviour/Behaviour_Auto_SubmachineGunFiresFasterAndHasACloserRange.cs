using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_SubmachineGunFiresFasterAndHasACloserRange : Behaviour {
        public Behaviour_Auto_SubmachineGunFiresFasterAndHasACloserRange(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Debug.Log("注册冲锋枪Buff " + entity.Sign);
        }

        public override void DelayedExecute() {
            
        }

        public override void Clear() {
            base.Clear();
        }
    }
}