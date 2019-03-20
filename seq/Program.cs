using Seq.Api;
using Seq.Api.Client;
using Seq.Api.Model.Inputs;
using Seq.Api.Model.Signals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace seq
{
    internal class Program
    {
        private static Dictionary<string, string> _envKV = new Dictionary<string, string> {
            { "qa", "QA" },
            { "sandbox", "Sandbox" }
        };

        private static string[] _ieArray = { "internal", "external", "recon" };
        private static string _seqConnectionString = "http://seq.ckotech.co";
        private static SeqConnection _seqConnection;
        private static string _productName = "APPLICATION_NAME_HERE_PLEASE";
        private static bool applyChanges = false;

        private static async Task Main(string[] args)
        {
            applyChanges = args.Length > 0 && args[0].Equals("apply");
            DryRunStatus();
            BeautifulLogger.Input("Enter Product Name: ");
            _productName = Console.ReadLine();
            await CredentialsLoop();

            foreach (string _env in _envKV.Keys)
            {
                foreach (string _ie in _ieArray)
                {
                    string outName = $"{_env}-{_productName}-{_ie}";
                    BeautifulLogger.Info($"Attempting to create key for [{outName}]");
                    //await CreateApiKey(outName, _env);
                    BeautifulLogger.Info($"Attempting to create signal for [{outName}]");
                    //await CreateSignal(outName);
                }
            }
            BeautifulLogger.Info($"All done, bye bye");
        }

        public static async Task CredentialsLoop()
        {
            try
            {
                BeautifulLogger.Input("Enter Paswword for SEQ:");
                string password = GetMaskedConsoleInput();

                BeautifulLogger.Info("Authenticating...");
                await Login("admin", password);
            }
            catch (SeqApiException)
            {
                BeautifulLogger.Err("Invalid Password.");
                await CredentialsLoop();
            }
        }

        public static async Task Login(string username, string password)
        {
            _seqConnection = new SeqConnection(_seqConnectionString);
            await _seqConnection.Users.LoginAsync(username, password);
        }

        public static async Task CreateSignal(string signalName)
        {
            SignalEntity signal = await _seqConnection.Signals.TemplateAsync();
            signal.Title = signalName;

            if (applyChanges)
            {
                await _seqConnection.Signals.AddAsync(signal);
            }
        }

        public static async Task DeleteSignal()
        {
            SignalEntity signal = await _seqConnection.Signals.FindAsync("signal-XXXX");
            await _seqConnection.Signals.RemoveAsync(signal);
        }

        public static async Task CreateApiKey(string keyName, string env)
        {
            ApiKeyEntity templateKey = await _seqConnection.ApiKeys.TemplateAsync();

            templateKey.Title = keyName;

            InputAppliedPropertyPart _property = new InputAppliedPropertyPart();
            _property.Name = "Environment";
            _property.Value = _envKV.GetValueOrDefault(env);
            templateKey.AppliedProperties.Add(_property);

            if (applyChanges)
            {
                await _seqConnection.ApiKeys.AddAsync(templateKey);
            }
        }

        public static void DryRunStatus()
        {
            if (!applyChanges)
            {
                BeautifulLogger.Info("This is a dry run!   Add argument 'apply' after command call to persist.");
            }
            else
            {
                BeautifulLogger.Warn("Running on apply changes mode. Changes will be persisted!");
            }
        }

        public static string GetMaskedConsoleInput()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                // exit
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                // erase
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b \b");
                        sb.Length--;
                    }
                }
                // replace
                else if (i.KeyChar != '\u0000')
                {
                    sb.Append(i.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return sb.ToString();
        }
    }
}