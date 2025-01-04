using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessDAL
{
    public class DataDAL
    {
        public DataSet GetDataSet()
        {
            var ds = new DataSet("DataBase");
            var queries = new (string query, string tableName)[]
            {
                ("SELECT * FROM Users", "Users"),
                ("SELECT * FROM Products", "Products"),
                ("SELECT * FROM Cart", "Cart"),
                ("SELECT * FROM Orders", "Orders")
            };
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MiniProjectDB"].ConnectionString))
                {
                    foreach (var (query, tableName) in queries)
                    {
                        using (var da = new SqlDataAdapter(query, conn))
                        {
                            da.Fill(ds, tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return ds;
        }
        public void UpdateUser(DataSet ds)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MiniProjectDB"].ConnectionString))
            {
                using (var da = new SqlDataAdapter("SELECT * FROM Users", conn))
                {
                    var commandBuilder = new SqlCommandBuilder(da);
                    da.Update(ds, "Users");
                }
            }
        }
        public void UpdateDB(DataSet ds)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MiniProjectDB"].ConnectionString))
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        using (var da = new SqlDataAdapter($"SELECT * FROM {table.TableName}", conn))
                        {
                            var commandBuilder = new SqlCommandBuilder(da);
                            da.Update(ds, table.TableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the database: {ex.Message}");
            }
        }
    }
}
