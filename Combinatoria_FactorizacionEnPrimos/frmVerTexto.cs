using System;
using System.Windows.Forms;

namespace Combinatoria_FactorizacionEnPrimos
{
    public partial class frmVerTexto : Form
    {
        string num;
        public frmVerTexto(string texto)
        {
            InitializeComponent();
            rtb.Text = texto;
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.DefaultExt = "txt";
            sfd.Filter = "txt|*.txt";
            sfd.FileName = "Primos";

            if (DialogResult.OK == sfd.ShowDialog())
            {
                    System.IO.File.WriteAllText(sfd.FileName, rtb.Text);
            }
        }
    }
}
