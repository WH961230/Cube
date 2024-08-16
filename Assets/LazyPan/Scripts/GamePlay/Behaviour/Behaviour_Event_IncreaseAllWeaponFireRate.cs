using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseAllWeaponFireRate : Behaviour {
        public Behaviour_Event_IncreaseAllWeaponFireRate(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ATTACK, LabelStr.SPEED, LabelStr.RATIO),
                out FloatData addAttackSpeedData);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.SPEED, LabelStr.RATIO),
                out FloatData floatData);
            float before = floatData.Float; 
            floatData.Float += addAttackSpeedData.Float;
            Debug.LogFormat("增加全体武器攻击速度:{0} 之前{1} 之后{2}", addAttackSpeedData.Float, before, floatData.Float);
        }

        public override void Clear() {
            base.Clear();
        }
    }
}