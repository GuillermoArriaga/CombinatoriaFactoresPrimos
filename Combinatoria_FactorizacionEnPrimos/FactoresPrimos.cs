using System.Linq;

namespace Combinatoria_FactorizacionEnPrimos
{
    public class FactoresPrimos
    {
        public uint[][] Factores { get; set; }
        public long[] PotenciasParaOperar { get; set; }

        /* Siendo k entero > 1 y fp un arreglo de factores primos,
         * las operaciones fp*fp, fp/fp, fp^k, fp*k, fp/k
         * sólo requieren de trabajar con un arreglo long de potencias
         * ya que, por su posición, harán referencia al mismo primo.
         */
        #region Constructores
        public FactoresPrimos()
        {

        }

        public FactoresPrimos(uint n)
        {
            if (n > 1)
            {
                Factores = FactoresPrimosDeN(n);
                int c = Factores.Count();
                PotenciasParaOperar = new long[c];

                for (int i = 0; i < c; i++)
                {
                    PotenciasParaOperar[i] = Factores[i][1];
                }
            }
        }

        public FactoresPrimos(FactoresPrimos fp)
        {
            int c = fp.Factores.Count();
            PotenciasParaOperar = new long[c];
            Factores = new uint[c][];

            for (int i = 0; i < c; i++)
            {
                Factores[i] = new uint[2];
            }

            for (int i = 0; i < c; i++)
            {
                Factores[i][0] = fp.Factores[i][0];
                Factores[i][1] = fp.Factores[i][1];
                PotenciasParaOperar[i] = fp.PotenciasParaOperar[i];
            }
        }

        public FactoresPrimos(uint[][] fp)
        {
            int c = fp.Count();
            PotenciasParaOperar = new long[c];
            Factores = new uint[c][];

            for (int i = 0; i < c; i++)
            {
                Factores[i] = new uint[2];
            }

            for (int i = 0; i < c; i++)
            {
                Factores[i][0] = fp[i][0];
                Factores[i][1] = fp[i][1];
                PotenciasParaOperar[i] = fp[i][1];
            }
        }
        
        #endregion

        #region Metodos Generales

        public string FactoresEnTexto(uint[][] fp)
        {
            string texto = "";
            int c = fp.Count();

            for (int i = 0; i < c; i++)
            {
                if (fp[i][1] == 0)
                {
                    continue;
                }

                if (fp[i][1] == 1)
                {
                    texto += "(" + fp[i][0] + ") ";
                }
                else
                {
                    texto += "(" + fp[i][0] + "^" + fp[i][1] + ") ";
                }                 
            }

            return texto;
        }

        public string FactoresConPotenciasParaOperarEnTexto(FactoresPrimos fp)
        {
            string texto = "";
            int c = fp.Factores.Count();

            for (int i = 0; i < c; i++)
            {
                if (fp.PotenciasParaOperar[i] == 0)
                {
                    continue;
                }

                if (fp.PotenciasParaOperar[i] == 1)
                {
                    texto += "(" + fp.Factores[i][0] + ") ";
                }
                else
                {
                    texto += "(" + fp.Factores[i][0] + "^" + fp.PotenciasParaOperar[i] + ") ";
                }
            }

            return texto;
        }

        public string PrimosHastaEnTexto(uint[][] fp)
        {
            int c = fp.Count();
            string texto = "";

            for (int i = 0; i < c - 1; i++)
            {
                texto += fp[i][0] + ", ";
            }

            texto += fp[c - 1][0];

            return texto;
        }

        public uint PotenciaPrimoDeFactorial(uint n, uint primo)
        {
            uint potencia = 0;
            uint k = n / primo;

            while (k > 0)
            {
                potencia += k;
                k /= primo;
            }
            return potencia;
        }

