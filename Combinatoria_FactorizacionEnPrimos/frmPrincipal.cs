/*
 *   Guillermo Arriaga García 2017 - 2019 guillermoarriagag@gmail.com
 */

using System;
using System.Linq;
using System.Windows.Forms;

namespace Combinatoria_FactorizacionEnPrimos
{
    public partial class frmPrincipal : Form
    {
        FactoresPrimos ofp;

        public frmPrincipal()
        {
            InitializeComponent();
            ofp = new FactoresPrimos();
        }

        #region Eventos Generales
        private void btUsoDeObjetos_Click(object sender, EventArgs e)
        {
            MuestraDeOperacionesConFactoresPrimos();
        }

        private void btInfoGeneral_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Desarrollado por \n\nGuillermo Arriaga García"+
                "\n\nguillermoarriagag@gmail.com\n\nLa complejidad de los procedimientos es O(n log n);" +
                "excepto las variaciones con repetición, que son del orden O(n*n log n).\n\n\n" +
                @"https://github.com/GuillermoArriaga " + "\n" + @"https://www.hackerrank.com/GuillermoArriaga ");
            (new frmInfo("fórmula y algoritmo base.", Properties.Resources._01Base)).Show();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.FileName = "CombinatoriaConFactoresPrimos";
            sfd.DefaultExt = "pdf";
            sfd.Filter = "(*.pdf)|*.pdf";

            if (DialogResult.OK == sfd.ShowDialog())
            {
                System.IO.File.WriteAllBytes(sfd.FileName, Properties.Resources.GAG_FactorialEnPrimos);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
        }

