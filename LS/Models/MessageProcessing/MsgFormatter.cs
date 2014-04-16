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
        private static string dateFormat = "dd.MM.yyyy HH:mm";

        public static string FormatUserLocation(UserLocation location)
        {
            return string.Format("{0}|{1}|{2}|{3}", location.id, location.lat, location.lng, location.time.ToString(dateFormat));
        }

        public static UserLocation ParseUserLocation(string text)
        {
            string[] parts = text.Split(splitChars);

            if (parts.Length> 3)
                throw new FormatException("Too many parts");

            double[] latLng = (
                from str in parts.Take(2)
                select double.Parse(str.Trim())).ToArray();

            DateTime time = DateTime.Now;
            if (parts.Length > 2)
                time = DateTime.ParseExact(parts[2], dateFormat, System.Globalization.CultureInfo.InvariantCulture);

            return new UserLocation() { lat = latLng[0], lng = latLng[1], time = time };
        }


        private class MyFormatProvider : IFormatProvider
        {
            public object GetFormat(Type formatType)
            {
                throw new NotImplementedException();
            }
        }
    }

}