        public uint[] PrimosHasta(uint n)
        {
            /* Complejidad O(n log n)
             * 
             * Los primos < n + 1 son los enteros p que
             * 1 < p < (n+1) no divididos por los primos
             * menores a la raíz cuadrada de (n+1), por
             * lo que la anulación de múltiplos ya no es
             * necesaria a partir del primo p: p * p > n
             * 
             * Impares como candidatos: 3, 5, ..., [(n+1)/2]
             * noEsPrimo[i] indica si 2*i+1 no es primo
             * 
             * Cada primo p puede anular nuevos candidatos
             * no anulados a partir de p*p y avanzando de
             * p en p posiciones.
             * p*p es impar pues p es impar, por lo que
             * está como candidato a primo. 
             * 
             * p = 2*i+1, p*p = 4*i*i + 4*i + 1
             * p está en posicion i = p/2
             * 
             */
            if (n < 2)
            {
                return null;
            }

            uint i, j;
            uint max = (1 + n) / 2;
            bool[] noEsPrimo = new bool[max];

            for (i = 0; i < max; i++)
            {
                noEsPrimo[i] = false;
            }

            uint posCuadradoPrimo = 4;

            /* posCuadradoPrimo = 2i(i+1)
             * p*p <= n  ->  2i(i+1) <= (n-1)/2 < max
             *  n par -> n=2m, (n-1)/2=m-1, max=m
             *  n impar -> n=2m+1, (n-1)/2=m, max=m+1
             * por lo que 2i(i+1) <= (n-1)/2
             * equivale a 2i(i+1) < max
             */

            for (i = 1; posCuadradoPrimo < max; i++)
            {
                if (noEsPrimo[i])
                {
                    continue;
                }
                // El impar 2*i+1 que llega aquí es primo
                uint primo = 2 * i + 1;
                posCuadradoPrimo = 2 * i * (i + 1);

                // Aseguramiento de anulación de múltiplos
                for (j = posCuadradoPrimo; j < max; j += primo)
                {
                    noEsPrimo[j] = true;
                }
            }

            // Conteo de primos
            uint cantidad = 1;   // primo 2 ya contado
            for (i = 1; i < max; i++)
            {
                if (!noEsPrimo[i])
                {
                    cantidad++;   // Conteo de primos
                }
            }

            uint[] primos = new uint[cantidad];

            primos[0] = 2;
            j = 1;

            for (i = 1; i < max; i++)
            {
                if (!noEsPrimo[i])
                {
                    primos[j] = 2 * i + 1;
                    j++;
                }
            }

            return primos;
        }

        public uint[][] PrimosConPotenciaHasta(uint n)
        {
            /* Complejidad O(n log n)
             * 
             * Los primos < n + 1 son los enteros p que
             * 1 < p < (n+1) no divididos por los primos
             * menores a la raíz cuadrada de (n+1), por
             * lo que la anulación de múltiplos ya no es
             * necesaria a partir del primo p: p * p > n
             * 
             * Impares como candidatos: 3, 5, ..., [(n+1)/2]
             * noEsPrimo[i] indica si 2*i+1 no es primo
             * 
             * Cada primo p puede anular nuevos candidatos
             * no anulados a partir de p*p y avanzando de
             * p en p posiciones.
             * p*p es impar pues p es impar, por lo que
             * está como candidato a primo. 
             * 
             * p = 2*i+1, p*p = 4*i*i + 4*i + 1
             * p está en posicion i = p/2
             * 
             */

            if (n < 2)
            {
                return null;
            }

            uint i, j;
            uint max = (1 + n) / 2;
            bool[] noEsPrimo = new bool[max];

            for (i = 0; i < max; i++)
            {
                noEsPrimo[i] = false;
            }

            uint posCuadradoPrimo = 4;

            /* posCuadradoPrimo = 2i(i+1)
             * p*p <= n  ->  2i(i+1) <= (n-1)/2 < max
             *  n par -> n=2m, (n-1)/2=m-1, max=m
             *  n impar -> n=2m+1, (n-1)/2=m, max=m+1
             * por lo que 2i(i+1) <= (n-1)/2
             * equivale a 2i(i+1) < max
             */

            for (i = 1; posCuadradoPrimo < max; i++)
            {
                if (noEsPrimo[i])
                {
                    continue;
                }
                // El impar 2*i+1 que llega aquí es primo
                uint primo = 2 * i + 1;
                posCuadradoPrimo = 2 * i * (i + 1);

                // Aseguramiento de anulación de múltiplos
                for (j = posCuadradoPrimo; j < max; j += primo)
                {
                    noEsPrimo[j] = true;
                }
            }

            // Conteo de primos
            uint cantidad = 1;   // primo 2 ya contado
            for (i = 1; i < max; i++)
            {
                if (!noEsPrimo[i])
                {
                    cantidad++;   // Conteo de primos
                }
            }

            uint[][] primos = new uint[cantidad][];

            for (j = 0; j < cantidad; j++)
            {
                primos[j] = new uint[2];
            }
            primos[0][0] = 2;
            primos[0][1] = 1;
            j = 1;

            for (i = 1; i < max; i++)
            {
                if (!noEsPrimo[i])
                {
                    primos[j][0] = 2 * i + 1;
                    primos[j][1] = 1;
                    j++;
                }
            }

            return primos;
        }

