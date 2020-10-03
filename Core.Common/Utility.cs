using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Common
{
  public class Utility
  {
    /// <summary>
    ///   Cerca di estrarre un decimal dalla stringa passata, indipendentemente dalla locale. Non gestisce le eccezioni
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public decimal? DecimalParse(string s)
    {
      if (string.IsNullOrEmpty(s)) return null;

      if (decimal.TryParse(s.Replace(".", ","),
        NumberStyles.Any, new CultureInfo("it-IT"), out var result)) return result;
      return null;
    }

    public string CleanUrl(string s, bool lowerCase = false)
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
}