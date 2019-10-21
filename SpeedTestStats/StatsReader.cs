using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpeedTestStats
{
  public class StatsReader
  {
    public static List<StatRow> Get()
    {
      var filePath = @"\\192.168.1.254\Public\speed_log.txt";
      var content = System.IO.File.ReadAllLines(filePath);
      var response = new List<StatRow>(6600);

      var tempData = default(DateTime);
      decimal tempPing = 0;
      decimal tempUpload = 0;
      decimal tempDownload = 0;
      foreach (var item in content)
      {
        try
        {
          if (item.StartsWith("--"))
          {
            // separatore
            response.Add(new StatRow()
            {
              Ping = tempPing,
              DataOra = tempData,
              Upload = tempUpload,
              Download = tempDownload
            });

            tempPing = 0;
            tempData = default;
            tempUpload = 0;
            tempDownload = 0;
          }
          else if (item.StartsWith("Ping"))
          {
            // ping
            tempPing = DecimalParse(item.Split(' ')[1]);
          }
          else if (item.StartsWith("Download"))
          {
            // download
            tempDownload = DecimalParse(item.Split(' ')[1]);
          }
          else if (item.StartsWith("Upload"))
          {
            // download
            tempUpload = DecimalParse(item.Split(' ')[1]);
          }
          else
          {
            var item1 = CleanUrl(item).Split('-').Skip(1).ToList();
            item1 = item1.Take(item1.Count - 1).ToList();
            var item2 = string.Join("-", item1);
            // data dom 20 gen 2019, 00.20.01
            if (DateTime.TryParseExact(item2, @"d-MMM-yyyy-HH-mm-ss", CultureInfo.CurrentCulture,
              DateTimeStyles.None, out tempData))
            {
              //
            }
            //else if (DateTime.TryParseExact(item1, @"ddd-dd-MMM-yyyy-HH-mm-ss-CET", CultureInfo.CurrentCulture,
            //  DateTimeStyles.None, out tempData))
            //{
            //  //
            //}
            else
            {
              Console.WriteLine(item2);
            }
          }
        }catch(Exception e)
        {
          Console.WriteLine(item);
        }
      }

      return response;
    }

    /// <summary>
    ///   Cerca di estrarre un decimal dalla stringa passata, indipendentemente dalla locale. Non gestisce le eccezioni
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static decimal DecimalParse(string s)
    {
      return decimal.Parse(s.Replace(".", ","), NumberStyles.Any, new CultureInfo("it-IT"));
    }
    public static string CleanUrl(string s, bool lowerCase = false)
    {
      if (string.IsNullOrEmpty(s)) return "";

      if (lowerCase) s = s.ToLower();
      var stringCleaner = new Regex(@"[^\w\s]"); // Rimuovo tutti i caratteri che non siano lettere o spazi
      var nonSpacingMarkRegex =
        new Regex(@"\p{Mn}", RegexOptions.Compiled); // Rimpiazzo caratteri accentati con quelli normali
      var multipleSpaces = new Regex(@"\s+"); // Rimpiazzo gli spazi multipli con i singoli
      var stringa = multipleSpaces
        .Replace(
          nonSpacingMarkRegex.Replace(stringCleaner.Replace(s, " ").Normalize(NormalizationForm.FormD), string.Empty)
            .Trim(), " ").Replace(' ', '-');

      return stringa;
    }
  }

  public class StatRow
  {
    public DateTime DataOra { get; set; }
    public decimal Ping { get; set; }
    public decimal Upload { get; set; }
    public decimal Download { get; set; }

  }

  public static class SystemHelper
  {
    public static string ToJSON(this object obj)
    {
      return JsonConvert.SerializeObject(obj);
    }
  }
}