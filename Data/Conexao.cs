using Microsoft.Data.SqlClient;

namespace ListaDeContatatos.Data
{
    public class Conexao
    {
        private readonly string conexao;

        public Conexao(IConfiguration configuration)
        {
            conexao = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");
        }
        public SqlConnection conectar()
        {
            return new SqlConnection(conexao);
        }
    }
}
