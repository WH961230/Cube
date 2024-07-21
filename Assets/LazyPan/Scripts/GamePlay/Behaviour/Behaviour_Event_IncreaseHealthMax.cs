using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_IncreaseHealthMax : Behaviour {
        public Behaviour_Event_IncreaseHealthMax(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            //玩家血量上限
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData _maxHealthData);
            //增加量
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, LabelStr.HEALTH, LabelStr.MAX), out FloatData _increaseHealthMaxData);
            //赋值
            _maxHealthData.Float += _increaseHealthMaxData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}