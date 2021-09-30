
namespace ProjetoPaises.Servicos
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using ProjetoPaises.Modelos;


    public class ApiService
    {
        /// <summary>
        /// Metodo que vai buscar as taxas, ele só tem uma tarefa e no fim devolve um objeto do tipo response
        /// </summary>
        /// <param name="urlBase"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public async Task<Response> GetPaises(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient(); // criamos um http para fazer a ligação externa via http

                client.BaseAddress = new Uri(urlBase); //Url da api

                var response = await client.GetAsync(controller); // controlador é uma pasta onde vamos ter os paises

                var result = await response.Content.ReadAsStringAsync(); // result vai ficar à espera de response e vai lê-la como uma string

                var result2 = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) // se algo correu mal no carregamento
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result,
                    };
                }

                var paises = JsonConvert.DeserializeObject<List<Pais>>(result); //Na prática convertemos numa lista de pais agarrando no result

                return new Response
                {
                    IsSucess = true,
                    Result = paises
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetPaises2(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient(); // criamos um http para fazer a ligação externa via http

                client.BaseAddress = new Uri(urlBase); //Url da api

                var response = await client.GetAsync(controller); // controlador é uma pasta onde vamos ter os paises

                var result = await response.Content.ReadAsStringAsync(); // result vai ficar à espera de response e vai lê-la como uma string

                if (!response.IsSuccessStatusCode) // se algo correu mal no carregamento
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result,
                    };
                }

                var paises = JsonConvert.DeserializeObject<List<LinguaPais>>(result); //Na prática convertemos numa lista de pais agarrando no result

                return new Response
                {
                    IsSucess = true,
                    Result = paises
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
