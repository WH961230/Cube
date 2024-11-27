using UnityEngine;

namespace LazyPan {
    public class Flow_SceneB : Flow {
		private Comp UI_SceneB;

		private Entity Obj_全局_全局;
		private Entity Obj_灯光_灯光B;
		private Entity Obj_相机_相机;
		private Entity Obj_地形_地形;
		private Entity Obj_玩家_玩家;
		private Entity Obj_塔_塔;
		private Entity Obj_事件_可激活物体创建器;
		private Entity Obj_事件_机器人创建器;
		private Entity Obj_事件_玩家三选一;
		private Entity Obj_事件_机器人三选一;
		private Entity Obj_事件_结算;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.ContentSave("flow", "Flow_SceneB  战斗场景B流程");
			UI_SceneB = UI.Instance.Open("UI_SceneB");

			Obj_全局_全局 = Obj.Instance.LoadEntity("Obj_全局_全局");
			Obj_灯光_灯光B = Obj.Instance.LoadEntity("Obj_灯光_灯光B");
			Obj_相机_相机 = Obj.Instance.LoadEntity("Obj_相机_相机");
			Obj_地形_地形 = Obj.Instance.LoadEntity("Obj_地形_地形");
			Obj_玩家_玩家 = Obj.Instance.LoadEntity("Obj_玩家_玩家");
			Obj_塔_塔 = Obj.Instance.LoadEntity("Obj_塔_塔");
			Obj_事件_可激活物体创建器 = Obj.Instance.LoadEntity("Obj_事件_可激活物体创建器");
			Obj_事件_机器人创建器 = Obj.Instance.LoadEntity("Obj_事件_机器人创建器");
			Obj_事件_玩家三选一 = Obj.Instance.LoadEntity("Obj_事件_玩家三选一");
			Obj_事件_机器人三选一 = Obj.Instance.LoadEntity("Obj_事件_机器人三选一");

        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_SceneB;
		}

		/*结算*/
		public void Settlement() {
			Obj_事件_结算 = Obj.Instance.LoadEntity("Obj_事件_结算");
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(Obj_事件_结算);
			Obj.Instance.UnLoadEntity(Obj_事件_机器人三选一);
			Obj.Instance.UnLoadEntity(Obj_事件_玩家三选一);
			Obj.Instance.UnLoadEntity(Obj_事件_机器人创建器);
			Obj.Instance.UnLoadEntity(Obj_事件_可激活物体创建器);
			Obj.Instance.UnLoadEntity(Obj_塔_塔);
			Obj.Instance.UnLoadEntity(Obj_玩家_玩家);
			Obj.Instance.UnLoadEntity(Obj_地形_地形);
			Obj.Instance.UnLoadEntity(Obj_相机_相机);
			Obj.Instance.UnLoadEntity(Obj_灯光_灯光B);
			Obj.Instance.UnLoadEntity(Obj_全局_全局);

			UI.Instance.Close("UI_SceneB");

        }
    }
}