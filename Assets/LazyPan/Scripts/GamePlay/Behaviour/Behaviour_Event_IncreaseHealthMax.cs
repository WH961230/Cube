using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_IncreaseHealthMax : Behaviour {
        public Behaviour_Event_IncreaseHealthMax(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            //玩家血量上限
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData _maxHealthData);
            float maxHealthBefore = _maxHealthData.Float;
            //增加量
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, LabelStr.HEALTH, LabelStr.MAX), out FloatData _increaseHealthMaxData);
            //赋值
            _maxHealthData.Float += _increaseHealthMaxData.Float;
            Debug.LogFormat("增加血量{0}: 之前{1} 之后{2}", _increaseHealthMaxData.Float, maxHealthBefore, _maxHealthData.Float);
        }

        public override void Clear() {
            base.Clear();
        }
    }
}