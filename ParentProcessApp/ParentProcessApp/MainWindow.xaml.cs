using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ParentProcessApp
{
    public partial class MainWindow : Window
    {
        private MemoryMappedFile mmf;
        private EventWaitHandle waitHandle;
        private Process childProcess;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Батьківський процес (Натискайте тут)";

            this.MouseDown += MainWindow_MouseDown;
            this.Closing += MainWindow_Closing;

            try
            {
                mmf = MemoryMappedFile.CreateNew("CoordMemoryMap", 8);

                waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "CoordClickEvent");

                childProcess = new Process();
                childProcess.StartInfo.FileName = @"C:\Users\makov\Desktop\ParentProcessApp\ChildProcessApp\bin\Debug\net8.0-windows\ChildProcessApp.exe";

                childProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка ініціалізації: " + ex.Message);
            }
        }
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            int x = (int)point.X;
            int y = (int)point.Y;

            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(x);
                writer.Write(y);
            }

            waitHandle.Set();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (childProcess != null && !childProcess.HasExited)
            {
                childProcess.Kill();
            }
            mmf?.Dispose();
            waitHandle?.Dispose();
        }
    }
}