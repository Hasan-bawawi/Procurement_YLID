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
    internal class POReminderService
    {

        public static async Task Process()
        {
            //Console.WriteLine("Running PO Reminder...");

            var data = GetPOReminderData();
            //Console.WriteLine("Total PO Data: " + data.Count);

            var grouped = data
                .GroupBy(x => new { x.NikEmail, x.EmailKaryawan , x.fullname})
                .ToList();

            //foreach (var group in grouped)
            //{
            //    var poList = group.Select(x => x.No).ToList();
            //    await EmailHelper.SendReminder(group.Key.EmailKaryawan, poList, "PO");
            //}

            foreach (var group in grouped)
            {
                try
                {
                    var rfList = group.Select(x => x.No).ToList();

                    await EmailHelper.SendReminderGraph(group.Key.EmailKaryawan, rfList, "PO", group.Key.fullname);
                }
                catch (Exception ex)
                {
                    Logger.Log($"PO Reminder Failed for {group.Key.EmailKaryawan}");
                    Logger.Log(ex);
                }
            }


            //Console.WriteLine("PO Reminder Done.");
        }



        static List<ReminderData> GetPOReminderData()
        {
            var list = new List<ReminderData>();

            string connString = ConfigurationManager
                .ConnectionStrings["dbpath"]
                .ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetReminderPO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new ReminderData
                    {
                        NikEmail = reader["nikemail"].ToString(),
                        No = reader["po_no"].ToString(),
                        EmailKaryawan = reader["email_karyawan"].ToString(),
                        fullname = reader["fullname"].ToString()
                    });
                }
            }

            return list;
        }







    }
}
