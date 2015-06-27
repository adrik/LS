using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Models.DB;

namespace MyMvc.Models
{
    public class Login
    {
        public string Name { get; private set; }
        public DbUser User { get; private set; }
        public DbDevice Device { get; private set; }

        [Obsolete]
        public Login(string name)
        {
            Name = name;

            var db = ModelContext.Instance;

            User = db.FindUserByLogin(name);
            if (User != null)
                Device = db.Devices.FirstOrDefault(x => x.UserId == User.Id);
        }

        [Obsolete]
        public Login(int deviceId)
        {
            Device = UserFunctions.SelectDevice(deviceId);
        }

        public Login(Guid key)
        {
            Device = UserFunctions.SelectDevice(key);
        }
    }
}