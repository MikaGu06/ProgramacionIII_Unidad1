using ProgramacionIII_Unidad1.EstructurasLinealess;
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

namespace ProgramacionIII_Unidad1.EstructurasNoLineales
{
    /// <summary>
    /// Lógica de interacción para EstNoLin.xaml
    /// </summary>
    public partial class EstNoLin : Page
    {
        public EstNoLin()
        {
            InitializeComponent();
        }
        private void RegresarInicio_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Inicio());
        }

        private void btnGrafoDirigido_Click(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new GrafoDirigido();
        }

        private void btnGrafoNoDirigido_Click(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new GrafoNoDirigido();
        }

        private void btnGrafoPonderado_Click(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new GrafoPonderado();
        }

        private void btnArbolbin_Click(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new ArbolBinario();
        }
    }
}
