using DokanNet;
using DokanNet.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AllClouds
{
    internal class MountFS
    {
        public void MountDrive(string diskletter)
        {
            try
            {
                diskletter = diskletter.ToLower();
                diskletter = diskletter + ":\\";
                using (var mre = new System.Threading.ManualResetEvent(false))
                using (var dokanLogger = new ConsoleLogger("[Dokan] "))
                using (var dokan = new Dokan(dokanLogger))
                {
                    Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
                    {
                        e.Cancel = true;
                        mre.Set();
                    };

                    var rfs = new TestCloudFS();
                    var dokanBuilder = new DokanInstanceBuilder(dokan)
                        .ConfigureOptions(options =>
                        {
                            options.Options = DokanOptions.DebugMode | DokanOptions.StderrOutput;
                            options.MountPoint = diskletter;
                        });
                    using (var dokanInstance = dokanBuilder.Build(rfs))
                    {
                        mre.WaitOne();
                    }

                }
            }
            catch (DokanException ex)
            {

            }
        }
        public void Mount(string diskletter)
        {
            Thread t = new Thread(x => MountDrive(diskletter));
            t.IsBackground = true;
            t.Start();

        }


        public void Unmount(string diskletter)
        {
            char letter = diskletter.ToCharArray()[0];
            var dokanLogger = new ConsoleLogger("[Dokan] ");
            Dokan dokan = new Dokan(dokanLogger);
            dokan.Unmount(letter);
        }

        public string[] FreeDiskLetter()
        {
            char[] lang = new char[26];

            //char lang[26];
            for (int i = 0; i < 26; i++)
            {
                int ch = i + 65;
                lang[i] = (char)ch; // в массиве будут храниться все буквы в верхнем регистре.
            }
            DriveInfo[] di = DriveInfo.GetDrives();
            ArrayList list = new ArrayList();
            foreach (DriveInfo d in di)
            {
                string dl = d.Name;
                string[] parts = dl.Split(':'); //разделение строки поставить, чтобы выдавать имена дисков (не законечннеая строка!)
                char letter = parts[0].ToCharArray()[0];
                list.Add(letter);

                //MessageBox.Show((d.Name.ToCharArray(0, 1)).ToString());
            }
            char[] busy_drive_letters = (char[])list.ToArray(typeof(char));
            IEnumerable<char> unique_letter_free = lang.Except<char>(busy_drive_letters);
            char[] free_drive_letters = unique_letter_free.ToArray();
            string[] diskletter = new string[free_drive_letters.Length];
            //char[] free_drive_letters = lang.Except(busy_drive_letters);
            for (int i = 0; i < free_drive_letters.Length; i++)
            {
                diskletter[i] = new string(free_drive_letters[i], 1);
            }
            return diskletter;
        }


    }
}
