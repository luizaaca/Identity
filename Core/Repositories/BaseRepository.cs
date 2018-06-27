using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.Repositories
{
    public class BaseRepository : IDisposable
    {
        protected SqlConnection connection;

        public BaseRepository()
        {
            string connectionString = @"Data Source=NOT-MC1SPO-A021\SQLEXPRESS;Initial Catalog=Identity;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}