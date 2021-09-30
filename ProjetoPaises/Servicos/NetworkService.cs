
namespace ProjetoPaises.Servicos
{
    using System.Net;
    using ProjetoPaises.Modelos;

    public class NetworkService
    {
        //É uma classe que na prática vai disponibilizar uma ligação à internet

        /// <summary>
        /// Metodo que vamos utilizar para saber se temos ligação à internet
        /// </summary>
        /// <returns></returns>
        public Response CheckConnection()
        {
            var client = new WebClient(); // vamos usar para testar se temos ligação À internet deste lado

            try
            {
                using(client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSucess = true,
                    };
                }
            }
            catch (System.Exception)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = "Configure a sua ligação à internet",

                };
            }
        }
    }
}
