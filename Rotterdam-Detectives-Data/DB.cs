using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;

namespace RotterdamDetectives_Data
{
    internal class DB(string connectionString)
    {
        public string LastQuery { get; private set; } = "";
        private string lastError = "";
        public string LastError {
            get { return lastError; }
            private set { lastError = value; Debug.WriteLine($"DB ERROR: {value}"); }
        }

        SqlCommand ConstructCommand(SqlConnection db, string query, object @params)
        {
            LastQuery = query;
            SqlCommand cmd = new(query, db);
            foreach (var prop in @params.GetType().GetProperties())
            {
                cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(@params) ?? DBNull.Value);
            }
            return cmd;
        }

        public bool Execute(string query, object @params)
        {
            using SqlConnection db = new(connectionString);
            db.Open();
            SqlCommand cmd = ConstructCommand(db, query, @params);
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

        public Out? Field<Out>(string query, object @params)
            where Out: struct
        {
            using SqlConnection db = new(connectionString);
            db.Open();
            var cmd = ConstructCommand(db, query, @params);
            try
            {
                var res = cmd.ExecuteScalar();
                if (res is DBNull)
                    return null;
                if (res == null)
                    return null;
                return (Out)res;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public string? String(string query, object @params)
        {
            using SqlConnection db = new(connectionString);
            db.Open();
            var cmd = ConstructCommand(db, query, @params);
            try
            {
                var res = cmd.ExecuteScalar();
                if (res is DBNull)
                    return null;
                if (res == null)
                    return null;
                return (string)res;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return null;
            }
        }

        public Out? Row<Out>(string query, object @params, Func<DbDataRecord, Out> read)
        {
            using SqlConnection db = new(connectionString);
            db.Open();
            SqlCommand cmd = ConstructCommand(db, query, @params);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                foreach (DbDataRecord item in reader)
                {
                    var r = read(item);
                    return r;
                }
                return default;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return default;
            }
        }

        public IEnumerable<Out>? Rows<Out>(string query, object @params, Func<DbDataRecord, Out?> read)
        {
            using SqlConnection db = new(connectionString);
            db.Open();
            SqlCommand cmd = ConstructCommand(db, query, @params);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                List<Out> list = [];
                foreach (DbDataRecord item in reader)
                {
                    Out? obj = read(item);
                    if (obj != null)
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
    }
}
