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

    //组合A+B
    public static string Assemble(string labelA, string labelB) {
        return string.Concat(labelA, labelB);
    }

    //组合A+B+C
    public static string Assemble(string labelA, string labelB, string labelC) {
        return string.Concat(labelA, labelB, labelC);
    }
}