using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Classlib;

namespace App1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Title = "APP1";
            HeartbeatReceive hbr = new HeartbeatReceive(9051);
            HeartbeatSend hbs = new HeartbeatSend(9050);
            hbs.message = "Skillsoft : Software";
            int eingabe;
            Thread empfangThread = new Thread(new ThreadStart(hbr.DoWork));
            Thread sendThread = new Thread(new ThreadStart(hbs.DoWork));
            bool isrunning = false;
            empfangThread.Start();
            do
            {
                eingabe = menu();
                switch (eingabe)
                {
                    case 1:
                        if (!sendThread.IsAlive && !isrunning)
                        {
                            sendThread = new Thread(new ThreadStart(hbs.DoWork));
                            sendThread.Start();
                            Console.WriteLine("Starting");
                            isrunning = true;
                        }
                        else
                        {
                            Console.WriteLine("Already Running...");
                        }
                        break;
                    case 2:
                        if (isrunning)
                        {
                            hbs.RequestStop();
                            isrunning = false;
                            Console.WriteLine("Now Stopping");
                        }
                        else
                        {
                            Console.WriteLine("Was not running...");
                        }
                        break;
                    case 3:
                        if ((DateTime.Now - hbr.lastrecieve).TotalSeconds > 3)
                        {
                            Console.WriteLine("Timeout happened");
                        }
                        else
                        {
                            Console.WriteLine($"Last message received at {hbr.lastrecieve.ToString()} everthing OK!");
                        }
                        break;
                    case 4:
                        Console.WriteLine($"Dienste: {hbr.lastmsg}");
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Falsche eingabe: " + eingabe);
                        break;
                }
                Thread.Sleep(500);
            } while (eingabe != 5);
            hbr.RequestStop();
            hbs.RequestStop();
        }
        public static int menu()
        {
            try
            {
                Console.WriteLine("1. Dienstankündigung einschalten");
                Console.WriteLine("2. Dienstankündigung ausschalten");
                Console.WriteLine("3. Dienstankündigung Zustand der anderen Rechner anzeigen");
                Console.WriteLine("4. Dienste anzeigen");
                Console.WriteLine("5. Exit");
                Console.Write("Eingabe: ");
                return int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
