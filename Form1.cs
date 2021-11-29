using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionProtocol;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace ESP32ConGUI
{
    public partial class Form1 : Form
    {
        int[] data = new int[3];
        ushort COMid = 5;
        ESP32Connect ESP32;
        Draw dr = new Draw();

        int xShape = 3000;
        int yShape = 3000;

        int xShape_z = 10;
        int yShape_z = 10;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeESP32(ushort _COMid)
        {
            ESP32 = new ESP32Connect(_COMid, 115200);

        }

        private async void GetData()
        {

            await Task.Run(() =>
            {
                while (true)
                {
                    data = ESP32.Get3Digits();
                    for (int i = 0; i < 3; i++)
                        Console.WriteLine(data[i].ToString());
                    Console.WriteLine('\n');
                    ESP32Dialog.Invoke(new Action(() =>
                    {
                        ESP32Dialog.Text = data[0].ToString() + " " + data[1].ToString() + " " + data[2].ToString();

                    }));
                }
            });


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            chart2.ChartAreas[0].AxisX.Minimum = -xShape;
            chart2.ChartAreas[0].AxisX.Maximum = xShape;
            chart2.ChartAreas[0].AxisY.Minimum = -yShape;
            chart2.ChartAreas[0].AxisY.Maximum = yShape;

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = xShape_z;
            chart1.ChartAreas[0].AxisY.Minimum = -yShape_z;
            chart1.ChartAreas[0].AxisY.Maximum = yShape_z;


        }


        private async void ShowGraph()
        {
            var random = new Random();
            await Task.Run(() =>
            {
                while (true)
                {
                    if (ESP32.PackageIsEmpty()) continue;
                    chart2.Invoke(new Action(() =>
                    {
                        dr.DrawCircle(chart2, data[0] - 2650, data[1] - 2650, 60, 30);
                    }
                    ));
                    chart1.Invoke(new Action(() =>
                    {
                        dr.DrawLine(chart1, data[2] + random.Next(-5, 5) ,xShape_z);
                    }));
                    Thread.Sleep(50);

                }
            });


        }

        private void Connect_Click(object sender, EventArgs e)
        {
            InitializeESP32(COMid);
            ESP32.EstablishConnection();
            GetData();
            ShowGraph();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "COM3") COMid = 3;
            if (comboBox1.SelectedItem.ToString() == "COM4") COMid = 4;
            if (comboBox1.SelectedItem.ToString() == "COM5") COMid = 5;

        }
    }
}
