using System.Collections.Generic;
using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_CreateRandomPositionActivatableCapability : Behaviour {
        public Behaviour_Event_CreateRandomPositionActivatableCapability(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        CreateActivatableCapability();
	        
	        #region Test

	        InputRegister.Instance.Load(InputCode.C, context => {
		        if (context.performed) {
			        CreateActivatableCapability();
		        }
	        });

	        #endregion
        }

		/*获取随机位置*/
		private LocationInformationData GetRandomPosition() {
			LocationInformationSetting setting = Loader.LoadLocationInfSetting(entity.ObjConfig.SetUpLocationInformationSign);
			List<LocationInformationData> datas = setting.locationInformationDatas;
			return datas[Random.Range(0, datas.Count)];
		}

		/*创建可激活物体*/
		private void CreateActivatableCapability() {
			LocationInformationData data = GetRandomPosition();
			Entity instanceEntity = Obj.Instance.LoadEntity("Obj_Activable_ActivatableCapability");
			instanceEntity.SetBeginLocationInfo(data);
		}

        public override void Clear() {
            base.Clear();
        }
    }
}