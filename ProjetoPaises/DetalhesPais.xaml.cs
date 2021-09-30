
namespace ProjetoPaises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using ProjetoPaises.Modelos;
    using ProjetoPaises.Servicos;


    /// <summary>
    /// Interaction logic for DetalhesPais.xaml
    /// </summary>
    public partial class DetalhesPais : Window
    {
        #region Atributos

        private Pais _pais;

        private List<LinguaPais> LinguaPaises;

        #endregion

        public DetalhesPais(Pais paisinho, List<LinguaPais> linguaPaises)
        {
            InitializeComponent();
            LinguaPaises = new List<LinguaPais>();
            //VerificarConexao();

            _pais = paisinho;

            LinguaPaises = linguaPaises;

            ListBoxDetalhes.Items.Add("Nome: " + _pais.Name);

            if (_pais.Capital == "")
            {
                ListBoxDetalhes.Items.Add("Capital: " + "Dados Indisponíveis");
            }
            else
            {
                ListBoxDetalhes.Items.Add("Capital: " + _pais.Capital);
            }

            if (_pais.Region == "")
            {
                ListBoxDetalhes.Items.Add("Região: " + "Dados Indisponíveis");
            }
            else
            {
                ListBoxDetalhes.Items.Add("Região: " + _pais.Region);
            }

            if (_pais.SubRegion == "")
            {
                ListBoxDetalhes.Items.Add("Subregião: " + "Dados Indisponíveis");
            }
            else
            {
                ListBoxDetalhes.Items.Add("Subregião: " + _pais.SubRegion);
            }

            if (_pais.Population == 0)
            {
                ListBoxDetalhes.Items.Add("População: " + "Dados Indisponíveis");
            }
            else
            {
                ListBoxDetalhes.Items.Add("População: " + _pais.Population.ToString() + " Pessoas");
            }

            if (_pais.Gini == "" || _pais.Gini == null)
            {
                ListBoxDetalhes.Items.Add("Gini: " + "Dados Indisponíveis");
            }
            else
            {
                ListBoxDetalhes.Items.Add("Gini: " + _pais.Gini);
            }

            foreach (var pais in LinguaPaises)
            {
                if (pais.Name == _pais.Name)
                {
                    ListBoxDetalhes.Items.Add("Language: " + pais.Language);

                    ListBoxDetalhes.Items.Add("Tradução da palavra Programador: ");

                    if (pais.Word == null)
                    {
                        ListBoxDetalhes.Items.Add("Word: " + "Dados Indisponíveis");
                    }
                    else
                    {
                        ListBoxDetalhes.Items.Add("Word: " + pais.Word);
                    }
                }
            }

            //Dar display na Flag

            imgBandeira.Source = null;

            var campos = (Pais)paisinho;

            string path = $"{ Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 17) }/Flags/";

            DirectoryInfo selectedFlag = new DirectoryInfo(path);

            foreach (FileInfo Files in selectedFlag.GetFiles("*png")) // *png vai buscar todas as files de png
            {
                if (File.Exists($"{path}{campos.Name}.png"))
                {
                    imgBandeira.Source = new BitmapImage(new Uri($"{path}{campos.Name}.png"));
                }
                else
                {
                    string path2 = $"{ Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 17) }/ImageNotFound/";

                    imgBandeira.Source = new BitmapImage(new Uri($"{path2}Flag.png"));
                }
            }
        }

        /// <summary>
        /// Botao que retorna ao MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
