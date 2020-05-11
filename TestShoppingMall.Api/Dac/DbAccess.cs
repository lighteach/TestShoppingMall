using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using TestShoppingMall.Api.Models;

namespace TestShoppingMall.Api.Dac
{
    public class DbAccess
    {
        #region Variables
        private static DbAccess _dbAcc = null;
        private string _conn = null;
        private SqlConnection _sqlConn = null;
        private SqlCommand _sqlCmd = null;
        private SqlDataAdapter _sqlDa = null; 
        #endregion

        #region Singleton Instance Create Define Area
        private DbAccess(string conn)
        {
            _conn = conn;
            _sqlConn = new SqlConnection(conn);
        }

        public static DbAccess CreateInstance(string conn)
        {
            if (_dbAcc != null)
                return _dbAcc;
            else
            {
                _dbAcc = new DbAccess(conn);
                return _dbAcc;
            }
        } 
        #endregion

        #region GetSpToDataTable
        public DataTable GetSpToDataTable(string spName, Dictionary<string, string> dictParams)
        {
            DataTable dt = new DataTable();
            DataSet ds = GetSpToDataSet(spName, dictParams);
            
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }
        #endregion

        #region GetSpToDataSet
        public DataSet GetSpToDataSet(string spName, Dictionary<string, string> dictParams)
        {
            DataSet ds = new DataSet();
            using (_sqlCmd = new SqlCommand(spName, _sqlConn))
            {
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> kv in dictParams)
                    _sqlCmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;

                using (_sqlDa = new SqlDataAdapter(_sqlCmd))
                    _sqlDa.Fill(ds);
            }
            
            return ds;
        }
        #endregion

        #region ExecProcedure
        public SpReturnModel ExecProcedure(string spName, Dictionary<string, string> dictParams)
        {
            SpReturnModel rm = new SpReturnModel("Fail", "");
            DataSet ds = new DataSet();
            using (_sqlCmd = new SqlCommand(spName, _sqlConn))
            {
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> kv in dictParams)
                    _sqlCmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;

                using (_sqlDa = new SqlDataAdapter(_sqlCmd))
                    _sqlDa.Fill(ds);
            }

            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                rm = new SpReturnModel(dr["ReturnCode"].ToString(), dr["ReturnMessage"].ToString());
            }
            return rm;
        }
        #endregion

        #region ExecProcedureToList
        public IEnumerable<T> ExecProcedureToList<T>(string spName, Dictionary<string, string> dictParams) where T : new()
        {
            List<T> listRtn = null;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            using (_sqlCmd = new SqlCommand(spName, _sqlConn))
            {
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> kv in dictParams)
                    _sqlCmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;

                using (_sqlDa = new SqlDataAdapter(_sqlCmd))
                    _sqlDa.Fill(ds);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                listRtn = new List<T>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    T tmpObj = ConvertToEntity<T>(dr);
                    listRtn.Add(tmpObj);
                }
            }

            return listRtn;
        }
        #endregion

        #region ExecProcedureToSingle
        public T ExecProcedureToSingle<T>(string spName, Dictionary<string, string> dictParams) where T : new()
        {
            T rtn = default(T);

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            using (_sqlCmd = new SqlCommand(spName, _sqlConn))
            {
                _sqlCmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> kv in dictParams)
                    _sqlCmd.Parameters.Add(kv.Key, SqlDbType.VarChar).Value = kv.Value;

                using (_sqlDa = new SqlDataAdapter(_sqlCmd))
                    _sqlDa.Fill(ds);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                rtn = ConvertToEntity<T>(dr);
            }

            return rtn;
        } 
        #endregion

        

        #region ConvertToEntity
        public T ConvertToEntity<T>(DataRow tableRow) where T : new()
        {
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        if (val is System.DBNull)
                            val = null;
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    pInfo.SetValue(returnObject, val, null);
                }
            }

            return returnObject;
        } 
        #endregion
    }
}