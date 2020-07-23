using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ConsoleApplication1
{
    class Program
    {
        public Program()
        {
            Init();
        }
        public async void Init()
        {
            try
            {
                string baseUrl = "http://fipeapi.appspot.com/api/1/";
                string[] tipos = { "carros", "motos", "caminhoes" };
                SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\dados\Fipe\fipe.db;");
                conn.Open();
                for (int i = 0; i < tipos.Length; i++)
                {

                    string jsonStr = await GetStringFromURL(string.Format("{0}/{1}/marcas.json", baseUrl, tipos[i]));
                    List<Marca> listMarcas = JsonConvert.DeserializeObject<List<Marca>>(jsonStr);
                    foreach (Marca marca in listMarcas)
                        marca.Gravar(conn, i);
                    foreach (Marca marca in listMarcas)
                    {
                        Console.WriteLine("Gravando: {0}\n", marca.Nome);
                        jsonStr = await GetStringFromURL(string.Format("{0}/{1}/veiculos/{2}.json", baseUrl, tipos[i], marca.Id));
                        List<Modelo> listModelos = JsonConvert.DeserializeObject<List<Modelo>>(jsonStr);
                        foreach (Modelo modelo in listModelos)
                        {
                            Console.WriteLine("Gravando: {0}\n", modelo.Nome);
                            modelo.Gravar(conn, marca);
                            //jsonStr = await GetStringFromURL(string.Format("{0}/{1}/veiculo/{2}/{3}.json", baseUrl, tipos[i], marca.Id, modelo.Id));
                            //List<AnoModelo> listAnoMod = JsonConvert.DeserializeObject<List<AnoModelo>>(jsonStr);
                            //foreach (AnoModelo anMod in listAnoMod)
                            //{
                            //    Console.WriteLine("Gravando: {0}\n", anMod.Name);
                            //    jsonStr = await GetStringFromURL(string.Format("{0}/{1}/veiculo/{2}/{3}/{4}.json", baseUrl, tipos[i], marca.Id, modelo.Id, anMod.Id));
                            //    Veiculo veic = JsonConvert.DeserializeObject<Veiculo>(jsonStr);
                            //    veic.Gravar(conn, modelo, anMod);
                            //}
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.StackTrace);
            }
        }
        public async Task<string> GetStringFromURL(string url)
        {
            HttpClient cliente = new HttpClient();
            HttpRequestMessage requerimento = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage resposta = await cliente.SendAsync(requerimento);
            string data = await resposta.Content.ReadAsStringAsync();
            return data;
        }
        public async Task<JArray> GetJsonArrayFromURL(string url)
        {
            HttpClient cliente = new HttpClient();
            HttpRequestMessage requerimento = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage resposta = await cliente.SendAsync(requerimento);
            string data = await resposta.Content.ReadAsStringAsync();
            return JArray.Parse(data);
        }
        public async Task<JObject> GetJsonObjectFromURL(string url)
        {
            HttpClient cliente = new HttpClient();
            HttpRequestMessage requerimento = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage resposta = await cliente.SendAsync(requerimento);
            string data = await resposta.Content.ReadAsStringAsync();
            return JObject.Parse(data);
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            Console.Read();
        }
    }
}
