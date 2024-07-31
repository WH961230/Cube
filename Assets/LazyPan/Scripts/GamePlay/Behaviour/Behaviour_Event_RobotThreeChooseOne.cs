using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LazyPan {
    public class Behaviour_Event_RobotThreeChooseOne : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private List<Entity> _parallelBuffs = new List<Entity>();
        private Dictionary<int, List<Entity>> _robotStrengthenBuffs = new Dictionary<int, List<Entity>>();
        public Behaviour_Event_RobotThreeChooseOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            InitWaveBuffs();
            InitRobotBuffs();
            MessageRegister.Instance.Reg(MessageCode.MsgRobotUp, MsgRobotThreeChooseOne);
            MessageRegister.Instance.Reg(MessageCode.MsgLevelUp, MsgDisplayLevelUp);
            
            DisplayLevelUI();
        }
        
        public override void DelayedExecute() {
            
        }

        //传入消息开始三选一
        private void MsgRobotThreeChooseOne() {
            ClockUtil.Instance.AlarmAfter(1, () => {
                OpenRobotThreeChooseOneUI();
            });
        }

        private void MsgDisplayLevelUp() {
            DisplayLevelUI();
        }

        private void DisplayLevelUI() {
            //UI弹出显示当前的关卡
            Comp levelComp = Cond.Instance.Get<Comp>(_ui, LabelStr.LEVEL);
            GameObject grid = Cond.Instance.Get<GameObject>(levelComp, LabelStr.GRID);
            Transform parent = Cond.Instance.Get<Transform>(levelComp, LabelStr.PARENT);

            //生成总关卡数的关卡 当前关卡以及之前的不透明度拉满 未参加的关卡 降低不透明度
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.MAX, LabelStr.LEVEL), out IntData maxLevel);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.LEVEL, out IntData level);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ROBOT, LabelStr.LEVEL), out IntData robotLevel);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.PLAYER, LabelStr.LEVEL), out IntData playerLevel);

            int tmpMaxLevel = maxLevel.Int;
            int tmpLevel = level.Int;

            foreach (Transform o in parent) {
                GameObject.Destroy(o.gameObject);
            }

            int count = tmpMaxLevel;
            while (count > 0) {
                GameObject tmpInstance = Object.Instantiate(grid, parent);
                tmpInstance.SetActive(true);

                Image gridImg = tmpInstance.GetComponent<Image>();
                Color color = gridImg.color;
                color.a = count > tmpMaxLevel - tmpLevel ? 1 : 0.5f;
                gridImg.color = color;

                count--;
            }

            ClockUtil.Instance.AlarmAfter(2, () => {
                foreach (Transform o in parent) {
                    GameObject.Destroy(o.gameObject);
                }
            });

            Comp countComp = Cond.Instance.Get<Comp>(_ui, LabelStr.COUNT);
            TextMeshProUGUI robot = Cond.Instance.Get<TextMeshProUGUI>(countComp, LabelStr.ROBOT);
            robot.text = robotLevel.Int.ToString();

            TextMeshProUGUI player = Cond.Instance.Get<TextMeshProUGUI>(countComp, LabelStr.PLAYER);
            player.text = playerLevel.Int.ToString();

            TextMeshProUGUI levelText = Cond.Instance.Get<TextMeshProUGUI>(countComp, LabelStr.LEVEL);
            levelText.text = level.Int.ToString();
        }

        //初始化Buff
        private void InitWaveBuffs() {
            List<string> objConfigs = ObjConfig.GetKeys();
            string[] types = new[] { "WaveBuff" };
            foreach (string keyStr in objConfigs) {
                string[] keys = keyStr.Split("|");
                if (!Flo.Instance.CurFlowSign.Contains(keys[0])) {
                    continue;
                }
                string key = keys[1];
                ObjConfig config = ObjConfig.Get(key);
                foreach (var type in types) {
                    if (config.Type == type) {
                        Entity buffInstanceEntity = Obj.Instance.LoadEntity(config.Sign);
                        _parallelBuffs.Add(buffInstanceEntity);
                        break;
                    }
                }
            }
        }

        private void InitRobotBuffs() {
            string objSign = "Obj_RobotBuff_RobotStrengthen";
            List<string> list = RobotStrengthenConfig.GetKeys();
            foreach (var tmpKey in list) {
                RobotStrengthenConfig config = RobotStrengthenConfig.Get(tmpKey);
                Entity buffEntity = Obj.Instance.LoadEntity(objSign);
                Cond.Instance.GetData(buffEntity, LabelStr.Assemble(LabelStr.MOVEMENT, LabelStr.SPEED), out FloatData speedData);
                speedData.Float = config.MovementSpeedPercentage / 100;
                Cond.Instance.GetData(buffEntity, LabelStr.DAMAGE, out FloatData damageData);
                damageData.Float = config.AttackPercentage / 100;
                Cond.Instance.GetData(buffEntity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out FloatData maxHealthData);
                maxHealthData.Float = config.HealthPercentage / 100;
                Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                infoData.String = config.Text;
                Cond.Instance.GetData(buffEntity, LabelStr.DIFFICULTY, out IntData difficultyData);
                difficultyData.Int = config.Level;

                if (_robotStrengthenBuffs.TryGetValue(config.Level, out List<Entity> entities)) {
                    entities.Add(buffEntity);
                } else {
                    List<Entity> buffs = new List<Entity>();
                    buffs.Add(buffEntity);
                    _robotStrengthenBuffs.Add(config.Level, buffs);
                }
            }
        }

        //打开机器人三选一界面
        private void OpenRobotThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (!choose.gameObject.activeSelf) {
                string buffType = "";
                int difficulty = 1;

                //等级
                Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.LEVEL,
                    out IntData globalLevelData);

                //拿到类型和关卡难度
                foreach (string key in LevelConfig.GetKeys()) {
                    LevelConfig level = LevelConfig.Get(key);
                    if (level.Wave == globalLevelData.Int) {
                        buffType = level.LevelType;
                        difficulty = Random.Range(level.DifficultyLowerLimit, level.DifficultyUpperLimit + 1);
                        break;
                    }
                }

                if (buffType == "WaveBuff") {
                    WaveBuff(difficulty);
                } else if (buffType == "RobotBuff") {
                    RobotBuff(difficulty);
                }

                InputRegister.Instance.Load(InputRegister.ESCAPE, InputCloseUI);
            }
        }

        private void WaveBuff(int difficulty) {
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            if (_parallelBuffs.Count == 0) {
                return;
            }
            string debug = "开始怪物三选一 增加怪物:";
            Time.timeScale = 0;
            choose.gameObject.SetActive(true);
            int resultCount = 3;
            int[] index = MathUtil.Instance.GetRandNoRepeatIndex(_parallelBuffs.Count, resultCount);
            if (index == null) {
                Debug.LogError("错误");
                return;
            }
            for (int i = 0; i < index.Length; i++) {
                int tmpIndex = index[i];
                Entity buffEntity = _parallelBuffs[tmpIndex];
                Comp item = Cond.Instance.Get<Comp>(choose,
                    LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, i.ToString()));

                //注册图片
                // Image image = Cond.Instance.Get<Image>(item, LabelStr.ICON);
                // Cond.Instance.GetData(buffEntity, LabelStr.ICON, out StringData spritePathData);
                // image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spritePathData.String);

                //注册说明
                TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                //提取波数数据 输出文本
                if (Cond.Instance.GetData<RobotWaveData, WaveData>(buffEntity, LabelStr.WAVE,
                        out WaveData waveData)) {
                    info.text = string.Format(infoData.String,
                        waveData.WaveInstanceDefaultList[0].InstanceNumber * difficulty);
                }

                debug += info.text + " ";

                //注册按钮事件
                Button button = Cond.Instance.Get<Button>(item, LabelStr.BUTTON);
                ButtonRegister.RemoveAllListener(button);
                ButtonRegister.AddListener(button, RegisterWave, buffEntity, difficulty);
            }

            Debug.Log(debug);
        }

        private void RobotBuff(int difficulty) {
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            string debug = "开始怪物三选一 强化怪物:";
            Time.timeScale = 0;
            choose.gameObject.SetActive(true);
            int resultCount = 3;
            if (_robotStrengthenBuffs.TryGetValue(difficulty, out List<Entity> takeOutRobotStrengthen)) {
                int[] index = MathUtil.Instance.GetRandNoRepeatIndex(takeOutRobotStrengthen.Count, resultCount);
                if (index == null) {
                    Debug.LogError("错误");
                    return;
                }
                for (int i = 0; i < index.Length; i++) {
                    int tmpIndex = index[i];
                    Entity buffEntity = takeOutRobotStrengthen[tmpIndex];
                    Comp item = Cond.Instance.Get<Comp>(choose,
                        LabelStr.Assemble(LabelStr.CHOOSE, LabelStr.ITEM, i.ToString()));

                    //注册图片
                    // Image image = Cond.Instance.Get<Image>(item, LabelStr.ICON);
                    // Cond.Instance.GetData(buffEntity, LabelStr.ICON, out StringData spritePathData);
                    // image.sprite = Loader.LoadAsset<Sprite>(AssetType.SPRITE, spritePathData.String);

                    //注册说明
                    TextMeshProUGUI info = Cond.Instance.Get<TextMeshProUGUI>(item, LabelStr.INFO);
                    Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData infoData);
                    //提取波数数据 输出文本
                    info.text = infoData.String;

                    debug += info.text + " ";

                    //注册按钮事件
                    Button button = Cond.Instance.Get<Button>(item, LabelStr.BUTTON);
                    ButtonRegister.RemoveAllListener(button);
                    ButtonRegister.AddListener(button, RegisterStrengthenBehaviour, buffEntity);
                }

                Debug.Log(debug);
            }
        }

        private void RegisterStrengthenBehaviour(Entity buffEntity) {
            if (Cond.Instance.GetData(buffEntity, LabelStr.USED, out BoolData usedBoolData)) {
                //无目标针对所有敌人 在敌人的生成位置 放置待注册Buff
                if (EntityRegister.TryGetEntityBySign("Obj_Creator_RobotCreator",
                        out Entity createRobotEntity)) {
                    if (BehaviourRegister.GetBehaviour(createRobotEntity.ID,
                            out Behaviour_Event_CreateRandomPositionRobot beh)) {
                        Cond.Instance.GetData(buffEntity, LabelStr.INFO, out StringData info);

                        Debug.Log("增加所有机器人能力:" + info);
                        string sign = "Behaviour_Auto_RobotStrengthen";
                        beh.RemoveSetUpBehaviourSign(sign);
                        beh.AddSetUpBehaviourSign(new SetUpBehaviourData() {
                            BehaviourSign = sign,
                            BehaviourData = buffEntity.Data
                        });
                        usedBoolData.Bool = true;
                        CloseRobotThreeChooseOneUI();
                        MessageRegister.Instance.Dis(MessageCode.MsgStartLevel);

                        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ROBOT, LabelStr.LEVEL),
                            out IntData robotLevel);

                        robotLevel.Int++;

                        Debug.Log("机器人升级后等级:" + robotLevel.Int);
                    }
                }
            }
        }

        //波次BUFF可以重复选择
        private void RegisterWave(Entity buffEntity, int difficulty) {
            if (Cond.Instance.GetData<RobotWaveData, WaveData>(buffEntity, LabelStr.WAVE, out WaveData waveData)) {
                //无目标针对所有敌人 在敌人的生成位置 放置待注册Buff
                if (EntityRegister.TryGetEntityBySign("Obj_Creator_RobotCreator",
                        out Entity createRobotEntity)) {
                    if (BehaviourRegister.GetBehaviour(createRobotEntity.ID,
                            out Behaviour_Event_CreateRandomPositionRobot beh)) {
                        WaveInstanceData waveDataSetting = waveData.WaveInstanceDefaultList[0];
                        
                        WaveInstanceData waveDataInstance = new WaveInstanceData();
                        waveDataInstance.InstanceRobotSign = waveDataSetting.InstanceRobotSign;
                        waveDataInstance.InstanceNumber = waveDataSetting.InstanceNumber * /*系数*/ 1 * difficulty;
                        waveDataInstance.InstanceDelayTime = waveDataSetting.InstanceDelayTime;
                        Debug.Log("增加机器人 :" + waveDataInstance.InstanceRobotSign + " 波增加个数: " + waveDataInstance.InstanceNumber);
                        beh.AddWaveInstanceData(waveDataInstance);
                        CloseRobotThreeChooseOneUI();
                        MessageRegister.Instance.Dis(MessageCode.MsgStartLevel);

                        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.ROBOT, LabelStr.LEVEL),
                            out IntData robotLevel);
                        robotLevel.Int++;
                        
                        Debug.Log("机器人升级后等级:" + robotLevel.Int);
                    }
                }
            }
        }

        //按键关闭UI
        private void InputCloseUI(InputAction.CallbackContext obj) {
            if (obj.started) {
                CloseRobotThreeChooseOneUI();
            }
        }

        //关闭角色三选一界面
        private void CloseRobotThreeChooseOneUI() {
            //打开UI
            Flo.Instance.GetFlow(out Flow_SceneB flow);
            Comp ui = flow.GetUI();
            //显示三选一界面
            Comp choose = Cond.Instance.Get<Comp>(ui, LabelStr.CHOOSE);
            choose.gameObject.SetActive(false);

            InputRegister.Instance.UnLoad(InputRegister.ESCAPE, InputCloseUI);
            Time.timeScale = 1;
        }

        public override void Clear() {
            base.Clear();
            CloseRobotThreeChooseOneUI();
            MessageRegister.Instance.UnReg(MessageCode.MsgRobotUp, MsgRobotThreeChooseOne);
            MessageRegister.Instance.UnReg(MessageCode.MsgLevelUp, MsgDisplayLevelUp);
        }
    }
}