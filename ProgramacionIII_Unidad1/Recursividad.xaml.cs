using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramacionIII_Unidad1
{
    public partial class Recursividad : Page
    {
        // Variable para saber qué botón se presionó
        private string funcionActual = "";

        public Recursividad()
        {
            InitializeComponent();
        }

        private void MostrarPanel(string codigo, string titulo, string descripcion, string formula)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            VistaNormal.Visibility = Visibility.Visible;
            ContenedorHanoi.Visibility = Visibility.Collapsed;
            ContenedorHanoi.Content = null; 

            funcionActual = codigo;
            TituloFuncion.Text = titulo;
            DescripcionFuncion.Text = descripcion;
            Definicion.Text = formula;
            TextoInfo.Text = "Esperando entrada...";
            txtValor.Text = "";
        }

        // lo que haria el for pero recursivo
        private void ConvertirTextoAVectorRecursivo(string[] partes, int[] vector, int indice)
        {
            
            if (indice == partes.Length)
            {
                return;
            }
            else
            {
                string textoLimpio = partes[indice].Trim();

                if (string.IsNullOrEmpty(textoLimpio))
                {
                    throw new Exception("Error: Hay una coma vacía o un espacio extra entre números.");
                }

                vector[indice] = int.Parse(textoLimpio);

                ConvertirTextoAVectorRecursivo(partes, vector, indice + 1);
            }
        }

        private void BtnFactorial_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("factorial", "Factorial (n!)", "Multiplica todos los números desde 1 hasta n.", "F(n) = n * F(n-1)");
        }

        private void BtnSuma_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("suma", "Suma Progresiva", "Suma los números del 1 al n.", "S(n) = n + S(n-1)");
        }

        private void BtnFibonacci_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("fib", "Serie de Fibonacci", "Calcula el valor en la posición n de la serie.", "F(n) = F(n-1) + F(n-2)");
        }

        private void BtnCapicua_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("capicua", "Número Capicúa", "Verifica si el número se lee igual al derecho y al revés.", "n == Invertir(n)");
        }

        private void BtnInvertir_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("invertir", "Invertir Número", "Da la vuelta a las cifras del número.", "Inv(n) = (n%10) + Inv(n/10)");
        }

        private void BtnSumarDigitos_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("digitos", "Suma de Dígitos", "Suma individualmente cada cifra del número.", "D(n) = (n%10) + D(n/10)");
        }

        private void BtnParImpar_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("par", "Par o Impar", "Determina si un número es par restando de 2 en 2.", "Par(n) = Par(n-2)");
        }

        private void BtnSigno_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("signo", "Signo del Número", "Verifica si el número es positivo o negativo.", "n >= 0");
        }

        private void BtnSumaVector_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("sv", "Suma de Vector", "Suma todos los elementos de un arreglo (Ejem: 1,2,3).", "V[i] + Suma(V, i+1)");
        }

        private void BtnMultiVector_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("mv", "Multiplicación de Vector", "Multiplica los elementos de un arreglo.", "V[i] * Multi(V, i+1)");
        }

        private void BtnMayor_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("mayor", "Valor Máximo", "Encuentra el número más grande en un vector.", "Max(V[i], Mayor(i+1))");
        }

        private void BtnMenor_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanel("menor", "Valor Mínimo", "Encuentra el número más pequeño en un vector.", "Min(V[i], Menor(i+1))");
        }

        private void BtnHanoi_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            VistaNormal.Visibility = Visibility.Collapsed;
            ContenedorHanoi.Visibility = Visibility.Visible;
            ContenedorHanoi.Content = new Hanoi();
        }


        private long CalcularFactorial(long n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * CalcularFactorial(n - 1);
            }
        }

        private long CalcularSuma(long n)
        {
            if (n == 0)
            {
                return 0;
            }
            else
            {
                return n + CalcularSuma(n - 1);
            }
        }

        private long CalcularFibonacci(long n)
        {
            if (n <= 1)
            {
                return n;
            }
            else
            {
                return CalcularFibonacci(n - 1) + CalcularFibonacci(n - 2);
            }
        }

        private int InvertirNumero(int n, int acumulado)
        {
            if (n == 0)
            {
                return acumulado;
            }
            else
            {
                int ultimoDigito = n % 10;
                int nuevoAcumulado = (acumulado * 10) + ultimoDigito;
                return InvertirNumero(n / 10, nuevoAcumulado);
            }
        }

        private int SumarDigitos(int n)
        {
            if (n == 0)
            {
                return 0;
            }
            else
            {
                return (n % 10) + SumarDigitos(n / 10);
            }
        }

        private bool EsPar(int n)
        {
            if (n < 0)
            {
                n = n * -1;
            }

            if (n == 0)
            {
                return true;
            }
            else if (n == 1)
            {
                return false;
            }
            else
            {
                return EsPar(n - 2);
            }
        }

        private int SumarVector(int[] v, int indice)
        {
            if (indice == v.Length)
            {
                return 0;
            }
            else
            {
                return v[indice] + SumarVector(v, indice + 1);
            }
        }

        private int MultiplicarVector(int[] v, int indice)
        {
            if (indice == v.Length)
            {
                return 1;
            }
            else
            {
                return v[indice] * MultiplicarVector(v, indice + 1);
            }
        }

        private int EncontrarMayor(int[] v, int indice)
        {
            if (indice == v.Length - 1)
            {
                return v[indice];
            }
            else
            {
                int siguiente = EncontrarMayor(v, indice + 1);
                if (v[indice] > siguiente)
                {
                    return v[indice];
                }
                else
                {
                    return siguiente;
                }
            }
        }

        private int EncontrarMenor(int[] v, int indice)
        {
            if (indice == v.Length - 1)
            {
                return v[indice];
            }
            else
            {
                int siguiente = EncontrarMenor(v, indice + 1);
                if (v[indice] < siguiente)
                {
                    return v[indice];
                }
                else
                {
                    return siguiente;
                }
            }
        }


        private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                TextoInfo.Text = "Por favor, ingresa un valor.";
                return;
            }

            // vectores
            if (funcionActual == "sv" || funcionActual == "mv" || funcionActual == "mayor" || funcionActual == "menor")
            {
                try
                {
                    string[] partes = txtValor.Text.Split(',');
                    int[] vector = new int[partes.Length];
                    ConvertirTextoAVectorRecursivo(partes, vector, 0);

                    if (funcionActual == "sv")
                    {
                        TextoInfo.Text = "Suma de Vector: " + SumarVector(vector, 0);
                    }
                    else if (funcionActual == "mv")
                    {
                        TextoInfo.Text = "Producto de Vector: " + MultiplicarVector(vector, 0);
                    }
                    else if (funcionActual == "mayor")
                    {
                        TextoInfo.Text = "Valor Máximo: " + EncontrarMayor(vector, 0);
                    }
                    else if (funcionActual == "menor")
                    {
                        TextoInfo.Text = "Valor Mínimo: " + EncontrarMenor(vector, 0);
                    }
                }
                catch (Exception ex)
                {
                    TextoInfo.Text = "Error: " + ex.Message;
                }
                return;
            }

            try
            {
                long num = long.Parse(txtValor.Text);

                if (funcionActual == "factorial")
                {
                    if (num > 20 || num < 0)
                    {
                        TextoInfo.Text = "Error: Rango permitido de 0 a 20.";
                    }
                    else
                    {
                        TextoInfo.Text = "Resultado Factorial: " + CalcularFactorial(num);
                    }
                }
                else if (funcionActual == "fib")
                {
                    if (num > 92 || num < 0)
                    {
                        TextoInfo.Text = "Error: Rango permitido de 0 a 92.";
                    }
                    else
                    {
                        TextoInfo.Text = "Fibonacci en posición " + num + ": " + CalcularFibonacci(num);
                    }
                }
                else if (funcionActual == "capicua")
                {
                    int n = (int)num;
                    if (n == InvertirNumero(n, 0))
                    {
                        TextoInfo.Text = "Resultado: Es Capicúa.";
                    }
                    else
                    {
                        TextoInfo.Text = "Resultado: No es Capicúa.";
                    }
                }
                else if (funcionActual == "suma")
                {
                    if (num > 1000 || num < 0)
                    {
                        TextoInfo.Text = "Error: Use un número entre 0 y 1000.";
                    }
                    else
                    {
                        TextoInfo.Text = "Suma Progresiva: " + CalcularSuma(num);
                    }
                }
                else if (funcionActual == "invertir")
                {
                    TextoInfo.Text = "Número Invertido: " + InvertirNumero((int)num, 0);
                }
                else if (funcionActual == "digitos")
                {
                    TextoInfo.Text = "Suma de los dígitos: " + SumarDigitos((int)num);
                }
                else if (funcionActual == "par")
                {
                    if (num > 2000)
                    {
                        TextoInfo.Text = "Límite: Use un número menor a 2000.";
                    }
                    else
                    {
                        if (EsPar((int)num))
                        {
                            TextoInfo.Text = "El número es: PAR";
                        }
                        else
                        {
                            TextoInfo.Text = "El número es: IMPAR";
                        }
                    }
                }
                else if (funcionActual == "signo")
                {
                    if (num >= 0)
                    {
                        TextoInfo.Text = "El número es: POSITIVO";
                    }
                    else
                    {
                        TextoInfo.Text = "El número es: NEGATIVO";
                    }
                }
            }
            catch (Exception ex)
            {
                TextoInfo.Text = "Error: " + ex.Message;
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}