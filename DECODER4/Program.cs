using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============================");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Base64 Decoder");
            Console.WriteLine("2. ASCII Decoder (Octal)");
            Console.WriteLine("3. ASCII Decoder (Hexadecimal)");
            Console.WriteLine("4. ASCII Decoder (Decimal)");
            Console.WriteLine("============================");
            int option = Convert.ToInt32(Console.ReadLine());

            string input, output = "";
            switch (option)
            {
                case 1:
                    Console.WriteLine("Please enter the Base64-encoded input:");
                    input = Console.ReadLine();
                    byte[] decodedBytes = Convert.FromBase64String(input);
                    output = Encoding.UTF8.GetString(decodedBytes);
                    break;

                case 2:
                    Console.WriteLine("Please enter the ASCII-encoded input (Octal format):");
                    input = Console.ReadLine();
                    string[] bytes1 = input.Split('\\');
                    foreach (string byteString in bytes1)
                    {
                        if (byteString.Length > 0 && byteString.All(char.IsDigit))
                        {
                            int byteValue = Convert.ToInt32(byteString, 8);
                            char decodedChar = (char)byteValue;
                            output += decodedChar;
                        }
                    }
                    break;

                case 3:
                    Console.WriteLine("Please enter the ASCII-encoded input (Hexadecimal format):");
                    input = Console.ReadLine();
                    string[] bytes2 = input.Split('\\');
                    foreach (string byteString in bytes2)
                    {
                        if (byteString.Length > 0 && byteString.All(char.IsLetterOrDigit))
                        {
                            int byteValue = Convert.ToInt32(byteString, 16);
                            char decodedChar = (char)byteValue;
                            output += decodedChar;
                        }
                    }
                    break;

                case 4:
                    Console.WriteLine("Please enter the ASCII-encoded input (Decimal format):");
                    input = Console.ReadLine();
                    string[] bytes3 = input.Split(',');
                    foreach (string byteString in bytes3)
                    {
                        if (byteString.Length > 0 && byteString.All(char.IsDigit))
                        {
                            int byteValue = Convert.ToInt32(byteString);
                            char decodedChar = (char)byteValue;
                            output += decodedChar.ToString();
                        }
                        else if (byteString.Length > 0)
                        {
                            string[] byteSplit = byteString.Split(' ');
                            foreach (string subByteString in byteSplit)
                            {
                                if (subByteString.Length > 0 && subByteString.All(char.IsDigit))
                                {
                                    int subByteValue = Convert.ToInt32(subByteString);
                                    char decodedSubChar = (char)subByteValue;
                                    output += decodedSubChar.ToString();
                                }
                            }
                        }
                    }
                    break;

            }

            Console.WriteLine("Decoded output:");
            Console.WriteLine(output);

            Console.WriteLine("Would you like to save the output as a .txt file? (y/n)");
            string saveOutput = Console.ReadLine();
            if (saveOutput == "y")
            {
                Console.WriteLine("Please enter the file name:");
                string fileName = Console.ReadLine();
                File.WriteAllText(fileName + ".txt", output);
                Console.WriteLine("File saved.");

                Console.WriteLine("Please enter the webhook URL:");
                string webhookUrl = Console.ReadLine();

                // Create a WebRequest object to send the webhook request
                WebRequest request = WebRequest.Create(webhookUrl);
                request.Method = "POST";
                var payload = new NameValueCollection();
                payload["content"] = "Here's your decoded output: " + output;

                using (var stream = request.GetRequestStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(payload.ToString());
                    stream.Write(bytes, 0, bytes.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Webhook sent successfully.");
                }
                else
                {
                    Console.WriteLine("Error sending webhook: " + response.StatusDescription);
                }
            }

            Console.WriteLine("Would you like to save the output as a .txt file? (y/n)");
            string saveOption2 = Console.ReadLine();
            if (saveOption2 == "y")
            {
                Console.WriteLine("Please enter the file name:");
                string fileName = Console.ReadLine();
                File.WriteAllText(fileName + ".txt", output);
                Console.WriteLine("File saved.");

                Console.WriteLine("Would you like to send the file to a webhook? (y/n)");
                string webhookOption = Console.ReadLine();
                if (webhookOption == "y")
                {
                    Console.WriteLine("Please enter the webhook URL:");
                    string webhookUrl = Console.ReadLine();

                    using (var client = new WebClient())
                    {
                        client.UploadFile(webhookUrl, fileName + ".txt");
                    }

                    Console.WriteLine("File sent to webhook successfully.");
                }
                else
                {
                    Console.WriteLine("Exiting program");
                }
            }
            else
            {
                Console.WriteLine("Exiting program");
            }
        }
    }
}



