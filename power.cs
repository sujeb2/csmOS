using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csmOS
{
    internal class power
    {
        public static void start()
        {
            while (true)
            {
                try
                {
                    csmOS.clear();
                    Console.WriteLine("");
                    Console.WriteLine("[1] Shutdown.");
                    Console.WriteLine("[2] Restart.");
                    Console.Write("> ");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            {
                                Cosmos.System.Power.Shutdown();
                                break;
                            }
                        case "2":
                            {
                                Cosmos.System.Power.Reboot();
                                break;
                            }
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR]: Invaild command.");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Beep();
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
