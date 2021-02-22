using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3.lib
{
    public static class Db
    {
        public static void SaveToDB(string username,int points,string timetobeat)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source = highScoreDB.db; Version=3;");
            conn.Open();
            SQLiteCommand sQLite_cmd;
            sQLite_cmd = conn.CreateCommand();
            string insertQuery = "Insert into score_log (user_name,scored_points,timeToBeat) Values('" + username + "','" + points + "','"+timetobeat+"')";
            sQLite_cmd.CommandText = insertQuery;
            sQLite_cmd.ExecuteNonQuery();
            conn.Close();

        }


        public static List<User> ReadHighScores()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source = highScoreDB.db; Version=3;");
            conn.Open();
            string query = "SELECT scored_points,user_name,date_time FROM score_log ORDER BY scored_points";
            var cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<User> lista = new List<User>();
            while (reader.Read())
            {
                lista.Add(new User
                {
                    timesClicked = reader.GetInt32(0),
                    name = reader.GetString(1),
                    date_time = reader.GetDateTime(2)

                });
            }
            return lista;

        }

    }

}