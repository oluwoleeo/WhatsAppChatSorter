using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WhatsAppChatSorter
{
    public class TraverseEventArgs : EventArgs
    {
        public int lines;
        public int words;

        public TraverseEventArgs(int l, int w)
        {
            lines = l;
            words = w;
        }
    }

    class Program
    {
        public static event EventHandler<TraverseEventArgs> TraverseFinished;

        protected virtual void OnTraverseFinished(TraverseEventArgs e)
        {
            if (TraverseFinished != null)
                TraverseFinished(this, e);
        }

        public static string getNow
        {
            get
            {
                string xxx = DateTime.Now.ToString();
                return xxx.Replace("-", "").Replace(":", "").Replace("\\", "").Replace("/", "");
            }
        }

        public void sortChat()
        {
            int lines = 0;
            int words = 0;

            Console.WriteLine("Enter path for file: ");
            string infile = Console.ReadLine().Replace("\"", "");

            Console.WriteLine("\n ");

            string outfile = Path.GetFileNameWithoutExtension(infile) + "  " + getNow + ".txt";
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            try
            {
                using (StreamReader sr = File.OpenText(infile))
                using (StreamWriter sw = File.CreateText(outfile))
                {
                    int count = 0;

                    while (sr.Peek() > -1)
                    {
                        //For each line
                        string line = sr.ReadLine();
                        string[] larrs = line.Split(' ');

                        for (int i = 0; i < larrs.Length; ++i)
                        {
                            //For the first day encountered
                            if (days.Any(d => larrs[i].Contains(d)) && count == 0)
                            {
                                sw.Write(larrs[i] + " ");
                                ++count;
                                ++words;
                            }
                            //For other days, go to the next line
                            else if (days.Any(d => larrs[i].Contains(d)) && count != 0)
                            {
                                sw.WriteLine();
                                //If it is the last word on the line
                                if (i == larrs.Length - 1)
                                {
                                    larrs[i].Trim();
                                    sw.Write(larrs[i]);
                                    ++lines;
                                }
                                else
                                    sw.Write(larrs[i] + " ");

                                ++words;
                            }
                            else
                            {
                                //If it is the last word on the line
                                if (i == larrs.Length - 1)
                                {
                                    larrs[i].Trim();
                                    sw.Write(larrs[i]);
                                    ++lines;
                                }
                                else
                                    sw.Write(larrs[i] + " ");

                                ++words;
                            }
                        }
                    }

                    OnTraverseFinished(new TraverseEventArgs(lines, words));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.Message);
            }
        }

        public static void TravComp(object sender, TraverseEventArgs tve)
        {
            Console.WriteLine("Chat sorted!\n");
            Console.WriteLine("Total number of lines: {0}", tve.lines);
            Console.WriteLine("Total number of words: {0}", tve.words);
        }

        public static void Main()
        {
            Program re = new Program();
            Assembly a = typeof(Program).Assembly;
            AssemblyName aa = new AssemblyName(a.FullName);
            Console.WriteLine(a.GetName());
            Console.WriteLine(a.EscapedCodeBase);
            Console.WriteLine(a.Evidence);
            TraverseFinished += TravComp;
            re.sortChat();
        }
    }
}