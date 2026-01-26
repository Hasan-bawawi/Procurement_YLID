using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Kiota.Abstractions;


namespace procurement_system
{
    public class GlobalHelper
    {

        public static readonly string[] DateFormats = {
        "dd/MM/yyyy HH:mm:ss",
        "M/d/yyyy h:mm:ss tt",
        "yyyy-MM-ddTHH:mm"


      };

        public static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        public static DateTime ParseFlexibleDate(string input)
        {
            return DateTime.ParseExact(input, DateFormats, InvariantCulture, DateTimeStyles.None);
        }
    }
}