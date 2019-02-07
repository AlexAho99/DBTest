using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DBtest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Skriver ut tidsangivelser längst till vänster
            for (int i = 8; i <= 17; i++)
            {
                Label tmpLabel = new Label
                {
                    Location = new System.Drawing.Point(0, (i - 8) * 36),
                    Name = "lbl" + i,
                    Size = new System.Drawing.Size(35, 15),
                    Text = i.ToString() + ":00",
                    TextAlign = System.Drawing.ContentAlignment.TopRight
                };
                this.Controls.Add(tmpLabel);
            }

            //Läser in tabeller från min databas
            GetFromDB();
        }

        public void GetFromDB()
        {
            
            //Server IP till vår Webbhotell 185.189.48.15
            //localhost
            string connStr = "server=localhost;user=phpuser;database=skolschema;port=3306;password=skolschema";
            MySqlConnection conn = new MySqlConnection(connStr);
            Lesson tmpLesson;
            //Testa att ansluta
            try
            {
                //Öppna databasen
                conn.Open();
                // Perform database operations
                //Skapa en sql-sträng

                

                string sql = "SELECT lektioner.veckodag, lektioner.timeBegin, lektioner.timeEnd, lektioner.undervisningsgruppID, lärare.firstName, lärare.lastName, lektioner.courseID, lektioner.veckoDag FROM lektioner INNER JOIN lärare ON lektioner.teacherID = lärare.teacherID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();



                //Läs en rad i taget i början
                while (rdr.Read())
                {
                    tmpLesson = new Lesson(DayOfWeek.Thursday, DateTime.Parse(rdr[1].ToString()), 80, rdr[3].ToString(), rdr[4].ToString() + " " + rdr[5].ToString(), "Sal " + rdr[6].ToString());
                    this.Controls.Add(tmpLesson);

                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Label tmpLabel = new Label();
                tmpLabel.Text += ex.ToString();
                tmpLabel.Location = new System.Drawing.Point(0,0);
                this.Controls.Add(tmpLabel);

            }
            //Stäng databasen
            conn.Close();

        }
    }
}
