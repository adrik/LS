using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Models.DB;

namespace MyMvc.Models
{
    public static class UserFunctions
    {
        public static IEnumerable<T> SelectUser<T>(int userId, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;

            var query = (
                from d in db.Devices
                join u in db.Users on d.UserId equals u.Id
                join l in db.RecentLocations on d.Id equals l.DeviceId
                where u.Id == userId
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }
        public static IEnumerable<T> SelectUser<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectUser<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        public static IEnumerable<T> SelectContacts<T>(int userId, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;

            var query = (
                from r in db.Relations
                join u in db.Users on r.ContactId equals u.Id
                join d in db.Devices on u.Id equals d.UserId
                join l in db.RecentLocations on d.Id equals l.DeviceId
                where r.UserId == userId && r.GroupId == 1
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }
        public static IEnumerable<T> SelectContacts<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectContacts<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        public static IEnumerable<T> SelectAll<T>(int userId, Func<DbSelection, T> selector)
        {
            return SelectUser(userId, selector).Union(SelectContacts(userId, selector));
        }
        public static IEnumerable<T> SelectAll<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectAll<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }


        public static void UpdateLocation(int userId, double lat, double lng)
        {
            var db = ModelContext.Instance;

            DbDevice device = db.Devices.FirstOrDefault(x => x.UserId == userId);
            db.Locations.Add(new DbLocation() { DeviceId = device.Id, Lat = lat, Lng = lng, Time = DateTime.Now });
            db.SaveChanges();
        }
        public static void UpdateLocation(string login, double lat, double lng)
        {
            UpdateLocation(ModelContext.Instance.FindUserByLogin(login).Id, lat, lng);
        }

        public static void CreateUpdateUserCode(string login, string code)
        {
            var db = ModelContext.Instance;
            var user = db.FindUserByLogin(login);
            
            if (user == null)
            {
                CreateUser(login, code);
            }
            else
            {
                user.Code = code;
                db.SaveChanges();
            }
        }

        public static bool Connect(string login, string code, out string other)
        {
            var db = ModelContext.Instance;

            var user = db.FindUserByLogin(login);
            var contact = db.Users.SingleOrDefault(x => x.Code == code);

            if (contact != null)
            {
                other = contact.Login;
                bool connected = db.Relations.Any(x => x.UserId == user.Id && x.ContactId == contact.Id);

                if (!connected)
                {
                    db.Relations.Add(new DbRelation() { UserId = user.Id, ContactId = contact.Id, GroupId = 1 });
                    db.Relations.Add(new DbRelation() { UserId = contact.Id, ContactId = user.Id, GroupId = 1 });
                    db.SaveChanges();

                    return true;
                }
            }
            else
                other = null;

            return false;
        }

        public static bool Disconnect(string login, string other)
        {
            var db = ModelContext.Instance;

            var user = db.FindUserByLogin(login);
            var contact = db.FindUserByLogin(other);

            if (contact != null)
            {
                DbRelation[] relations = (
                    from rel in db.Relations
                    where
                        (rel.UserId == user.Id && rel.ContactId == contact.Id) ||
                        (rel.UserId == contact.Id && rel.ContactId == user.Id)
                    select rel).ToArray();

                if (relations.Length > 0)
                {
                    foreach (DbRelation rel in relations)
                        db.Relations.Remove(rel);

                    db.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public static DbUser CreateUser(string login, string code)
        {
            var db = ModelContext.Instance;

            var user = new DbUser() { Login = login, Name = login, Code = code };
            db.Users.Add(user);
            db.SaveChanges();
            var device = new DbDevice() { DeviceKey = login, Name = login + " device", Status = 1, UserId = user.Id };
            db.Devices.Add(device);
            db.SaveChanges();

            return user;
        }
    }
}