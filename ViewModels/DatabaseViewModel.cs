using MonitoringSensor.Models;
using MonitoringSensor.Services;
using MonitoringSensor.ViewModels.Command;
using MonitoringSensor.Views.PopView;
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace MonitoringSensor.ViewModels
{
    class DatabaseViewModel : ViewModelBase
    {
        DatabasePopView databasePopView;
        DatabasePopViewModel databasePopViewModel;
        DatabaseModel databaseModel;

        private MySqlConnection connection;
        private string tableName;


        private bool mysqlState;
        public bool MysqlState
        {
            get => mysqlState;
            set => SetProperty(ref mysqlState, value);
        }

        private string createQuery;

        private RelayCommand mysqlCommand;
        public RelayCommand MysqlCommand
        {
            get => mysqlCommand;
            set => SetProperty(ref mysqlCommand, value);
        }

        public DatabaseViewModel()
        {
            MysqlState = false;
            createQuery = "` (`Pk` INT NOT NULL AUTO_INCREMENT, " +
                    "`Time` VARCHAR(45) NULL, " +
                    "`Humidity` VARCHAR(45) NULL, " +
                    "`Temperature` VARCHAR(45) NULL, " +
                    "`PM1_0` VARCHAR(45) NULL, " +
                    "`PM2_5` VARCHAR(45) NULL, " +
                    "`PM10` VARCHAR(45) NULL, " +
                    "`VOC` VARCHAR(45) NULL, " +
                    "`MiCS` VARCHAR(45) NULL, " +
                    "`CJMCU` VARCHAR(45) NULL, " +
                    "`MQ` VARCHAR(45) NULL, " +
                    "`HCHO` VARCHAR(45) NULL, " +
                    "PRIMARY KEY (`Pk`));";

            MysqlCommand = new RelayCommand(OpenDatabase);
            databaseModel = new DatabaseModel();
        }

        public DatabaseViewModel(bool boo)
        {
            MysqlState = false;
            createQuery = "` (`Pk` INT NOT NULL AUTO_INCREMENT, " +
                    "`Time` VARCHAR(45) NULL, " +
                    "`Sensor` VARCHAR(45) NULL, " +
                    "`Humidity` VARCHAR(45) NULL, " +
                    "`Temperature` VARCHAR(45) NULL, " +
                    "`PM1_0` VARCHAR(45) NULL, " +
                    "`PM2_5` VARCHAR(45) NULL, " +
                    "`PM10` VARCHAR(45) NULL, " +
                    "`VOC` VARCHAR(45) NULL, " +
                    "`MiCS` VARCHAR(45) NULL, " +
                    "`CJMCU` VARCHAR(45) NULL, " +
                    "`MQ` VARCHAR(45) NULL, " +
                    "`HCHO` VARCHAR(45) NULL, " +
                    "PRIMARY KEY (`Pk`));";
            MysqlCommand = new RelayCommand(OpenDatabase);
            databaseModel = new DatabaseModel();
        }

        public void OpenDatabase()
        {
            databasePopView = new DatabasePopView();
            databasePopView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            databaseModel.State = false;
            databaseModel.TableName = DateTime.Now.ToString("yyMMdd_HHmm");
            databasePopViewModel = new DatabasePopViewModel(databaseModel, this);
            databasePopView.DataContext = databasePopViewModel;
            databasePopView.ShowDialog();
            if (databaseModel.State == false)
            {
                Close();
                return;
            }

            MysqlState = DatabaseOpen(databaseModel.UserName, databaseModel.Password, databaseModel.Server, databaseModel.DatabaseServer, databaseModel.TableName);
            if (MysqlState)
            {
                MysqlCommand = new RelayCommand(CloseDatabase);
            }
        }

        public void Close()
        {
            databasePopView.Close();
        }

        public void CloseDatabase()
        {
            MysqlState = DatabaseClose();
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(OpenDatabase);
            }
        }


        
        private bool DatabaseOpen(string _userName, string _pw, string _server, string _database, string _tableName)
        {
            try
            {
                string uid = _userName;
                string password = _pw;
                string server = _server;
                string database = _database;
                string defaultTableName = DateTime.Now.ToString("yyMMdd_HHmm");
                tableName = _tableName;
                string connectionString = $"server={server};database={database};uid={uid};password={password};";
                connection = new MySqlConnection(connectionString);
                string createTableQuery = "CREATE TABLE IF NOT EXISTS `" + tableName + createQuery;

                connection.Open();
                MySqlCommand createTableCommand = new MySqlCommand(createTableQuery, connection);
                createTableCommand.ExecuteNonQuery();

                MessageBox.Show(tableName + " Connect !");
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        public void DatabaseAdd(string timer, string data)
        {
            string[] splitData = data.Split('/');

            string insertDataQuery = "INSERT INTO " + tableName + " (Time, Humidity, Temperature, PM1_0, PM2_5, PM10, VOC, MiCS, CJMCU, MQ, HCHO) " +
                        "VALUES (@Time, @Humidity, @Temperature, @PM1_0, @PM2_5, @PM10, @VOC, @MiCS, @CJMCU, @MQ, @HCHO);";
            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);

            insertDataCommand.Parameters.AddWithValue("@Time", timer);
            insertDataCommand.Parameters.AddWithValue("@Humidity", splitData[0]);
            insertDataCommand.Parameters.AddWithValue("@Temperature", splitData[1]);
            insertDataCommand.Parameters.AddWithValue("@PM1_0", splitData[2]);
            insertDataCommand.Parameters.AddWithValue("@PM2_5", splitData[3]);
            insertDataCommand.Parameters.AddWithValue("@PM10", splitData[4]);
            insertDataCommand.Parameters.AddWithValue("@VOC", splitData[5]);
            insertDataCommand.Parameters.AddWithValue("@MiCS", splitData[6]);
            insertDataCommand.Parameters.AddWithValue("@CJMCU", splitData[7]);
            insertDataCommand.Parameters.AddWithValue("@MQ", splitData[8]);
            insertDataCommand.Parameters.AddWithValue("@HCHO", splitData[9]);
            insertDataCommand.ExecuteNonQuery();
        }

        public void DatabaseAdd(string timer, string data, bool boo)
        {
            string[] splitData = data.Split('/');

            string insertDataQuery = "INSERT INTO " + tableName + " (Time, Sensor, Humidity, Temperature, PM1_0, PM2_5, PM10, VOC, MiCS, CJMCU, MQ, HCHO) " +
                        "VALUES (@Time, @Sensor, @Humidity, @Temperature, @PM1_0, @PM2_5, @PM10, @VOC, @MiCS, @CJMCU, @MQ, @HCHO);";
            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);

            insertDataCommand.Parameters.AddWithValue("@Time", timer);
            insertDataCommand.Parameters.AddWithValue("@Sensor", splitData[0]);
            insertDataCommand.Parameters.AddWithValue("@Humidity", splitData[1]);
            insertDataCommand.Parameters.AddWithValue("@Temperature", splitData[2]);
            insertDataCommand.Parameters.AddWithValue("@PM1_0", splitData[3]);
            insertDataCommand.Parameters.AddWithValue("@PM2_5", splitData[4]);
            insertDataCommand.Parameters.AddWithValue("@PM10", splitData[5]);
            insertDataCommand.Parameters.AddWithValue("@VOC", splitData[6]);
            insertDataCommand.Parameters.AddWithValue("@MiCS", splitData[7]);
            insertDataCommand.Parameters.AddWithValue("@CJMCU", splitData[8]);
            insertDataCommand.Parameters.AddWithValue("@MQ", splitData[9]);
            insertDataCommand.Parameters.AddWithValue("@HCHO", splitData[10]);
            insertDataCommand.ExecuteNonQuery();
        }

        private bool DatabaseClose()
        {
            try
            {
                connection.Close();
                MessageBox.Show(tableName + " Disconnect !");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return true;
            }

        }
    }
}
