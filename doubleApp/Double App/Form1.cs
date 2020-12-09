using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Double_App
{
    public partial class Form1 : Form
    {
        private static String app1 = "";
        private static String app2 = "";

        private static Process proc1;
        private static Process proc2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public Form1()
        {
            InitializeComponent();

            // reading the config file to get the applications
            ReadConfiguration("config.opena");

            ProcessStartInfo info1 = new ProcessStartInfo(app1);
            info1.WindowStyle = ProcessWindowStyle.Maximized;
            proc1 = Process.Start(info1);
            proc1.WaitForInputIdle();
            while (proc1.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(100);
                proc1.Refresh();
            }
            SetParent(proc1.MainWindowHandle, splitContainer1.Panel1.Handle);
            MoveWindow(proc1.MainWindowHandle, 0, 0, splitContainer1.Panel1.Width, splitContainer1.Panel1.Height, true);

            ProcessStartInfo info2 = new ProcessStartInfo(app2);
            info2.WindowStyle = ProcessWindowStyle.Maximized;
            proc2 = Process.Start(info2);
            proc2.WaitForInputIdle();
            while (proc2.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(100);
                proc2.Refresh();
            }
            SetParent(proc2.MainWindowHandle, splitContainer1.Panel2.Handle);
            MoveWindow(proc2.MainWindowHandle, 0, 0, splitContainer1.Panel2.Width, splitContainer1.Panel2.Height, true);
        }

        private void ReadConfiguration(String configFile)
        {
            // todo: check the file existance and number of application
            StreamReader config = new StreamReader("config");

            app1 = config.ReadLine();
            app2 = config.ReadLine();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            MoveWindow(proc1.MainWindowHandle, 0, 0, splitContainer1.Panel1.Width, splitContainer1.Panel1.Height, true);
            MoveWindow(proc2.MainWindowHandle, 0, 0, splitContainer1.Panel2.Width, splitContainer1.Panel2.Height, true);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            MoveWindow(proc1.MainWindowHandle, 0, 0, splitContainer1.Panel1.Width, splitContainer1.Panel1.Height, true);
            MoveWindow(proc2.MainWindowHandle, 0, 0, splitContainer1.Panel2.Width, splitContainer1.Panel2.Height, true);
        }
    }
}
