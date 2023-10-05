using Microsoft.Win32;
using MonitoringSensor.Services;
using MonitoringSensor.ViewModels.Command;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace MonitoringSensor.ViewModels
{
    class CsvViewModel : ViewModelBase, ICsvService
    {

        private string csvName;
        private string csvFilePath; // CSV 파일 경로
        private StreamWriter writer; // CSV 파일 작성자

        private bool csvState;
        public bool CsvState
        {
            get => csvState;
            set => SetProperty(ref csvState, value);
        
        }

        private string line;

        private RelayCommand csvCommand;
        public RelayCommand CsvCommand
        {
            get => csvCommand;
            set => SetProperty(ref csvCommand, value);
            
        }

        public CsvViewModel(string line)
        {
            this.line = line;
            CsvCommand = new RelayCommand(Open);
            CsvState = false;
        }

        public void Open()
        {
            if (CsvState = CsvCreate(line))
            {
                CsvCommand = new RelayCommand(Close);
            }

        }
        public void Close()
        {
            CsvState = CsvClose();
            CsvCommand = new RelayCommand(Open);
        }
       


        // 인터페이스 구현
        public bool CsvCreate(string line)
        {
            string currentDate = DateTime.Now.ToString("yyMMdd_HHmm");

            var dialog = new SaveFileDialog
            {
                FileName = currentDate,
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                AddExtension = true
            };

            if (dialog.ShowDialog() == true)
            {
                // 선택한 경로로 CSV 파일을 저장합니다.
                csvFilePath = dialog.FileName;

                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                writer.WriteLine(line);
                writer.Close();
                MessageBox.Show("CSV file saved successfully.");
                return true;
            }

            return false;
        }

        public void CsvAdd(string timer, string data)
        {
            writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
            string[] splitData = data.Split('/');
            string result = timer;
            for (int i = 0; i < splitData.Length; i++)
            {
                result += "," + splitData[i];
            }
            writer.WriteLine(result);
            writer.Close();
        }

        public bool CsvClose()
        {
            MessageBox.Show(csvName + " Disconnect !");
            return false;
        }
    }
}
