using System.Data.SqlClient;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Repositories
{
    public class SqlProcedimientoRepository : IProcedimientoRepository
    {
        private readonly string _connectionString;
        public SqlProcedimientoRepository(string connectionString) { _connectionString = connectionString; }

        public void Add(Procedimiento entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "INSERT INTO Procedimiento (Nombre, Descripcion, Precio, Activo) VALUES (@Nombre, @Desc, @Precio, @Activo)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Desc", entity.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", entity.Precio);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Procedimiento entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "UPDATE Procedimiento SET Nombre=@Nombre, Descripcion=@Desc, Precio=@Precio, Activo=@Activo WHERE Id=@Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Nombre", entity.Nombre);
                    cmd.Parameters.AddWithValue("@Desc", entity.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", entity.Precio);
                    cmd.Parameters.AddWithValue("@Activo", entity.Activo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                new SqlCommand("DELETE FROM Procedimiento WHERE Id = " + id, conn).ExecuteNonQuery();
            }
        }

        public Procedimiento? GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Procedimiento WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var r = cmd.ExecuteReader()) if (r.Read()) return Map(r);
            }
            return null;
        }

        public IEnumerable<Procedimiento> GetAll()
        {
            var l = new List<Procedimiento>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var r = new SqlCommand("SELECT * FROM Procedimiento", conn).ExecuteReader())
                    while (r.Read()) l.Add(Map(r));
            }
            return l;
        }

        public IEnumerable<Procedimiento> BuscarPorNombre(string nombre)
        {
            var l = new List<Procedimiento>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Procedimiento WHERE Nombre LIKE @Nombre", conn);
                cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) l.Add(Map(r));
            }
            return l;
        }

        private Procedimiento Map(SqlDataReader r) => new Procedimiento
        {
            Id = (int)r["Id"],
            Nombre = r["Nombre"].ToString(),
            Descripcion = r["Descripcion"].ToString(),
            Precio = (decimal)r["Precio"],
            Activo = (bool)r["Activo"]
        };
    }
}