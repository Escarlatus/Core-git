using System.Data.SqlClient;
using Asilo.Core.Entities.Financiero;
using Asilo.Core.Enums;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlMovimientoRepository : IMovimientoRepository
    {
        private readonly string _connectionString;

        public SqlMovimientoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Movimiento entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Movimiento (CuentaId, Monto, Fecha, Tipo, Detalles) VALUES (@C, @M, @F, @T, @D)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@C", entity.CuentaId);
                    cmd.Parameters.AddWithValue("@M", entity.Monto);
                    cmd.Parameters.AddWithValue("@F", entity.Fecha);
                    cmd.Parameters.AddWithValue("@T", (int)entity.Tipo);
                    cmd.Parameters.AddWithValue("@D", entity.Detalles ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Movimiento entity) { /* Implementar si necesario */ }
        public void Delete(int id) { /* Implementar si necesario */ }

        public Movimiento? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Movimiento WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read()) return MapReader(reader);
            }
            return null;
        }

        public IEnumerable<Movimiento> GetAll()
        {
            var lista = new List<Movimiento>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Movimiento", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read()) lista.Add(MapReader(reader));
            }
            return lista;
        }

        public IEnumerable<Movimiento> GetByCuenta(int cuentaId)
        {
            var lista = new List<Movimiento>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Movimiento WHERE CuentaId = @C", conn);
                cmd.Parameters.AddWithValue("@C", cuentaId);
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read()) lista.Add(MapReader(reader));
            }
            return lista;
        }

        private Movimiento MapReader(SqlDataReader reader)
        {
            return new Movimiento
            {
                Id = Convert.ToInt32(reader["Id"]),
                CuentaId = Convert.ToInt32(reader["CuentaId"]),
                Monto = Convert.ToDecimal(reader["Monto"]),
                Fecha = Convert.ToDateTime(reader["Fecha"]),
                Tipo = (TipoMovimiento)Convert.ToInt32(reader["Tipo"]),
                Detalles = reader["Detalles"].ToString()
            };
        }
    }
}