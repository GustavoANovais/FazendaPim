using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            CarrProd();
        }

        public static class Sess
        {
            public static List<Produto> carrinho { get; set; } = new List<Produto>();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        private void CarrProd()
        {
            string query = "SELECT Nome, Preco, Descricao, Img FROM Produtos";

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    flowLayoutPanel1.Controls.Clear();

                    while (reader.Read())
                    {
                        string nome = reader["Nome"].ToString();
                        string descricao = reader["Descricao"].ToString();
                        string img = reader["Img"].ToString();
                        decimal preco = Convert.ToDecimal(reader["Preco"]);

                        Panel panel = new Panel
                        {
                            Width = 180,
                            Height = 210,
                            BorderStyle = BorderStyle.FixedSingle,
                            Margin = new Padding(10)
                        };

                        PictureBox pictureBox = new PictureBox
                        {
                            Width = 160,
                            Height = 90,
                            ImageLocation = img, 
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };

                        Label nomeLabel = new Label
                        {
                            Text = nome,
                            AutoSize = true,
                            Font = new Font("Arial", 10, FontStyle.Bold),
                            Location = new Point(10, 110)
                        };

                        Label precoLabel = new Label
                        {
                            Text = "R$" + preco.ToString("F2"),
                            AutoSize = true,
                            Font = new Font("Arial", 9, FontStyle.Regular),
                            Location = new Point(10, 130)
                        };

                        Label descLabel = new Label
                        {
                            Text = descricao,
                            AutoSize = true,
                            Font = new Font("Arial", 8, FontStyle.Italic),
                            Location = new Point(10, 150),
                            MaximumSize = new Size(180, 40),
                            TextAlign = ContentAlignment.TopLeft
                        };

                        Button carrBtn = new Button
                        {
                            Text = "Adicionar",
                            AutoSize = true,
                            Font = new Font("Arial", 8, FontStyle.Regular),
                            Location = new Point(10, 180),
                        };

                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(nomeLabel); 
                        panel.Controls.Add(precoLabel);
                        panel.Controls.Add(descLabel);
                        panel.Controls.Add(carrBtn);

                        string nomeC = nome;
                        string descC = descricao;
                        string imgC = img;
                        decimal precoC = preco;

                        carrBtn.Click += (s, e) =>
                        {
                            Produto produto = new Produto
                            {
                                Nome = nomeC,
                                Descricao = descricao,
                                Preco = preco,
                                Img = img
                            };

                            Sess.carrinho.Add(produto);

                            MessageBox.Show($"{nomeC} adicionado ao carrinho!", "Adicionado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        };

                        flowLayoutPanel1.Controls.Add(panel);
                    }

                    reader.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuPrincipalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.Show();

            this.Hide();
        }

        private void meuCarrinhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Sess.carrinho.Count == 0)
            {
                MessageBox.Show("O carrinho está vazio!", "Carrinho", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            Form5 form = new Form5(Sess.carrinho);
            form.Show();
        }
    }
}
