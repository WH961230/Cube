using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_MoreDrops : Behaviour {
        public Behaviour_Event_MoreDrops(Entity entity, string behaviourSign) : base(entity, behaviourSign) {

        }

        public override void DelayedExecute() {
            Cond.Instance.GetData(base.entity, LabelStr.Assemble(LabelStr.DROP, LabelStr.RATIO), out FloatData _dropRatioData);
            BehaviourData.Get(LabelStr.Assemble(LabelStr.INCREASE, LabelStr.DROP, LabelStr.RATIO), out FloatData _increaseDropRatioData);
            _dropRatioData.Float += _increaseDropRatioData.Float;
        }

        public override void Clear() {
            base.Clear();
        }
    }
}