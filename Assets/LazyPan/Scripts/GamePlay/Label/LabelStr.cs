using LazyPan;

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
    public static string ATTACK = "Attack";
    public static string SOUND = "Sound";
    public static string BE = "Be";
    public static string HIT = "Hit";
    public static string COMBO = "Combo";
    public static string RECOVER = "Recover";
    public static string REDUCE = "Reduce";
    public static string BEHEAD = "Behead";
    public static string PICK = "Pick";
    public static string CREATE = "Create";
    public static string TIME = "Time";
    public static string PENETRATE = "Penetrate";//穿透
    public static string DISTANCE = "Distance";//距离
    public static string GET = "Get";
    public static string AUTO = "Auto";
    public static string ABSORB = "Absorb";
    public static string DESTROY = "Destroy";
    public static string KILL = "Kill";
    public static string BURN = "Burn";
    public static string FROST = "Frost";
    public static string SLOW = "Slow";
    public static string IGNORE = "Ignore";
    public static string LOGO = "Logo";
    public static string FROZEN = "Frozen";
    public static string EFFECT = "Effect";
    public static string BINGO = "Bingo";
    public static string ANTIBODY = "Antibody";
    public static string REBORN = "Reborn";
    public static string DEAD = "Dead";
    public static string ABSORBS = "Absorbs";
    public static string CONVERSION = "Conversion";
    public static string DETAIL = "Detail";
    public static string SURE = "Sure";

    //组合A+B
    public static string Assemble(string labelA, string labelB) {
        return string.Concat(labelA, labelB);
    }

    //组合A+B+C
    public static string Assemble(string labelA, string labelB, string labelC) {
        return string.Concat(labelA, labelB, labelC);
    }

    //组合A+B+C+D
    public static string Assemble(string labelA, string labelB, string labelC, string labelD) {
        return string.Concat(labelA, labelB, labelC, labelD);
    }

    //组合A+B+C+D+E
    public static string Assemble(string labelA, string labelB, string labelC, string labelD, string labelE) {
        return string.Concat(labelA, labelB, labelC, labelD, labelE);
    }
}