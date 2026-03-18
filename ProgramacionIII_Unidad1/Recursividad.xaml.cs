using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProgramacionIII_Unidad1
{
    public partial class Recursividad : Page
    {
        string funcionActual = "";

        public Recursividad()
        {
            InitializeComponent();
        }
//esto es visual nomas

        private void BtnCapicua_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "capicua";

            TituloFuncion.Text = "Número Capicúa";
            DescripcionFuncion.Text = "Verifica si un número se lee igual al revés";
            TextoInfo.Text = "Ejemplo: 121, 1331";

            Definicion.Text = "Comparar primer y último dígito recursivamente";
        }

        private void BtnSumaVector_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "sumaVector";

            TituloFuncion.Text = "Suma de Vector";
            DescripcionFuncion.Text = "Suma todos los elementos de un vector";
            TextoInfo.Text = "Ejemplo: 2,5,1,9";

            Definicion.Text = "v[n] + suma(n-1)";
        }

        private void BtnMultiplicarVector_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "multiVector";

            TituloFuncion.Text = "Multiplicación de Vector";
            DescripcionFuncion.Text = "Multiplica todos los elementos";
            TextoInfo.Text = "Ejemplo: 2,5,1,9";

            Definicion.Text = "v[n] * multi(n-1)";
        }

        private void BtnMenor_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "menor";

            TituloFuncion.Text = "Menor del Vector";
            DescripcionFuncion.Text = "Encuentra el número menor";
            TextoInfo.Text = "Ejemplo: 2,5,1,9";

            Definicion.Text = "Comparar elementos recursivamente";
        }

        private void BtnMayor_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "mayor";

            TituloFuncion.Text = "Mayor del Vector";
            DescripcionFuncion.Text = "Encuentra el número mayor";
            TextoInfo.Text = "Ejemplo: 2,5,1,9";

            Definicion.Text = "Comparar elementos recursivamente";
        }

        private void BtnFactorial_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "factorial";

            TituloFuncion.Text = "Factorial";
            DescripcionFuncion.Text = "Calcula n!";
            TextoInfo.Text = "Ingresa un número";

            Definicion.Text = "n * factorial(n-1)";
        }

        private void BtnFibonacci_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "fibo";

            TituloFuncion.Text = "Fibonacci";
            DescripcionFuncion.Text = "Serie Fibonacci";
            TextoInfo.Text = "Ejemplo: 6 → 8";

            Definicion.Text = "f(n) = f(n-1) + f(n-2)";
        }

        private void BtnInvertir_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "invertir";

            TituloFuncion.Text = "Invertir Número";
            DescripcionFuncion.Text = "Invierte los dígitos";
            TextoInfo.Text = "Ejemplo: 123 → 321";

            Definicion.Text = "Invertir recursivamente";
        }

        private void BtnSumarDigitos_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "digitos";

            TituloFuncion.Text = "Suma de Dígitos";
            DescripcionFuncion.Text = "Suma cada dígito";
            TextoInfo.Text = "Ejemplo: 123 → 6";

            Definicion.Text = "n % 10 + recursión";
        }

        private void BtnSuma_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "suma";

            TituloFuncion.Text = "Suma 1 hasta n";
            DescripcionFuncion.Text = "1+2+...+n";
            TextoInfo.Text = "Ejemplo: 5 → 15";

            Definicion.Text = "n + suma(n-1)";
        }

        private void BtnParImpar_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "par";

            TituloFuncion.Text = "Par o Impar";
            DescripcionFuncion.Text = "Determina si es par";
            TextoInfo.Text = "Ejemplo: 4 → Par";

            Definicion.Text = "n - 2 recursivo";
        }

        private void BtnPositivoNegativo_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;
            funcionActual = "signo";

            TituloFuncion.Text = "Positivo o Negativo";
            DescripcionFuncion.Text = "Determina el signo";
            TextoInfo.Text = "Ejemplo: -5 → Negativo";

            Definicion.Text = "Comparación directa";
        }

//los ejercicios 
        private int Factorial(int n) => n == 0 ? 1 : n * Factorial(n - 1);

        private int Suma(int n) => n == 0 ? 0 : n + Suma(n - 1);

        private int Fibonacci(int n)
        {
            if (n <= 1) return n;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        private int Invertir(int n, int inv = 0)
        {
            if (n == 0) return inv;
            return Invertir(n / 10, inv * 10 + n % 10);
        }

        private int SumarDigitos(int n)
        {
            if (n == 0) return 0;
            return (n % 10) + SumarDigitos(n / 10);
        }

        private bool EsPar(int n)
        {
            if (n == 0) return true;
            if (n == 1) return false;
            return EsPar(n - 2);
        }

        private bool Capicua(string s, int i, int j)
        {
            if (i >= j) return true;
            if (s[i] != s[j]) return false;
            return Capicua(s, i + 1, j - 1);
        }

        private int SumarVector(int[] v, int n) => n == 0 ? v[0] : v[n] + SumarVector(v, n - 1);

        private int MultiplicarVector(int[] v, int n) => n == 0 ? v[0] : v[n] * MultiplicarVector(v, n - 1);

        private int Menor(int[] v, int n)
        {
            if (n == 0) return v[0];
            int m = Menor(v, n - 1);
            return v[n] < m ? v[n] : m;
        }

        private int Mayor(int[] v, int n)
        {
            if (n == 0) return v[0];
            int m = Mayor(v, n - 1);
            return v[n] > m ? v[n] : m;
        }

//el boton que inicia la cosa esta

        private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (funcionActual == "capicua")
                {
                    bool r = Capicua(txtValor.Text, 0, txtValor.Text.Length - 1);
                    TextoInfo.Text = r ? "Es capicúa" : "No es capicúa";
                }
                else if (funcionActual == "sumaVector" || funcionActual == "multiVector"
                      || funcionActual == "menor" || funcionActual == "mayor")
                {
                    int[] v = txtValor.Text.Split(',').Select(int.Parse).ToArray();

                    if (funcionActual == "sumaVector")
                        TextoInfo.Text = "Resultado: " + SumarVector(v, v.Length - 1);

                    if (funcionActual == "multiVector")
                        TextoInfo.Text = "Resultado: " + MultiplicarVector(v, v.Length - 1);

                    if (funcionActual == "menor")
                        TextoInfo.Text = "Menor: " + Menor(v, v.Length - 1);

                    if (funcionActual == "mayor")
                        TextoInfo.Text = "Mayor: " + Mayor(v, v.Length - 1);
                }
                else
                {
                    int n = int.Parse(txtValor.Text);

                    if (funcionActual == "factorial")
                        TextoInfo.Text = "Resultado: " + Factorial(n);

                    else if (funcionActual == "fibo")
                        TextoInfo.Text = "Resultado: " + Fibonacci(n);

                    else if (funcionActual == "invertir")
                        TextoInfo.Text = "Resultado: " + Invertir(n);

                    else if (funcionActual == "digitos")
                        TextoInfo.Text = "Resultado: " + SumarDigitos(n);

                    else if (funcionActual == "suma")
                        TextoInfo.Text = "Resultado: " + Suma(n);

                    else if (funcionActual == "par")
                        TextoInfo.Text = EsPar(n) ? "Par" : "Impar";

                    else if (funcionActual == "signo")
                        TextoInfo.Text = n > 0 ? "Positivo" : n < 0 ? "Negativo" : "Cero";

                    else
                        TextoInfo.Text = "Selecciona una opción";
                }
            }
            catch
            {
                TextoInfo.Text = "Entrada inválida";
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Inicio());
        }
    }
}
