using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_CreateRandomPositionRobot : Behaviour {
	    private List<string> _setUpBehaviours = new List<string>();
	    private List<Entity> _robots = new List<Entity>();
        public Behaviour_Event_CreateRandomPositionRobot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        MessageRegister.Instance.Reg(MessageCode.MsgStartGame, MsgStartGame);
	        
	        #region Test

	        InputRegister.Instance.Load(InputCode.SPACE, context => {
		        if (context.performed) {
			        MessageRegister.Instance.Dis(MessageCode.MsgStartGame);
		        }
	        });

	        #endregion
        }

        private void MsgStartGame() {
	        //开始游戏
	        
	        //根据机器人生成的数据 依次创建机器人

	        CreateRobot();
        }

        //获取随机位置
		private LocationInformationData GetRandomPosition() {
			LocationInformationSetting setting = Loader.LoadLocationInfSetting(entity.ObjConfig.SetUpLocationInformationSign);
			List<LocationInformationData> datas = setting.locationInformationDatas;
			return datas[Random.Range(0, datas.Count)];
		}

		//创建机器人
		private void CreateRobot() {
			//获取随机位置
			LocationInformationData locationData = GetRandomPosition();
			Entity instance = Obj.Instance.LoadEntity("Obj_Robot_SmallTriangle");
			//设置位置
			instance.SetBeginLocationInfo(locationData);
			//注册事件
			foreach (var behaviourSign in _setUpBehaviours) {
				BehaviourRegister.RegisterBehaviour(instance.ID, behaviourSign);
			}
		}

		public void AddSetUpBehaviourSign(string behaviourSign) {
			if (!_setUpBehaviours.Contains(behaviourSign)) {
				_setUpBehaviours.Add(behaviourSign);
			}
		}

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.MsgStartGame, MsgStartGame);
        }
    }
}