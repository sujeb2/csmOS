using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;
using csmOS;
using Cosmos.Core;
using System.Security.Cryptography;

namespace csmOS
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            String err = "[ERROR]: ";
            String okk = "[OK]: ";

            try
            {
                var fs = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                long availableSpace = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace("0:\\");
                Console.WriteLine(okk + "File system loaded.");
            }
            catch (Exception e)
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(err + e.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
            String ok = "[OK]: ";
            String inf = "[INFO]: ";
            String er = "[ERROR]: ";
            ulong fullRam = Cosmos.Core.GCImplementation.GetAvailableRAM();
            ulong alivRam = Cosmos.Core.GCImplementation.GetAvailableRAM();
            var ver = 1.1;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(ok + "Boot success");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(ok + "Kernel Loaded.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(inf + fullRam + "MB OK.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                                  //   ) ) //   ) ) ");
            Console.WriteLine("    ___      ___      _   __     //   / / ((        ");
            Console.WriteLine("  //   ) ) ((   ) ) // ) )  ) ) //   / /    \\      ");
            Console.WriteLine(" //         \\ \\    // / /  / / //   / /       ) )");
            Console.WriteLine("((____   //   ) ) // / /  / / ((___/ / ((___ / /      " + ver);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  Open-source TUI OS");
            Console.WriteLine("=========================================================");
        }

        protected override void Run()
        {
            ulong fullRam = Cosmos.Core.GCImplementation.GetAvailableRAM();
            ulong alivRam = Cosmos.Core.GCImplementation.GetAvailableRAM();
            ulong usedRam = fullRam - alivRam;
            string cpu = Cosmos.Core.CPU.GetCPUBrandString();
            ulong cpuuptime = Cosmos.Core.CPU.GetCPUUptime();
            long cpuspeed = Cosmos.Core.CPU.GetCPUCycleSpeed();
            string cpuvendor = Cosmos.Core.CPU.GetCPUVendorName();
            String hostname = "virtual";
            String username = "root";
            String er = "[ERROR]: ";
            String wn = "[WARN]: ";
            String qs = "[Q]: ";
            String ok = "[OK]: ";
            String inf = "[INFO]: ";

            Console.Write(username + "@" + hostname +"# > ");
            string command = Console.ReadLine();
            switch (command)
            {
                case "shutdown":
                    {
                        Cosmos.System.Power.Shutdown();
                        Console.WriteLine(inf + "Shutting Down...");
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine(ok + "Complete.");
                        break;
                    }
                case "reset":
                    {
                        Cosmos.System.Power.Reboot();
                        break;
                    }
                case "about":
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("                                  //   ) ) //   ) ) ");
                        Console.WriteLine("    ___      ___      _   __     //   / / ((        ");
                        Console.WriteLine("  //   ) ) ((   ) ) // ) )  ) ) //   / /    \\      ");
                        Console.WriteLine(" //         \\ \\    // / /  / / //   / /       ) )");
                        Console.WriteLine("((____   //   ) ) // / /  / / ((___/ / ((___ / /      1.0");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("=========================================================");
                        Console.WriteLine(" ");
                        Console.WriteLine("Copyright sujeb2 2021-2022");
                        Console.WriteLine("Write license to check 3rd party license");
                        Console.WriteLine("");
                        break;
                    }
                case "license":
                    {
                        Console.WriteLine("BSD 3-Clause License\n\nCopyright (c) 2022, CosmosOS, COSMOS Project\nAll rights reserved.\n\nRedistribution and use in source and binary forms, with or without\nmodification, are permitted provided that the following conditions are met:\n\n1. Redistributions of source code must retain the above copyright notice, this\n   list of conditions and the following disclaimer.\n\n2. Redistributions in binary form must reproduce the above copyright notice,\n   this list of conditions and the following disclaimer in the documentation\n   and/or other materials provided with the distribution.\n\n3. Neither the name of the copyright holder nor the names of its\n   contributors may be used to endorse or promote products derived from\n   this software without specific prior written permission.");
                        Console.WriteLine("\nGNU GENERAL PUBLIC LICENSE\nVersion 2, June 1991\nCopyright (C) 1989, 1991 Free Software Foundation, Inc.,\n51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA\n Everyone is permitted to copy and distribute verbatim copies\n of this license document, but changing it is not allowed.");
                        break;
                    }
                case "help":
                    {
                        Console.WriteLine("shutdown - shutdown your computer");
                        Console.WriteLine("reset - resets your computer");
                        Console.WriteLine("help - this command");
                        Console.WriteLine("about - shows about section");
                        Console.WriteLine("license - shows 3rd party license");
                        Console.WriteLine("clear - clear your screen");
                        Console.WriteLine("dir - show directory");
                        Console.WriteLine("rdf - readfiles from everything in disk");
                        Console.WriteLine("rdsf - readfiles from args");
                        Console.WriteLine("cnf - create file");
                        Console.WriteLine("vi - write file");
                        Console.WriteLine("drm - make directory");
                        Console.WriteLine("color - changes background color");
                        Console.WriteLine("debug - shows pc info");
                        Console.WriteLine("date - shows current date");
                        Console.WriteLine("pause - pause system");
                        Console.WriteLine("power - another way to shutdown, restart your system");
                        break;
                    }
                case "clear":
                    {
                        Console.Clear();
                        break;
                    }
                default:
                    {
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(er + "No such command found, type 'help' to check commands.");
                        Console.ForegroundColor = ConsoleColor.White;
                        command = null; break;
                    }
                // file system
                case "dir":
                    {
                        string[] filePaths = Directory.GetFiles(@"0:\");
                        var drive = new DriveInfo("0");
                        string fsType = Sys.FileSystem.VFS.VFSManager.GetFileSystemType("0:\\");

                        Console.WriteLine("Volume in drive 0 is " + $"{drive.VolumeLabel}");
                        Console.WriteLine("Directory of " + @"0:\");
                        Console.WriteLine("File System Type: " + fsType);
                        Console.WriteLine("\n");
                        for (int i = 0; i < filePaths.Length; ++i)
                        {
                            string path = filePaths[i];
                            Console.WriteLine(System.IO.Path.GetFileName(path));
                        }
                        foreach (var d in System.IO.Directory.GetDirectories(@"0:\"))
                        {
                            var dir = new DirectoryInfo(d);
                            var dirName = dir.Name;

                            Console.WriteLine(dirName + " <DIR>");
                        }
                        Console.WriteLine("\n");
                        Console.WriteLine("        " + $"{drive.TotalSize}" + " bytes");
                        Console.WriteLine("        " + $"{drive.AvailableFreeSpace}" + " bytes free");
                        break;
                    }
                case "rdf":
                    {
                        var directoryList = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing("0:\\");
                        try
                        {
                            foreach (var directoryEntry in directoryList)
                            {
                                var fileStream = directoryEntry.GetFileStream();
                                var entryType = directoryEntry.mEntryType;
                                if (entryType == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                                {
                                    byte[] content = new byte[fileStream.Length];
                                    fileStream.Read(content, 0, (int)fileStream.Length);
                                    Console.WriteLine("File name: " + directoryEntry.mName);
                                    Console.WriteLine("File size: " + directoryEntry.mSize);
                                    Console.WriteLine("Content: ");
                                    foreach (char ch in content)
                                    {
                                        Console.Write(ch.ToString());
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "rdsf":
                    {
                        try
                        {
                            Console.Write("Please enter file name. > ");
                            var input = Console.ReadLine();
                            var helloFile = Sys.FileSystem.VFS.VFSManager.GetFile(@"0:\" + input);
                            var helloFileStream = helloFile.GetFileStream();

                            if (helloFileStream.CanRead)
                            {
                                Console.WriteLine("Content: \n");
                                byte[] textToRead = new byte[helloFileStream.Length];
                                helloFileStream.Read(textToRead, 0, (int)helloFileStream.Length);
                                Console.WriteLine(Encoding.Default.GetString(textToRead));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "cnf":
                    {
                        try
                        {
                            Console.Write("Please enter file name. > ");
                            var input = Console.ReadLine();
                            Sys.FileSystem.VFS.VFSManager.CreateFile(@"0:\" + input);
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "wf":
                    {
                        try
                        {
                            Console.Write("Please enter file name. > ");
                            var input = Console.ReadLine();
                            var helloFile = Sys.FileSystem.VFS.VFSManager.GetFile(@"0:" + input);
                            var helloFileStream = helloFile.GetFileStream();

                            if (helloFileStream.CanWrite)
                            {
                                Console.WriteLine("-WRITE-\nPress 'ENTER' to finish.");
                                var input2 = Console.ReadLine();
                                byte[] textToWrite = Encoding.ASCII.GetBytes(input2);
                                helloFileStream.Write(textToWrite, 0, textToWrite.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                // end
                case "colortest":
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Hi");
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine("Hi");
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Hi");
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    }
                case "color":
                    {
                            Console.WriteLine("Pick a background color.");
                            Console.WriteLine("1. Black;");
                            Console.WriteLine("2. Blue.");
                            Console.WriteLine("3. Orange.");
                            Console.WriteLine("4. Cyan");
                            Console.WriteLine("5. Exit");
                            Console.Write(">>: ");
                            string color_select = Console.ReadLine();

                            if (color_select == "1")
                            {

                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Clear();

                            }

                            else if (color_select == "2")
                            {

                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.Clear();

                            }

                            else if (color_select == "3")
                            {

                                Console.BackgroundColor = ConsoleColor.Yellow; // The "Yellow" here actually looks like orange.
                                Console.Clear();

                            }

                            else if (color_select == "4")
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                Console.Clear();
                            }

                            else if (color_select == "5")
                            {
                                csmOS.clear();
                                break;
                            }

                            else
                            {
                                Console.Beep();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR]: Invaild command.");
                                Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "showall":
                    {
                        Console.WriteLine(er);
                        Console.WriteLine(wn);
                        Console.WriteLine(ok);
                        Console.WriteLine(inf);
                        Console.WriteLine(qs);
                        break;
                    }
                case "rd":
                    {
                        try
                        {
                            Console.Write("Insert file name. >  ");
                            string file_delete = Console.ReadLine();
                            File.Delete(@"0:\" + file_delete);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(ok + "Complete.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "debug":
                    {
                        Console.WriteLine("");
                        Console.WriteLine(username + "@" + hostname);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("CPU: " + cpuvendor + " " + cpu + " @ " + cpuspeed);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("RAM: " + alivRam + "mb" + " / " + fullRam + "mb");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Uptime: " + cpuuptime);
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case "drm":
                    {
                        try
                        {
                            Console.Write("Insert file name. >  ");
                            string dirmake = Console.ReadLine();
                            Sys.FileSystem.VFS.VFSManager.CreateDirectory(@"0:\" + dirmake);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(ok + "Complete.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "vi":
                    {
                        vi.textpad();
                        break;
                    }
                case "ls":
                    {
                        string[] filePaths = Directory.GetFiles(@"0:\");
                        var drive = new DriveInfo("0");
                        string fsType = Sys.FileSystem.VFS.VFSManager.GetFileSystemType("0:\\");
                        for (int i = 0; i < filePaths.Length; ++i)
                        {
                            string path = filePaths[i];
                            Console.WriteLine(System.IO.Path.GetFileName(path));
                        }
                        foreach (var d in System.IO.Directory.GetDirectories(@"0:\"))
                        {
                            var dir = new DirectoryInfo(d);
                            var dirName = dir.Name;

                            Console.WriteLine(dirName + " <DIR>");
                        }
                        break;
                    }
                case "upupdowndownleftrightleftrightba":
                    {
                        Console.WriteLine("THANK YOU FOR USING!");
                        break;
                    }
                case "date":
                    {
                        try
                        {
                            object currdate = DateTime.Now;
                            Console.WriteLine("Current date is: " + currdate);
                        }
                        catch (Exception e)
                        {
                            Console.Beep();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(er + e.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                case "pause":
                    {
                        Console.WriteLine("System Paused, Press any key to return.");
                        Console.ReadKey();
                        break;
                    }
                case "power":
                    {
                        power.start();
                        break;
                    }
            }
        }
    }

    // idk
    public class csmOS
    {
        public static void clear()
        {
            Console.Clear();
        }

        public static void error()
        {
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: ");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
