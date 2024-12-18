using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AKSS_Management
{
    public class Helper_SP
    {

        private readonly string _connectionString;

        public Helper_SP()
        {
            // Retrieve connection string from Web.config
            _connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE).
        /// </summary>
        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a SQL query and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
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
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a scalar SQL query and returns the result as an object.
        /// </summary>
        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
        /// Executes a stored procedure and returns the result as a DataSet.
        /// </summary>
        public DataSet ExecuteStoredProcedureDataSet(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;
                    }
                }
            }
        }


        /// <summary>
        /// Executes a stored procedure and returns the number of rows affected.
        /// </summary>
        public int ExecuteStoredProcedureNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an object.
        /// </summary>
        public object ExecuteStoredProcedureScalar(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a DataSet.
        /// </summary>
        public DataSet ExecuteDataSet(string query, params SqlParameter[] parameters)
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
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a table contains any rows based on a query.
        /// </summary>
        public bool HasRows(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a bulk copy operation to insert a DataTable into a database table.
        /// </summary>
        public void BulkInsert(string destinationTableName, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = destinationTableName;
                    connection.Open();
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public SqlTransaction BeginTransaction()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Rolls back a transaction.
        /// </summary>
        public void RollbackTransaction(SqlTransaction transaction)
        {
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Commits a transaction.
        /// </summary>
        public void CommitTransaction(SqlTransaction transaction)
        {
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Commit();
            }
        }

        /// <summary>
        /// Converts a DataTable to a list of dynamic objects.
        /// </summary>
        public List<dynamic> ConvertToDynamicList(DataTable table)
        {
            var result = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                foreach (DataColumn column in table.Columns)
                {
                    expando[column.ColumnName] = row[column];
                }
                result.Add(expando);
            }
            return result;
        }

        /// <summary>
        /// Executes a command and retrieves multiple result sets.
        /// </summary>
        public List<DataTable> ExecuteMultipleResultSets(string query, params SqlParameter[] parameters)
        {
            var tables = new List<DataTable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        do
                        {
                            var table = new DataTable();
                            table.Load(reader);
                            tables.Add(table);
                        } while (!reader.IsClosed);
                    }
                }
            }

            return tables;
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

    }
}