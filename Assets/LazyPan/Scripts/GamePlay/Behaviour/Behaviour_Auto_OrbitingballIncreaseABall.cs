﻿using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_OrbitingballIncreaseABall : Behaviour {
        public Behaviour_Auto_OrbitingballIncreaseABall(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //增加环绕数量
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.SURROUND, LabelStr.COUNT), out IntData _surroundCount);
            _surroundCount.Int++;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}