using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper()
    {
        // Retrieve connection string from Web.config
        _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    /// <summary>
    /// Executes a non-query SQL command (INSERT, UPDATE, DELETE) asynchronously.
    /// </summary>
    /// 
//    DatabaseHelper dbHelper = new DatabaseHelper();
//    string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
//    var parameters = new[]
//    {
//    dbHelper.CreateParameter("@Name", "John Doe", SqlDbType.NVarChar),
//    dbHelper.CreateParameter("@Age", 30, SqlDbType.Int)
//};
//    int rowsAffected = await dbHelper.ExecuteNonQueryAsync(insertQuery, parameters);
//    Console.WriteLine($"Rows affected: {rowsAffected}");


    public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }
    }

    /// <summary>
    /// Executes a scalar SQL query asynchronously and returns the result as an object.
    /// string scalarQuery = "SELECT COUNT(*) FROM Users";
    //object result = await dbHelper.ExecuteScalarAsync(scalarQuery);
    //Console.WriteLine($"Total users: {result}");

    /// </summary>
    public async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();
                return await command.ExecuteScalarAsync();
            }
        }
    }

    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as a DataSet.
    /// string selectQuery = "SELECT * FROM Users";
    //DataSet usersDataSet = await dbHelper.ExecuteDataSetAsync(selectQuery);
    //Console.WriteLine($"Data retrieved: {usersDataSet.Tables[0].Rows.Count} rows");

    /// </summary>
    public async Task<DataSet> ExecuteDataSetAsync(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet dataSet = new DataSet();
                    await Task.Run(() => adapter.Fill(dataSet));
                    return dataSet;
                }
            }
        }
    }

    /// <summary>
    /// Creates a new SqlParameter with the specified name, value, and type.
    /// SqlParameter param = dbHelper.CreateParameter("@Id", 1, SqlDbType.Int);
    //Console.WriteLine($"Parameter created: {param.ParameterName}, Value: {param.Value}");

    /// </summary>
    public SqlParameter CreateParameter(string name, object value, SqlDbType dbType, ParameterDirection direction = ParameterDirection.Input)
    {
        return new SqlParameter
        {
            ParameterName = name,
            Value = value ?? DBNull.Value,
            SqlDbType = dbType,
            Direction = direction
        };
    }

    /// <summary>
    /// Validates the connection state and ensures it is open.
    /// </summary>
    public void ValidateConnection(SqlConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    /// <summary>
    /// Executes a query with a custom command timeout.
    /// DatabaseHelper dbHelper = new DatabaseHelper();
    //string timeoutQuery = "SELECT * FROM Orders";
    //int timeout = 60; // 60 seconds
    //DataTable orders = dbHelper.ExecuteDataTableWithTimeout(timeoutQuery, timeout);
    //Console.WriteLine($"Orders retrieved: {orders.Rows.Count}");


    /// </summary>
    public DataTable ExecuteDataTableWithTimeout(string query, int timeout, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = timeout;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }

    /// <summary>
    /// Exports a DataTable to a CSV file.
    /// string selectUsersQuery = "SELECT * FROM Users";
//    DataSet usersDataSet = await dbHelper.ExecuteDataSetAsync(selectUsersQuery);
//    dbHelper.ExportToCsv(usersDataSet.Tables[0], "Users.csv");
//Console.WriteLine("Users exported to CSV.");

    /// </summary>
    public void ExportToCsv(DataTable table, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write column headers
            for (int i = 0; i < table.Columns.Count; i++)
            {
                writer.Write(table.Columns[i].ColumnName);
                if (i < table.Columns.Count - 1)
                {
                    writer.Write(",");
                }
            }
            writer.WriteLine();

            // Write rows
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    writer.Write(row[i].ToString());
                    if (i < table.Columns.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
        }
    }

    /// <summary>
    /// Imports data from a CSV file into a DataTable.
    /// DataTable importedTable = dbHelper.ImportFromCsv("Users.csv");
    //Console.WriteLine($"Imported rows: {importedTable.Rows.Count}");

    /// </summary>
    public DataTable ImportFromCsv(string filePath)
    {
        DataTable table = new DataTable();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string[] headers = reader.ReadLine().Split(',');
            foreach (string header in headers)
            {
                table.Columns.Add(header);
            }

            while (!reader.EndOfStream)
            {
                string[] rows = reader.ReadLine().Split(',');
                table.Rows.Add(rows);
            }
        }

        return table;
    }

    /// <summary>
    /// Exports a DataTable to a JSON file.
    /// 
    /// string selectProductsQuery = "SELECT * FROM Products";
//    DataSet productsDataSet = await dbHelper.ExecuteDataSetAsync(selectProductsQuery);
//    dbHelper.ExportToJson(productsDataSet.Tables[0], "Products.json");
//Console.WriteLine("Products exported to JSON.");

    /// </summary>
    public void ExportToJson(DataTable table, string filePath)
    {
        string json = JsonConvert.SerializeObject(table);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Fetches column metadata for a given table.
    /// DataTable metadata = dbHelper.GetColumnMetadata("Users");
//foreach (DataRow row in metadata.Rows)
//{
//    Console.WriteLine($"Column: {row["COLUMN_NAME"]}, Type: {row["DATA_TYPE"]}, Nullable: {row["IS_NULLABLE"]}");
//}

/// </summary>
public DataTable GetColumnMetadata(string tableName)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable metadataTable = new DataTable();
                    adapter.Fill(metadataTable);
                    return metadataTable;
                }
            }
        }
    }

}
