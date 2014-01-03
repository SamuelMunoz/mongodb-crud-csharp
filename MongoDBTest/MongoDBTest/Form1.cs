using System;
using System.Linq;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace MongoDBTest
{
    public partial class Form1 : Form
    {
        private readonly MongoDatabase _db = new MongoClient("mongodb://localhost").GetServer().GetDatabase("sam");
        private bool _esNuevo = true;
        private string _id;
        public Form1()
        {
            InitializeComponent();
            CargarDatos();
        }

        void Limpiar()
        {
            textBox1.Text = textBox2.Text = String.Empty;
        }

        void CargarDatos()
        {
            //Read all
            var usuarios = _db.GetCollection<Usuarios>("usuarios").FindAll().ToList();
            dataGridView1.DataSource = usuarios;
        }

        string ConseguirId()
        {
            // ReSharper disable once PossibleNullReferenceException
            return Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_esNuevo)
            {
                //Insert
                var u = new Usuarios
                        {
                            Usuario = textBox1.Text,
                            Password = textBox2.Text
                        };
                var user = _db.GetCollection<Usuarios>("usuarios");
                user.Insert(u);
                CargarDatos();
                Limpiar();
            }
            else
            {
                //Update
                var user = _db.GetCollection<Usuarios>("usuarios");
                var query = new QueryDocument
                            {
                                {"_id", ObjectId.Parse(_id)}
                            };
                var update = new UpdateDocument
                             {
                                 {"$set", new BsonDocument
                                          {
                                              {"Usuario", textBox1.Text},
                                              {"Password", textBox2.Text}
                                          }
                                 }
                             };
                user.Update(query, update);
                CargarDatos();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            _id = ConseguirId();
            var query = Query.EQ("_id", ObjectId.Parse(_id));
            var usuario = _db.GetCollection<Usuarios>("usuarios").Find(query);

            foreach (var u in usuario)
            {
                textBox1.Text = u.Usuario;
                textBox2.Text = u.Password;
            }
            _esNuevo = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Delete
            _id = ConseguirId();
            var usuario = _db.GetCollection<Usuarios>("usuarios");
            var query = Query.EQ("_id", ObjectId.Parse(_id));
            usuario.Remove(query);
            CargarDatos();
            Limpiar();
        }
    }
}
