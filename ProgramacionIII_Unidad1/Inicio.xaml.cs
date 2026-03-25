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
using ProgramacionIII_Unidad1.EstructurasLinealess;

namespace ProgramacionIII_Unidad1
{
    /// <summary>
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio : Page
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void BtnAbrirOrdenamiento_Click(object sender, RoutedEventArgs e)
        {

            AlgoritmosDeOrdenamiento paginaOrdenamiento = new AlgoritmosDeOrdenamiento();

            this.NavigationService.Navigate(paginaOrdenamiento);
        }

        private void BtnAbrirBusqueda_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new AlgoritmosDeBusqueda());
        }

        private void EstructurasLinealesBtn_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new EstructurasLineales());
        }
    }
}
