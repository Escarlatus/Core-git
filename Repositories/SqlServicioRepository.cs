using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlServicioRepository : IServicioRepository
    {
        private readonly string _connectionString;
        public SqlServicioRepository(string connectionString) { _connectionString = connectionString; }

        public void Add(Servicio entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Servicio (Nombre, Precio) VALUES (@Nombre, @Precio)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", entity.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Servicio entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "UPDATE Servicio SET Nombre=@Nombre, Precio=@Precio WHERE Id=@Id";
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
                new SqlCommand("DELETE FROM Servicio WHERE Id = " + id, conn).ExecuteNonQuery();
            }
        }

        public Servicio? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Servicio WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read()) return Map(reader);
            }
            return null;
        }

        public IEnumerable<Servicio> GetAll()
        {
            var l = new List<Servicio>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var reader = new SqlCommand("SELECT * FROM Servicio", conn).ExecuteReader())
                    while (reader.Read()) l.Add(Map(reader));
            }
            return l;
        }

        private Servicio Map(SqlDataReader r) => new Servicio { Id = (int)r["Id"], Nombre = r["Nombre"].ToString(), Precio = (decimal)r["Precio"] };
    }
}