using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _i
{
    public partial class SQLite : Form
    {
        public SQLite()
        {
            InitializeComponent();
        }

        public void createConection()
        {
        
        }

        public void closeConnection()
        {
        }

        // Create your POCO class
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string[] Phones { get; set; }
            public bool IsActive { get; set; }
        }



private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //SQLiteConnection source = new SQLiteConnection("Data Source=c:\\test.db");
                //source.Open();
                //// Get customer collection
                //var col = db.GetCollection("MAP");

                //GAMEDIC.MapNameId.ToList().ForEach(kvp =>
                //{
                //    BsonDocument b = new BsonDocument() { { "ID", kvp.Key }, { "NAME", kvp.Value } };
                //    col.Insert(b);
                //});


                //List<string> ImportedFiles = new List<string>();
                //using (SQLiteConnection connect = new SQLiteConnection(@"Data Source=nier.db"))
                //{
                //    connect.Open();
                //    using (SQLiteCommand command = connect.CreateCommand())
                //    {

                //        //command.CommandText = "SELECT name FROM sqlite_master WHERE name='account'";
                //        //var name = command.ExecuteScalar();

                //        //// check account table exist or not 
                //        //// if exist do nothing 
                //        //if (name != null && name.ToString() == "account")
                //        //    return;
                //        // acount table not exist, create table and insert 
                //        command.CommandText = "CREATE TABLE  IF NOT EXISTS Import(FileName TEXT);";
                //        command.ExecuteNonQuery();
                //        //command.CommandText = "INSERT INTO account (rowID, user, pass) VALUES (0, '', '')";
                //        //command.ExecuteNonQuery();

                //    }

              
                //    using (SQLiteCommand fmd = connect.CreateCommand())
                //    {
                //        fmd.CommandText = @"SELECT DISTINCT FileName FROM Import WHERE FileName LIKE '%1%'";
                //        fmd.CommandType = CommandType.Text;
                //        SQLiteDataReader r = fmd.ExecuteReader();
                //        while (r.Read())
                //        {
                //            ImportedFiles.Add(Convert.ToString(r["FileName"]));
                //            //r.
                //        }
                //    }
                //}
                //foreach(var name in ImportedFiles)
                //{
                //    //Main.PushLog(name);
                //}
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //LITESQL sql = new LITESQL();

                ////Main.PushLog(sql.Load("nier.db") + "=>loaded");

                //foreach (SQLiteDataReader r in sql.Select("SELECT * FROM Import"))
                //{
                //    //Main.PushLog(Convert.ToString(r["FileName"]));
                //}
            }
            catch { }
            //try
            //{
            //    Stopwatch sw = Stopwatch.StartNew();
            //    var col = db.GetCollection("MAP");

            //    // Use LINQ to query documents (with no index)
            //    var results = col.FindOne(Query.EQ("ID", 1));

            //    if (results != null)
            //    {
            //        Main.PushLog(results["NAME"]);
            //    }
            //    Main.PushLog(sw.Elapsed.TotalSeconds.ToString());
            //    sw = Stopwatch.StartNew();
            //    if (GAMEDIC.MapNameId.ContainsKey(1))
            //    {
            //        Main.PushLog(GAMEDIC.MapNameId[1]);
            //    }
            //    Main.PushLog(sw.Elapsed.TotalSeconds.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Main.PushLog(ex.Message + "=>" + ex.StackTrace);
            //}
        }
        //LiteDatabase db;
        //MemoryStream mem;
        private void SQLite_Load(object sender, EventArgs e)
        {
            //if (!File.Exists("nier.lite"))
            //{
            //    db = new LiteDatabase("nier.lite");
            //    db.GetCollection("ADMIN");
            //}
            //mem = new MemoryStream(File.ReadAllBytes("nier.lite"));
            //db = new LiteDatabase(mem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //File.WriteAllBytes("nier.db", mem.ToArray());
        }
    }
}
