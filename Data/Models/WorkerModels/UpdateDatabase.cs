using HUD.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HUD.Data.Models.WorkerModels
{
    public class UpdateDatabase
    {

        /// <summary>
        /// Return full user settings from GUID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static UserSettings GetSettings(ApplicationDbContext context, string guid)
        {
            try
            {
                return context.userSettings.First(a => a.UserGuid == guid);
            } 
            catch (Exception e)
            {
                Debug.WriteLine($"Error finding user settings from GUID: {0}", e);
                return null;
            }
        }

        /// <summary>
        /// Update the users specified ticket order in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <param name="lst"></param>
        public static void UpdateSortedListInDb(ApplicationDbContext context, string userGuid, List<string> lst)
        {
            if (context is not null && lst is not null && lst.Count() > 0)
            {
                var result = String.Join(", ", lst.ToArray());
                GetSettings(context, userGuid).ticketOrder = result;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update the tickets the user wants to display in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <param name="lst"></param>
        public static void UpdateIncludeListInDb(ApplicationDbContext context, string userGuid, List<string> lst)
        {
            if (context is not null && lst is not null && lst.Count() > 0)
            {
                var result = String.Join(", ", lst.ToArray());
                GetSettings(context, userGuid).ticketsToShow = result;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update the total number of tickets to show in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <param name="numTicketsPerRow"></param>
        public static void UpdateNumTickets(ApplicationDbContext context, string userGuid, int numTicketsPerRow)
        {
            GetSettings(context, userGuid).numTickets = numTicketsPerRow;
            context.SaveChanges();
        }

        /// <summary>
        /// Update the total tickets to show per status in database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <param name="numTicketsPerRow"></param>
        public static void UpdateNumTicketsPerRow(ApplicationDbContext context, string userGuid, int numTicketsPerRow)
        {
            GetSettings(context, userGuid).numTicketsPerStatus = numTicketsPerRow;
            context.SaveChanges();
        }

        /// <summary>
        /// Update full user settings in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        public static void UpdateUserSettingsInDb(ApplicationDbContext context, UserSettings settings)
        {
            UserSettings usm = context.userSettings.First(a => a.UserGuid == settings.UserGuid);
            usm.location = settings.location;
            usm.ticketOrder = settings.ticketOrder;
            usm.ticketsToShow = settings.ticketsToShow;
            usm.numTicketsPerStatus = settings.numTicketsPerStatus;
            usm.numTickets = settings.numTickets;
            context.SaveChanges();
        }

        /// <summary>
        /// Retrieve the users ticket order from DB.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static List<string> GetTicketOrder(ApplicationDbContext context, string userGuid)
        {
            UserSettings usett = GetSettings(context, userGuid);
            List<string> res = usett.ticketOrder.Split(", ").ToList();
            return res;
            //return usett is not null ? usett.ticketOrder.Split(", ").ToList() : null;
            //return GetSettings(context, userGuid).ticketOrder.Split(",").ToList();
        }

        /// <summary>
        /// Retrieve included tickets from DB.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static List<string> GetIncludeTickets(ApplicationDbContext context, string userGuid)
        {

            UserSettings usett = GetSettings(context, userGuid);
            List<string> res = usett.ticketsToShow.Split(", ").ToList();
            return res;
            //return usett is not null ? usett.ticketsToShow.Split(", ").ToList() : null;
            //return GetSettings(context, userGuid).ticketsToShow.Split(",").ToList();
        }

        /// <summary>
        /// Get user settings from DB.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static UserSettings GetCurrentUserSettings(ApplicationDbContext context, string userGuid)
        {
            return context.userSettings.FirstOrDefault(a => a.UserGuid == userGuid);
        }

    }
}
