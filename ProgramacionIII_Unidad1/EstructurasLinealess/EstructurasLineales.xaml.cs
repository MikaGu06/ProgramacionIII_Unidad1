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

namespace ProgramacionIII_Unidad1.EstructurasLinealess
{
    /// <summary>
    /// Lógica de interacción para EstructurasLineales.xaml
    /// </summary>
    public partial class EstructurasLineales : Page
    {
        public EstructurasLineales()
        {
            InitializeComponent();

            ContenidoDinamico.Content = new ListasSimplementeEnlazadas();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new ListasSimplementeEnlazadas();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new ListasDoblementeEnlazadas();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new Pilas();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ContenidoDinamico.Content = new Colas();
        }
    }
}
