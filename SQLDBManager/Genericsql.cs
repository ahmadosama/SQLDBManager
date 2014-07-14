using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace SQLDBManager
{
    public class Genericsql
    {
        public Genericsql() { }
        public DataTable databaselist(QueryExecution qe)
        {
            try
            {
                string _querytolistdatabases = "Select name,CASE is_cdc_enabled WHEN 0 Then 'False' WHEN 1 THEN 'True' END AS is_cdc_enabled from " +
                   " sys.databases where database_id>4 and state_desc='Online' order by is_cdc_enabled desc ";
                DataTable _dtdbs = qe.fn_ExecuteTable(_querytolistdatabases);
                return _dtdbs;
            }catch(Exception)
            {
                throw;
            }
        }
    }
}
