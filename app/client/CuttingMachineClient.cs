using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using lantek.model;
using Microsoft.Extensions.Configuration;

namespace lantek.client;

public interface ICuttingMachineClient{

    Task<List<CuttingMachine>> getCuttingMachines();
}

public class CuttingMachineClient : ICuttingMachineClient
{
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpClient HttpClient { get; }
        private readonly IConfiguration _config;

        public CuttingMachineClient(HttpClient client, IConfiguration config)
        {
            _config = config;
            HttpClient = client;

            var authvalue = Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", _config.GetValue<string>("user"), _config.GetValue<string>("password"))));
            HttpClient.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Basic", authvalue);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<CuttingMachine>> getCuttingMachines() 
        {
            var machines = new List<CuttingMachine>();
            try
            {               
                var response = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("url")));
                response.EnsureSuccessStatusCode();

                var resultContent = response.Content.ReadAsStringAsync().Result;
                machines = JsonSerializer.Deserialize<List<CuttingMachine>>(resultContent);
                
                machines.OrderBy(x => x.name).ToList().ForEach(delegate(CuttingMachine cm) {
                    Console.WriteLine(cm.name + " - "+cm.manufacturer);
                });
            }
            catch (Exception e)
            {       
                Console.WriteLine(e.Message);
                log.Error(e.Message);
            }

            return machines;
        }
}
