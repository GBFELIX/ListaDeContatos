using ListaDeContatatos.Data;
using ListaDeContatatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ListaDeContatatos.Controllers
{
    public class ContatoController : Controller
    {
        private readonly Conexao _conexao;

        public ContatoController(Conexao conexao)
        {
            _conexao = conexao;
        }

        public IActionResult Index()
        {
            List<Pessoa> listaPessoas = new();


            using (var con = _conexao.conectar())
            {

                string sql = "SELECT Id, Nome, Email, Status FROM Pessoa";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();

                    // 3. Executa o Reader para ler linha por linha
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            listaPessoas.Add(new Pessoa
                            {
                                Id = reader.GetInt32("Id"),
                                Nome = reader.GetString("Nome"),
                                Email = reader.GetString("Email"),
                                Status = reader.GetString("Status")
                            });
                        }
                    }
                }
            }

            return View(listaPessoas);
        }
    }
}