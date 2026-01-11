using System.Data.SqlClient;
using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlAuditRepository : IAuditRepository
    {
        private readonly string _connectionString;

        public SqlAuditRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Log(AuditRecord entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Tabla corregida a singular
                string query = @"INSERT INTO AuditRecord (Timestamp, Usuario, Accion, Detalles) 
                                 VALUES (@Timestamp, @Usuario, @Accion, @Detalles)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Timestamp", entity.Timestamp);
                    cmd.Parameters.AddWithValue("@Usuario", entity.Usuario);
                    cmd.Parameters.AddWithValue("@Accion", entity.Accion);
                    cmd.Parameters.AddWithValue("@Detalles", entity.Detalles ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Métodos de compatibilidad
        public void Add(AuditRecord entity) => Log(entity);
        public void Delete(int id) => throw new NotImplementedException();
        public void Update(AuditRecord entity) => throw new NotImplementedException();
        public IEnumerable<AuditRecord> GetAll() => throw new NotImplementedException();
        public AuditRecord GetById(int id) => throw new NotImplementedException();
    }
}