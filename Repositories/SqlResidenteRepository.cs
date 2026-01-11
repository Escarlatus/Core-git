using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlResidenteRepository : IResidenteRepository
    {
        private readonly string _connectionString;

        // El segundo parámetro es opcional para compatibilidad, pero no se usa en el Core
        public SqlResidenteRepository(string connectionString, string unused = "")
        {
            _connectionString = connectionString;
        }

        public void Add(Residente entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Residente (Nombre, Documento, FechaNacimiento, EstadoSalud, HabitacionId) VALUES (@Nombre, @Documento, @FechaNacimiento, @EstadoSalud, @HabitacionId)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    ConfigParams(cmd, entity);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Residente entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "UPDATE Residente SET Nombre=@Nombre, Documento=@Documento, FechaNacimiento=@FechaNacimiento, EstadoSalud=@EstadoSalud, HabitacionId=@HabitacionId WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    ConfigParams(cmd, entity);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                new SqlCommand("DELETE FROM Residente WHERE Id = " + id, conn).ExecuteNonQuery();
            }
        }

        public Residente? GetById(int id)
        {
            return GetByQuery("SELECT * FROM Residente WHERE Id = @Id", new SqlParameter("@Id", id));
        }

        public Residente? GetByDocumento(string documento)
        {
            return GetByQuery("SELECT * FROM Residente WHERE Documento = @Doc", new SqlParameter("@Doc", documento));
        }

        public IEnumerable<Residente> GetAll()
        {
            var lista = new List<Residente>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Residente", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) lista.Add(MapReader(reader));
                }
            }
            return lista;
        }

        private void ConfigParams(SqlCommand cmd, Residente r)
        {
            cmd.Parameters.AddWithValue("@Nombre", r.Nombre);
            cmd.Parameters.AddWithValue("@Documento", r.Documento);
            cmd.Parameters.AddWithValue("@FechaNacimiento", r.FechaNacimiento);
            cmd.Parameters.AddWithValue("@EstadoSalud", r.EstadoSalud ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@HabitacionId", r.HabitacionId ?? (object)DBNull.Value);
        }

        private Residente? GetByQuery(string query, SqlParameter param)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(param);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapReader(reader);
                    }
                }
            }
            return null;
        }

        private Residente MapReader(SqlDataReader reader)
        {
            return new Residente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString(),
                Documento = reader["Documento"].ToString(),
                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                EstadoSalud = reader["EstadoSalud"].ToString(),
                HabitacionId = reader["HabitacionId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["HabitacionId"])
            };
        }
    }
}