﻿using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_StrafesatelliteAddThirdBullet : Behaviour {
        public Behaviour_Auto_StrafesatelliteAddThirdBullet(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.GetData(entity, LabelStr.LEVEL, out IntData _level);
            _level.Int++;
        }

        public override void DelayedExecute() {
        }

        public override void Clear() {
            base.Clear();
        }
    }
}