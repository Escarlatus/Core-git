using System.Data.SqlClient;
using Asilo.Core.Entities.Financiero;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlCuentaRepository : ICuentaRepository
    {
        private readonly string _connectionString;

        public SqlCuentaRepository(string connectionString, string unused = "")
        {
            _connectionString = connectionString;
        }

        // POST (Crear)
        public void Add(Cuenta entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Cuenta (ResidenteId, Balance) VALUES (@R, @B)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@R", entity.ResidenteId);
                    cmd.Parameters.AddWithValue("@B", entity.Balance);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // PUT (Actualizar) - ESTE ES EL QUE NECESITAS PARA EL EDITAR
        public void Update(Cuenta entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Actualiza el Balance basado en el ID de la cuenta
                var query = "UPDATE Cuenta SET Balance=@B WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@B", entity.Balance);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE (Borrar) - ESTE ES EL QUE NECESITAS PARA EL BORRAR
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // CUIDADO: En SQL Server, si hay movimientos ligados a esta cuenta,
                // esto podría dar error de "Foreign Key" a menos que borres los movimientos primero.
                // Para la U, a veces basta con esto.
                var query = "DELETE FROM Cuenta WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Cuenta? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Cuenta WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return Map(reader);
                }
            }
            return null;
        }

        public Cuenta? GetByResidente(int residenteId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Cuenta WHERE ResidenteId=@R", conn);
                cmd.Parameters.AddWithValue("@R", residenteId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return Map(reader);
                }
            }
            return null;
        }

        public IEnumerable<Cuenta> GetAll()
        {
            var l = new List<Cuenta>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var r = new SqlCommand("SELECT * FROM Cuenta", conn).ExecuteReader())
                    while (r.Read()) l.Add(Map(r));
            }
            return l;
        }

        private Cuenta Map(SqlDataReader r) => new Cuenta
        {
            Id = (int)r["Id"],
            ResidenteId = (int)r["ResidenteId"],
            Balance = (decimal)r["Balance"]
        };
    }
}