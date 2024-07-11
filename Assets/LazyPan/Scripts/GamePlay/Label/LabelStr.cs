public class LabelStr {
    public static string CHOOSE = "Choose";
    public static string ITEM = "Item";
    public static string SETTLEMENT = "Settlement";
    public static string BACK = "Back";
    public static string A = "A";
    public static string B = "B";
    public static string C = "C";
    public static string D = "D";
    public static string E = "E";
    public static string F = "F";
    public static string G = "G";
    public static string BUTTON = "Button";
    public static string WEAPON = "Weapon";
    public static string ICON = "Icon";
    public static string FOOT = "Foot";
    public static string USED = "Used";
    public static string TARGET = "Target";
    public static string SIGN = "Sign";
    public static string INFO = "Info";
    public static string MOVEMENT = "Movement";
    public static string SPEED = "Speed";
    public static string NAVMESHAGENT = "NavMeshAgent";
    public static string BODY = "Body";
    public static string FIRE = "Fire";
    public static string RATE = "Rate";
    public static string INTERVAL = "Interval";
    public static string RANGE = "Range";
    public static string DAMAGE = "Damage";
    public static string MUZZLE = "Muzzle";
    public static string BULLET = "Bullet";
    public static string TRIGGER = "Trigger";
    public static string KNOCKBACK = "Knockback";
    public static string ACCELERATION = "Acceleration";
    public static string HEALTH = "Health";
    public static string MAX = "Max";
    public static string TELEPORT = "Teleport";
    public static string DIRECTION = "Direction";
    public static string INVINCIBLE = "Invincible";
    public static string WAVE = "Wave";

    //组合A+B
    public static string Assemble(string labelA, string labelB) {
        return string.Concat(labelA, labelB);
    }

    //组合A+B+C
    public static string Assemble(string labelA, string labelB, string labelC) {
        return string.Concat(labelA, labelB, labelC);
    }
}