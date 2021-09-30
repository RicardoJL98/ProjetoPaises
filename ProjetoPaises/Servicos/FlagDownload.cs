
namespace ProjetoPaises.Servicos
{

    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using ProjetoPaises.Modelos;
    using Svg;


    class FlagDownload
    {
        #region Atributos

        private List<Pais> Paises;

        private ApiService apiService;


        #endregion

        public async Task LoadApiBandeiras()
        {
            apiService = new ApiService();

            var response = await apiService.GetPaises("http://restcountries.eu/", "/rest/v2/all");

            Paises = (List<Pais>)response.Result;
            string path = $"{Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 17)}/Flags/";

            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");

                foreach (var item in Paises)
                {
                    //Download ficheiro svg
                    string svgFileName = item.Flag;

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(svgFileName, $"{path}bandeira.svg");
                    }

                    //Conversão ficheiro svg para png
                    var byteArray = Encoding.ASCII.GetBytes($"{path}bandeira.svg");

                    using (var stream = new MemoryStream(byteArray))
                    {
                        var svgDocument = SvgDocument.Open($"{path}bandeira.svg");
                        try
                        {
                            var bitmap = svgDocument.Draw();
                            bitmap.Save($"{path}{item.Name}.png", ImageFormat.Png);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Erro");
                        }
                    }
                }
            }
        }
    }
}
