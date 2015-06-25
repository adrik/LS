using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Models.DB;

namespace MyMvc.Models
{
    public static class UserFunctions
    {
        [Obsolete]
        public static IEnumerable<T> SelectUser<T>(int userId, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;

            var query = (
                from d in db.Devices
                join u in db.Users on d.UserId equals u.Id
                from l in db.Locations.Where(x => x.DeviceId == d.Id).DefaultIfEmpty()
                where u.Id == userId
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }

        public static IEnumerable<T> SelectUser<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectUser<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        [Obsolete]
        public static IEnumerable<T> SelectContacts<T>(int userId, Func<DbSelection, T> selector, bool master = false)
        {
            var db = ModelContext.Instance;

            if (userId == 19 && master)
            {
                var query = (
                    from u in db.Users
                    join d in db.Devices on u.Id equals d.UserId
                    join l in db.Locations on d.Id equals l.DeviceId
                    where u.Id != userId
                    select new DbSelection() { User = u, Device = d, Location = l }).ToList();

                return query.Select(x => selector(x));
            }
            else
            {
                var query = (
                    from myd in db.Devices
                    join r in db.DeviceRelations on myd.Id equals r.DeviceId
                    join d in db.Devices on r.OtherDeviceId equals d.Id
                    join u in db.Users on d.UserId equals u.Id
                    join l in db.Locations on d.Id equals l.DeviceId
                    where myd.UserId == userId && r.GroupId == 1
                    select new DbSelection() { User = u, Device = d, Location = l }).ToList();

                return query.Select(x => selector(x));
            }
        }

        public static IEnumerable<T> SelectMasterContactsForUser<T>(int userId, string name, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;

            if (userId != 19)
                return Enumerable.Empty<DbSelection>().Select(selector);

            var query = (
                from myd in db.Devices
                join r in db.DeviceRelations on myd.Id equals r.DeviceId
                join d in db.Devices on r.OtherDeviceId equals d.Id
                join u in db.Users on d.UserId equals u.Id
                join l in db.Locations on d.Id equals l.DeviceId
                join myu in db.Users on myd.UserId equals myu.Id
                where (myu.Login.Contains(name) || myu.Name.Contains(name)) && r.GroupId == 1
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(selector);
        }

        public static IQueryable<DbLocation> SelectLocationsForDemo(int userId)
        {
            var db = ModelContext.Instance;

            if (userId != 19)
                return Enumerable.Empty<DbLocation>().AsQueryable();

            return
                from d in db.Devices
                join l in db.Locations on d.Id equals l.DeviceId
                select l;
        }

        [Obsolete]
        public static IEnumerable<T> SelectContactsForDevice<T>(DbDevice device, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;
            
            var query = (
                from r in db.DeviceRelations
                join d in db.Devices on r.OtherDeviceId equals d.Id
                join u in db.Users on d.UserId equals u.Id
                join l in db.Locations on d.Id equals l.DeviceId
                where r.DeviceId == device.Id && r.GroupId == 1 && 
                    (!device.LastUpdate.HasValue || l.Time > device.LastUpdate.Value)
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }

        // --- V2 ---
        public static Tuple<DbDevice, DbLocation>[] SelelectContacts(DbDevice device)
        {
            var db = ModelContext.Instance;

            var query = (
                from r in db.DeviceRelations
                join d in db.Devices on r.OtherDeviceId equals d.Id
                from l in db.Locations.Where(x => x.DeviceId == d.Id).DefaultIfEmpty()
                where r.DeviceId == device.Id && r.GroupId == 1
                select new { Device = d, Location = l }).ToArray();

            return query.Select(x => new Tuple<DbDevice, DbLocation>(x.Device, x.Location)).ToArray();
        }

        public static DbLocation[] SelectContactLocations(DbDevice device)
        {
            var db = ModelContext.Instance;

            var query =
                from r in db.DeviceRelations
                join l in db.Locations.AsNoTracking() on r.OtherDeviceId equals l.DeviceId
                where r.DeviceId == device.Id && r.GroupId == 1 &&
                    (!device.LastUpdate.HasValue || l.Time > device.LastUpdate.Value)
                select l;

            return query.ToArray();
        }

        public static DbDevice SelectDevice(int deviceId)
        {
            return ModelContext.Instance.Devices.FirstOrDefault(x => x.Id == deviceId);
        }

        public static DbLocation SelectLocation(int deviceId)
        {
            return ModelContext.Instance.Locations.FirstOrDefault(x => x.DeviceId == deviceId);
        }

        public static DbUser SelectUser(int userId)
        {
            return ModelContext.Instance.Users.FirstOrDefault(x => x.Id == userId);
        }

        public static DbDevice SelectAnyDevice(string login)
        {
            var db = ModelContext.Instance;

            return (
                from u in db.Users
                join d in db.Devices on u.Id equals d.UserId
                where u.Login == login
                orderby d.Id descending
                select d).FirstOrDefault();
        }
        // ----------

        public static IEnumerable<T> SelectContacts<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectContacts<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        public static IEnumerable<T> SelectAll<T>(int userId, Func<DbSelection, T> selector, bool master = false)
        {
            return SelectUser(userId, selector).Union(SelectContacts(userId, selector, master));
        }
        public static IEnumerable<T> SelectAll<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectAll<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        #region UpdateLocation overloads
        [Obsolete]
        public static void UpdateLocation(int userId, double lat, double lng, DateTime time)
        {
            var db = ModelContext.Instance;
            
            var location = (
                from d in db.Devices
                join l in db.Locations on d.Id equals l.DeviceId
                where d.UserId == userId
                select l).FirstOrDefault();

            if (location == null)
            {
                DbDevice device = db.Devices.FirstOrDefault(x => x.UserId == userId);
                db.Locations.Add(new DbLocation() { DeviceId = device.Id, Lat = lat, Lng = lng, Time = time });
            }
            else
            {
                location.Lat = lat;
                location.Lng = lng;
                location.Time = time;
            }
            db.SaveChanges();
        }
        public static void UpdateLocation(int userId, double lat, double lng)
        {
            UpdateLocation(userId, lat, lng, DateTime.Now.ToUniversalTime());
        }
        public static void UpdateLocation(string login, double lat, double lng, DateTime time)
        {
            UpdateLocation(ModelContext.Instance.FindUserByLogin(login).Id, lat, lng, time);
        }
        public static void UpdateLocation(string login, double lat, double lng)
        {
            UpdateLocation(login, lat, lng, DateTime.Now.ToUniversalTime());
        }

        public static void UpdateLocation(int deviceId, DbLocation source)
        {
            var db = ModelContext.Instance;
            var location = SelectLocation(deviceId);
            var time = DateTime.UtcNow;

            if (location == null)
            {
                source.DeviceId = deviceId;
                source.Time = time;
                db.Locations.Add(source);
            }
            else
            {
                location.Lat = source.Lat;
                location.Lng = source.Lng;
                location.Accuracy = source.Accuracy;
                location.Time = time;
            }
            db.SaveChanges();
        }
        #endregion

        public static void CreateOrUpdateUserCode(Login login, string code)
        {
            var db = ModelContext.Instance;
            
            if (login.Device == null)
            {
                CreateDevice(login.Name, code);
            }
            else
            {
                login.Device.Code = code; // CODE IS CACHED!
                db.SaveChanges();
            }
        }

        public static string MakeNewCode()
        {
            string code;
            do
                code = CodeGen.Next();
            while (IsCodeTaken(code));

            return code;
        }

        public static DbDevice Register(string login, string name)
        {
            var db = ModelContext.Instance;

            var user = db.FindUserByLogin(login);
            if (user == null)
            {
                user = new DbUser() { Login = login, Name = login, PasswordHash = string.Empty };
                db.Users.Add(user);
                db.SaveChanges();
            }

            var device = new DbDevice() { DeviceKey = string.Empty, Name = name, Code = MakeNewCode(), Status = 1, UserId = user.Id };
            db.Devices.Add(device);
            db.SaveChanges();

            return device;
        }

        public static void SetDeviceCode(DbDevice device, string code)
        {
            device.Code = code; // CODE IS CACHED!
            ModelContext.Instance.SaveChanges();
        }

        //[Obsolete]
        //public static bool Connect(DbUser user, string code, out DbUser contact)
        //{
        //    var db = ModelContext.Instance;
        //    contact = db.Users.SingleOrDefault(x => x.Code == code);

        //    if (contact != null)
        //    {
        //        int contactId = contact.Id;
        //        bool alreadyConnected = db.Relations.Any(x => x.UserId == user.Id && x.ContactId == contactId);

        //        if (!alreadyConnected)
        //        {
        //            db.Relations.Add(new DbRelation() { UserId = user.Id, ContactId = contactId, GroupId = 1 });
        //            db.Relations.Add(new DbRelation() { UserId = contactId, ContactId = user.Id, GroupId = 1 });
        //            db.SaveChanges();

        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public static bool Connect(DbDevice device, string code, out DbDevice contact)
        {
            var db = ModelContext.Instance;
            contact = db.Devices.FirstOrDefault(x => x.Code == code);

            if (contact != null)
            {
                int contactId = contact.Id;
                bool alreadyConnected = db.DeviceRelations.Any(x => x.DeviceId == device.Id && x.OtherDeviceId == contactId);

                if (!alreadyConnected)
                {
                    db.DeviceRelations.Add(new DbDeviceRelation() { DeviceId = device.Id, OtherDeviceId = contactId, GroupId = 1 });
                    db.DeviceRelations.Add(new DbDeviceRelation() { DeviceId = contactId, OtherDeviceId = device.Id, GroupId = 1 });
                    db.SaveChanges();

                    return true;
                }
            }
            else
            {
                db.Logs.Add(new DbLog() { ActorId = device.Id, Message = string.Format("Wrong code \"{0}\" entered by {1}", code, device.Name), Timestamp = DateTime.UtcNow });
                db.SaveChanges();
            }

            return false;
        }

        //[Obsolete]
        //public static bool Disconnect(DbUser user, DbUser contact)
        //{
        //    var db = ModelContext.Instance;

        //    if (contact != null)
        //    {
        //        DbRelation[] relations = (
        //            from rel in db.Relations
        //            where
        //                (rel.UserId == user.Id && rel.ContactId == contact.Id) ||
        //                (rel.UserId == contact.Id && rel.ContactId == user.Id)
        //            select rel).ToArray();

        //        if (relations.Length > 0)
        //        {
        //            foreach (DbRelation rel in relations)
        //                db.Relations.Remove(rel);

        //            db.SaveChanges();

        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool Disconnect(DbDevice device, int contactDeviceId)
        {
            var db = ModelContext.Instance;

            DbDeviceRelation[] relations = (
                from rel in db.DeviceRelations
                where
                    (rel.DeviceId == device.Id && rel.OtherDeviceId == contactDeviceId) ||
                    (rel.DeviceId == contactDeviceId && rel.OtherDeviceId == device.Id)
                select rel).ToArray();

            if (relations.Length > 0)
            {
                foreach (DbDeviceRelation rel in relations)
                    db.DeviceRelations.Remove(rel);
                db.SaveChanges();

                return true;
            }
            return false;
        }

        public static DbUser CreateDevice(string login, string code)
        {
            var db = ModelContext.Instance;

            var user = new DbUser() { Login = login, Name = login, PasswordHash = string.Empty };
            db.Users.Add(user);
            db.SaveChanges();
            var device = new DbDevice() { DeviceKey = string.Empty, Name = login, Code = code, Status = 1, UserId = user.Id };
            db.Devices.Add(device);
            db.SaveChanges();

            return user;
        }

        public static bool IsCodeTaken(string code)
        {
            return ModelContext.Instance.Devices.Any(x => x.Code == code);
        }

        public static void SaveUserRating(DbDevice device, int rating)
        {
            var db = ModelContext.Instance;
            db.Ratings.Add(new DbRating() { ActorId = device.Id, Value = rating, Timestamp = DateTime.UtcNow });
            db.SaveChanges();
        }

        public static void SetGcmToken(DbDevice device, string token)
        {
            device.GcmToken = token;
            ModelContext.Instance.SaveChanges();
        }
    }
}