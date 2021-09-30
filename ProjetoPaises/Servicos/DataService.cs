

namespace ProjetoPaises.Servicos
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Threading.Tasks;
    using ProjetoPaises.Modelos;

    public class DataService
    {
        #region Atributos

        /// <summary>
        /// Atributo que vai fazer a conexão
        /// </summary>
        private SQLiteConnection connection;

        /// <summary>
        /// Atributo onde vamos fazer os comandos
        /// </summary>
        private SQLiteCommand command;

        /// <summary>
        /// Atributo para termos acesso aos nossos dialog services
        /// </summary>
        private DialogService dialogService;

        private NetworkService networkService;

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor da class
        /// </summary>
        public DataService()
        {
            // vamos optar por na raiz criar uma pasta Data e vai ser nessa pasta que vamos criar a Base de Dados
            // se a pasta não existir ele vai criar

            dialogService = new DialogService();

            networkService = new NetworkService();

            var connection2 = networkService.CheckConnection();

            if (!Directory.Exists("Data")) // se a pasta não existir vamos cria-la
            {
                Directory.CreateDirectory("Data"); // criamos a pasta
            }

            //if (File.Exists(@"Data\Paises.sqlite") && connection2.IsSucess)
            //{
            //    return;
            //}
            //else if (!File.Exists(@"Data\Paises.sqlite") && !connection2.IsSucess)
            //{
            //    return;
            //}

            var path = @"Data\Paises.sqlite"; //vai servir para o caminho da base de dados principal

            try
            {
                connection = new SQLiteConnection("DataSource=" + path); // na prática o que está dentro de () é a connection string
                connection.Open(); // abre a base de dados ou cria se ela não existir

                string sqlcommand =
                    "create table if not exists paises(Name varchar(50), Capital varchar(50), Region varchar(50), SubRegion varchar(50), Population int, Gini varchar(20), Flag varchar(250))"; // cria a tabela

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery(); // na prática vai executar o comando que aqui está
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }

            // aqui básicamente se não existir ele cria a base de dados e a tabela se existir ele vai abrir a base de dados
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo que vai preencher a tabela da base de dados com os valores
        /// </summary>
        /// <param name="Paises"></param>
        public async Task Savedata(List<Pais> Paises, IProgress<int> progress)
        {
            int cont = 0;

            try
            {
                foreach (var pais in Paises)
                {
                    string sql = string.Format("insert into Paises (Name, Capital, Region, SubRegion, Population, Gini, Flag) values('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}')",
                        pais.Name.Replace("'","''"), pais.Capital.Replace("'", "''"), pais.Region.Replace("'", "''"), pais.SubRegion.Replace("'", "''"), pais.Population, pais.Gini, pais.Flag);

                    cont++;
                    command = new SQLiteCommand(sql, connection);

                    progress.Report(cont);
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        /// <summary>
        /// Metodo que retorna a lista de paises para fora já com os valores
        /// </summary>
        public List<Pais> GetData()
        {
            List<Pais> paises = new List<Pais>();

            try
            {
                string sql = "select Name, Capital, Region, SubRegion, Population, Gini, Flag from Paises";

                command = new SQLiteCommand(sql, connection);

                //Lê cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while(reader.Read()) // o while vai à tabela ler registo a registo
                {
                    paises.Add(new Pais
                    {
                        Name = (string) reader["Name"],
                        Capital = (string) reader["Capital"],
                        Region = (string) reader["Region"],
                        SubRegion = (string) reader["Subregion"],
                        Population = (int) reader["Population"],
                        Gini = (string) reader ["Gini"],
                        Flag = (string) reader["Flag"]
                        //Aqui vai carregar da base de dados para dentro da lista
                    });
                }

                connection.Close();

                return paises;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Vai limpar a base de dados para ser atualizada com novos dados
        /// </summary>
        public void DeleteData()
        {
            try
            {
                string sql = "delete from Paises";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        #endregion
    }
}
