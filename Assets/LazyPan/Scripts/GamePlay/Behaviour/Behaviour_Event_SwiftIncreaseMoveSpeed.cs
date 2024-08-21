using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_SwiftIncreaseMoveSpeed : Behaviour {
        public Behaviour_Event_SwiftIncreaseMoveSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED),
                out FloatData moveSpeed);
            moveSpeed.Float *= 1.2f;
            Debug.Log("玩家增加移动速度 当前速度为:" + moveSpeed.Float);
        }

        public override void DelayedExecute() {
        }



        public override void Clear() {
            base.Clear();
        }
    }
}