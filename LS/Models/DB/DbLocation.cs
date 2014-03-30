using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbLocation
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime Time { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}