using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace MonitoringSensor.Services
{
    class CsvService
    {
        private string csvName;
        private string csvFilePath; // CSV 파일 경로
        private StreamWriter writer; // CSV 파일 작성자


        public bool CreateCsv(string line)
        {
            string defaultCsvName = DateTime.Now.ToString("yyMMdd_HHmm");
            csvName = Interaction.InputBox("저장할 csv파일의 이름을 입력하세요:", "사용할 csv파일 이름 입력", defaultCsvName);
            if (csvName == "")
            {
                return false;
            }
            else
            {
                string projectPath = Directory.GetCurrentDirectory();
                csvFilePath = System.IO.Path.Combine(projectPath, csvName + ".csv");
                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                writer.WriteLine(line);
                writer.Close();
                MessageBox.Show(csvName + ".csv 저장 완료");
                return true;

            }
        }

        public void AddCsv(string timer, string data)
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

        public bool CloseCsv()
        {
            MessageBox.Show(csvName + " Disconnect !");
            return false;
        }
    }
}
