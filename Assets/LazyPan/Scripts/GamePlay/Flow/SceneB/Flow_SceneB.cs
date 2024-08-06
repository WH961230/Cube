using UnityEngine;

namespace LazyPan {
    public class Flow_SceneB : Flow {
		private Comp UI_SceneB;

		private Entity A1_物体_全局_全局;
		private Entity A2_物体_相机_相机;
		private Entity A4_物体_地形_地形;
		private Entity A3_物体_玩家_玩家;
		private Entity A5_物体_塔_塔;
		private Entity A12_物体_事件_可激活物体创建器;
		private Entity A13_物体_事件_机器人创建器;
		private Entity A24_物体_事件_玩家三选一;
		private Entity A25_物体_事件_机器人三选一;
		private Entity A26_物体_事件_结算;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.ContentSave("flow", "Flow_SceneB  战斗场景B流程");
			UI_SceneB = UI.Instance.Open("UI_SceneB");

			A1_物体_全局_全局 = Obj.Instance.LoadEntity("A1_物体_全局_全局");
			A2_物体_相机_相机 = Obj.Instance.LoadEntity("A2_物体_相机_相机");
			A4_物体_地形_地形 = Obj.Instance.LoadEntity("A4_物体_地形_地形");
			A3_物体_玩家_玩家 = Obj.Instance.LoadEntity("A3_物体_玩家_玩家");
			A5_物体_塔_塔 = Obj.Instance.LoadEntity("A5_物体_塔_塔");
			A12_物体_事件_可激活物体创建器 = Obj.Instance.LoadEntity("A12_物体_事件_可激活物体创建器");
			A13_物体_事件_机器人创建器 = Obj.Instance.LoadEntity("A13_物体_事件_机器人创建器");
			A24_物体_事件_玩家三选一 = Obj.Instance.LoadEntity("A24_物体_事件_玩家三选一");
			A25_物体_事件_机器人三选一 = Obj.Instance.LoadEntity("A25_物体_事件_机器人三选一");

        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_SceneB;
		}

		/*结算*/
		public void Settlement() {
			A26_物体_事件_结算 = Obj.Instance.LoadEntity("A26_物体_事件_结算");
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(A26_物体_事件_结算);
			Obj.Instance.UnLoadEntity(A25_物体_事件_机器人三选一);
			Obj.Instance.UnLoadEntity(A24_物体_事件_玩家三选一);
			Obj.Instance.UnLoadEntity(A13_物体_事件_机器人创建器);
			Obj.Instance.UnLoadEntity(A12_物体_事件_可激活物体创建器);
			Obj.Instance.UnLoadEntity(A5_物体_塔_塔);
			Obj.Instance.UnLoadEntity(A3_物体_玩家_玩家);
			Obj.Instance.UnLoadEntity(A4_物体_地形_地形);
			Obj.Instance.UnLoadEntity(A2_物体_相机_相机);
			Obj.Instance.UnLoadEntity(A1_物体_全局_全局);

			UI.Instance.Close("UI_SceneB");

        }
    }
}