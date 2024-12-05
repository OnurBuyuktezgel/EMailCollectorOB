// See https://aka.ms/new-console-template for more information

using EMailCollector.POCApp;
using System;
using System.Text;
using System.Threading;

const int NO_ARGUMENT_EXIT_CODE = -1;
const string USER_AGENT_KEY = "User-Agent";
const string USER_AGENT_VALUE = "Mozilla/5.0 (Macintosh; Intel Mac OS X 12_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";

// Get text file name from arg
var path = string.Empty;
string ERROR_MESSAGE_NO_ARGS_TEMPLATE = $"No text file name is given as argument, exiting. ({NO_ARGUMENT_EXIT_CODE})";

if (args.Length != 1)
{
    Console.WriteLine(ERROR_MESSAGE_NO_ARGS_TEMPLATE);
    enterToExit();
    Environment.Exit(NO_ARGUMENT_EXIT_CODE);
}

var fileName = args[0];
Console.WriteLine($"File Name: {fileName}");

List<string> domainErrors = new List<string>();
HashSet<string> emailsFound = new HashSet<string>();

// Get Domain List from a text file 

using (StreamReader sr = File.OpenText(fileName))
{
    string s = String.Empty;
    while ((s = sr.ReadLine()) != null)
    {
        Console.WriteLine($"{s}");

        try
        {
            string content = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(USER_AGENT_KEY, USER_AGENT_VALUE);

                //set timeout to 20 secs
                client.Timeout = TimeSpan.FromSeconds(20);

                content = await client.GetStringAsync(s);
            }

            List<string> emails = Helpers.ExtractEmails(content);

            foreach (var item in emails)
            {
                emailsFound.Add(item);
            }
        }
        catch (HttpRequestException httpRequestException)
        {
            var exMessageText = $"Error getting content for: {s} Error:{httpRequestException.Message}";
            domainErrors.Add(exMessageText);
            Console.WriteLine(exMessageText);
        }
        catch (TaskCanceledException taskCanceledException)
        {
            var exMessageText = $"Error getting content for: {s} Error:{taskCanceledException.Message}";
            domainErrors.Add(exMessageText);
            Console.WriteLine(exMessageText);
        }
        catch (Exception ex)
        {
            var exMessageText = $"Error getting content for: {s} Error:{ex.Message}";
            domainErrors.Add(exMessageText);
            Console.WriteLine(exMessageText);
        }
    }
}

StringBuilder stringBuilder = new StringBuilder();

// add errors
addSeparator(stringBuilder);
var errorMessage = $"Found Error(s): {domainErrors.Count}";
stringBuilder.AppendLine(errorMessage);
Console.WriteLine(errorMessage);

foreach (string item in domainErrors)
{
    stringBuilder.AppendLine(item);
    Console.WriteLine(item);
}

// add found email(s)
addSeparator(stringBuilder);
var emailHeaderMessage = $"Found Email(s): {emailsFound.Count}";
stringBuilder.AppendLine(emailHeaderMessage);
Console.WriteLine(emailHeaderMessage);

foreach (string item in emailsFound)
{
    stringBuilder.AppendLine(item);
    Console.WriteLine(item);
}

var fileContent = stringBuilder.ToString();
var dateText = DateTime.Now.ToString("s").Replace(':','-'); 
var exportFileName = $"Emails_{dateText}.txt";

await File.WriteAllTextAsync(exportFileName, fileContent);

enterToExit();

// methods 
void addSeparator(StringBuilder stringBuilder)
{
    const string separator = "=============================";
    stringBuilder.AppendLine(separator);
}

void enterToExit()
{
    Console.WriteLine("Press enter to exit.");
    Console.ReadLine();
}
