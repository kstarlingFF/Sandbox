using System.Text;
using FormFree.Plumbing.Storage;

namespace FreddieShiz
{
    public class Worker
    {
        private readonly IStorage _storage;

        public Worker(IStorage storage)
        {
            this._storage = storage;
        }

        public async Task Run(List<string> reportIds)
        {
            foreach (var id in reportIds)
            {
                var lowerId = id.ToLower();
                byte[] data;
                try
                {
                    data = await this._storage.FetchAsync(lowerId);

                }
                catch (Exception)
                {
                    Console.WriteLine(id);
                    continue;
                }
                var report = Encoding.UTF8.GetString(data);
                FinalRestingPlace.OGHome(report, id);
                var result = Corrections.Dispatcher(report, id);
                if (result != null)
                {
                    //await this._storage.StoreAsync(lowerId, Encoding.UTF8.GetBytes(result));
                }
            }
        }
    }
}
