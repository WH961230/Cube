using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_PickUpExperiencePoint : Behaviour {
        public Behaviour_Trigger_PickUpExperiencePoint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.AddListener(OnTriggerEnter);
        }

        private void OnTriggerEnter(Collider arg0) {
            if (EntityRegister.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity playerEntity)) {
                if (playerEntity.ObjConfig.Type == "Player") {
                    Cond.Instance.GetData(entity, LabelStr.EXPERIENCE, out FloatData addExpData);
                    Cond.Instance.GetData(playerEntity, LabelStr.EXPERIENCE, out FloatData expData);
                    Cond.Instance.GetData(playerEntity, LabelStr.Assemble(LabelStr.MAX, LabelStr.EXPERIENCE), out FloatData maxExpData);
                    float afterExp = expData.Float + addExpData.Float;
                    if (afterExp < maxExpData.Float) {
                        expData.Float = afterExp;
                    } else {
                        expData.Float = afterExp - maxExpData.Float;
                        //升级三选一
                        MessageRegister.Instance.Dis(MessageCode.MsgPlayerLevelUp);
                    }
                    Obj.Instance.UnLoadEntity(entity);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(OnTriggerEnter);
        }
    }
}