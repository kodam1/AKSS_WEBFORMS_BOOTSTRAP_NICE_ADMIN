﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text.Json;

namespace AKSS_Management
{
    public class Helper_QueryString
    {
        private readonly string _connectionString;

        public Helper_QueryString(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Async Support for Common Methods
        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();
                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<DataSet> ExecuteDataSetAsync(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                DataSet dataSet = new DataSet();
                await connection.OpenAsync();
                await Task.Run(() => adapter.Fill(dataSet));
                return dataSet;
            }
        }

        // Parameter Utility Methods
        public SqlParameter CreateParameter(string name, object value, SqlDbType type)
        {
            return new SqlParameter(name, value) { SqlDbType = type };
        }

        // Connection State Validation
        public async Task ValidateConnectionStateAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("Connection failed to open.");
            }
        }

        // Query Execution with Timeout
        public async Task<int> ExecuteNonQueryWithTimeoutAsync(string query, int timeout, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                command.CommandTimeout = timeout;
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        // Export Data to CSV
        //public async Task ExportToCsvAsync(string query, string filePath, SqlParameter[] parameters = null)
        //{
        //    DataSet dataSet = await ExecuteDataSetAsync(query, parameters);
        //    DataTable table = dataSet.Tables[0];

        //    StringBuilder csvData = new StringBuilder();

        //    // Add column headers
        //    foreach (DataColumn column in table.Columns)
        //    {
        //        csvData.Append(column.ColumnName + ",");
        //    }
        //    csvData.AppendLine();

        //    // Add rows
        //    foreach (DataRow row in table.Rows)
        //    {
        //        foreach (DataColumn column in table.Columns)
        //        {
        //            csvData.Append(row[column].ToString() + ",");
        //        }
        //        csvData.AppendLine();
        //    }

        //    await File.WriteAllTextAsync(filePath, csvData.ToString());
        //}

        // Export Data to JSON
        //public async Task ExportToJsonAsync(string query, string filePath, SqlParameter[] parameters = null)
        //{
        //    DataSet dataSet = await ExecuteDataSetAsync(query, parameters);
        //    string jsonData = JsonSerializer.Serialize(dataSet.Tables[0]);

        //    await File.WriteAllText(filePath, jsonData);
        //}

        // Import Data from CSV
        //public async Task ImportFromCsvAsync(string tableName, string filePath)
        //{
        //    string[] csvData = await File.ReadAllLinesAsync(filePath);
        //    string[] columnNames = csvData[0].Split(',');

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        foreach (string row in csvData)
        //        {
        //            string[] values = row.Split(',');
        //            List<SqlParameter> parameters = new List<SqlParameter>();

        //            for (int i = 0; i < columnNames.Length; i++)
        //            {
        //                parameters.Add(new SqlParameter("@" + columnNames[i], values[i]));
        //            }

        //            string query = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameters)})";
        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                command.Parameters.AddRange(parameters.ToArray());
        //                await command.ExecuteNonQueryAsync();
        //            }
        //        }
        //    }
        //}

        // Column Metadata Retrieval
        public async Task<DataTable> GetColumnMetadataAsync(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName} WHERE 1 = 0", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable schemaTable = new DataTable();
                await connection.OpenAsync();
                await Task.Run(() => adapter.FillSchema(schemaTable, SchemaType.Source));
                return schemaTable;
            }
        }
    }

}