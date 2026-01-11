using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Enums;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlIngresoRepository : IIngresoRepository
    {
        private readonly string _connectionString;
        public SqlIngresoRepository(string connectionString) { _connectionString = connectionString; }

        public void Add(Ingreso entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Ingreso (ResidenteId, FechaIngreso, DiagnosticoEntrada, HabitacionId, Estado) VALUES (@R, @F, @D, @H, @E)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    ConfigParams(cmd, entity);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Update(Ingreso entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Actualiza fecha de alta y libera habitación si es necesario
                var query = "UPDATE Ingreso SET FechaAlta=@Alta, Estado=@E, HabitacionId=@H WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Alta", entity.FechaAlta ?? (object)DBNull.Value);
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
                new SqlCommand("DELETE FROM Ingreso WHERE Id=" + id, conn).ExecuteNonQuery();
            }
        }

        public Ingreso? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Ingreso WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return MapReader(reader);
                }
            }
            return null;
        }

        public Ingreso? GetIngresoActivo(int residenteId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Asegúrate de que el SQL busque el Estado 1 (Activo)
                // O mejor aún, pasamos el valor del Enum como parámetro para ser más limpios
                var cmd = new SqlCommand("SELECT * FROM Ingreso WHERE ResidenteId=@R AND Estado=@E", conn);

                cmd.Parameters.AddWithValue("@R", residenteId);
                cmd.Parameters.AddWithValue("@E", (int)EstadoIngreso.Activo); // <--- ASÍ NUNCA FALLA

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return MapReader(reader);
                }
            }
            return null;
        }

        public IEnumerable<Ingreso> GetAll()
        {
            var l = new List<Ingreso>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var r = new SqlCommand("SELECT * FROM Ingreso", conn).ExecuteReader())
                    while (r.Read()) l.Add(MapReader(r));
            }
            return l;
        }

        private void ConfigParams(SqlCommand cmd, Ingreso i)
        {
            if (!cmd.Parameters.Contains("@R")) cmd.Parameters.AddWithValue("@R", i.ResidenteId);
            if (!cmd.Parameters.Contains("@F")) cmd.Parameters.AddWithValue("@F", i.FechaIngreso);
            if (!cmd.Parameters.Contains("@D")) cmd.Parameters.AddWithValue("@D", i.DiagnosticoEntrada);
            if (!cmd.Parameters.Contains("@H")) cmd.Parameters.AddWithValue("@H", i.HabitacionId ?? (object)DBNull.Value);
            if (!cmd.Parameters.Contains("@E")) cmd.Parameters.AddWithValue("@E", (int)i.Estado);
        }

        private Ingreso MapReader(SqlDataReader reader)
        {
            return new Ingreso
            {
                Id = Convert.ToInt32(reader["Id"]),
                ResidenteId = Convert.ToInt32(reader["ResidenteId"]),
                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                FechaAlta = reader["FechaAlta"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaAlta"]),
                DiagnosticoEntrada = reader["DiagnosticoEntrada"].ToString(),
                HabitacionId = reader["HabitacionId"] == DBNull.Value ? null : Convert.ToInt32(reader["HabitacionId"]),
                Estado = (EstadoIngreso)Convert.ToInt32(reader["Estado"])
            };
        }
    }
}