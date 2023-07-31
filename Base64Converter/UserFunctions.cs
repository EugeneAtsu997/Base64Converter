using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Base64Converter
{
    public static class UserFunctions
    {
        public static string ConvertToBase64(string filePath)
        {
            string base64String = string.Empty;
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);

                base64String = Convert.ToBase64String(fileBytes);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

            }

            return base64String;

        }


        public static bool APICall(string fileName,string base64, string proposalNum)
        {
            string serviceName =ConfigurationManager.AppSettings["ServiceName"];
            string endpoint = ConfigurationManager.AppSettings["Endpoint"];
            bool status = false;
            try
            {
                string url = File.ReadAllText(endpoint + "\\" + "URL" + "."+"txt");

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent("{\r\n    \"Document\": \"" + base64 + "\", \r\n    \"Document_Type\": \"PR\",\r\n    \"Proposal_No\": \"" + proposalNum + "\",\r\n    \"Api_User\": \"ECL\",\r\n    \"Api_Key\": \"3(LK3Y\"\r\n}", null, "application/json");
                request.Content = content;
                var response = client.SendAsync(request).Result;
                status = true;
                string result= response.Content.ReadAsStringAsync().Result;
                WriteLog(proposalNum + " || " + fileName, "",result, serviceName, "APICall");
                
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine();
                
            }
            return status;
        }

        public static string GetValueBetween(string input, string startValue, string endValue)
        {
            int startIndex = input.IndexOf(startValue) + startValue.Length;
            int endIndex = input.IndexOf(endValue, startIndex);

            if (startIndex < 0 || endIndex < 0 || endIndex <= startIndex)
            {
                // Values not found or in incorrect order
                return string.Empty;
            }

            return input.Substring(startIndex, endIndex - startIndex);
        }

        public static void WriteLog(string secureId, string request, string response, string servicename, [CallerMemberName] string callerName = "")
        {
            string logFilePath = "C:\\Logs\\" + servicename + "\\";
            logFilePath = logFilePath + "Log-" + DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            try
            {
                using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    FileInfo logFileInfo;



                    logFileInfo = new FileInfo(logFilePath);
                    DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                    if (!logDirInfo.Exists) logDirInfo.Create();



                    StreamWriter log = new StreamWriter(fileStream);



                    if (!logFileInfo.Exists)
                    {
                        _ = logFileInfo.Create();
                    }
                    else
                    {
                        log.WriteLine(secureId);
                        log.WriteLine(DateTime.Now.ToString());  //log.WriteLine(DateTime.UtcNow.ToString());
                        log.WriteLine(request);
                        log.WriteLine(response);
                        log.WriteLine(callerName);
                        log.WriteLine("_________________________________________________________________________________________________________");
                        log.Close();
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception)
            {
            }

            Console.WriteLine(Console.Error);



        }


    }
}
