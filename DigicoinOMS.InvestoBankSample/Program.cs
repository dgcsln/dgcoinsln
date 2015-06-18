using System;
using System.Globalization;

using DigicoinOMS.Core.Api.Dto;

namespace DigicoinOMS.InvestoBankSample
{
    class Program
    {
        private const String PositionReport = "ReportClientPositions";
        private const String VolumeReport = "ReportBrokerVolumes";
        private const String RunTestPack = "RunTestPack";
        private const String Quit = "Quit";
        
        static void Main(string[] args)
        {
            var sampleBank = new SampleBank();
            sampleBank.Initialise();
            Console.WriteLine("Welcome to InvestoBank Sample. Available commands are:\n");
            Console.WriteLine("[Buy|Sell],[Client Name],[Quantity]\n{0}\n{1}\n{2}\n{3}", PositionReport, VolumeReport, RunTestPack, Quit);
            while (true)
            {   
                Console.WriteLine();
                string command = Console.ReadLine();

                if (command == null || command.ToLower() == "quit")
                {
                    return;
                }

                if (command.ToLower() == PositionReport.ToLower())
                {
                    Console.WriteLine(sampleBank.ReportInterface.GetClientNetPositionsReport());
                }
                else if (command.ToLower() == VolumeReport.ToLower())
                {
                    Console.WriteLine(sampleBank.ReportInterface.GetBrokerReport());                    
                }
                else if (command.ToLower().StartsWith("buy") || command.ToLower().StartsWith("sell"))
                {
                    try
                    {
                        var orderParams = command.Split(new[] { "," }, StringSplitOptions.None);

                        var order = new OrderRequest()
                                        {
                                            ClientName = orderParams[1].TrimEnd().TrimStart(),
                                            Quantity =
                                                Int32.Parse(orderParams[2].Trim(), NumberStyles.Integer),
                                            Side = orderParams[0].Trim()
                                        };

                        sampleBank.ClientInterface.ProcessClientOrder(order);
                    }
                    catch
                    {
                        Console.WriteLine("Error processing client order command. Please check syntax.");
                    }
                }
                else if (command.ToLower() == RunTestPack.ToLower())
                {
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client A", Quantity = 10, Side = "Buy" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client B", Quantity = 40, Side = "Buy" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client A", Quantity = 50, Side = "Buy" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client B", Quantity = 100, Side = "Buy" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client B", Quantity = 80, Side = "Sell" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client C", Quantity = 70, Side = "Sell" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client A", Quantity = 130, Side = "Buy" });
                    sampleBank.ClientInterface.ProcessClientOrder(
                        new OrderRequest { ClientName = "Client B", Quantity = 60, Side = "Sell" });
                }
                else if (command == "log")
                {
                    foreach (var entry in sampleBank.AuditLogReader.GetLogEntries())
                    {
                        Console.WriteLine(entry);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Unrecognised command");
                }
            }
        }
    }
}
