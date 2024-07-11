using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        }

        private void OnUpdate() {
	        if (delayDeployTime > 0) {
		        delayDeployTime -= Time.deltaTime;
	        } else {
		        if (operatorWave == null && _waveInstanceDatas.Any()) {
			        operatorWave = _waveInstanceDatas.Dequeue();
			        delayDeployTime = operatorWave.InstanceDelayTime;
		        }

		        if (operatorWave != null) {
			        CreateRobot(operatorWave.InstanceRobotSign);
		        }
	        }
        }

        private void MsgStartGame() {
	        Cond.Instance.GetData(entity, LabelStr.WAVE, out _waveData);
	        
	        foreach (var wave in _waveData.WaveInstanceDefaultList) {
		        _waveData.WaveInstanceList.Add(wave);;
	        }

	        foreach (var wave in _waveData.WaveInstanceList) {
		        _waveInstanceDatas.Enqueue(wave);
	        }

	        delayDeployTime = 0;
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

            foreach (var tmpRobot in _robots) {
	            Obj.Instance.UnLoadEntity(tmpRobot);
            }
        }
    }
}