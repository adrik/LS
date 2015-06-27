using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.RequestHandling
{
    public static class Formatter
    {
        private static char[] splitChars = new[] { '|' };
        private static DateTime date2014 = new DateTime(2014, 1, 1, 0, 0, 0);

        public static string FormatContact(Guid relationKey, DB.DbDevice device, DB.DbLocation location)
        {
            if (location != null)
                return string.Format("{0}|{1}|{2:0.######}|{3:0.######}|{4:0.#}|{5}",
                    device.Name,
                    relationKey,
                    location.Lat,
                    location.Lng,
                    location.Accuracy,
                    SecondsFrom2014(location.Time));
            else
                return string.Format("{0}|{1}", device.Name, relationKey);
        }

        public static string FormatLocation(Guid relationKey, DB.DbLocation location)
        {
            return string.Format("{0}|{1:0.######}|{2:0.######}|{3:0.#}|{4}",
                    relationKey,
                    location.Lat,
                    location.Lng,
                    location.Accuracy,
                    SecondsFrom2014(location.Time));
        }

        public static DB.DbLocation ParseLocation(string text)
        {
            string[] parts = text.Trim().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            double latitude = double.Parse(parts[0]);
            double longitude = double.Parse(parts[1]);
            double accuracy = double.Parse(parts[2]);

            return new DB.DbLocation() { Lat = latitude, Lng = longitude, Accuracy = accuracy };
        }

        public static long SecondsFrom2014(DateTime date)
        {
            return (long)(date - date2014).TotalSeconds;
        }
    }

}