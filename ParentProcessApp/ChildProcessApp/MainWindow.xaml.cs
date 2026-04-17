using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChildProcessApp
{
    public partial class MainWindow : Window
    {
        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => ListenForCoordinates());
        }

        private void ListenForCoordinates()
        {
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("CoordMemoryMap"))
                using (EventWaitHandle waitHandle = EventWaitHandle.OpenExisting("CoordClickEvent"))
                {
                    while (true)
                    {
                        waitHandle.WaitOne();

                        int x, y;
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                        {
                            BinaryReader reader = new BinaryReader(stream);
                            x = reader.ReadInt32();
                            y = reader.ReadInt32();
                        }

                        Dispatcher.Invoke(() => DrawRectangle(x, y));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DrawRectangle(int x, int y)
        {
            int width = rand.Next(30, 100);
            int height = rand.Next(30, 100);

            Rectangle rect = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256))),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rect, x - width / 2);
            Canvas.SetTop(rect, y - height / 2);

            DrawingCanvas.Children.Add(rect);
        }
    }
}