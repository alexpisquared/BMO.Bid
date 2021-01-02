using AsLink;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace BMO.OLP.Common
{
  public class ExcelLoader
  {
    //nst string _cs = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR={1};{2}"";"; // @"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};dbq=D:\bmo\OL\AppData\AtrD2.xls;defaultdir=D:\bmo\OL\AppData;driverid=1046;fil=excel 12.0;filedsn=D:\bmo\OL\AppData\AtrD2.xls.dsn;maxbuffersize=2048;maxscanrows=11;pagetimeout=5;readonly=1;safetransactions=0;threads=3;uid=admin;usercommitsync=Yes;HDR=NO";
    const string _cs = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""{3}HDR={1};{2}"";";
    static string getConString(string file, bool useHeader, bool isCsv) { return string.Format(_cs, file, useHeader ? "Yes" : "No", isCsv ? "FORMAT=Delimited;" : "Excel 12.0;", isCsv ? "Text;" : ""); }
    public static List<string> GetSheetsFromExcel(string fileForXlsOrFolderForCsv, bool useHeader = true, bool isCsv = false)
    {
      var sw = Stopwatch.StartNew();
      var rv = new List<string>();

      try
      {
        using (var con = new OleDbConnection(getConString(fileForXlsOrFolderForCsv, useHeader, isCsv)))
        {
          con.Open();          //nogo: var rrrr = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows.OfType<object>();

          var sheets = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows;
          foreach (DataRow item in sheets)
            rv.Add(item["TABLE_NAME"].ToString().Replace("$", "").ToUpper());

          con.Close();
        }
      }
      catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); throw; }
      finally { Trace.WriteLine(string.Format("\tInfo: {0}.{1}()  {2:s\\.f} sec  {3:#,###} rows", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, sw.Elapsed, rv.Count)); }

      return rv;
    }


    public static DataTable GetTableFromExcel(string file, string sheet, bool useHeader = true, bool isCsv = false, string sql = "select * from [{0}]")
    {
      var tbl = new DataTable();
      var sw = Stopwatch.StartNew();

      try
      {
        using (var con = new OleDbConnection(getConString(file, useHeader, isCsv)))
        {
          con.Open();

          var da = new OleDbDataAdapter(string.Format(sql, isCsv?sheet: (sheet.EndsWith("$") ? sheet : sheet + "$")), con);
          var qnt = da.Fill(tbl);
          con.Close();
        }
      }
      catch (Exception ex) { DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod()); throw; }
      //finally { Trace.WriteLine(string.Format("\tInfo: {0}.{1}()  {2:s\\.f} sec  {3:#,###} rows", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, sw.Elapsed, tbl.Rows.Count)); }

      return tbl;
    }

    static string getThisOr1stSheet(string sheet, OleDbConnection con)
    {
      foreach (DataRow item in con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows)
        if (item["TABLE_NAME"].ToString() == sheet) return sheet;

      return con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
    }

  }
}
