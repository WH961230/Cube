using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TowerWeaponManagement : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private List<Entity> AssembledWeapons = new List<Entity>();
        public Behaviour_Auto_TowerWeaponManagement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();
            InitDefaultWeapon();
            RefreshWeapons();
        }

        private void InitDefaultWeapon() {
            AssembledWeapons.Clear();
            Entity defaultWeapon = Obj.Instance.LoadEntity("Obj_Weapon_SubmachineGun");
            AssembledWeapons.Add(defaultWeapon);
        }

        private void RefreshWeapons() {
            //获取UI
            Comp weapon = Cond.Instance.Get<Comp>(_ui, LabelStr.WEAPON);

            for (int i = 0; i < AssembledWeapons.Count; i++) {
                Entity tmpEntity = AssembledWeapons[i];
                Image image = Cond.Instance.Get<Image>(weapon, i.ToString());
                Cond.Instance.GetData(tmpEntity, LabelStr.ICON, out StringData spriteName);
                image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spriteName.String);
            }
        }

        public override void Clear() {
            base.Clear();
        }
    }
}