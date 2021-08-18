using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SpeedTestStats.BL.Interfaces;
using SpeedTestStats.BL.Models;

namespace SpeedTestStats.BL
{
    public class StatsReader : IStatsReader
    {
        private readonly IGlobalSettings _globalSettings;
        private readonly CultureInfo _italianCulture = new CultureInfo("it-IT");

        public StatsReader(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public IEnumerable<StatRow> GetRows()
        {
            var rows = DownloadRemoteFile();
            var response = new List<StatRow>();

            var tempData = default(DateTime);
            decimal? tempPing = null;
            decimal? tempUpload = null;
            decimal? tempDownload = null;
            foreach (var row in rows)
                try
                {
                    if (row.StartsWith("--"))
                    {
                        response.Add(new StatRow
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
                    else if (row.StartsWith("Ping"))
                    {
                        tempPing = DecimalParse(row.Split(' ')[1]);
                    }
                    else if (row.StartsWith("Download"))
                    {
                        tempDownload = DecimalParse(row.Split(' ')[1]);
                    }
                    else if (row.StartsWith("Upload"))
                    {
                        tempUpload = DecimalParse(row.Split(' ')[1]);
                    }
                    else
                    {
                        tempData = ParseDate(row);
                    }
                }
                catch
                {
                    // ignored
                }

            return response.Where(item => item.DataOra != new DateTime());
        }

        private IEnumerable<string> DownloadRemoteFile()
        {
            var webClient = new WebClient();
            return webClient.DownloadString(_globalSettings.SpeedStatsUrl).Split('\r', '\n');
        }

        private DateTime ParseDate(string dateString)
        {
            var dirtDate = CleanUrl(dateString).Split('-').Skip(1).ToList();
            dirtDate = dirtDate.Take(dirtDate.Count - 1).ToList();
            var dirtyDateWithoutFullDay = string.Join("-", dirtDate);
            DateTime.TryParseExact(dirtyDateWithoutFullDay, @"d-MMM-yyyy-HH-mm-ss", _italianCulture,
                DateTimeStyles.None, out var parsedDate);
            return parsedDate;
        }

        private decimal? DecimalParse(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            if (decimal.TryParse(s.Replace(".", ","),
                NumberStyles.Any, _italianCulture, out var result)) return result;
            return null;
        }

        private string CleanUrl(string s, bool lowerCase = false)
        {
            if (string.IsNullOrEmpty(s)) return "";

            if (lowerCase) s = s.ToLower();
            var stringCleaner = new Regex(@"[^\w\s]");
            var nonSpacingMarkRegex =
                new Regex(@"\p{Mn}", RegexOptions.Compiled);
            var multipleSpaces = new Regex(@"\s+");
            return multipleSpaces
                .Replace(
                    nonSpacingMarkRegex.Replace(stringCleaner.Replace(s, " ").Normalize(NormalizationForm.FormD),
                            string.Empty)
                        .Trim(), " ").Replace(' ', '-');
        }
    }
}