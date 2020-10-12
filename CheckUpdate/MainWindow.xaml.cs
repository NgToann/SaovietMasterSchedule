using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System;

namespace CheckUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bwLoad;
        int timeSleep = 1500;
        bool canOpen = true;
        public MainWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                bwLoad.RunWorkerAsync();
            }
        }
        string filePath = "";
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            filePath = @"MasterSchedule.exe";
            string checkPath = @"\\10.2.1.228\QC\SV-Master\Master Schedule\MasterSchedule.exe";

            string reportsPath = @"Reports";
            string checkReportsPath = @"\\10.2.1.228\QC\SV-Master\Master Schedule\Reports";

            Dispatcher.Invoke(new Action(() => {
                prgStatus.Maximum = 7;
                lblStatus.Content = "Checking application file ...";
                prgStatus.Value = 1;
            }));

            // Check Application Client
            Thread.Sleep(timeSleep);
            lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Checking MasterSchedule Client..."));
            if (File.Exists(filePath) == false)
            {
                MessageBox.Show(String.Format("{0} not found !", filePath), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                canOpen = false;
                return;
            }

            // Check Application Server
            prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 2));
            lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Checking MasterSchedule at server..."));
            Thread.Sleep(timeSleep);
            if (File.Exists(checkPath) == false)
            {
                checkPath = @"\\SV-QC\QC\SV-Master\Master Schedule\MasterSchedule.exe"; 
                checkReportsPath = @"\\SV-QC\QC\SV-Master\Master Schedule\Reports";
                if (File.Exists(checkPath) == false)
                {
                    MessageBox.Show(String.Format("{0} not found at server !", filePath), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
            FileVersionInfo fviUpdate = FileVersionInfo.GetVersionInfo(checkPath);

            if (fvi.ProductVersion.CompareTo(fviUpdate.ProductVersion) < 0)
            {
                txtResult.Dispatcher.Invoke((Action)(() => txtResult.Text = String.Format("New version\n{0}", fviUpdate.FileVersion.ToString())));
                // Copy App
                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 3));
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Downloading application..."));
                Thread.Sleep(timeSleep);
                try
                {
                    Mutex.OpenExisting("MasterSchedule");
                    MessageBox.Show("MasterSchedule Running... Exit & Try Again!", "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Stop);
                    canOpen = false;
                    return;
                }
                catch
                {
                    File.Copy(checkPath, filePath, true);
                }

                // Checking reports
                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 4));
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Checking report files..."));
                Thread.Sleep(timeSleep);

                //if (File.Exists(reportsPath) == false)
                if (Directory.Exists(reportsPath) == false)
                {
                    if (MessageBox.Show(String.Format("{0} folder not found !\n\nDo you want to create Reports folder?", reportsPath), "Master Schedule", MessageBoxButton.OKCancel, MessageBoxImage.Error) != MessageBoxResult.OK)
                    {
                        return;
                    }
                    Directory.CreateDirectory(reportsPath);
                }

                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 5));
                Thread.Sleep(timeSleep);
                //if (File.Exists(checkReportsPath) == false)

                if (Directory.Exists(checkReportsPath) == false)
                {
                    MessageBox.Show(String.Format("{0} folder not found at server !", reportsPath), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Copy reports
                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 6));
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Downloading reports..."));
                Thread.Sleep(timeSleep);
                string reportName = "";
                string[] reportFiles = Directory.GetFiles(checkReportsPath);
                foreach (string s in reportFiles)
                {
                    try
                    {
                        reportName = System.IO.Path.GetFileName(s);
                        lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = String.Format("Downloading: {0}...", reportName)));
                        File.Copy(s, reportsPath + "/" + reportName, true);
                        Thread.Sleep(150);
                    }
                    catch (System.IO.IOException io)
                    {
                        MessageBox.Show(io.ToString(), "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 7));
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Running Master Schedule !"));
                Thread.Sleep(3000);
            }

            else
            {
                prgStatus.Dispatcher.Invoke((Action)(() => prgStatus.Value = 7));
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Not found new version !"));
                Thread.Sleep(3000);
                lblStatus.Dispatcher.Invoke((Action)(() => lblStatus.Content = "Running Master Schedule !"));

                try
                {
                    Mutex.OpenExisting("MasterSchedule");
                    MessageBox.Show("MasterSchedule Running... Exit & Try Again!", "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Stop);
                    canOpen = false;
                    return;
                }
                catch
                {
                }
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (canOpen == true)
            {
                Process.Start(filePath);
            }
            this.Close();
        }
    }
}
