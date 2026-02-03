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
                string sql = "SELECT Id, Telefone, Nome, Email, Status FROM Pessoa";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            listaPessoas.Add(new Pessoa
                            {
                                Id = reader.GetInt32("Id"),
                                Nome = reader.GetString("Nome"),
                                Email = reader.GetString("Email"),
                                Status = reader.GetString("Status"),
                                Telefone = reader.GetString("Telefone")
                            });
                        }
                    }
                }
            }

            return View(listaPessoas);
        }
        [HttpPost]
        public IActionResult Criar(Pessoa pessoa)
        {
            using (var con = _conexao.conectar())
            {
                string sql = "INSERT INTO Pessoa (Nome, Email, Telefone, Status) VALUES (@Nome, @Email, @Telefone, @Status)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nome", pessoa.Nome);
                    cmd.Parameters.AddWithValue("@Email", pessoa.Email);
                    cmd.Parameters.AddWithValue("@Telefone", pessoa.Telefone);
                    cmd.Parameters.AddWithValue("@Status", pessoa.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            Pessoa pessoa = null;

            using (var con = _conexao.conectar())
            {
                string sql = "SELECT Id, Nome, Email, Telefone, Status FROM Pessoa WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pessoa = new Pessoa
                            {
                                Id = reader.GetInt32("Id"),
                                Nome = reader.GetString("Nome"),
                                Email = reader.GetString("Email"),
                                Telefone = reader.GetString("Telefone"),
                                Status = reader.GetString("Status")


                            };
                        }
                    }
                }
            }

            if (pessoa == null)
            {
                return NotFound();
            }

            return Json(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPessoa(Pessoa pessoa)
        {
            try
            {
                using (var con = _conexao.conectar())
                {
                    string sql = "UPDATE Pessoa SET Nome = @Nome, Status = @Status, Email = @Email, Telefone = @Telefone WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", pessoa.Id);
                        cmd.Parameters.AddWithValue("@Nome", pessoa.Nome ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", pessoa.Status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", pessoa.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Telefone", pessoa.Telefone ?? (object)DBNull.Value);

                        await con.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();


                        return Json(new { success = rowsAffected > 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Deletar(int Id)
        {
            try
            {
                using (var con = _conexao.conectar())
                {
                    string sql = "DELETE FROM Pessoa WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);

                        await con.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();


                        return Json(new { success = rowsAffected > 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult ExportarExcel([FromBody] List<int> ids)
        {
        }
    }
