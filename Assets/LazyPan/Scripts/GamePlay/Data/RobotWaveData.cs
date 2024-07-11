using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class RobotWaveData : LazyPan.Data {
    public List<WaveData> Waves = new List<WaveData>();
}

//波次
[Serializable]
public class WaveData {
    public string Sign;
    public string Description;
    public List<WaveInstanceData> WaveInstanceDefaultList;
    public List<WaveInstanceData> WaveInstanceList;
}

[Serializable]
public class WaveInstanceData {
    public int InstanceNumber;//机器人创建个数
    public string InstanceRobotSign;//机器人标识
    public float InstanceDelayTime;//创建延时时间
}