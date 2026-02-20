using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReminderaApprovalRFPO.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using ReminderaApprovalRFPO.Helper;




namespace ReminderaApprovalRFPO.Service
{
    internal class RFReminderService
    {
        public static async Task Process()
        {
            //Console.WriteLine("Running RF Reminder...");

            var data = GetRFReminderData();
            //Console.WriteLine("Total RF Data: " + data.Count);


            var grouped = data
                .GroupBy(x => new { x.NikEmail, x.EmailKaryawan , x.fullname})
                .ToList();

            //foreach (var group in grouped)
            //{
            //    var rfList = group.Select(x => x.No).ToList();
            //    await EmailHelper.SendReminder(group.Key.EmailKaryawan, rfList, "RF");
            //}

            foreach (var group in grouped)
            {
                try
                {
                    var rfList = group.Select(x => x.No).ToList();

                    await EmailHelper.SendReminderGraph(group.Key.EmailKaryawan,rfList,"RF",group.Key.fullname);
                }
                catch (Exception ex)
                {
                    Logger.Log($"RF Reminder Failed for {group.Key.EmailKaryawan}");
                    Logger.Log(ex);
                }
            }

            //Console.WriteLine("RF Reminder Done.");
        }


        static List<ReminderData> GetRFReminderData()
        {
            var list = new List<ReminderData>();

            string connString = ConfigurationManager
                .ConnectionStrings["dbpath"]
                .ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetReminderRF", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new ReminderData
                    {
                        NikEmail = reader["nikemail"].ToString(),
                        No = reader["rf_no"].ToString(),
                        EmailKaryawan = reader["email_karyawan"].ToString(),
                        fullname = reader["fullname"].ToString()
                    });
                }
            }

            return list;
        }


    }
}
