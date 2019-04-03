using System.Drawing;
using System.Windows.Forms;

namespace Combinatoria_FactorizacionEnPrimos
{
    public partial class frmInfo : Form
    {
        public frmInfo(string titulo, Image imagen)
        {
            InitializeComponent();

            Text += titulo;
            pictureBox1.BackgroundImage = imagen;
        }
    }
}
