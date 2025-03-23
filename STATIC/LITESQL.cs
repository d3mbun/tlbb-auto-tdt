//using System;
//using System.Collections.Generic;
//using System.Data;

//namespace MicroAuto
//{
//    class LITESQL
//    {
//        public LITESQL()
//        {
//            //Connect = new SQLiteConnection("Data Source=:memory:");
//            //Connect.Open();
//        }

//        //SQLiteConnection Connect;

//        //public string Load(string path)
//        //{
//        //    //try
//        //    //{
//        //    //    SQLiteConnection source = new SQLiteConnection("Data Source=" + path);
//        //    //    source.Open();
//        //    //    Connect = new SQLiteConnection("Data Source=:memory:");
//        //    //    Connect.Open();
//        //    //    source.BackupDatabase(Connect, "main", "main", -1, null, 0);
//        //    //    source.Close();
//        //    //}
//        //    //catch (Exception ex)
//        //    //{
//        //    //    return ex.Message;
//        //    //}
//        //    //return "";
//        //}
        

//        public string Save(string path)
//        {
//            //try
//            //{
//            //    SQLiteConnection des = new SQLiteConnection("Data Source=" + path);
//            //    des.Open();
//            //    Connect.BackupDatabase(des, "main", "main", -1, null, 0);
//            //    des.Close();
//            //}
//            //catch(Exception ex)
//            //{
//            //    return ex.Message;
//            //}
//            //return "";
//        }

//        public string Query(string query)
//        {
//            try
//            {
//                using (SQLiteCommand command = Connect.CreateCommand())
//                {
//                    command.CommandText = query;
//                    command.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//            return "";
//        }

//        public List<SQLiteDataReader> Select(string query)
//        {
//            List<SQLiteDataReader> list = new List<SQLiteDataReader>();
//            try
//            {
              
//                using (SQLiteCommand fmd = Connect.CreateCommand())
//                {
//                    fmd.CommandText = query;
//                    fmd.CommandType = CommandType.Text;
//                    SQLiteDataReader r = fmd.ExecuteReader();
//                    while (r.Read())
//                    {
//                        list.Add(r);
//                        //Main.PushLog(r[0]);
//                    }
//                }
           
//            }
//            catch { }
//            return list;
//        }
//    }
//}
