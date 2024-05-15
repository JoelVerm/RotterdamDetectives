using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;

namespace RotterdamDetectives_Data
{
    internal class DB(string connectionString)
    {
        readonly SqlConnection db = new(connectionString);

        public string LastQuery { get; private set; } = "";
        public string LastError { get; private set; } = "";

        SqlCommand ConstructCommand(string query, object @params)
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
            SqlCommand cmd = ConstructCommand(query, @params);
            try
            {
                db.Open();
                cmd.ExecuteNonQuery();
                db.Close();
                return true;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return false;
            }
            finally
            {
                db.Close();
            }
        }

        public Out? Field<Out>(string query, object @params)
            where Out: struct
        {
            var cmd = ConstructCommand(query, @params);
            try
            {
                db.Open();
                var res = cmd.ExecuteScalar();
                db.Close();
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
            finally
            {
                db.Close();
            }
        }

        public string? String(string query, object @params)
        {
            var cmd = ConstructCommand(query, @params);
            try
            {
                db.Open();
                var res = cmd.ExecuteScalar();
                db.Close();
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
            finally
            {
                db.Close();
            }
        }

        public Out? Row<Out>(string query, object @params, Func<DbDataRecord, Out> read)
        {
            SqlCommand cmd = ConstructCommand(query, @params);
            try
            {
                db.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                foreach (DbDataRecord item in reader)
                {
                    var r = read(item);
                    db.Close();
                    return r;
                }
                return default;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return default;
            }
            finally
            {
                db.Close();
            }
        }

        public IEnumerable<Out>? Rows<Out>(string query, object @params, Func<DbDataRecord, Out?> read)
        {
            SqlCommand cmd = ConstructCommand(query, @params);
            try
            {
                db.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                List<Out> list = [];
                foreach (DbDataRecord item in reader)
                {
                    Debug.WriteLine(item);
                    Out? obj = read(item);
                    if (obj != null)
                        list.Add(obj);
                }
                db.Close();
                return list;
            }
            catch (SqlException e)
            {
                LastError = e.Message;
                return null;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
