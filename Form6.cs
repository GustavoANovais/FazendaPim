using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label
            {
                Text = "Pedido Finalizado! Retire na Loja!",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            Button button = new Button
            {
                Text = "Fechar",
                Dock = DockStyle.Bottom,
                Height = 40
            };

            button.Click += (s, e) => Close();

            this.Controls.Add(button);
            this.Controls.Add(label);
        }
    }
}
