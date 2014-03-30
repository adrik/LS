using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbDevice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string DeviceKey { get; set; }
        public int Status { get; set; }
    }

    public enum DeviceStatus
    {
        Active = 1,
        Available = 2,
        DND = 3
    }
}