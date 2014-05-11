using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public static class MsgFormatter
    {
        private static char[] splitChars = new[] { '|', ',' };
        private static string exactDateFormat = "dd.MM.yyyy HH:mm:ss";
        private static string dateFormat = "dd.MM.yyyy HH:mm";
        private static DateTime date2014 = new DateTime(2014, 1, 1, 0, 0, 0);

        public static string FormatUserLocation(UserLocation location)
        {
            if (location.time.HasValue)
                return string.Format("{0}|{1}|{2}|{3}",
                    location.id,
                    location.lat,
                    location.lng,
                    location.time.Value.ToString(dateFormat));
            else
                return location.id;
        }

        public static UserLocation ParseUserLocation(string text)
        {
            string[] parts = text.Trim().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 3)
                throw new FormatException("Too many parts");

            double[] latLng = (
                from str in parts.Take(2)
                select double.Parse(str.Trim())).ToArray();

            DateTime time = DateTime.Now.ToUniversalTime();
            if (parts.Length > 2)
            {
                if (!DateTime.TryParseExact(parts[2], exactDateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out time))
                    time = DateTime.ParseExact(parts[2], dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
            }

            return new UserLocation() { lat = latLng[0], lng = latLng[1], time = time };
        }

        public static DateTime FromSeconds(long seconds)
        {
            return date2014.AddSeconds(seconds);
        }

        public static long ToSeconds(DateTime date)
        {
            return (long)(date - date2014).TotalSeconds;
        }
    }

}