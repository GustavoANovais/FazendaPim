using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        string ConStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string senhaC = textBox2.Text;
            string tpUser = "";

            if(radioButton1.Checked)
            {
                tpUser = "comprador";
            }

            else if(radioButton2.Checked)
            {
                tpUser = "vendedor";
            }

            if(string.IsNullOrEmpty(tpUser))
            {
                MessageBox.Show("Selecione o tipo de usuário!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!ValidLog(user, senhaC))
            {
                MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string query = "SELECT TpUser FROM Usuarios WHERE Nome = @Nome AND Senha = @Senha";

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                try
                {
                    conexao.Open();
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Nome", user);
                    cmd.Parameters.AddWithValue("@Senha", senhaC);
                    cmd.Parameters.AddWithValue("@TpUser", tpUser);
                    var tpUserDB = cmd.ExecuteScalar();

                    if(tpUserDB != null)
                    {
                        if(tpUserDB.ToString() == tpUser)
                        {
                            if (tpUser == "vendedor")
                            {
                                MessageBox.Show("Login com Sucesso!", "Sucesso", MessageBoxButtons.OK);
                                Form1 formVenda = new Form1();
                                formVenda.Show();
                                this.Hide();
                            }

                            else if (tpUser == "comprador")
                            {
                                MessageBox.Show("Login com Sucesso!", "Sucesso", MessageBoxButtons.OK);
                                Form4 formCompra = new Form4();
                                formCompra.Show();
                                this.Hide();
                            }
                        }
                        
                        else
                        {
                            MessageBox.Show("Incorreto para as credenciais!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }

                    else
                    {
                        MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidLog(string user, string senha)
        {

            if(string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(senha))
            {
                return false;
            }

            using (SqlConnection conexao = new SqlConnection(ConStr))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE Nome = @Nome AND Senha = @Senha";
                using (SqlCommand cmd = new SqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", user);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    conexao.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

            this.Hide();
        }

        private bool senVi = false;

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (senVi)
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
