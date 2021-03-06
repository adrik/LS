﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public static class MessageSystem
    {
        private static object msgLock = new object();

        public static void SaveMessageForDevice(int deviceId, DataMessage msg)
        {
            var db = Models.DB.ModelContext.Instance;

            db.DeviceMessages.Add(new DB.DbDeviceMessage() { DeviceId = deviceId, Type = (int)msg.t, Content = msg.c });
            db.SaveChanges();
        }

        public static DataMessage[] GetSavedMessages(Login login)
        {
            var db = Models.DB.ModelContext.Instance;

            if (login.Device != null && db.DeviceMessages.Any(x => x.DeviceId == login.Device.Id))
            {
                lock (msgLock)
                {
                    var messages = db.DeviceMessages.Where(x => x.DeviceId == login.Device.Id).ToArray();

                    foreach (var msg in messages)
                        db.DeviceMessages.Remove(msg);
                    db.SaveChanges(); // DbUpdateConcurrencyException

                    return messages.Select(x => new DataMessage() { t = (MessageType)x.Type, c = x.Content }).ToArray();
                }
            }
            return new DataMessage[0];
        }
    }
}