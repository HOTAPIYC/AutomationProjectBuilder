using AutomationProjectBuilder.Data;
using AutomationProjectBuilder.Export;
using AutomationProjectBuilder.Gui;
using System;

namespace AutomationProjectBuilder
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Automation Project Builder version 0.0.1.\nDon't close this window while the application is running!");

            var dataService = new DataService();

            var plcExport = new PlcExport(dataService);
            var userinterface = new UserInterface(dataService);
        }
    }
}
