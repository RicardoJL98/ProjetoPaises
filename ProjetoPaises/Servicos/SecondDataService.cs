
namespace ProjetoPaises.Servicos
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Threading.Tasks;
    using ProjetoPaises.Modelos;


    public class SecondDataService
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
        public SecondDataService()
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

            //if (File.Exists(@"Data\LinguaPaises.sqlite") && connection2.IsSucess)
            //{
            //    return;
            //}
            //else if (!File.Exists(@"Data\LinguaPaises.sqlite") && !connection2.IsSucess)
            //{
            //    return;
            //}

            var path = @"Data\LinguaPaises.sqlite"; //vai servir para o caminho da base de dados principal

            try
            {
                connection = new SQLiteConnection("DataSource=" + path); // na prática o que está dentro de () é a connection string
                connection.Open(); // abre a base de dados ou cria se ela não existir

                string sqlcommand =
                    "create table if not exists linguapaises(Id int, Name varchar(60), Language varchar(50), Word varchar(50))"; // cria a tabela

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
        public async Task Savedata(List<LinguaPais> LinguaPais, IProgress<int> progress)
        {
            int cont = 0;

            try
            {
                foreach (var linguapais in LinguaPais)
                {
                    string sql = string.Format("insert into linguapaises (Id, Name, Language, Word) values({0}, '{1}', '{2}', '{3}')",
                        linguapais.Id, linguapais.Name.Replace("'", "''"), linguapais.Language.Replace("'", "''"), linguapais.Word);

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
        public List<LinguaPais> GetData()
        {
            List<LinguaPais> linguapais = new List<LinguaPais>();

            try
            {
                string sql = "select Id, Name, Language, Word from linguapaises";

                command = new SQLiteCommand(sql, connection);

                //Lê cada registo
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read()) // o while vai à tabela ler registo a registo
                {
                    linguapais.Add(new LinguaPais
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Language = (string)reader["Language"],
                        Word = (string)reader["Word"]
                        //Aqui vai carregar da base de dados para dentro da lista
                    });
                }

                connection.Close();

                return linguapais;
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
                string sql = "delete from linguapaises";

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
