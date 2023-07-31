using System;
using System.Configuration;
using System.IO;
using System.Net.Http;

namespace Base64Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string input = ConfigurationManager.AppSettings["Input"];
                string output = ConfigurationManager.AppSettings["Output"];
                string backup = ConfigurationManager.AppSettings["Backup"];
              

                string[] files = Directory.GetFiles(input);

                foreach (string file in files)
                {
                   
                     
                    //string proposalNum = "PWPP1007713231079846";
                    string filename = Path.GetFileNameWithoutExtension(file);
                    string proposalNum = UserFunctions.GetValueBetween(filename, "-", "-");

                    string base64String = UserFunctions.ConvertToBase64(file);

                    bool status = UserFunctions.APICall(Path.GetFileName(file), base64String, proposalNum);
                    if (status)
                    {
                        File.Move(file, backup + Path.GetFileName(file));
                    }

                    


                    Console.WriteLine(filename + " Completed Successfully");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine();
            Console.WriteLine("Process Completed Successfully");
            Console.ReadKey();
        }
    }
}
