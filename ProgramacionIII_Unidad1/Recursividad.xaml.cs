using System;
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

        

        private void BtnFactorial_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            funcionActual = "factorial";

            TituloFuncion.Text = "Factorial";
            DescripcionFuncion.Text = "Calcula n! = n × (n-1) × ... × 1";
            TextoInfo.Text = "Ingresa un número para calcular su factorial";

            Definicion.Text = "factorial(n) = n × factorial(n-1)\nfactorial(0) = 1";
        }

        private void BtnSuma_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            funcionActual = "suma";

            TituloFuncion.Text = "Suma Recursiva";
            DescripcionFuncion.Text = "Suma de 1 hasta n";
            TextoInfo.Text = "Ejemplo: 5 → 1+2+3+4+5";

            Definicion.Text = "suma(n) = n + suma(n-1)\nsuma(0) = 0";
        }

        private void BtnHanoi_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            funcionActual = "hanoi";

            TituloFuncion.Text = "Torres de Hanói";
            DescripcionFuncion.Text = "Resolver el problema de discos";
            TextoInfo.Text = "Ingresa la cantidad de discos";

            Definicion.Text = "T(n) = 2T(n-1) + 1";
        }

        

        private int Factorial(int n)
        {
            if (n == 0) return 1;
            return n * Factorial(n - 1);
        }

        private int Suma(int n)
        {
            if (n == 0) return 0;
            return n + Suma(n - 1);
        }

        

        private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = int.Parse(txtValor.Text);

                if (n < 0)
                {
                    TextoInfo.Text = "Ingresa un número positivo";
                    return;
                }

                if (funcionActual == "factorial")
                {
                    int resultado = Factorial(n);
                    TextoInfo.Text = $"Resultado: {resultado}";
                }
                else if (funcionActual == "suma")
                {
                    int resultado = Suma(n);
                    TextoInfo.Text = $"Resultado: {resultado}";
                }
                else if (funcionActual == "hanoi")
                {
                    TextoInfo.Text = "Jiji todavia no esto";
                }
                else
                {
                    TextoInfo.Text = "Selecciona una opcion";
                }
            }
            catch
            {
                TextoInfo.Text = "Ingresa un número válido";
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Inicio());
        }
    }
}
