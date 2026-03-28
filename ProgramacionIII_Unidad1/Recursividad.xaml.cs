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

        // Función recursiva para convertir texto a vector
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

        // Función para contar dígitos recursivamente
        private int ContarDigitos(long n)
        {
            if (n == 0)
            {
                return 0;
            }
            else
            {
                return 1 + ContarDigitos(n / 10);
            }
        }

        // Botones de funciones
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
            MostrarPanel("signo", "Signo del Número", "Verifica si el número es positivo o negativo usando recursividad.", "Signo(n) = Signo(n-1) si n>0, Signo(n+1) si n<0");
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

        // Funciones recursivas
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

        private long InvertirNumero(long n, long acumulado)
        {
            if (n == 0)
            {
                return acumulado;
            }
            else
            {
                long ultimoDigito = n % 10;
                long nuevoAcumulado = (acumulado * 10) + ultimoDigito;
                return InvertirNumero(n / 10, nuevoAcumulado);
            }
        }

        private long SumarDigitos(long n)
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

        private bool EsPar(long n)
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

        // Función recursiva para determinar el signo
        private string ObtenerSignoRecursivo(long n)
        {
            if (n == 0)
            {
                return "CERO";
            }
            else if (n > 0)
            {
                if (n == 1)
                {
                    return "POSITIVO";
                }
                else
                {
                    return ObtenerSignoRecursivo(n - 1);
                }
            }
            else
            {
                if (n == -1)
                {
                    return "NEGATIVO";
                }
                else
                {
                    return ObtenerSignoRecursivo(n + 1);
                }
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

        // Botón ejecutar
        private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                TextoInfo.Text = "Por favor, ingresa un valor.";
                return;
            }

            // Validación para funciones de vectores
            if (funcionActual == "sv" || funcionActual == "mv" || funcionActual == "mayor" || funcionActual == "menor")
            {
                try
                {
                    string[] partes = txtValor.Text.Split(',');

                    // Validar cantidad de elementos
                    if (partes.Length > 100)
                    {
                        TextoInfo.Text = "Error: Máximo 100 elementos en el vector.";
                        return;
                    }

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

            // Validación para funciones con números
            try
            {
                long num = long.Parse(txtValor.Text);
                int cantidadDigitos = ContarDigitos(Math.Abs(num));

                // Validar según la función
                if (funcionActual == "factorial")
                {
                    if (num > 20)
                    {
                        TextoInfo.Text = "Error: Número máximo 20 para factorial (el resultado sería demasiado grande).";
                    }
                    else if (num < 0)
                    {
                        TextoInfo.Text = "Error: El factorial no está definido para números negativos.";
                    }
                    else
                    {
                        TextoInfo.Text = "Resultado Factorial: " + CalcularFactorial(num);
                    }
                }
                else if (funcionActual == "fib")
                {
                    if (num > 92)
                    {
                        TextoInfo.Text = "Error: Número máximo 92 para Fibonacci (el resultado excede long).";
                    }
                    else if (num < 0)
                    {
                        TextoInfo.Text = "Error: La posición en Fibonacci no puede ser negativa.";
                    }
                    else
                    {
                        TextoInfo.Text = "Fibonacci en posición " + num + ": " + CalcularFibonacci(num);
                    }
                }
                else if (funcionActual == "suma")
                {
                    if (num > 100000)
                    {
                        TextoInfo.Text = "Error: Use un número máximo de 100,000 para evitar desbordamiento de pila.";
                    }
                    else if (num < 0)
                    {
                        TextoInfo.Text = "Error: La suma progresiva no está definida para números negativos.";
                    }
                    else
                    {
                        TextoInfo.Text = "Suma Progresiva: " + CalcularSuma(num);
                    }
                }
                else if (funcionActual == "capicua")
                {
                    // Validar cantidad de dígitos (máximo 10 dígitos para evitar desbordamiento)
                    if (cantidadDigitos > 10)
                    {
                        TextoInfo.Text = $"Error: El número tiene {cantidadDigitos} dígitos. Máximo permitido: 10 dígitos.";
                    }
                    else if (num > 9999999999 || num < -9999999999)
                    {
                        TextoInfo.Text = "Error: Use un número entre -9,999,999,999 y 9,999,999,999.";
                    }
                    else
                    {
                        long valorAbsoluto = Math.Abs(num);
                        long invertido = InvertirNumero(valorAbsoluto, 0);

                        if (valorAbsoluto == invertido)
                        {
                            TextoInfo.Text = $"Resultado: El número {num} ES Capicúa.";
                        }
                        else
                        {
                            TextoInfo.Text = $"Resultado: El número {num} NO es Capicúa. (Invertido: {invertido})";
                        }
                    }
                }
                else if (funcionActual == "invertir")
                {
                    // Validar cantidad de dígitos (máximo 10 dígitos)
                    if (cantidadDigitos > 10)
                    {
                        TextoInfo.Text = $"Error: El número tiene {cantidadDigitos} dígitos. Máximo permitido: 10 dígitos.";
                    }
                    else if (num > 9999999999 || num < -9999999999)
                    {
                        TextoInfo.Text = "Error: Use un número entre -9,999,999,999 y 9,999,999,999.";
                    }
                    else
                    {
                        long valorAbsoluto = Math.Abs(num);
                        long invertido = InvertirNumero(valorAbsoluto, 0);

                        if (num < 0)
                        {
                            TextoInfo.Text = $"Número Invertido: {invertido}";
                        }
                        else
                        {
                            TextoInfo.Text = $"Número Invertido: {invertido}";
                        }
                    }
                }
                else if (funcionActual == "digitos")
                {
                    // Validar cantidad de dígitos (máximo 10 dígitos para rendimiento)
                    if (cantidadDigitos > 10)
                    {
                        TextoInfo.Text = $"Error: El número tiene {cantidadDigitos} dígitos. Máximo permitido: 10 dígitos.";
                    }
                    else if (num > 9999999999 || num < -9999999999)
                    {
                        TextoInfo.Text = "Error: Use un número entre -9,999,999,999 y 9,999,999,999.";
                    }
                    else
                    {
                        long valorAbsoluto = Math.Abs(num);
                        long suma = SumarDigitos(valorAbsoluto);
                        TextoInfo.Text = $"Suma de los dígitos: {suma} (el número tiene {cantidadDigitos} dígitos)";
                    }
                }
                else if (funcionActual == "par")
                {
                    if (Math.Abs(num) > 10000)
                    {
                        TextoInfo.Text = "Error: Use un número entre -10,000 y 10,000 para evitar desbordamiento de pila.";
                    }
                    else
                    {
                        bool esPar = EsPar(num);
                        if (esPar)
                        {
                            TextoInfo.Text = $"El número {num} es: PAR ";
                        }
                        else
                        {
                            TextoInfo.Text = $"El número {num} es: IMPAR ";
                        }
                    }
                }
                else if (funcionActual == "signo")
                {
                    // Validar cantidad de dígitos para recursividad
                    if (cantidadDigitos > 6)
                    {
                        TextoInfo.Text = $"Error: El número tiene {cantidadDigitos} dígitos. Máximo permitido: 6 dígitos para recursividad de signo.";
                    }
                    else if (Math.Abs(num) > 999999)
                    {
                        TextoInfo.Text = "Error: Use un número entre -999,999 y 999,999 para evitar desbordamiento de pila.";
                    }
                    else
                    {
                        string signo = ObtenerSignoRecursivo(num);
                        TextoInfo.Text = $"El número {num} es: {signo} ";
                    }
                }
            }
            catch (OverflowException)
            {
                TextoInfo.Text = "Error: Número demasiado grande. Use un número con máximo 10 dígitos.";
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