        private void btNinfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("primos hasta n", Properties.Resources._02PrimosHastaN)).Show();
        }

        private void btNfactores_Click(object sender, EventArgs e)
        {
            try
            {
                uint n = Convert.ToUInt32(tbN.Text);
                ofp.Factores = ofp.FactoresPrimosDeN(n);
                tbNfactoresprimos.Text = n.ToString("N0") + " = " + ofp.FactoresEnTexto(ofp.Factores);

                ofp.Factores = ofp.PrimosConPotenciaHasta(n);
                if (ofp.Factores is null)
                {
                    tbNprimoshasta.Text = "Hasta " + n.ToString("N0") + " no hay primos.";
                }
                else
                {
                    MessageBox.Show("Hasta " + n.ToString("N0") + " hay " + ofp.Factores.Count().ToString("N0") + " primos.");
                    tbNprimoshasta.Text = ofp.PrimosHastaEnTexto(ofp.Factores);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbFactoresTexto_Click(object sender, EventArgs e)
        {
            (new frmVerTexto(((TextBox)sender).Text)).Show();
        }

        private void tbFactoresValor_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((TextBox)sender).Text + "\n\nSi el valor fue excediendo a \n" + UInt64.MaxValue.ToString("N0") + "\nse fue aplicando este valor máximo de uint64 = ulong como módulo al ir multiplicando el producto acumulado por cada factor.");
        }

        private bool RevisionDatos(string strN, string strR)
        {
            try
            {
                if (strN == "" || strR == "")
                {
                    MessageBox.Show("Falta ingresar algún valor necesario.");
                    return false;
                }

                uint n = Convert.ToUInt32(strN);
                uint r = Convert.ToUInt32(strR);

                if (n < 0 || r < 0 || r > n)
                {
                    MessageBox.Show("Los valores deben ser enteros no negativos y r <= n.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void MuestraDeOperacionesConFactoresPrimos()
        {
            uint n = 50, r = 7;
            FactoresPrimos fp1 = new FactoresPrimos(ofp.FactorialEnPrimos(n - 1));
            fp1 = fp1 * n; // n!
            fp1 = (fp1 * (n+10))/(n+10);
            FactoresPrimos fp2 = new FactoresPrimos(ofp.FactorialEnPrimos(n - r));
            FactoresPrimos fp3 = new FactoresPrimos(ofp.FactorialEnPrimos(r));
            FactoresPrimos fp4 = fp2 * ofp.FactorialEnPrimos(r);
            FactoresPrimos fpres1 = fp1 / (fp2 * fp3);
            FactoresPrimos fpres2 = fp1 / fp4;
            FactoresPrimos fpres3 = fp1 / fp2;
            fpres3 = fpres3 / fp3.Factores;

            MessageBox.Show("Muestra de operaciones con objetos FactoresPrimos\n\n\n" + 
                "fp1: ((49! * 50) * 60)/60 = " + n + "! = " + fp1.FactoresConPotenciasParaOperarEnTexto(fp1) + "\n\n" +
                "fp2: " + (n - r) + "! = " + fp2.FactoresConPotenciasParaOperarEnTexto(fp2) + "\n\n" +
                "fp3: " + r + "! = " + fp3.FactoresConPotenciasParaOperarEnTexto(fp3) + "\n\n" +
                "fp1 / (fp2 * fp3) = " + fpres1.FactoresConPotenciasParaOperarEnTexto(fpres1) + "\n\n" +
                "fp1 / (fp2 * fp3.factores) = " + fpres2.FactoresConPotenciasParaOperarEnTexto(fpres2) + "\n\n" +
                "(fp1 / fp2) / fp3.factores = " + fpres3.FactoresConPotenciasParaOperarEnTexto(fpres3) + "\n\n" +
                "Debe ser " + n + " C " + r + " = " + ofp.FactoresEnTexto(ofp.CombinacionesSinRepEnPrimos(n,r)) +
                "\n\n  fp3 ^ 9 = " + fp3.FactoresConPotenciasParaOperarEnTexto(fp3^9));
        }
        #endregion

        #region Combinaciones
        private void btCSRval_Click(object sender, EventArgs e)
        {
            if (!RevisionDatos(tbCSRn.Text, tbCSRr.Text))
            {
                return;
            }

            uint n = Convert.ToUInt32(tbCSRn.Text);
            uint r = Convert.ToUInt32(tbCSRr.Text);

            tbCSRtexto.Text = "";
            tbCSRvalor.Text = "";
            tbCSRvalormodulo.Text = "";

            try
            {
                ofp.Factores = ofp.CombinacionesSinRepEnPrimos(n, r);
                tbCSRtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbCSRvalor.Text = ofp.CombinacionesSinRepeticion(n, r).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbCSRvalor.Text = "supera las posibilidades de cálculo";
            }

            try
            {
                if (tbCSRmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong m = Convert.ToUInt64(tbCSRmod.Text);
                tbCSRvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, m).ToString("N0") + " mod(" + m.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbCSRvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btCSRinfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("combinaciones sin repetición", Properties.Resources._03CombinacionesSinRepeticion)).Show();
        }

        private void btCCRval_Click(object sender, EventArgs e)
        {
            if (!RevisionDatos(tbCCRn.Text, tbCCRr.Text))
            {
                return;
            }

            uint n = Convert.ToUInt32(tbCCRn.Text);
            uint r = Convert.ToUInt32(tbCCRr.Text);

            tbCCRtexto.Text = "";
            tbCCRvalor.Text = "";
            tbCCRvalormodulo.Text = "";

            try
            {
                ofp.Factores = ofp.CombinacionesConRepEnPrimos(n, r);
                tbCCRtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbCCRvalor.Text = ofp.CombinacionesConRepeticion(n, r).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (tbCCRmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong mod = Convert.ToUInt64(tbCCRmod.Text);
                tbCCRvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, mod).ToString("N0") + " mod(" + mod.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbCCRvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btCCRinfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("combinaciones con repetición", Properties.Resources._04CombinacionesConRepeticion)).Show();
        }
        #endregion

        #region Permutaciones
        private void btPval_Click(object sender, EventArgs e)
        {
            if (tbPn.Text == "")
            {
                MessageBox.Show("Falta ingresar algún valor necesario.");
                return;
            }

            uint n;

            tbPtexto.Text = "";
            tbPvalor.Text = "";
            tbPvalormodulo.Text = "";

            try
            {
                n = Convert.ToUInt32(tbPn.Text);
                if (n < 0)
                {
                    MessageBox.Show("El valor n debe ser un entero no negativo");
                    return;
                }

                ofp.Factores = ofp.PermutacionesSinRepEnPrimos(n);
                tbPtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbPvalor.Text = ofp.PermutacionesSinRepeticion(n).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (tbPmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong mod = Convert.ToUInt64(tbPmod.Text);
                tbPvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, mod).ToString("N0") + " mod(" + mod.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbPvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btPinfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("permutaciones sin repetición", Properties.Resources._05PermutacionesSinRepeticion)).Show();
        }

        private void btPRval_Click(object sender, EventArgs e)
        {
            if (tbPRn.Text == "" || tbPRr.Text == "")
            {
                MessageBox.Show("Falta ingresar algún valor necesario.");
                return;
            }

            uint n;
            int m = tbPRr.Lines.Length;
            uint[] r = new uint[m];

            tbPRtexto.Text = "";
            tbPRvalor.Text = "";
            tbPRvalormodulo.Text = "";

            try
            {
                uint conta = 0;
                n = Convert.ToUInt32(tbPRn.Text);

                if (n < 0)
                {
                    MessageBox.Show("El valor de n debe ser un entero no negativo.");
                    return;
                }

                for (int i = 0; i < m; i++)
                {
                    r[i] = Convert.ToUInt32(tbPRr.Lines[i]);
                    conta += r[i];
                }

                if (conta != n)
                {
                    MessageBox.Show("Las cantidades de repeticiones no suman " + n + ", sino " + conta.ToString("N0") + ".");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cada cantidad de repeticiones de los valores para PR debe estar separado por un salto de línea y ninguna línea debe estar vacía.\n\n" + ex.Message);
                return;
            }

            try
            {
                n = Convert.ToUInt32(tbPRn.Text);

                ofp.Factores = ofp.PermutacionesConRepEnPrimos(n, r);
                tbPRtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbPRvalor.Text = ofp.PermutacionesConRepeticion(n, r).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (tbPRmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong mod = Convert.ToUInt64(tbPRmod.Text);
                tbPRvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, mod).ToString("N0") + " mod(" + mod.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbPRvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btPRinfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("permutaciones con repetición", Properties.Resources._06PermutacionesConRepeticion)).Show();
        }
        #endregion

        #region Variaciones
        private void btVval_Click(object sender, EventArgs e)
        {
            if (!RevisionDatos(tbVn.Text, tbVr.Text))
            {
                return;
            }

            uint n = Convert.ToUInt32(tbVn.Text);
            uint r = Convert.ToUInt32(tbVr.Text);

            tbVtexto.Text = "";
            tbVvalor.Text = "";
            tbVvalormodulo.Text = "";

            try
            {
                n = Convert.ToUInt32(tbVn.Text);
                r = Convert.ToUInt32(tbVr.Text);

                ofp.Factores = ofp.VariacionesSinRepEnPrimos(n, r);
                tbVtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbVvalor.Text = ofp.VariacionesSinRepeticion(n, r).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (tbVmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong mod = Convert.ToUInt64(tbVmod.Text);
                tbVvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, mod).ToString("N0") + " mod(" + mod.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbVvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btVInfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("variaciones sin repetición", Properties.Resources._07VariacionesSinRepeticion)).Show();
        }

        private void btVRval_Click(object sender, EventArgs e)
        {
            if (tbVRn.Text == "" || tbVRr.Text == "")
            {
                MessageBox.Show("Falta ingresar algún valor necesario.");
                return;
            }

            uint n, r;

            tbVRtexto.Text = "";
            tbVRvalor.Text = "";
            tbVRvalormodulo.Text = "";

            try
            {
                n = Convert.ToUInt32(tbVRn.Text);
                r = Convert.ToUInt32(tbVRr.Text);

                if (n < 2 || r < 0)
                {
                    MessageBox.Show("El valor de n debe ser mayor a 1 y el de r debe no ser negativo. Ambos deben ser enteros.");
                    return;
                }

                ofp.Factores = ofp.VariacionesConRepEnPrimos(n, r);
                tbVRtexto.Text = ofp.FactoresEnTexto(ofp.Factores);
                tbVRvalor.Text = ofp.VariacionesConRepeticion(n, r).ToString("N0") + " mod(" + UInt64.MaxValue.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (tbVRmod.Text == "")
                {
                    MessageBox.Show("Falta ingresar el valor del modulo.");
                    return;
                }
                ulong mod = Convert.ToUInt64(tbVRmod.Text);
                tbVRvalormodulo.Text = ofp.ValorNumericoPrimosConPotenciaModulo(ofp.Factores, mod).ToString("N0") + " mod(" + mod.ToString("N0") + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tbVRvalormodulo.Text = "supera las posibilidades de cálculo";
            }
        }

        private void btVRInfo_Click(object sender, EventArgs e)
        {
            (new frmInfo("variaciones con repetición", Properties.Resources._08VariacionesConRepeticion)).Show();
        }
        #endregion
    }
}
