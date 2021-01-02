using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BMO.OLP.Common.Common
{
  public class ApmsMetadata
  {
    public ApmsMetadata(string ln)
    {
      var c = ln.Split('^');
      switch (c.Length)
      {
        case 0: break;
        case 1: Id = Nm = D2 = c[0].Trim(); break;
        case 2: Id = Nm = D2 = c[1].Trim(); break;
        case 3: Id = c[1].Trim(); Nm = c[2].Trim(); break;
        case 4: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); break;
        case 5: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); break;
        case 6: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); D4 = c[5].Trim(); break;
        case 7: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); D4 = c[5].Trim(); D5 = c[6].Trim(); break;
        case 8: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); D4 = c[5].Trim(); D5 = c[6].Trim(); D6 = c[7].Trim(); break;
        case 9: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); D4 = c[5].Trim(); D5 = c[6].Trim(); D6 = c[7].Trim(); D7 = c[8].Trim(); break;
        default: Id = c[1].Trim(); Nm = c[2].Trim(); D2 = c[3].Trim(); D3 = c[4].Trim(); D4 = c[5].Trim(); D5 = c[6].Trim(); D6 = c[7].Trim(); D7 = c[8].Trim(); D8 = c[9].Trim(); break;
      }
    }

    public string Id { get; set; }
    public string Nm { get; set; }
    public string D2 { get; set; }
    public string D3 { get; set; }
    public string D4 { get; set; }
    public string D5 { get; set; }
    public string D6 { get; set; }
    public string D7 { get; set; }
    public string D8 { get; set; }
    public string IdNm { get { return string.Format("{0} - {1}", Id, Nm); } }
    public string D2D3 { get { return string.Format("{0} - {1}", D2, D3); } }

    public static IEnumerable<string> GetFiles(string zipPath)
    {
      using (var archive = ZipFile.OpenRead(zipPath))
      {
        return archive.Entries.Select(s => s.FullName).OrderBy(s => s);
      }
    }

    public static IEnumerable<ApmsMetadata> GetRows(string zipPath, string filename, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = ZipFile.OpenRead(zipPath))
      {
        foreach (ZipArchiveEntry entry in archive.Entries.Where(e => e.FullName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.Open()))
          {
            var content = reader.ReadToEnd(); //Title = string.Format("{0} rows", r.Split('\n').Length);						Debug.WriteLine(r);
            content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l => dc.Add(new ApmsMetadata(l.Trim()))); // .OrderBy(s => s) - seems like the files are sorted in some meaningful way already.
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }
    public static IEnumerable<ApmsMetadata> GetRsD2(string zipPath, string filename, string filterD2, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = ZipFile.OpenRead(zipPath))
      {
        foreach (ZipArchiveEntry entry in archive.Entries.Where(e => e.FullName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.Open()))
          {
            var content = reader.ReadToEnd();
            content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l =>
            {
              var am = new ApmsMetadata(l.Trim());
              if (string.Compare(am.D2, filterD2, true) == 0)
              {
                //am.Nm = string.Format("{0} {1}", am.Nm, am.D2);
                dc.Add(am);
              }
            });
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }
    public static IEnumerable<ApmsMetadata> GetRsD3(string zipPath, string filename, string filterD3 = "RM", bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = ZipFile.OpenRead(zipPath))
      {
        foreach (ZipArchiveEntry entry in archive.Entries.Where(e => e.FullName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.Open()))
          {
            var content = reader.ReadToEnd();
            content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l =>
            {
              var am = new ApmsMetadata(l.Trim());
              if (string.Compare(am.D3, filterD3, true) == 0)
              {
                am.Nm = string.Format("{0} {1}", am.Nm, am.D2);
                dc.Add(am);
              }
            });
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }

    public static IEnumerable<ApmsMetadata> GetRows(string content = "D^B^BMO\nD^H^Haris")
    {
      var dc = new List<ApmsMetadata>();
      content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l => dc.Add(new ApmsMetadata(l.Trim()))); // .OrderBy(s => s) - seems like the files are sorted in some meaningful way already.

      return dc.OrderBy(s => s.Nm);
    }

    public static IEnumerable<ApmsMetadata> GetPsv(byte[] zipContent, string filename, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();

      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));

      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd().Replace("|","^").Replace("\n1","\nD^1"); //Title = string.Format("{0} rows", r.Split('\n').Length);						Debug.WriteLine(r);
            content.Split('\n').Where(s => !s.StartsWith("APMS")).Distinct().ToList().ForEach(l => dc.Add(new ApmsMetadata(l.Trim()))); // .OrderBy(s => s) - seems like the files are sorted in some meaningful way already.
          }
        }
      }
      return dc.OrderBy(s => s.D4);
      /*
          APMS|20151103|00000|SUBSECTOR|2015110409:42:40
          100|20040814|1|CLE|Competitive Local Exchange Carrier|20040814|1|1
          101|20040814|1|ILE|Incumbent Local Exchange Carrier|20040814|1|1
          ...
          112|20040814|1|UTC|Utility / Contracted|20040814|1|1
          APMS|20151103|99999|SUBSECTOR|2015110409:42:40|15
      */
    }

    public static IEnumerable<ApmsMetadata> GetRows(byte[] zipContent, string filename, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd(); //Title = string.Format("{0} rows", r.Split('\n').Length);						Debug.WriteLine(r);
            content.Split('\n').Where(s => s.StartsWith("D")).Distinct().ToList().ForEach(l => dc.Add(new ApmsMetadata(l.Trim()))); // .OrderBy(s => s) - seems like the files are sorted in some meaningful way already.
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }
    public static IEnumerable<ApmsMetadata> GetIdNm(byte[] zipContent, string filename, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd();
            foreach (var l in content.Split('\n').Where(s => s.StartsWith("D")).Distinct().ToList())
            {
              var na = new ApmsMetadata(l.Trim());
              if (!dc.Any(r => r.Id == na.Id && r.Nm == na.Nm))
                dc.Add(na);
            }
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }
    public static IEnumerable<ApmsMetadata> GetD2D3(byte[] zipContent, string filename, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd();
            foreach (var l in content.Split('\n').Where(s => s.StartsWith("D")).Distinct().ToList())
            {
              var na = new ApmsMetadata(l.Trim());
              if (!dc.Any(r => r.D2 == na.D2 && r.D3 == na.D3))
                dc.Add(na);
            }
          }
        }
      }
      return dc.OrderBy(s => s.D2);
    }
    public static IEnumerable<ApmsMetadata> GetRsD2(byte[] zipContent, string filename, string filterD2, bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd();
            content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l =>
            {
              var am = new ApmsMetadata(l.Trim());
              if (string.Compare(am.D2, filterD2, true) == 0)
              {
                //am.Nm = string.Format("{0} {1}", am.Nm, am.D2);
                dc.Add(am);
              }
            });
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }
    public static IEnumerable<ApmsMetadata> GetRsD3(byte[] zipContent, string filename, string filterD3 = "RM", bool allowEmpty = false)
    {
      var dc = new List<ApmsMetadata>();
      if (allowEmpty)
        dc.Add(new ApmsMetadata("^^"));
      using (var archive = Ionic.Zip.ZipFile.Read(new MemoryStream(zipContent)))
      {
        foreach (var entry in archive.Entries.Where(e => e.FileName.EndsWith(filename, StringComparison.OrdinalIgnoreCase)))
        {
          using (var reader = new StreamReader(entry.OpenReader()))
          {
            var content = reader.ReadToEnd();
            content.Split('\n').Where(s => s.StartsWith("D")).ToList().ForEach(l =>
            {
              var am = new ApmsMetadata(l.Trim());
              if (string.Compare(am.D3, filterD3, true) == 0)
              {
                am.Nm = string.Format("{0} {1}", am.Nm, am.D2);
                dc.Add(am);
              }
            });
          }
        }
      }
      return dc.OrderBy(s => s.Nm);
    }

    public override string ToString() { return Nm; }
  }
}
