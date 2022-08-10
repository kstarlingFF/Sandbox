namespace FreddieShiz;
public class FinalRestingPlace 
{
    private const string BrokePath = "C:\\workspace\\DVOE\\FreddieShiz\\cte\\Broke\\{0}";
    private const string GoodPath = "C:\\workspace\\DVOE\\FreddieShiz\\cte\\Good\\{0}";
    private const string OGPath = "C:\\workspace\\DVOE\\FreddieShiz\\cte\\Original\\{0}";
    public FinalRestingPlace() { }

    public static void ItWasAGoodBoy(string report, string id)
    {
        File.WriteAllText(string.Format(GoodPath, id), report);
    }

    public static void WhereTheBrokenGo(string report, string id)
    {
        File.WriteAllText(string.Format(BrokePath, id), report);
    }

    public static void OGHome(string report, string id)
    {
        File.WriteAllText(string.Format(OGPath, id), report);
    }
}
