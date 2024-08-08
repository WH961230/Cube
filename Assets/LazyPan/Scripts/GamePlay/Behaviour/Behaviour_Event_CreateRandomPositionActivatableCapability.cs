using System.Collections.Generic;
using UnityEngine;


namespace LazyPan {
    public class Behaviour_Event_CreateRandomPositionActivatableCapability : Behaviour {
	    private StringData _defaultActivatableCapability;
	    private List<string> _activatableCapabilitys = new List<string>();
	    public List<Entity> ActivatableCapabilityEntities = new List<Entity>();
        public Behaviour_Event_CreateRandomPositionActivatableCapability(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Cond.Instance.GetData(entity, Label.Assemble(LabelStr.DEFAULT, LabelStr.ACTIVATABLE), out _defaultActivatableCapability);

	        //get all activatable capability
	        _activatableCapabilitys.Clear();
	        List<string> keys = ObjConfig.GetKeys();
	        foreach (var tmpKey in keys) {
		        string[] keySplit = tmpKey.Split("|");
		        if (!Flo.Instance.CurFlowSign.Contains(keySplit[0])) {
			        continue;
		        }
		        ObjConfig config = ObjConfig.Get(keySplit[1]);
		        if (config.Type == "可激活") {
			        _activatableCapabilitys.Add(config.Sign);
		        }
	        }

	        ActivatableCapabilityEntities.Clear();
	        CreateActivatableCapability(true);

	        MessageRegister.Instance.Reg(MessageCode.MsgCreateActivatableObj, MsgCreateActivatableObj);
        }

        private void MsgCreateActivatableObj() {
	        CreateActivatableCapability(false);
        }

        public override void DelayedExecute() {
            
        }

        /*获取随机位置*/
		private LocationInformationData GetRandomPosition() {
			LocationInformationSetting setting = Loader.LoadLocationInfSetting(entity.ObjConfig.SetUpLocationInformationSign);
			List<LocationInformationData> datas = setting.locationInformationDatas;
			return datas[Random.Range(0, datas.Count)];
		}

		/*创建可激活物体*/
		private void CreateActivatableCapability(bool isDefault) {
			string objSign = isDefault
				? _defaultActivatableCapability.String
				: _activatableCapabilitys[Random.Range(0, _activatableCapabilitys.Count)];
			LocationInformationData data = GetRandomPosition();
			Entity instanceEntity = Obj.Instance.LoadEntity(objSign);
			instanceEntity.SetBeginLocationInfo(data);
			ActivatableCapabilityEntities.Add(instanceEntity);
		}

        public override void Clear() {
            base.Clear();
            foreach (var activatable in ActivatableCapabilityEntities) {
	            Obj.Instance.UnLoadEntity(activatable);
            }
        }
    }
}