using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;

namespace RotterdamDetectives_Data
{
    internal class DB
    {
        SqlConnection db;

        public string LastQuery { get; private set; } = "";
        public string LastError { get; private set; } = "";
            
        public DB(string connectionString)
        {
            db = new(connectionString);
            db.Open();
        }

        ~DB()
        {
            db.Close();
        }

        SqlCommand ConstructCommand<In>(string query, In @params)
            where In : class
        {
            LastQuery = query;
            SqlCommand cmd = new(query, db);
            foreach (var prop in @params.GetType().GetProperties())
            {
                cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(@params) ?? DBNull.Value);
            }
            return cmd;
        }

        public bool Execute<In>(string query, In @params)
            where In : class
        {
            SqlCommand cmd = ConstructCommand(query, @params);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return false;
            }
        }

        public Out? Field<In, Out>(string query, In @params)
            where In : class
            where Out: struct
        {
            var cmd = ConstructCommand(query, @params);
            try
            {
                return (Out)cmd.ExecuteScalar();
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return default;
            }
        }

        public IEnumerable<Out>? Rows<In, Out>(string query, In @params)
            where In : class
            where Out: class, new()
        {
            SqlCommand cmd = ConstructCommand(query, @params);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                List<Out> list = new();
                foreach (DbDataRecord item in reader)
                {
                    Debug.WriteLine(item);
                    Out obj = new();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        if (item[prop.Name] != DBNull.Value)
                            prop.SetValue(
                                obj,
                                Convert.ChangeType(
                                    item[prop.Name], prop.PropertyType));
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public IEnumerable<T>? Rows<T>(string query, T @params)
            where T : class, new()
            => Rows<T, T>(query, @params);
    }
}