        public uint[][] FactorialEnPrimos(uint n)
        {
            uint[][] res;

            if (n < 2)   // 0!, 1! = 1 = 2^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }

            res = PrimosConPotenciaHasta(n);
            int c = res.Count();
            uint k;

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;   // potencia
                k = n / res[i][0];

                while (k > 0)
                {
                    res[i][1] += k;
                    k /= res[i][0];
                }
            }

            return res;
        }

        public ulong ValorNumericoPrimosConPotencia(uint[][] fp)
        {
            ulong val = 1;
            int c;   // c es cantidad

            c = fp.Count();

            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < fp[i][1]; j++)
                {
                    val *= fp[i][0];
                }
            }

            return val;
        }

        public ulong ValorNumericoPrimosConPotenciaModulo(uint[][] fp, ulong mod)
        {
            ulong val = 1;
            int c;   // c es cantidad

            c = fp.Count();

            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < fp[i][1]; j++)
                {
                    val = (val * fp[i][0]) % mod;

                    if (val == 0)
                    {
                        break;
                    }
                }
            }

            return val;
        }

        public uint[][] FactoresPrimosDeN(uint n)
        {
            return VariacionesConRepEnPrimos(n, 1);
        }
        #endregion

        #region Combinatoria

        public uint[][] CombinacionesSinRepEnPrimos(uint n, uint r)
        {
            uint[][] res;
            if (n < 2)   // 0C0, 1C0, 1C1 = 1 = 2^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }
            res = PrimosConPotenciaHasta(n);
            int c = res.Count();
            uint k;

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;   // potencia
                k = n / res[i][0];

                while (k > 0)
                {
                    res[i][1] += k;
                    k /= res[i][0];
                }

                k = r / res[i][0];

                while (k > 0)
                {
                    res[i][1] -= k;
                    k /= res[i][0];
                }

                k = (n - r) / res[i][0];

                while (k > 0)
                {
                    res[i][1] -= k;
                    k /= res[i][0];
                }
            }

            return res;
        }

        public ulong CombinacionesSinRepeticion(uint n, uint r)
        {
            return ValorNumericoPrimosConPotencia(CombinacionesSinRepEnPrimos(n, r));
        }

        public uint[][] CombinacionesConRepEnPrimos(uint n, uint r)
        {
            uint[][] res;
            if (n + r - 1 < 2)   // 0CR0, 1CR0, 1CR1 = 1 = 2^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }

            res = PrimosConPotenciaHasta(n + r - 1);
            int c = res.Count();
            uint k;

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;   // potencia
                k = (n + r - 1) / res[i][0];

                while (k > 0)
                {
                    res[i][1] += k;
                    k /= res[i][0];
                }

                k = r / res[i][0];

                while (k > 0)
                {
                    res[i][1] -= k;
                    k /= res[i][0];
                }

                k = (n - 1) / res[i][0];

                while (k > 0)
                {
                    res[i][1] -= k;
                    k /= res[i][0];
                }
            }

            return res;
        }

        public ulong CombinacionesConRepeticion(uint n, uint r)
        {
            return ValorNumericoPrimosConPotencia(CombinacionesConRepEnPrimos(n, r));
        }

        public uint[][] PermutacionesSinRepEnPrimos(uint n)
        {
            return FactorialEnPrimos(n);
        }

        public ulong PermutacionesSinRepeticion(uint n)
        {
            return ValorNumericoPrimosConPotencia(FactorialEnPrimos(n));
        }

        public uint[][] PermutacionesConRepEnPrimos(uint n, uint[] r)
        {
            uint[][] res;

            if (n < 2)   // 0PR0, 1PR1 = 1 = 2^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }

            res = PrimosConPotenciaHasta(n);
            uint k;
            int c = res.Count();
            int m = r.Count();

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;   // potencia
                k = n / res[i][0];

                while (k > 0)
                {
                    res[i][1] += k;
                    k /= res[i][0];
                }

                for (int j = 0; j < m; j++)
                {
                    k = r[j] / res[i][0];

                    while (k > 0)
                    {
                        res[i][1] -= k;
                        k /= res[i][0];
                    }
                }
            }

            return res;
        }

        public ulong PermutacionesConRepeticion(uint n, uint[] r)
        {
            return ValorNumericoPrimosConPotencia(PermutacionesConRepEnPrimos(n, r));
        }

        public uint[][] VariacionesSinRepEnPrimos(uint n, uint r)
        {
            uint[][] res;

            if (n < 2)   // 0V0, 1V0, 1V1 = 1 = 1^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }

            res = PrimosConPotenciaHasta(n);
            uint k;
            int c = res.Count();

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;   // potencia
                k = n / res[i][0];

                while (k > 0)
                {
                    res[i][1] += k;
                    k /= res[i][0];
                }

                k = (n - r) / res[i][0];

                while (k > 0)
                {
                    res[i][1] -= k;
                    k /= res[i][0];
                }
            }

            return res;
        }

        public ulong VariacionesSinRepeticion(uint n, uint r)
        {
            return ValorNumericoPrimosConPotencia(VariacionesSinRepEnPrimos(n, r));
        }

        public uint[][] VariacionesConRepEnPrimos(uint n, uint r)
        {
            uint[][] res;

            if (n < 2)   // 0V0, 1V0, 1V1 = 1 = 2^0
            {
                res = new uint[1][];
                res[0] = new uint[2];
                res[0][0] = 2;
                res[0][1] = 0;
                return res;
            }

            res = PrimosConPotenciaHasta(n);
            uint k1, k2;
            int c = res.Count();

            for (int i = 0; i < c; i++)
            {
                res[i][1] = 0;    // potencia
                k1 = n / res[i][0];
                k2 = (n - 1) / res[i][0];
                /* k1-k2=0,1 y es indicador
                 *   de si el primo res[i][0]
                 *   es factor o no de n.
                 * A partir de que k1 = k2
                 *   ya no hay aporte para
                 *   la potencia.
                 */

                while ( k1 > k2 )
                {
                    k1 /= res[i][0];
                    k2 /= res[i][0];
                    res[i][1] += r;
                }
            }

            return res;
        }

        public ulong VariacionesConRepeticion(uint n, uint r)
        {
            return ValorNumericoPrimosConPotencia(VariacionesConRepEnPrimos(n, r));
        }
        #endregion

        #region Algunas operaciones (fp, fp) y (fp, entero > 0)

        public long[] PasoDePotenciasUintALong(uint[][] fp)
        {
            int c = fp.Count();
            long[] pl = new long[c];

            for (uint i = 0; i < c; i++)
            {
                pl[i] = fp[i][1];
            }

            return pl;
        }

        public static FactoresPrimos operator ^(FactoresPrimos fp1, uint k)
        {
            FactoresPrimos fpRes = new FactoresPrimos(fp1);

            for (int i = 0; i < fp1.Factores.Count(); i++)
            {
                fpRes.PotenciasParaOperar[i] *= k;
            }

            return fpRes;
        }

        public static FactoresPrimos operator *(FactoresPrimos fp1, FactoresPrimos fp2)
        {
            FactoresPrimos fpRes;
            int min = fp1.PotenciasParaOperar.Count();
            int max = fp2.PotenciasParaOperar.Count();

            if (max < min)
            {
                min = max;
                fpRes = new FactoresPrimos(fp1);

                for (int i = 0; i < min; i++)
                {
                    fpRes.PotenciasParaOperar[i] += fp2.PotenciasParaOperar[i];
                }
            }
            else
            {
                fpRes = new FactoresPrimos(fp2);

                for (int i = 0; i < min; i++)
                {
                    fpRes.PotenciasParaOperar[i] += fp1.PotenciasParaOperar[i];
                }
            }

            return fpRes;
        }

        public static FactoresPrimos operator *(FactoresPrimos fp1, uint[][] fp2)
        {
            FactoresPrimos fpRes;
            int min = fp1.PotenciasParaOperar.Count();
            int max = fp2.Count();

            if (max < min)
            {
                min = max;
                fpRes = new FactoresPrimos(fp1);

                for (int i = 0; i < min; i++)
                {
                    fpRes.PotenciasParaOperar[i] += fp2[i][1];
                }
            }
            else
            {
                fpRes = new FactoresPrimos(fp2);

                for (int i = 0; i < min; i++)
                {
                    fpRes.PotenciasParaOperar[i] += fp1.PotenciasParaOperar[i];
                }
            }

            return fpRes;
        }

        public static FactoresPrimos operator *(FactoresPrimos fp1, uint k)
        {
            if (k == 0)
            {
                return null;
            }
            if (k == 1)
            {
                return new FactoresPrimos(fp1);
            }
            return new FactoresPrimos(fp1 * fp1.FactoresPrimosDeN(k));
        }

        public static FactoresPrimos operator /(FactoresPrimos fp1, FactoresPrimos fp2)
        {
            FactoresPrimos fpRes = new FactoresPrimos();
            int min = fp1.PotenciasParaOperar.Count();
            int max = fp2.PotenciasParaOperar.Count();

            if (max < min)
            {
                min = max;
                fpRes = new FactoresPrimos(fp1);
            }
            else
            {
                fpRes = new FactoresPrimos(fp2);
                for (int i = 0; i < max; i++)
                {
                    fpRes.PotenciasParaOperar[i] *= -1;
                }
            }

            for (int i = 0; i < min; i++)
            {
                fpRes.PotenciasParaOperar[i] = fp1.PotenciasParaOperar[i] - fp2.PotenciasParaOperar[i];
            }

            return fpRes;
        }

        public static FactoresPrimos operator /(FactoresPrimos fp1, uint[][] fp2)
        {
            FactoresPrimos fpRes = new FactoresPrimos();
            int min = fp1.PotenciasParaOperar.Count();
            int max = fp2.Count();

            if (max < min)
            {
                min = max;
                fpRes = new FactoresPrimos(fp1);
            }
            else
            {
                fpRes = new FactoresPrimos(fp2);
                for (int i = 0; i < max; i++)
                {
                    fpRes.PotenciasParaOperar[i] *= -1;
                }
            }

            for (int i = 0; i < min; i++)
            {
                fpRes.PotenciasParaOperar[i] = fp1.PotenciasParaOperar[i] - fp2[i][1];
            }

            return fpRes;
        }

        public static FactoresPrimos operator /(FactoresPrimos fp1, uint k)
        {
            if (k == 0)
            {
                return null;
            }
            if (k == 1)
            {
                return new FactoresPrimos(fp1);
            }
            return new FactoresPrimos(fp1 / fp1.FactoresPrimosDeN(k));
        }

        #endregion
    }
}
