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
    /// <summary>
    /// Lógica de interacción para Recursividad.xaml
    /// </summary>
    public partial class Recursividad : Page
    {
        public Recursividad()
        {
            InitializeComponent();
        }
        private void BtnFactorial_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            TituloFuncion.Text = "Factorial";
            DescripcionFuncion.Text = "Calcula n! = n × (n-1) × ... × 1";
            TextoInfo.Text = "Ingresa un número para calcular su factorial";

            Definicion.Text = "factorial(n) = n × factorial(n-1)\nfactorial(0) = 1";
        }


        private void BtnSuma_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            TituloFuncion.Text = "Suma Recursiva";
            DescripcionFuncion.Text = "Suma de 1 hasta n";
            TextoInfo.Text = "Ejemplo: 5 → 1+2+3+4+5";

            Definicion.Text = "suma(n) = n + suma(n-1)\nsuma(0) = 0";
        }


        private void BtnHanoi_Click(object sender, RoutedEventArgs e)
        {
            PanelPrincipal.Visibility = Visibility.Visible;

            TituloFuncion.Text = "Torres de Hanói";
            DescripcionFuncion.Text = "Resolver el problema de discos";
            TextoInfo.Text = "Mover discos entre torres";

            Definicion.Text = "T(n) = 2T(n-1) + 1";
        }


    }
}
