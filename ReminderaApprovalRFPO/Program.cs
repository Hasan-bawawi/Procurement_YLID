//using ReminderaApprovalRFPO.Service;
//using ReminderaApprovalRFPO.Helper;
//static async Task Main(string[] args)
//{
//    Console.WriteLine("=== START REMINDER JOB ===");

//    try
//    {
//        var rfTask = RFReminderService.Process();
//        var poTask = POReminderService.Process();

//        await Task.WhenAll(rfTask, poTask);

//        Console.WriteLine("=== ALL JOB COMPLETED ===");
//    }
//    catch (Exception ex)
//    {
//        Logger.Log("FATAL ERROR");
//        Logger.Log(ex);
//    }


//    Console.WriteLine("Press any key to exit...");
//    Console.ReadKey();
//}

using ReminderaApprovalRFPO.Service;
using ReminderaApprovalRFPO.Helper;

namespace ReminderaApprovalRFPO
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Console.WriteLine("=== START REMINDER JOB ===");

            try
            {
                var rfTask = RFReminderService.Process();
                var poTask = POReminderService.Process();

                await Task.WhenAll(rfTask, poTask);

                //Console.WriteLine("=== ALL JOB COMPLETED ===");
            }
            catch (Exception ex)
            {
                Logger.Log("FATAL ERROR");
                Logger.Log(ex);
            }

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }
    }
}
