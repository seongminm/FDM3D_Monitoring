﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MonitoringSensor.Services
{
    class SerialService
    {
        SerialPort serialPort;
        GetDataService getDataService;

        public List<string> SerialPorts { get; set; }
        public List<int> SerialBaudRate { get; set; }

        public SerialService(GetDataService getDataService)
        {
            this.getDataService = getDataService;

            LoadSerialPorts();
            SerialBaudRate = new List<int> { 9600, 14400, 19200, 38400, 57600, 115200 };
        }

        public  void LoadSerialPorts()
        {
            var ports = new List<string>(SerialPort.GetPortNames());
            SerialPorts = new List<string>(ports);
        }

        public void OpenSerial(string portName, string baudRate)
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = int.Parse(baudRate);
            serialPort.DataReceived += SerialPort_DataReceived;
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        public void CloseSerial()
        {
            serialPort.DataReceived -= SerialPort_DataReceived;
            serialPort.DiscardInBuffer();
            try
            {
                serialPort.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public bool isOpen()
        {
            bool state = serialPort.IsOpen;
            return state;
        }

        public void SendSerial(string message)
        {
            serialPort.WriteLine(message);
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            getDataService.StringData = serialPort.ReadLine();
        }
    }
}
