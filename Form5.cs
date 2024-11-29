using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form5 : Form
    {
        private List<Produto> carrinho = new List<Produto>();
        
        public Form5(List<Produto> carrinho)
        {
            InitializeComponent();
            this.carrinho = carrinho;
            CarrCarr();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void CarrCarr()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var produto in carrinho)
            {

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
                            ImageLocation = produto.Img,
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };

                Label nomeLabel = new Label
                {
                    Text = produto.Nome,
                    AutoSize = true,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Location = new Point(10, 110)
                };

                Label precoLabel = new Label
                {
                    Text = "R$" + produto.Preco.ToString("F2"),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Regular),
                    Location = new Point(10, 130)
                };

                Label descLabel = new Label
                {
                    Text = produto.Descricao,
                    AutoSize = true,
                    Font = new Font("Arial", 8, FontStyle.Italic),
                    Location = new Point(10, 150),
                    MaximumSize = new Size(180, 40),
                    TextAlign = ContentAlignment.TopLeft
                };

                panel.Controls.Add(pictureBox);
                panel.Controls.Add(precoLabel);
                panel.Controls.Add(descLabel);
                panel.Controls.Add(nomeLabel);

                flowLayoutPanel1.Controls.Add(panel);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form = new Form6();
            form.Show();
            this.Hide();

            carrinho.Clear();
            flowLayoutPanel1.Controls.Clear();
        }
    }
}
