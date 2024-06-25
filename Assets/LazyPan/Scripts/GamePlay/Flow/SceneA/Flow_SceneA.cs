using UnityEngine;

namespace LazyPan {
    public class Flow_SceneA : Flow {
		private Comp UI_SceneA;

		private Entity Obj_Camera_Camera;
		private Entity Obj_Terrain_Terrain;
		private Entity Obj_Player_Player;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.ContentSave("flow", "Flow_SceneA  开始场景A流程");
			UI_SceneA = UI.Instance.Open("UI_SceneA");

			Obj_Camera_Camera = Obj.Instance.LoadEntity("Obj_Camera_Camera");
			Obj_Terrain_Terrain = Obj.Instance.LoadEntity("Obj_Terrain_Terrain");
			Obj_Player_Player = Obj.Instance.LoadEntity("Obj_Player_Player");

        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_SceneA;
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(Obj_Player_Player);
			Obj.Instance.UnLoadEntity(Obj_Terrain_Terrain);
			Obj.Instance.UnLoadEntity(Obj_Camera_Camera);

			UI.Instance.Close("UI_SceneA");

        }
    }
}