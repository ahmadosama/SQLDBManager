using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SQLDBManager

{
   public class QueryExecution
    {
       private string Connectionstring;
       public QueryExecution()
       { }
       
       public QueryExecution(string server,string user,string password,string database,string authenticationtype)
       {
       //    Connectionstring = _connectionstring;
           try
           {
               if (authenticationtype == "Windows")
               {
                   Connectionstring = "Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=True;";
               }
               if (authenticationtype == "SQL")
               {
                   Connectionstring = "Initial Catalog=" + database + ";Data Source=" + server + ";user id=" + user + ";password=" + password;
               }
           }catch(Exception)
           {
               throw;
           }
       }

       public QueryExecution(string _connectionstring)
       {
           Connectionstring = _connectionstring;
       }
       /// <summary>
       /// overrides sqlcommand.executenonquery
       /// </summary>
       /// <param name="Query"></param>
       /// <param name="Connectionstring"></param>
       public void fn_ExecuteQuery(string Query)
       {
           try
           {
               using (SqlConnection _Sqlcon = new SqlConnection(Connectionstring))
               {
                   _Sqlcon.Open();
                   SqlCommand SqlCmd = new SqlCommand(Query, _Sqlcon);
                   SqlCmd.ExecuteNonQuery();
               }
           }
           catch (Exception)
           {
               throw;
           }
       }
        
       /// <summary>
       /// returns query results in a datatable
       /// </summary>
       /// <param name="Query"></param>
       /// <param name="Connectionstring"></param>
       /// <returns></returns>
       public DataTable fn_ExecuteTable(string Query)
       {
         try
           {
               using (SqlConnection _Sqlcon = new SqlConnection(Connectionstring))
               {
                   _Sqlcon.Open();
                   SqlCommand SqlCmd = new SqlCommand(Query, _Sqlcon);
                   SqlDataReader dr = SqlCmd.ExecuteReader();

                   var dt = new DataTable();
                   dt.Load(dr);

                   return dt;
               }
           }
           catch (Exception)
           {
               throw;
           }
       }

       /// <summary>
       /// returns query result as a string
       /// </summary>
       /// <param name="Query"></param>
       /// <param name="Connectionstring"></param>
       /// <returns></returns>
       public string fn_ExecuteScalor(string Query)
       {
           try
           {
               using (SqlConnection _Sqlcon = new SqlConnection(Connectionstring))
               {
                   _Sqlcon.Open();
                   SqlCommand SqlCmd = new SqlCommand(Query, _Sqlcon);
                   string output = ConvertFromDBVal<string>(SqlCmd.ExecuteScalar());
                   return output;
                   //return ConvertFromDBVal<string>(output);
                  // return (output == null) ? string.Empty : output.ToString();
               }
           }
           catch (Exception)
           {
               throw;
           }
       }
      
       /// <summary>
       /// bulk insert from source to destination
       /// </summary>
       /// <param name="srctable"></param>
       /// <param name="desttable"></param>
       public void fn_sqlbulkinsert(DataTable srctable,string desttable)
       {
           try
           {
               using (SqlConnection _Sqlcon = new SqlConnection(Connectionstring))
               {
                   _Sqlcon.Open();
                   using (SqlBulkCopy sbc = new SqlBulkCopy(_Sqlcon))
                   {
                       sbc.DestinationTableName = desttable;
                       foreach (var column in srctable.Columns)
                           sbc.ColumnMappings.Add(column.ToString(), column.ToString());
                       sbc.WriteToServer(srctable);
                   }
               }
           }catch(Exception)
           {
               throw;
           }
       }

       public static T ConvertFromDBVal<T>(object obj)
       {
           if (obj == null || obj == DBNull.Value)
           {
               return default(T); // returns the default value for the type
           }
           else
           {
               return (T)obj;
           }
       }

    }
}
