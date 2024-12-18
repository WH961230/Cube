using UnityEngine.InputSystem;

public class MessageCode {
    public static int MsgPlayerLevelUp = 0;//升级
    public static int MsgRobotUp = 1;//机器人升级
    public static int MsgKnockbackPlayer = 2;//击退玩家
    public static int MsgDamagePlayer = 3;//对玩家造成伤害
    public static int MsgDamageRobot = 4;//对机器人造成伤害
    public static int MsgStartLevel = 5;//开始关卡
    public static int MsgGlobalLevelUp = 6;//全局关卡等级提升
    public static int MsgRobotDead = 7;//机器人死亡
    public static int MsgLevelUp = 8;//关卡等级升级
    public static int MsgRecoverHealth = 9;//恢复血量
    public static int MsgCreateActivatableObj = 10;//创建可激活物体
    public static int MsgKnockbackRobot = 11;//击退机器人
    public static int MsgBurnEntity = 12;//燃烧
    public static int MsgFrostEntity = 13;//冰霜
    public static int MsgBoomEntity = 14;//爆炸
    public static int MsgFrozenEntity = 15;//冰冻
    public static int MsgAbsorbsDamageToHealthMax = 16;//吸收伤害为血量上限
    public static int MsgSetTimeScale = 17;//时间暂停
}