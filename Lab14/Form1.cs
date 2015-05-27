using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsersPrject
{

    /// <summary>
    /// This project is for test purposes only for students ;
    /// TODO 1) Update user and save methods do it in one method ;
    /// TODO 2) Use Try Catch Statement for methods ;
    /// TODO 3) Good validation for user properties ;
    /// TODO 4) Confirm Message Box for delete operation;
    /// </summary>

    public partial class Form1 : Form
    {
      
        int userId = 0;
       

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
            
            if (userId > 0)
            {
                using (TEMPEntities db = new TEMPEntities())
                {
                    User u = db.Users.First(x => x.Id == userId);
                    u.Name = txtUser.Text;
                    u.Surname = txtSurname.Text;
                    db.SaveChanges();
                }
            }
            else
            {
                SaveUser();
            }


            LoadUsers();
        }

        private void SaveUser()
        {

            using (TEMPEntities db = new TEMPEntities())
            {
                User user = new User();
                user.Name = txtUser.Text;
                user.Surname = txtSurname.Text;

                int position = Convert.ToInt32(comboPositions.SelectedValue);
                user.PositionId = position;

                db.Users.Add(user);
                db.SaveChanges();
             

                txtSurname.Text = string.Empty;
                txtUser.Text = string.Empty;

            }

        
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            LoadUsers();

          

        }

        private void LoadUsers()
        {
            using (TEMPEntities db = new TEMPEntities())
            {
               // var listUsers = db.Users.ToList();
               // var postions = db.Positions.ToList();
               
                var userPositions  =
                    (from u in db.Users
                     join p in db.Positions on u.PositionId equals p.ID into ps
                     from p in ps.DefaultIfEmpty()
                    select new {
                                 UserId = u.Id,   
                                 Name = u.Name, 
                                 Surname = u.Surname,
                                 UserPosition = (p==null) ? string.Empty: p.Name
                         }
                        ).ToList();

              
                BindingSource bs = new BindingSource();
                bs.DataSource = userPositions;
                dataGridView1.DataSource = bs;

                


            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = userBindingSource.Current as User;

            if (user == null) return;

            using (TEMPEntities db = new TEMPEntities())
            {
                var u = db.Users.First(x => x.Id == user.Id);
                db.Users.Remove(u);
                db.SaveChanges();
                LoadUsers();
                
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = userBindingSource.Current as User;

            if (user == null) return;

            userId = user.Id;
            txtUser.Text = user.Name;
            txtSurname.Text = user.Surname;

        }
    }
}
