using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlAnalisisRepository : IAnalisisRepository
    {
        private readonly string _connectionString;
        public SqlAnalisisRepository(string connectionString) { _connectionString = connectionString; }

        public void Add(Analisis entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Analisis (Nombre, Precio) VALUES (@Nombre, @Precio)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", entity.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Analisis entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "UPDATE Analisis SET Nombre=@Nombre, Precio=@Precio WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", entity.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                new SqlCommand("DELETE FROM Analisis WHERE Id = " + id, conn).ExecuteNonQuery();
            }
        }

        public Analisis? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Analisis WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var r = cmd.ExecuteReader()) if (r.Read()) return Map(r);
            }
            return null;
        }

        public IEnumerable<Analisis> GetAll()
        {
            var l = new List<Analisis>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var r = new SqlCommand("SELECT * FROM Analisis", conn).ExecuteReader())
                    while (r.Read()) l.Add(Map(r));
            }
            return l;
        }

        private Analisis Map(SqlDataReader r) => new Analisis { Id = (int)r["Id"], Nombre = r["Nombre"].ToString(), Precio = (decimal)r["Precio"] };
    }
}