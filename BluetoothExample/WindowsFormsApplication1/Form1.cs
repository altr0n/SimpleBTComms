using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Msft;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.IrDA;
using InTheHand.Net.Mime;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using InTheHand.Net;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        BluetoothClient client = new BluetoothClient();
        SelectBluetoothDeviceDialog dlg = new SelectBluetoothDeviceDialog();
        Timer Listener = new Timer();
        Timer timer = new Timer();
        Timer UITimer = new Timer();
        String DeviceID;
        bool writing = false;
        public static Stream testStream;   
        bool logging = false;
        byte[] IncomingMessage = new Byte[0x1024];
        int logCounter = 0; // Amount of variables that we wish to log

        public Form1()
        {         
            InitializeComponent();          
            Listener.Tick += new EventHandler(ListenerTick);
            Listener.Interval = 100; 
            timer.Tick += new EventHandler(timer_Tick);
            UITimer.Interval = 5;            
            Listener.Start();
            timer.Start();
            timer.Interval = 250;
        }

        String NewMessage;
       
        public void ListenerTick(object sender, EventArgs e)
        {
            try
            {
                
                NewMessage += testStream.ReadByte().ToString();

            }
            catch (Exception exc)
            {

            }
            
        }
        private void connectToolStripMenuItem_Click(object sender, EventArgs e) // Connect To A device//
        {
            DialogResult result = dlg.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return;
            }
            BluetoothDeviceInfo device = dlg.SelectedDevice;
            output.Text += "Device = " + device.DeviceName.ToString() + "   " + device.ClassOfDevice.ToString() + '\r';
            DeviceID = device.DeviceName.ToString();
            BluetoothAddress addr = device.DeviceAddress;
            output.Text += "Address: " + addr.ToString() + '\r';
            Guid serviceClass = BluetoothService.SerialPort;

            try
            {
                client.Connect(new BluetoothEndPoint(addr, serviceClass));
                output.Text += "Connected!" + '\r';
                toolStripStatusLabel1.Text = "Connected!";
            }
            catch (Exception exc)
            {
                output.Text += "Exception Occured " + exc + '\r';
            }

            // Testing for connection!//
            try
            {
                output.Text += "Testing Connection..." + '\r';
                if (client.Connected)
                {
                    output.Text += "Connected Succesfully" + '\r';
                }
                else
                {
                    output.Text += "Nope, try again!" + '\r';
                }
            }
            catch (Exception exc1)
            {
                output.Text += "Exception Occured: " + exc1 + '\r';
            }
            try{
                testStream = client.GetStream();
            
              
            }catch(Exception excs)
            {
                output.Text += "Could not open stream!";
            }

        }

        private void pairToolStripMenuItem_Click(object sender, EventArgs e) // Pair a device (First Time Usage) pin for BT module is 1234
        {
            try
            {
                Process p = Process.Start("C:\\Windows\\System32\\DevicePairingWizard.exe");

            }
            catch (Exception ee)
            {
                output.Text += "Exception Occured: " + ee;
            }
        }

        private void stopConnectionToolStripMenuItem_Click(object sender, EventArgs e) // Stop the BT Connection
        {
            try
            {
                client.Close();
            }
            catch (Exception closeExc)
            {
                MessageBox.Show("Exception Occured --> " + closeExc);
            }
        }

        private void refreshClearAllValuesToolStripMenuItem_Click(object sender, EventArgs e) // Stop connection and clear values completely
        {
            output.Clear();
        }


        public void send(byte[] command)
        {


            try
            {

                testStream.Write(command, 0, command.Length); 
                int t = 0;
                foreach (byte element in command)
                {
                    output.Text += "Byte " + t + ": " + element.ToString("X2") + '\r';
                    t++;
                }
            }
            catch (Exception exc4)
            {
                output.Text += "Sending using the stream -->  Exc: " + exc4 + '\r';
            }

        }

        void timer_Tick(object sender, EventArgs e) // Here we display
        {
            try
            {
                rec.Text += DeviceID + NewMessage.ToString() + "\r";
            }
            catch (NullReferenceException esc)
            {

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte[] command = Encoding.ASCII.GetBytes(textBox1.Text);
            send(command);
        }






    

  

     

  

       
       

       
    }
}
