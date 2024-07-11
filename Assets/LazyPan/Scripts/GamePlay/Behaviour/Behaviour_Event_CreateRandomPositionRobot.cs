using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Event_CreateRandomPositionRobot : Behaviour {
	    private List<string> _setUpBehaviours = new List<string>();
	    private List<Entity> _robots = new List<Entity>();
	    private WaveData _waveData = new WaveData();
	    private Queue<WaveInstanceData> _waveInstanceDatas = new Queue<WaveInstanceData>();
	    private WaveInstanceData operatorWave;
	    private float delayDeployTime;
        public Behaviour_Event_CreateRandomPositionRobot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        MessageRegister.Instance.Reg(MessageCode.MsgStartGame, MsgStartGame);
	        _robots.Clear();
	        _waveInstanceDatas.Clear();
	        _setUpBehaviours.Clear();
	        Game.instance.OnUpdateEvent.AddListener(OnUpdate);

	        InputRegister.Instance.Load(InputCode.T, InputStartGame);
        }

        private void InputStartGame(InputAction.CallbackContext obj) {
	        if (obj.started) {
		        MsgStartGame();
	        }
        }

        private void OnUpdate() {
	        if (delayDeployTime > 0) {
		        delayDeployTime -= Time.deltaTime;
	        } else {
		        //当前波数不为空
		        if (operatorWave != null) {
			        if (operatorWave.InstanceNumber > 0) {
				        CreateRobot(operatorWave.InstanceRobotSign);
				        delayDeployTime = operatorWave.InstanceDelayTime;
				        operatorWave.InstanceNumber--;
			        } else {
				        operatorWave = null;
			        }
		        } else {
			        if (_waveInstanceDatas.Any()) {
				        operatorWave = _waveInstanceDatas.Dequeue();
			        } else {
				        delayDeployTime = 0;
			        }
		        }
	        }
        }

        private void MsgStartGame() {
	        Debug.Log("开始游戏");
	        Cond.Instance.GetData(entity, LabelStr.WAVE, out _waveData);

	        foreach (var wave in _waveData.WaveInstanceDefaultList) {
		        _waveData.WaveInstanceList.Add(wave);
	        }

	        foreach (var wave in _waveData.WaveInstanceList) {
		        _waveInstanceDatas.Enqueue(wave);
	        }
        }

        #region 创建机器人

        //创建机器人
        private void CreateRobot(string robotSign) {
	        //获取随机位置
	        LocationInformationData locationData = GetRandomPosition();
	        Entity instance = Obj.Instance.LoadEntity(robotSign);
	        //设置位置
	        instance.SetBeginLocationInfo(locationData);
	        //注册行为
	        RegisterSetUpBehaviour(instance);

	        _robots.Add(instance);
        }

        //获取随机位置
        private LocationInformationData GetRandomPosition() {
	        LocationInformationSetting setting = Loader.LoadLocationInfSetting(entity.ObjConfig.SetUpLocationInformationSign);
	        List<LocationInformationData> datas = setting.locationInformationDatas;
	        return datas[Random.Range(0, datas.Count)];
        }

        #endregion

        #region 机器人生成注册行为

        private void RegisterSetUpBehaviour(Entity robot) {
	        //注册事件
	        foreach (var behaviourSign in _setUpBehaviours) {
		        BehaviourRegister.RegisterBehaviour(robot.ID, behaviourSign);
	        }
        }

        public void AddSetUpBehaviourSign(string behaviourSign) {
	        if (!_setUpBehaviours.Contains(behaviourSign)) {
		        _setUpBehaviours.Add(behaviourSign);
	        }
        }

        public void AddWaveInstanceData(WaveInstanceData waveInstanceData) {
	        bool isAddNumber = false;
	        foreach (var tmpWaveData in _waveData.WaveInstanceList) {
		        if (tmpWaveData.InstanceRobotSign == waveInstanceData.InstanceRobotSign) {
			        tmpWaveData.InstanceNumber += waveInstanceData.InstanceNumber;
			        isAddNumber = true;
			        break;
		        }
	        }

	        if (!isAddNumber) {
		        _waveData.WaveInstanceList.Add(waveInstanceData);
	        }
        }

        #endregion

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg(MessageCode.MsgStartGame, MsgStartGame);
            InputRegister.Instance.UnLoad(InputCode.T, InputStartGame);

            foreach (var tmpRobot in _robots) {
	            Obj.Instance.UnLoadEntity(tmpRobot);
            }
        }
    }
}