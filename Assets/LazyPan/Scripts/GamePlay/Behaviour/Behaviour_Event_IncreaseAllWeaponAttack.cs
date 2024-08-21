using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_IncreaseAllWeaponAttack : Behaviour {
        public Behaviour_Event_IncreaseAllWeaponAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void DelayedExecute() {
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out FloatData addAttackData);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ATTACK, LabelStr.RATIO), out FloatData floatData);
            float before = floatData.Float; 
            floatData.Float += addAttackData.Float;
            Debug.LogFormat("增加全体武器攻击力:{0} 之前{1} 之后{2}", addAttackData.Float, before, floatData.Float);
        }

        public override void Clear() {
            base.Clear();
        }
    }
}