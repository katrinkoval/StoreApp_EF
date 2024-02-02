using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StoreAppTest
{
    static class StoreDataBaseCopier
    {
        const string FOLDER_PATH = "C:\\Users\\Katerina\\Documents\\SQL Server Management Studio\\StoreTestDB"; //TODO: ".\\StoreTestDB", clone from git

        #region === Backup database ===
        //public static void CreateStoreDbCopy(DbContext dBContext, string copyDbname)
        //{
        //    string queryStr = String.Format("BACKUP DATABASE Store TO DISK = 'D:\\TestStoreDB\\{0}.bak'"
        //        + " RESTORE DATABASE {0} FROM DISK = 'D:\\TestStoreDB\\{0}.bak' "
        //        + "WITH"
        //        + " MOVE 'Store' TO 'D:\\TestStoreDB\\{0}.mdf',"
        //        + " MOVE 'Store_log' TO 'D:\\TestStoreDB\\{0}.ldf', "
        //        + " REPLACE", copyDbname);

        //    dBContext.Database.ExecuteSqlCommand(queryStr);
        //}

        //public static void RemoveStoreDbCopy(DbContext dBContext, string copyDbname)
        //{
        //    string queryStr = String.Format("DROP DATABASE {0}", copyDbname);

        //    dBContext.Database.ExecuteSqlCommand(queryStr);
        //}

        #endregion

        public static void CreateStoreDbCopy()
        {
            string[] fileNames = Directory.GetFiles(FOLDER_PATH + "\\CreateTestStoreDB", "*.sql");   

            foreach (string filename in fileNames)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process.StartInfo.FileName = "sqlcmd.exe";

                    process.StartInfo.Arguments = String.Format("-S localhost\\sqlexpress -i \"{0}\" \n", filename);

                    process.EnableRaisingEvents = true;


                    process.Start();
                    process.WaitForExit(2500);

                }

            }

        }

        //public static void CreateStoreDbCopy()
        //{
        //    string[] fileNames = Directory.GetFiles(FOLDER_PATH + "\\CreateTestStoreDB");

        //    using (SqlConnection conn = new SqlConnection("Data Source = localhost\\sqlexpress; Initial Catalog = Store; Integrated Security = True"))
        //    {
        //        conn.Open();
        //        foreach (string filename in fileNames)
        //        {
        //            FileInfo file = new FileInfo(filename);
        //            string script = file.OpenText().ReadToEnd();

        //            SqlCommand cmd = new SqlCommand(script, conn);
        //            cmd.ExecuteNonQuery();

        //        }
        //        conn.Close();
        //    }
        //}

        public static void RemoveStoreDbCopy()
        {
            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.StartInfo.FileName = "cmd.exe";


            process.StartInfo.Arguments = String.Format("sqlcmd -S localhost\\sqlexpress -i \"{0}\\DropTestStoreDB.sql\"", FOLDER_PATH);
            process.Start();

        }
    }

}