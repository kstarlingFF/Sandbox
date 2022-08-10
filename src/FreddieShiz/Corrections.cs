using FormFree.Plumbing.GSE.FHLMC.DVOE.v24;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreddieShiz;
public class Corrections
{
    private const string Misspelling = "AssetTransctionDescription";
    private const string CorrectSpelling = "AssetTransactionDescription";

    private const string LOANIDENTIFIERasArray = @"LOAN_IDENTIFIER"":[";
    private const string VERIFICATIONOFASSETasArray = @"VERIFICATION_OF_ASSET";

    public static string Dispatcher(string report, string id)
    { 
        report = FindAndReplaceMisspelling(report);
        report = RemoveLOANIDENTIFIERasArray(report);
        report = RemoveVERIFICATIONOFASSETasArray(report);
        if (CanDeserialize(report))
        {
            FinalRestingPlace.ItWasAGoodBoy(report, id);
            return report;
        }
        else
        {
            FinalRestingPlace.WhereTheBrokenGo(report, id);
            return null;
        }
    }

    public static string RemoveLOANIDENTIFIERasArray(string report)
    {
        if (report.Contains(LOANIDENTIFIERasArray))
        {
            var startIndex = report.IndexOf(LOANIDENTIFIERasArray);
            var openIndex = report.IndexOf("[");
            report = report.Remove(openIndex, 1);
            var closeIndex = report.IndexOf("]");
            report = report.Remove(closeIndex, 1);
        }
        return report;  
    }

    public static string RemoveVERIFICATIONOFASSETasArray(string report)
    {
        var jobj = JObject.Parse(report);
        Func<JObject, JToken, string> func = (r, t) =>
        {
            JToken token = t[VERIFICATIONOFASSETasArray];
            if (token != null && token.Type == JTokenType.Array)
            {
                var rString = JsonConvert.SerializeObject(r);
                var objString = JsonConvert.SerializeObject((JObject)token[0]);
                var tokenString = JsonConvert.SerializeObject(token);
                var start = rString.IndexOf(tokenString);
                rString = rString.Remove(start, tokenString.Length);
                rString = rString.Insert(start, objString);
                return rString;
            }
            return null;
        };

        Find(jobj, jobj, func);
        if (ReportWithCorrectedVerification != null)
        {
            return ReportWithCorrectedVerification;
        }
        else
        {
            return report;
        }
    }

    public static string ReportWithCorrectedVerification { get; set; }

    public static void Find(
        JObject fullReport, 
        JToken token, 
        Func<JObject, JToken, string> func)
    {
        string correct = "";
        if (token.Type == JTokenType.Object)
        {
            var r = func(fullReport, token);
            if (r != null)
            {
                ReportWithCorrectedVerification =  r;
            }
            foreach (JProperty child in token.Children<JProperty>())
            {
                Find(fullReport, child.Value, func);
                continue;
            }
        }
    }

    public static string FindAndReplaceMisspelling(string report)
    {
        while (report.Contains(Misspelling))
        {
            var startIndex = report.IndexOf(Misspelling);
            var count = Misspelling.Length;
            report = report.Remove(startIndex, count);
            report = report.Insert(startIndex, CorrectSpelling);
        }
        return report;
    }

    public static bool CanDeserialize(string report)
    {
        try
        {
            var dvoe = JsonConvert.DeserializeObject<FreddieCommonDVOE>(report);
            return true;
        }
        catch (Exception e)
        {
            // broke during deserailization so it needs to be fixed
            return false;
        }
    }
}
