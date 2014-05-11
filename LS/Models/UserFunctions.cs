﻿using System;
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
                from l in db.RecentLocations.Where(x => x.DeviceId == d.Id).DefaultIfEmpty()
                where u.Id == userId
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }
        public static IEnumerable<T> SelectUser<T>(string login, Func<DbSelection, T> selector)
        {
            return SelectUser<T>(ModelContext.Instance.FindUserByLogin(login).Id, selector);
        }

        public static IEnumerable<T> SelectContacts<T>(int userId, Func<DbSelection, T> selector, bool master = false)
        {
            var db = ModelContext.Instance;

            if (userId == 19 && master)
            {
                var query = (
                    from u in db.Users
                    join d in db.Devices on u.Id equals d.UserId
                    join l in db.RecentLocations on d.Id equals l.DeviceId
                    where u.Id != userId
                    select new DbSelection() { User = u, Device = d, Location = l }).ToList();

                return query.Select(x => selector(x));
            }
            else
            {
                var query = (
                    from r in db.Relations
                    join u in db.Users on r.ContactId equals u.Id
                    join d in db.Devices on u.Id equals d.UserId
                    join l in db.RecentLocations on d.Id equals l.DeviceId
                    where r.UserId == userId && r.GroupId == 1
                    select new DbSelection() { User = u, Device = d, Location = l }).ToList();

                return query.Select(x => selector(x));
            }
        }

        public static IEnumerable<T> SelectContactsForDevice<T>(DbDevice device, Func<DbSelection, T> selector)
        {
            var db = ModelContext.Instance;
            
            var query = (
                from r in db.Relations
                join u in db.Users on r.ContactId equals u.Id
                join d in db.Devices on u.Id equals d.UserId
                join l in db.RecentLocations on d.Id equals l.DeviceId
                where r.UserId == device.UserId && r.GroupId == 1 && 
                    (!device.LastUpdate.HasValue || l.Time > device.LastUpdate.Value)
                select new DbSelection() { User = u, Device = d, Location = l }).ToList();

            return query.Select(x => selector(x));
        }
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
        #endregion

        public static void CreateOrUpdateUserCode(Login login, string code)
        {
            var db = ModelContext.Instance;
            
            if (login.User == null)
            {
                CreateUser(login.Name, code);
            }
            else
            {
                login.User.Code = code;
                db.SaveChanges();
            }
        }

        public static bool Connect(DbUser user, string code, out DbUser contact)
        {
            var db = ModelContext.Instance;
            contact = db.Users.SingleOrDefault(x => x.Code == code);

            if (contact != null)
            {
                int contactId = contact.Id;
                bool alreadyConnected = db.Relations.Any(x => x.UserId == user.Id && x.ContactId == contactId);

                if (!alreadyConnected)
                {
                    db.Relations.Add(new DbRelation() { UserId = user.Id, ContactId = contactId, GroupId = 1 });
                    db.Relations.Add(new DbRelation() { UserId = contactId, ContactId = user.Id, GroupId = 1 });
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public static bool Disconnect(DbUser user, DbUser contact)
        {
            var db = ModelContext.Instance;

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

            var user = new DbUser() { Login = login, Name = login, Code = code, PasswordHash = string.Empty };
            db.Users.Add(user);
            db.SaveChanges();
            var device = new DbDevice() { DeviceKey = login, Name = login + " device", Status = 1, UserId = user.Id };
            db.Devices.Add(device);
            db.SaveChanges();

            return user;
        }

        public static bool IsCodeTaken(string code)
        {
            return ModelContext.Instance.Users.Any(x => x.Code == code);
        }
    }
}