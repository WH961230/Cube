﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Event_CreateRandomPositionRobot : Behaviour {
	    private Flow_SceneB _flow;
	    private List<SetUpBehaviourData> _setUpBehaviours = new List<SetUpBehaviourData>();
	    private List<Entity> 
		    _robots = new List<Entity>();
	    private WaveData _waveData = new WaveData();
	    private Queue<WaveInstanceData> _waveInstanceQueue = new Queue<WaveInstanceData>();
	    private WaveInstanceData operatorWave;
	    private IntData _globalLevelData;
	    private IntData _globalMaxLevelData;
	    private IntData _robotCreateLevelData;
	    private FloatData _delayCreateTime;
	    private bool startLevelCreate;
	    private float delayDeployTime;
	    private GameObject _tipGo;
	    private TextMeshProUGUI _tipText;
	    private Animation _tipAnimation;
        public Behaviour_Event_CreateRandomPositionRobot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Flo.Instance.GetFlow(out _flow);
	        MessageRegister.Instance.Reg(MessageCode.MsgStartLevel, MsgStartLevel);
	        MessageRegister.Instance.Reg(MessageCode.MsgGlobalLevelUp, MsgGlobalLevelUp);
	        MessageRegister.Instance.Reg<int>(MessageCode.MsgRobotDead, MsgRobotDead);
	        _robots.Clear();
	        _waveInstanceQueue.Clear();
	        _setUpBehaviours.Clear();
	        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.LEVEL, out _globalLevelData);
	        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.MAX, LabelStr.LEVEL), out _globalMaxLevelData);
	        Cond.Instance.GetData<RobotWaveData, IntData>(entity, LabelStr.LEVEL, out _robotCreateLevelData);
	        Cond.Instance.GetData<RobotWaveData, WaveData>(entity, LabelStr.WAVE, out _waveData);
	        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
		        LabelStr.Assemble(LabelStr.CREATE, LabelStr.ROBOT, LabelStr.DELAY, LabelStr.TIME),
		        out _delayCreateTime);
	        //提示
	        Comp tip = Cond.Instance.Get<Comp>(_flow.GetUI(), "Tip");
	        _tipGo = Cond.Instance.Get<GameObject>(tip, "Tip");
	        _tipAnimation = _tipGo.GetComponent<Animation>();
	        _tipText = Cond.Instance.Get<TextMeshProUGUI>(tip, "Text");
	        
	        foreach (var wave in _waveData.WaveInstanceDefaultList) {
		        WaveInstanceData instanceWave = new WaveInstanceData();
		        instanceWave.InstanceNumber = wave.InstanceNumber;
		        instanceWave.InstanceRobotSign = wave.InstanceRobotSign;
		        instanceWave.InstanceDelayTime = wave.InstanceDelayTime;
		        _waveData.WaveInstanceList.Add(instanceWave);
	        }

	        MsgStartLevel();
	        Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }
        
        public override void DelayedExecute() {
            
        }

        private void InputStartLevel(InputAction.CallbackContext obj) {
	        if (obj.started) {
		        MsgStartLevel();
	        }
        }

        private void OnUpdate() {
	        if (startLevelCreate) {
		        if (delayDeployTime > 0) {
			        delayDeployTime -= Time.deltaTime;
		        } else {
			        //当前波数不为空
			        if (operatorWave != null) {
				        if (operatorWave.InstanceNumber > 0) {
					        CreateRobot(operatorWave.InstanceRobotSign);
					        delayDeployTime = Random.Range(1, operatorWave.InstanceDelayTime);
					        operatorWave.InstanceNumber--;
					        Debug.Log("正在创建机器人:" + operatorWave.InstanceRobotSign + " 剩余数量 " + operatorWave.InstanceNumber);
				        } else {
					        operatorWave = null;
				        }
			        } else {
				        if (_waveInstanceQueue.Any()) {
					        operatorWave = _waveInstanceQueue.Dequeue();
					        Debug.Log("创建机器人:" + operatorWave.InstanceRobotSign + " 总共创建 " + operatorWave.InstanceNumber);
				        } else {
					        delayDeployTime = 0;
					        startLevelCreate = false;
					        Debug.Log("创建完毕");
				        }
			        }
		        }
	        }
        }

        private void MsgStartLevel() {
	        StartLevel();
        }

        private void MsgGlobalLevelUp() {
	        if (_globalLevelData.Int < _globalMaxLevelData.Int) {
		        _globalLevelData.Int++;
		        //清空击杀机器人数量
		        Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.KILL, LabelStr.ROBOT),
			        out IntData intData);
		        intData.Int = 0;
		        Debug.Log("全局关卡升级后等级:" + _globalLevelData.Int);
	        }
        }

        private void MsgRobotDead(int entityId) {
	        if (_robots.Count > 0) {
		        for (int i = _robots.Count - 1; i >= 0; i--) {
			        Entity tmpRobot = _robots[i];
			        if (tmpRobot.ID == entityId) {
				        Debug.Log("机器人死亡:" + tmpRobot.Prefab.name);
				        _robots.Remove(tmpRobot);
				        Obj.Instance.UnLoadEntity(tmpRobot);
			        }
		        }

		        if (_robots.Count == 0 && !startLevelCreate) {
			        MessageRegister.Instance.Dis(MessageCode.MsgGlobalLevelUp);
			        MessageRegister.Instance.Dis(MessageCode.MsgRobotUp);
			        MessageRegister.Instance.Dis(MessageCode.MsgLevelUp);
		        }
	        }
        }

        private void StartLevel() {
	        //当Level升级后开始生成 当前等级低于全局等级
	        if (_robotCreateLevelData.Int < _globalLevelData.Int) {
		        _robotCreateLevelData.Int++;

		        foreach (var wave in _waveData.WaveInstanceList) {
			        WaveInstanceData instanceWave = new WaveInstanceData();
			        instanceWave.InstanceNumber = wave.InstanceNumber;
			        instanceWave.InstanceRobotSign = wave.InstanceRobotSign;
			        instanceWave.InstanceDelayTime = wave.InstanceDelayTime + _delayCreateTime.Float;
			        _waveInstanceQueue.Enqueue(instanceWave);
		        }

		        startLevelCreate = true;
		        _tipText.text = string.Concat("开始关卡 ", _robotCreateLevelData.Int.ToString());
		        _tipAnimation.Rewind();
		        _tipAnimation.Play();
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
	        Debug.Log("生成机器人:" + instance.Prefab.name);

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
	        foreach (var behaviourData in _setUpBehaviours) {
		        if (BehaviourRegister.RegisterBehaviour(robot.ID, behaviourData.BehaviourName, out Behaviour outBehaviour)) {
			        outBehaviour.SetBehaviourData(behaviourData.BehaviourData);
			        outBehaviour.DelayedExecute();
		        } else {
			        Debug.LogErrorFormat("机器人:{0} 行为:{1}注册失败: ", robot.ID, behaviourData.BehaviourName);
		        }
	        }
        }

        public void AddSetUpBehaviourSign(SetUpBehaviourData behaviourData) {
	        if (!_setUpBehaviours.Contains(behaviourData)) {
		        _setUpBehaviours.Add(behaviourData);
	        }
        }

        public void RemoveSetUpBehaviourSign(string behaviourName) {
	        for (int i = _setUpBehaviours.Count - 1; i > 0; i++) {
		        SetUpBehaviourData data = _setUpBehaviours[i];
		        if (data.BehaviourName == behaviourName) {
			        _setUpBehaviours.Remove(data);
		        }
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
            MessageRegister.Instance.UnReg(MessageCode.MsgStartLevel, MsgStartLevel);
            MessageRegister.Instance.UnReg(MessageCode.MsgGlobalLevelUp, MsgGlobalLevelUp);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgRobotDead, MsgRobotDead);

            foreach (var tmpRobot in _robots) {
	            Obj.Instance.UnLoadEntity(tmpRobot);
            }
        }
    }

    public class SetUpBehaviourData {
	    public string BehaviourName;
	    public Data BehaviourData;
    }
}