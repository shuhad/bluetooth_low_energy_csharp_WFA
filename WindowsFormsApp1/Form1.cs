using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        SerialPort mySerialPort = new SerialPort("COM8", 57600, Parity.None, 8, StopBits.One);
        public Form1()
        {
            InitializeComponent();            
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(mySerialPort_DataReceived);
            mySerialPort.Open();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            lbl_test.Text = "Connected";
        }
        
        //print response from the dongle
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string s = sp.ReadExisting();
            output_data.Invoke(new EventHandler(delegate { output_data.Text +=  s + "\r\n"; })); 

            //lbl_output.Invoke(this.myDelegate, new Object[] { s });
        }


        private void tb_cmd_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void submit_cmd_Click(object sender, EventArgs e)
        {
            // BleuIO firmware version above v2.1.2 doesnot need to write enter seperately
            // Therefore concating would work
            // ex.  bytes = bytes.Concat(inputByte).ToArray();
   
            var enterCMD = new byte[] { 13 };
            byte[] bytes = Encoding.UTF8.GetBytes(tb_cmd.Text);
            bytes = bytes.ToArray();
            mySerialPort.Write(bytes, 0, bytes.Length);

            // BleuIO firmware version below v2.1.2 needs to write enter seperately
            // Following lines are for seperate enter command

            Thread.Sleep(500);
            enterCMD = enterCMD.ToArray();
            mySerialPort.Write(enterCMD, 0, enterCMD.Length);


        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            mySerialPort.Close();
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("\u0003");
            var inputByte = new byte[] { 13 };
            bytes = bytes.Concat(inputByte).ToArray();
            mySerialPort.Write(bytes, 0, bytes.Length);
        }

        private void output_data_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
