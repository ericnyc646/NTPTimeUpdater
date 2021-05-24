using System;
using System.Net;
using System.Timers;
using GuerrillaNtp;

namespace NTPTimeUpdater
{
    class Program
    {
        private static Timer aTimer;
        static void Main(string[] args)
        {
            SetTimer();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application starting");
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(15000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TimeSpan offset;

            try
            {
                using (var ntp = new NtpClient(Dns.GetHostAddresses("pool.ntp.org")[0]))
                {
                    offset = ntp.GetCorrectionOffset();
                }

                DateTime accurateTime = DateTime.UtcNow + offset;

                Console.WriteLine($"NTP time is: { accurateTime }");
            }
            catch (Exception ex)
            {
                offset = TimeSpan.Zero;
                Console.WriteLine(ex.Message);
            }
        }
    }
}
