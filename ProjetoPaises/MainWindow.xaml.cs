
namespace ProjetoPaises
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using ProjetoPaises.Modelos;
    using ProjetoPaises.Servicos;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributos

        //Api Principal
        private List<Pais> Paises; // na prática só precisamos da referência, não precisamos de estar a instanciar a lista

        private NetworkService networkService;

        private ApiService apiService;

        private DialogService dialogService;

        private DataService dataService;

        private FlagDownload flagDownload;

        //Api secundária
        private List<LinguaPais> LinguaPaises;

        private SecondDataService secondDataService;

        #endregion


        public MainWindow()
        {
            InitializeComponent();
            networkService = new NetworkService();
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService(); // na prática estamos a chamar o construtor ele vai logo tratar da parte da pasta
            flagDownload = new FlagDownload();
            secondDataService = new SecondDataService();
            Loadpaises();
        }

        /// <summary>
        /// Metodo que carrega os Paises da Api principal
        /// </summary>
        private async void Loadpaises()
        {
            await flagDownload.LoadApiBandeiras(); // Se não tiver já feito o download faz o download das Bandeiras

            bool load; // variável para saber se foi carregada ou não

            lblResultado.Content = "A atualizar países...";

            var connection = networkService.CheckConnection(); // vamos verificar a conexão

            // Consoante o resultado da conexão acima ele vai decidir carregar os dados da API se tiver internet ou carregar os dados apartir
            // da base de dados se não tiver internet
            if (!connection.IsSucess)
            {
                LoadLocalPaises();
                load = false; // quer dizer que ele não conseguiu carregar
            }
            else
            {
                await LoadApiPaises();
                load = true;
            }

            // se por algum motivo na primeira vez que o utilizador se liga a base de dados não estar preenchida
            if (Paises.Count == 0) //se a lista de paises não for carregada independentemente de vir localmente ou da api
            {
                lblResultado.Content = "Não há ligação à internet" + Environment.NewLine +
                    "e não foram previamente carregados os países." + Environment.NewLine +
                    "Tente mais tarde!";

                lblSatus.Content = "Primeira inicialização deverá ter ligação à internet";

                return;
            }

            foreach (Pais pais in Paises) //Adiciona os paises à ListBox
            {
                ListBoxPaises.Items.Add(pais);
                ListBoxPaises.DisplayMemberPath = "Name";
            }

            btnSelecionar.IsEnabled = true; // depois dos paises estarem carregados deixamos ativo o botão que seleciona o pais

            lblResultado.Content = "Países atualizados...";

            if (load)
            {
                lblSatus.Content = string.Format("Países carregados da internet em {0:F}", DateTime.Now);
            }
            else // significa que os paises foram carregados da base de dados
            {
                lblSatus.Content = string.Format("Países carregados da Base de Dados.");
            }

            pbCarregamento.Value = 250;
        }

        /// <summary>
        /// Metodo que carrega os países da Base de Dados
        /// </summary>
        private void LoadLocalPaises()
        {
            Paises = dataService.GetData(); // o getData retorna uma lista então temos de a ir buscar

            LinguaPaises = secondDataService.GetData();
        }

        /// <summary>
        /// Metodo que carrega os países da Api
        /// </summary>
        /// <returns></returns>
        private async Task LoadApiPaises()
        {
            pbCarregamento.Value = 0;

            var progress = new Progress<int>(p => pbCarregamento.Value = p);

            //Damos Load na Api principal
            var response = await apiService.GetPaises("http://restcountries.eu", "/rest/v2/all");

            Paises = (List<Pais>)response.Result; // estamos a ir buscar uma referência da lista que ja foi criada através do response

            //Damos Load na Api secundária
            var response2 = await apiService.GetPaises2("http://PaisesApi.somee.com", "/api/Pais");

            LinguaPaises = (List<LinguaPais>)response2.Result;

            dataService.DeleteData(); //Apaga os dados da base de dados da Api Principal

            secondDataService.DeleteData(); //Apaga os dados da base de dados da Api Secundária
            //Guardamos também os paises na Base de Dados
            await Task.Run(() => dataService.Savedata(Paises, progress));// Atualizamos a base de dados da Api Principal com os novos dados

            await Task.Run(() => secondDataService.Savedata(LinguaPaises, progress)); // Atualizamos a base de dados da Api Secundária com os novos dados
        }

        /// <summary>
        /// Metodo que determina a função do botao selecionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            LoadPaisSelecionado();

            if (ListBoxPaises.SelectedItem != null)
            {
                Pais paisinho = (Pais)ListBoxPaises.SelectedItem;

                DetalhesPais detalhesPaises = new DetalhesPais(paisinho, LinguaPaises); //Passamos o paisinho que tem as propriedades do pais Selecionado
                detalhesPaises.Show(); // Abre a nova página
            }
        }

        /// <summary>
        /// Metodo que nos vai levar à pagina que mostra todos os dados do pais selecionado
        /// </summary>
        private void LoadPaisSelecionado()
        {
            if (ListBoxPaises.SelectedItem == null)
            {
                dialogService.ShowMessage("Erro", "Selecione um país da lista dos países");
                return;
            }
            else
            {
                BtnCancelar.IsEnabled = true;
            }
        }

        /// <summary>
        /// Metodo para o botao que cancela o Pais selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ListBoxPaises.SelectedItem = null;
        }
    }
}
