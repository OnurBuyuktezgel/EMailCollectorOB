using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMailCollector.POCApp;

public class Helpers
{
    public static List<string> ExtractEmails(string SourceText)
    {
        //const string validationPattern =
        //  @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        //  + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        //  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        //  + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

        //const string validationPattern =
        //@"^\w + (?:[.-]\w +)*@\w + (?:[.-]\w +)*(?:\.\w{ 2,3})+$";

        const string validationPattern =
        @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";

        Regex rx = new Regex(validationPattern,
          RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline, 
          TimeSpan.FromSeconds(20));

        // Find matches.
        MatchCollection matches = rx.Matches(SourceText.ToLower());

        // Report the number of matches found.
        int noOfMatches = matches.Count;

        List<string> resultList = new List<string>();

        if (noOfMatches != 0)
        {
            // Report on each match.
            foreach (Match match in matches)
            {
                resultList.Add(match.Value.ToString());
            }
        }

        return resultList;
    }

    
}
