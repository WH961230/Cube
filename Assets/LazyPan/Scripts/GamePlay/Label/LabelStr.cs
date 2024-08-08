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
    public static string LEVEL = "Level";
    public static string EXPERIENCE = "Experience";
    public static string GRID = "Grid";
    public static string PARENT = "Parent";
    public static string COUNT = "Count";
    public static string ROBOT = "Robot";
    public static string GLOBAL = "Global";
    public static string PLAYER = "Player";
    public static string LINE = "Line";
    public static string SURROUND = "Surround";
    public static string BALL = "Ball";
    public static string INCREASE = "Increase";
    public static string CHARGE = "Charge";
    public static string DROP = "Drop";
    public static string RATIO = "Ratio";
    public static string ENERGY = "Energy";
    public static string RING = "Ring";
    public static string START = "Start";
    public static string RADIU = "Radiu";
    public static string PULSE = "Pulse";
    public static string DURATION = "Duration";
    public static string DELAY = "Delay";
    public static string DOWN = "Down";
    public static string ROTATE = "Rotate";
    public static string MOVE = "Move";
    public static string BOOM = "Boom";
    public static string DIFFICULTY = "Difficulty";
    public static string DEFAULT = "Default";
    public static string ACTIVATABLE = "Activatable";
    public static string CURRENT = "Current";
    public static string PATH = "Path";
    public static string TYPE = "Type";

    //组合A+B
    public static string Assemble(string labelA, string labelB) {
        return string.Concat(labelA, labelB);
    }

    //组合A+B+C
    public static string Assemble(string labelA, string labelB, string labelC) {
        return string.Concat(labelA, labelB, labelC);
    }
}