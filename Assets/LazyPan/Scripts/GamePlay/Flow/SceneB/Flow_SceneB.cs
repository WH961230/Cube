using UnityEngine;

namespace LazyPan {
    public class Flow_SceneB : Flow {
		private Comp UI_SceneB;

		private Entity Obj_Global_Global;
		private Entity Obj_Camera_Camera;
		private Entity Obj_Terrain_Terrain;
		private Entity Obj_Player_Player;
		private Entity Obj_Tower_Tower;
		private Entity Obj_Creator_ActivatableCapabilityCreator;
		private Entity Obj_Creator_RobotCreator;
		private Entity Obj_Event_PlayerThreeChooseOne;
		private Entity Obj_Event_RobotThreeChooseOne;
		private Entity Obj_Event_Settlement;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.ContentSave("flow", "Flow_SceneB  战斗场景B流程");
			UI_SceneB = UI.Instance.Open("UI_SceneB");

			Obj_Global_Global = Obj.Instance.LoadEntity("Obj_Global_Global");
			Obj_Camera_Camera = Obj.Instance.LoadEntity("Obj_Camera_Camera");
			Obj_Terrain_Terrain = Obj.Instance.LoadEntity("Obj_Terrain_Terrain");
			Obj_Player_Player = Obj.Instance.LoadEntity("Obj_Player_Player");
			Obj_Tower_Tower = Obj.Instance.LoadEntity("Obj_Tower_Tower");
			Obj_Creator_ActivatableCapabilityCreator = Obj.Instance.LoadEntity("Obj_Creator_ActivatableCapabilityCreator");
			Obj_Creator_RobotCreator = Obj.Instance.LoadEntity("Obj_Creator_RobotCreator");
			Obj_Event_PlayerThreeChooseOne = Obj.Instance.LoadEntity("Obj_Event_PlayerThreeChooseOne");
			Obj_Event_RobotThreeChooseOne = Obj.Instance.LoadEntity("Obj_Event_RobotThreeChooseOne");

        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_SceneB;
		}

		/*结算*/
		public void Settlement() {
			Obj_Event_Settlement = Obj.Instance.LoadEntity("Obj_Event_Settlement");
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(Obj_Event_Settlement);
			Obj.Instance.UnLoadEntity(Obj_Event_PlayerThreeChooseOne);
			Obj.Instance.UnLoadEntity(Obj_Event_RobotThreeChooseOne);
			Obj.Instance.UnLoadEntity(Obj_Creator_RobotCreator);
			Obj.Instance.UnLoadEntity(Obj_Creator_ActivatableCapabilityCreator);
			Obj.Instance.UnLoadEntity(Obj_Tower_Tower);
			Obj.Instance.UnLoadEntity(Obj_Player_Player);
			Obj.Instance.UnLoadEntity(Obj_Terrain_Terrain);
			Obj.Instance.UnLoadEntity(Obj_Camera_Camera);
			Obj.Instance.UnLoadEntity(Obj_Global_Global);

			UI.Instance.Close("UI_SceneB");

        }
    }
}