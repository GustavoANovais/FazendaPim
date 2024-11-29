using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            textBox3.KeyPress += textBox3_KeyPress;

            textBox1.Text = "Ex: Banana Prata";
            textBox1.ForeColor = Color.Gray;
            textBox1.GotFocus += RPH;
            textBox1.LostFocus += APH;

            textBox2.Text = "Ex: Por Kilo";
            textBox2.ForeColor = Color.Gray;
            textBox2.GotFocus += RPH;
            textBox2.LostFocus += APH;

            textBox3.Text = "Ex: 4,99";
            textBox3.ForeColor = Color.Gray;
            textBox3.GotFocus += RPH;
            textBox3.LostFocus += APH;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarrProd();
        }

        //Parte "Backend"

        private void RPH(object sender, EventArgs e)
        {
            if(textBox1.Text == "Ex: Banana Prata")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }

            if (textBox2.Text == "Ex: Por Kilo")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }

            if (textBox3.Text == "Ex: 4,99")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void APH(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Ex: Banana Prata";
                textBox1.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Ex: Por Kilo";
                textBox2.ForeColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = "Ex: 4,99";
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar !=(char)Keys.Back && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            TextBox textBox = sender as TextBox;
            if(e.KeyChar == ',' && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private int Seleciona()
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells["Id"].Value != null)
            {
                return Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
            }

            else
            {
                throw new Exception("Nenhum produto selecionado");
            }
        }

        private void LimpCamp()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            pictureBox1.Image = null;
            pictureBox1.Tag = null;
        }

        public static class Sess
        {
            public static string Nome { get; set; }
            public static string TpUser { get; set; }
            public static int Id { get; set; }
        }

        private void CarrProd()
        {
            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    string query = "SELECT Id, Nome, Descricao, Preco, Img FROM Produtos";
                    SqlDataAdapter DA = new SqlDataAdapter(query, conexao);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);

                    dataGridView1.DataSource = DT;
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImgBanco(int id)
        {
            string query = "SELECT Img FROM Produtos WHERE Id = @id";

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                SqlCommand cmd = new SqlCommand(query, conexao);
                cmd.Parameters.AddWithValue("@id", id);
                conexao.Open();
                var cImg = cmd.ExecuteScalar();

                if (cImg != null && !string.IsNullOrEmpty(cImg.ToString()))
                {
                    string caImg = cImg.ToString();
                    
                    if(File.Exists(caImg))
                    {
                        pictureBox1.Image = Image.FromFile(caImg);
                    }

                    else
                    {
                        MessageBox.Show("Imagem não encontrada!!!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    MessageBox.Show("Imagem não encontrada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Parte CRUD

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text) || 
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                pictureBox1.Tag == null)
            {
                MessageBox.Show("Por favor, preencha os campos faltantes!", "Faltando!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(!decimal.TryParse(textBox3.Text, out decimal Valid))
            {
                MessageBox.Show("Apenas Números!");
                return;
            }

            string cImg = pictureBox1.Tag?.ToString();
            if(pictureBox1.Tag != null)
            {
                cImg = pictureBox1.Tag.ToString();
                
                if(!File.Exists(cImg))
                {
                    MessageBox.Show("Caminho não válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            else
            {
                MessageBox.Show("Nenhuma Imagem Carregada");
                return;
            }


            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    string query = "INSERT INTO Produtos (Nome, Preco, Descricao, Img) VALUES (@Nome, @Preco, @Descricao, @img); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Nome", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Descricao", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Preco", Valid);
                    cmd.Parameters.AddWithValue("@img", cImg);

                    int idProd = Convert.ToInt32(cmd.ExecuteNonQuery());

                    MessageBox.Show("Produto Adicionado", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpCamp();
                    ImgBanco(idProd);
                    CarrProd();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao adicionar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBox3.Text, out decimal Valid))
            {
                MessageBox.Show("Apenas Números!");
                return;
            }

            string cImg = pictureBox1.Tag?.ToString();

            if (string.IsNullOrEmpty(cImg) || !File.Exists(cImg))
            {
                MessageBox.Show("Selecione uma Imagem", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    string query = "UPDATE Produtos SET Nome = @Nome, Preco = @Preco, Descricao = @Descricao, Img = @img WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Id", Seleciona());
                    cmd.Parameters.AddWithValue("@Nome", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Descricao", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Preco", Valid);
                    cmd.Parameters.AddWithValue("@img", cImg);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produto Atualizado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpCamp();
                    CarrProd();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao Atualizar:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using(SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    string query = "DELETE FROM Produtos WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Id", Seleciona());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produto Deletado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpCamp();
                    CarrProd();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao deletar: " + ex.Message);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens|*.jpg;*.png;*.jpeg;*";
                ofd.Title = "Selecione a imagem do seu Produto";

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    pictureBox1.Tag = ofd.FileName;
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem Certeza?", "Confirmção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Sess.Nome = null;
                Sess.TpUser = null;
                Sess.Id = 0;

                Form2 form2 = new Form2();
                form2.Show();

                this.Hide();
            }
        }

        //Parte DataGridView

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value);
                ImgBanco(id);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["Nome"].Value?.ToString();
                textBox2.Text = row.Cells["Descricao"].Value?.ToString();
                textBox3.Text = row.Cells["Preco"].Value?.ToString();

                string cImg = dataGridView1.Rows[e.RowIndex].Cells["Img"].Value?.ToString();

                if(!string.IsNullOrEmpty(cImg) && File.Exists(cImg))
                {
                    pictureBox1.Image = Image.FromFile(cImg);
                }

                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        //Parte MenuStrip

        private void minhaLojaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();

            this.Hide();
        }
    }
}
