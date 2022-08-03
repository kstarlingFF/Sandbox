using System.Text;
using FormFree.Plumbing.GSE.FHLMC.DVOE.v24;
using FormFree.Plumbing.Storage;
using Newtonsoft.Json;

namespace FreddieShiz
{
    public class Worker
    {
        private const string Misspelling = "AssetTransctionDescription";
        private const string CorrectSpelling = "AssetTransactionDescription";

        private readonly IStorage _storage;

        public Worker(IStorage storage)
        {
            this._storage = storage;
        }

        public async Task Run(List<string> reportIds)
        {
            foreach(var id in reportIds)
            {
                var lowerId = id.ToLower();
                var data = await this._storage.FetchAsync(lowerId);
                var report = Encoding.UTF8.GetString(data);
                var response = FindAndReplaceMisspelling(report);
                if (response.IsValid)
                {
                    if (!response.ShouldReplace)
                    {
                        continue;
                    }
                    //await this._storage.StoreAsync(id, Encoding.UTF8.GetBytes(response.Report));
                    var goodpath = $"C:\\workspace\\DVOE\\FreddieShiz\\Good\\{lowerId}";
                    File.WriteAllText(goodpath, response.Report);
                }
                var brokepath = $"C:\\workspace\\DVOE\\FreddieShiz\\Broke\\{lowerId}";
                File.WriteAllText(brokepath, response.Report);
            }
        }


        public bool CanDeserialize(string report)
        {
            try
            {
                var dvoe = JsonConvert.DeserializeObject<FreddieCommonDVOE>(report);
                return true;
            }
            catch (Exception e)
            {
                // broke during deserailization so it needs to be fixed
                Console.WriteLine($"{e.Message}");
                return false;
            }
        }

        public Response FindAndReplaceMisspelling(string report)
        {
            if (CanDeserialize(report))
                return new Response(true, false, report);
                
            if (report.Contains(Misspelling))
            {
                var startIndex = report.IndexOf(Misspelling);
                var count = Misspelling.Length;
                report = report.Remove(startIndex, count);
                report = report.Insert(startIndex, CorrectSpelling);
                if (CanDeserialize(report))
                {
                    return new Response(true, true, report);
                }
                else
                {
                    return new Response(false, false, report);
                }
            }
            return new Response(false, false, report);
        }
    }

    public struct Response
    {
        public bool IsValid { get; private set; }
        public bool ShouldReplace { get; private set; }
        public string Report { get; private set; }

        public Response(bool isValid, bool shouldReplace, string report = null)
        {
            this.Report = report;
            this.IsValid = isValid;
            this.ShouldReplace = shouldReplace;
        }
    }

}
