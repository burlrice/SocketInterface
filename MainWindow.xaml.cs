using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketInterface
{
    public class WaitCursor : IDisposable
    {
        private Cursor previous;

        public WaitCursor()
        {
            previous = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = previous;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var remove = new List<string>() 
            {
                Process.GetCurrentProcess().MainModule.ModuleName,
                "Debug", 
                "Release", 
                "bin" 
            };
            var dir = Process.GetCurrentProcess().MainModule.FileName;

            remove.ForEach(s => dir = dir.Replace($"\\{s}", ""));
            InitializeComponent();
            address.Text = "10.1.2.3";
            send.Text = File.ReadAllText($"{dir}\\nestle_zpl_02.txt");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            recv.Text = "";

            using (new WaitCursor())
            {
                try
                {
                    var addr = IPAddress.Parse(address.Text);
                    byte[] bytes = new byte[1024];
                    var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                    socket.Connect(new IPEndPoint(addr, 9100));
                    socket.Send(Encoding.ASCII.GetBytes(send.Text));
                    //var receive = socket.Receive(bytes);
                    //recv.Text = Encoding.ASCII.GetString(bytes, 0, receive);
                }
                catch (Exception ex)
                {
                    recv.Text = ex.Message;
                }
            }
        }
    }
}
