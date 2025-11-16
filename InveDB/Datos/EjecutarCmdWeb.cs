using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace InveDB.Datos
{
    public class EjecutarCmdWeb
    {
        private readonly string _connectionString;

        public EjecutarCmdWeb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int EjecutarNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable EjecutarConsulta(string sql)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
