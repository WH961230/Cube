using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_TowerWeaponManagement : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private List<Entity> _allWeapons = new List<Entity>();
        private List<Entity> _assembledWeapons = new List<Entity>();
        private StringData _defaultWeapon;

        public Behaviour_Auto_TowerWeaponManagement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();
            Cond.Instance.GetData(entity, Label.Assemble(LabelStr.DEFAULT, LabelStr.WEAPON), out  _defaultWeapon);
            InitAllWeapon();
            InitDefaultWeapon();
        }

        public override void DelayedExecute() {
            
        }

        private void InitAllWeapon() {
            _allWeapons.Clear();
            string[] types = new[] { "武器"};
            List<string> objConfigs = ObjConfig.GetKeys();
            foreach (string keyStr in objConfigs) {
                string[] keys = keyStr.Split("|");
                if (!Flo.Instance.CurFlowSign.Contains(keys[0])) {
                    continue;
                }
                string key = keys[1];
                ObjConfig config = ObjConfig.Get(key);
                foreach (var type in types) {
                    if (config.Type == type) {
                        Entity instanceEntity = Obj.Instance.LoadEntity(config.Sign);
                        instanceEntity.Prefab.SetActive(false);
                        _allWeapons.Add(instanceEntity);
                        Debug.Log("初始化武器:" + config.Name);
                        break;
                    }
                }
            }
        }

        private Entity GetWeapon(string weaponSign) {
            foreach (var tmpEntity in _allWeapons) {
                if (tmpEntity.Sign == weaponSign) {
                    return tmpEntity;
                }
            }

            return null;
        }

        private void InitDefaultWeapon() {
            _assembledWeapons.Clear();
            InitWeapon(_defaultWeapon.String);
        }

        public void InitWeapon(string weaponSign) {
            Entity prepareAssembledWeapon = GetWeapon(weaponSign);
            prepareAssembledWeapon.Prefab.SetActive(true);
            _assembledWeapons.Add(prepareAssembledWeapon);
            RefreshWeapons();
            Debug.Log("默认武器:" + prepareAssembledWeapon.ObjConfig.Name);
        }

        public int GetAssembledWeaponCount() {
            return _assembledWeapons.Count;
        }

        private void RefreshWeapons() {
            //获取UI
            Comp weapon = Cond.Instance.Get<Comp>(_ui, LabelStr.WEAPON);

            for (int i = 0; i < _assembledWeapons.Count; i++) {
                Entity tmpEntity = _assembledWeapons[i];
                Image image = Cond.Instance.Get<Image>(weapon, i.ToString());
                Cond.Instance.GetData(tmpEntity, LabelStr.ICON, out StringData spriteName);
                image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spriteName.String);
            }
        }

        public override void Clear() {
            base.Clear();
            foreach (Entity tmpWeapon in _allWeapons) {
                Obj.Instance.UnLoadEntity(tmpWeapon);
            }

            foreach (Entity tmpWeapon in _assembledWeapons) {
                Obj.Instance.UnLoadEntity(tmpWeapon);
            }
        }
    }
}