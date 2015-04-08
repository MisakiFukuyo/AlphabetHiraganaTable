using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Hiraganaizer
{
    class Program
    {
        static string delimiter = "{ENTER}";
        static string allkeysString = "qwertyuiopasdfghjklzxcvbnm";
        static string allkeysZenString = "ｑｗｅｒｔｙｕｉｏｐａｓｄｆｇｈｊｋｌｚｘｃｖｂｎｍ";
        static string allSmallHirgs = "ぁぃぅぇぉゕゖっゃゅょゎ";
        static string allBoinHirags= "あいうえお";
        static void SendKeyMessages()
        {
            if (allkeysString.Length != 26)
            {
                Console.WriteLine("Warn!");
            }
            Console.WriteLine("enter to start(10 sec after)");
            Console.ReadLine();
            Console.WriteLine("10 sec waiting");
            Thread.Sleep(10 * 1000);
            foreach (var c in allkeysString)
            {
                SendKeys.SendWait(c.ToString());
                SendKeys.SendWait(delimiter);
                SendKeys.SendWait(delimiter);
            }

            foreach (var c in allkeysString)
            {
                foreach (var d in allkeysString)
                {
                    SendKeys.SendWait(c.ToString());
                    SendKeys.SendWait(d.ToString());
                    SendKeys.SendWait(delimiter);
                    SendKeys.SendWait(delimiter);
                }
            }

            foreach (var c in allkeysString)
            {
                foreach (var d in allkeysString)
                {
                    foreach (var e in allkeysString)
                    {
                        SendKeys.SendWait(c.ToString());
                        SendKeys.SendWait(d.ToString());
                        SendKeys.SendWait(e.ToString());
                        SendKeys.SendWait(delimiter);
                        SendKeys.SendWait(delimiter);
                    }
                }
            }
        }

        static void AlphabetFileWrite(string filename)
        {
            var fs = new StreamWriter(new FileStream(filename,FileMode.CreateNew));
            foreach (var c in allkeysString)
            {
                fs.WriteLine(c.ToString());
            }

            foreach (var c in allkeysString)
            {
                foreach (var d in allkeysString)
                {
                    fs.WriteLine(c.ToString() + d.ToString());
                }
            }

            foreach (var c in allkeysString)
            {
                foreach (var d in allkeysString)
                {
                    foreach (var e in allkeysString)
                    {
                        fs.WriteLine(c.ToString() + d.ToString() + e.ToString());
                    }
                }
            }
            fs.Flush();
            fs.Close();
        }

        static void MatchAlphaHiragLevel1(string fileAlpha,string fileHirag)
        {
            var fra = new StreamReader(fileAlpha, Encoding.UTF8);
            var frh = new StreamReader(fileHirag, Encoding.UTF8);
            var s = "";
            var a = "";
            while(true)
            {
                a = fra.ReadLine();
                s = frh.ReadLine();
                if (s == null || s.Length == 0)
                {
                    return;
                }
                bool continueFlag = false;
                foreach (var z in allkeysZenString)
                {
                    if (s.Contains(z))
                    {
                        continueFlag = true;
                        break;
                    }
                }
                if (continueFlag) continue;
                Console.WriteLine(a + "\t" + s);
            }
        }

        static void MatchedAlphaHiragToLevel2(string level1File)
        {
            var frl = new StreamReader(level1File, Encoding.UTF8);
            var l = "";
            while (true) 
            {
                l = frl.ReadLine();
                if(l == null || l.Length == 0)
                {
                    return;
                }
                var ls = l.Split('\t');
                var a = ls[0];
                var h = ls[1];
                if (h.Length > 1)
                {
                    bool continueFlag = true;
                    foreach (var sh in allSmallHirgs)
                    {
                        if (h.Contains(sh))
                        {
                            continueFlag = false;
                        }
                    }
                    foreach (var bh in allBoinHirags)
                    {
                        if (h.StartsWith(bh.ToString()))
                        {
                            continueFlag = true;
                        }
                    }
                    if (continueFlag)
                    {
                        continue;
                    }
                }
                if (h.Length > 2)
                {
                    continue;
                }
                Console.WriteLine(a + "\t" + h);
            }
        }

        static void Main(string[] args)
        {
            string mode = "";
            string file = "";
            if (args.Length == 1)
            {
                mode = args[0];
            }
            if (mode == "input")
            {
                SendKeyMessages();
            }

            if (args.Length == 2)
            {
                mode = args[0];
                file = args[1];
            }
            if (mode == "write")
            {
                Console.WriteLine("Write file,OK? if OK,Enter");
                Console.ReadLine();
                AlphabetFileWrite(file);
            }
            string fileAlpha;
            string fileHirag;
            if (args.Length == 3)
            {
                mode = args[0];
                fileAlpha = args[1];
                fileHirag = args[2];
                if (mode == "read")
                {
                    MatchAlphaHiragLevel1(fileAlpha, fileHirag);
                }
            }
            string leveledFile = "";
            if (args.Length == 2)
            {
                mode = args[0];
                leveledFile = args[1];
            }
            if (mode == "level2")
            {
                MatchedAlphaHiragToLevel2(leveledFile);
            }
        }
    }
}
