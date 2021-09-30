namespace ProjetoPaises.Modelos
{
    public class Response
    {
        //Vamos utilizar esta classe para perceber se está tudo bem e o que pode ter corrido mal

        /// <summary>
        /// Propriedade serve para perceber se as ações correram bem ou não
        /// </summary>
        public bool IsSucess { get; set; }

        /// <summary>
        /// A Api devolve uma mensagem caso algo tenha corrido mal a dizer o que se passou
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Caso a Api carregue bem vai guardar um objecto Result
        /// </summary>
        public object Result { get; set; }
    }
}
