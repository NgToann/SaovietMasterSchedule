using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using MasterSchedule.Views;
using System.Reflection;

using MasterSchedule.Models;
using MasterSchedule.Helpers;
namespace MasterSchedule
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex _m;
        private App()
        {
            InitializeComponent();
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }
        [STAThread]
        private static void Main()
        {
            var singleInstance = AppSettingsHelper.ReadSetting("SingleInstance").ToUpper().ToString().Equals("3TX") == true ? false : true;
            if (singleInstance == true)
            {
                try
                {
                    Mutex.OpenExisting("MasterSchedule");
                    MessageBox.Show("Application Running...", "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                catch
                {
                    App._m = new Mutex(true, "MasterSchedule");
                    App app = new App();
                    app.Run(new LoginWindow());
                    //app.Run(new ChartScheduleWindow());
                    _m.ReleaseMutex();
                }
            }
            else
            {
                App._m = new Mutex(true, "MasterSchedule");
                App app = new App();
                app.Run(new LoginWindow());
                //app.Run(new ChartScheduleWindow());
                _m.ReleaseMutex();
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            // Get the connectionStrings section. 
            ConfigurationSection configSection = config.GetSection("connectionStrings");
            //Ensures that the section is not already protected.
            if (configSection.SectionInformation.IsProtected == false)
            {
                //Uses the Windows Data Protection API (DPAPI) to encrypt the configuration section using a machine-specific secret key.
                configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }

            // disable encrypt the connection string
            //var configTemp = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            //var configCurrentFile = config.GetSection("connectionStrings");
            //if (configCurrentFile.SectionInformation.IsProtected == false)
            //{
            //    configCurrentFile.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            //    configTemp.Save();
            //}
        }
        public void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(String.Format("An unhandled exception just occurred: {0} !", e.Exception.Message), "Master Schedule System", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
