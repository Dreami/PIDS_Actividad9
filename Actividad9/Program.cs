using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileOutputs;

namespace Actividad9
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(Outputs.getAllFiles());
            FileInfo[] Files = d.GetFiles("*.txt");

            string output_path9 = @"C:\Users\maple\Documents\9° Semester\CS13309_Archivos_HTML\a9_matricula.txt";
            string output;

            d = new DirectoryInfo(Outputs.getOutputFiles());
            FileInfo stopListFile = d.GetFiles("stoplist.txt")[0];
            string stopListAll = File.ReadAllText(stopListFile.FullName);
            stopListAll.Trim();
            
            string[] stopList = stopListAll.Split('\n');
            for(int i = 0; i < stopList.Length; i++)
            {
                stopList[i] = stopList[i].Replace("\r", "");
            }
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int count = 0;
            foreach (FileInfo file in Files)
            {
                output = "";
                var watchEach = System.Diagnostics.Stopwatch.StartNew();
                string htmlContent = File.ReadAllText(file.FullName);
                htmlContent.Trim();

                string[] eachWord = htmlContent.Split(' ');
                Dictionary<string, int> wordCount = new Dictionary<string, int>();
                try
                {
                    for (int i = 0; i < eachWord.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(eachWord[i]))
                        {
                            if (!eachWord[i].Equals(" "))
                            {
                                eachWord[i] = eachWord[i].ToLower();
                                eachWord[i] = eachWord[i].Replace(",", "")
                                    .Replace(".", "")
                                    .Replace("\r", "")
                                    .Replace("\t", "")
                                    .Replace("\n", "")
                                    .Replace("(", "")
                                    .Replace(")", "");
                                if (wordCount.ContainsKey(eachWord[i]))
                                {
                                    wordCount[eachWord[i]] = wordCount[eachWord[i]] + 1;
                                } else if(eachWord[i].Length > 1)
                                {
                                    wordCount.Add(eachWord[i], 0);
                                }
                            }
                        }
                    }
                }
                catch (ArgumentNullException argExc)
                {
                    Console.WriteLine(argExc.StackTrace);
                }
                catch (KeyNotFoundException keyNotFoundExc)
                {
                    Console.WriteLine(keyNotFoundExc.StackTrace);
                }

                foreach(var key in wordCount.Keys.ToList())
                {
                    for (int i = 0; i < stopList.Length; i++)
                    {
                        wordCount.Remove(stopList[i]);
                    }
                    if(wordCount.ContainsKey(key))
                    {
                        if (wordCount[key] < 5)
                        {
                            wordCount.Remove(key);
                        } else
                        {
                            output = key + " - " + wordCount[key] + "\n";
                            Outputs.output_print(output_path9, output);
                            Console.WriteLine(output);
                        }
                    }
                }

                output = file.Name + " finished in " + watchEach.Elapsed.TotalMilliseconds.ToString() + " ms";
                Console.WriteLine(output);
                watchEach.Stop();
                Outputs.output_print(output_path9, output);
                count++;
            }

            output = "\nFiles cleaned in\t" + watch.Elapsed.TotalMilliseconds.ToString() + " ms";
            Console.WriteLine(output);
            watch.Stop();
            Outputs.output_print(output_path9, output);
            
            Console.Read();
        }
    }
}
