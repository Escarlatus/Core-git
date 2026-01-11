using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlHabitacionRepository : IHabitacionRepository
    {
        private readonly string _connectionString;
        public SqlHabitacionRepository(string connectionString) { _connectionString = connectionString; }

        public IEnumerable<Habitacion> GetDisponibles()
        {
            var lista = new List<Habitacion>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Esta query es vital: Cuenta cuántos ingresos activos hay por habitación y los compara con la capacidad
                var query = @"
                    SELECT h.* FROM Habitacion h
                    LEFT JOIN Ingreso i ON h.Id = i.HabitacionId AND i.Estado = 1
                    GROUP BY h.Id, h.Numero, h.Capacidad
                    HAVING COUNT(i.Id) < h.Capacidad";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) lista.Add(MapReader(reader));
                }
            }
            return lista;
        }

        public void Add(Habitacion entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Habitacion (Numero, Capacidad) VALUES (@Numero, @Capacidad)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Numero", entity.Numero);
                    cmd.Parameters.AddWithValue("@Capacidad", entity.Capacidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Habitacion entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "UPDATE Habitacion SET Numero=@Numero, Capacidad=@Capacidad WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Numero", entity.Numero);
                    cmd.Parameters.AddWithValue("@Capacidad", entity.Capacidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                new SqlCommand("DELETE FROM Habitacion WHERE Id = " + id, conn).ExecuteNonQuery();
            }
        }

        public Habitacion? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Habitacion WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapReader(reader);
                    }
                }
            }
            return null;
        }

        public IEnumerable<Habitacion> GetAll()
        {
            var lista = new List<Habitacion>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Habitacion", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) lista.Add(MapReader(reader));
                }
            }
            return lista;
        }

        private Habitacion MapReader(SqlDataReader reader)
        {
            return new Habitacion
            {
                Id = Convert.ToInt32(reader["Id"]),
                Numero = reader["Numero"].ToString(),
                Capacidad = Convert.ToInt32(reader["Capacidad"])
            };
        }
    }
}