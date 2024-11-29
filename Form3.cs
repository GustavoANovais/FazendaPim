using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Preencha todos os campos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string TpUser = "comprador";

            if(radioButton2.Checked)
            {
                TpUser = "vendedor";
            }
            else if(radioButton1.Checked)
            {
                TpUser = "comprador";
            }

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    string query = "INSERT INTO Usuarios (Nome, Senha, TpUser) VALUES (@Nome, @Senha, @TpUser)";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Nome", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Senha", textBox2.Text);
                    cmd.Parameters.AddWithValue("@TpUser", TpUser);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cadastro feito com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Form2 form2 = new Form2();
                    form2.Show();

                    this.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool senVi = false;
        
        private void button2_Click(object sender, EventArgs e)
        {
            if(senVi)
            {
                textBox2.PasswordChar = '*';
                button2.Text = "Mostrar Senha";
            }

            else
            {
                textBox2.PasswordChar = '\0';
                button2.Text = "Ocultar Senha";
            }

            senVi = !senVi;
        }
    }
}
