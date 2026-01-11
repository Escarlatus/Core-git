using System.Data.SqlClient;
using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlUsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        public SqlUsuarioRepository(string connectionString) { _connectionString = connectionString; }

        public Usuario? GetByUsername(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Usuario WHERE Username = @U", conn);
                cmd.Parameters.AddWithValue("@U", username);
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read()) return Map(reader);
            }
            return null;
        }

        public void Add(Usuario entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Usuario (Username, PasswordHash, RolId) VALUES (@U, @P, @R)", conn);
                cmd.Parameters.AddWithValue("@U", entity.Username);
                cmd.Parameters.AddWithValue("@P", entity.PasswordHash);
                cmd.Parameters.AddWithValue("@R", entity.RolId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Usuario entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Usuario SET Username=@U, PasswordHash=@P, RolId=@R WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@U", entity.Username);
                cmd.Parameters.AddWithValue("@P", entity.PasswordHash);
                cmd.Parameters.AddWithValue("@R", entity.RolId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                new SqlCommand("DELETE FROM Usuario WHERE Id=" + id, conn).ExecuteNonQuery();
            }
        }

        public Usuario? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read()) return Map(reader);
            }
            return null;
        }

        public IEnumerable<Usuario> GetAll()
        {
            var l = new List<Usuario>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var r = new SqlCommand("SELECT * FROM Usuario", conn).ExecuteReader())
                    while (r.Read()) l.Add(Map(r));
            }
            return l;
        }

        private Usuario Map(SqlDataReader r) => new Usuario
        {
            Id = (int)r["Id"],
            Username = r["Username"].ToString(),
            PasswordHash = r["PasswordHash"].ToString(),
            RolId = (int)r["RolId"]
        };
    }
}