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
using ProgramacionIII_Unidad1.EstructurasNoLineales;

namespace ProgramacionIII_Unidad1

{

    public partial class Inicio : Page
    {
        public Inicio()

        {
            InitializeComponent();

        }

        private void BtnAbrirOrdenamiento_Click(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new AlgoritmosDeOrdenamiento());

        }

        private void BtnAbrirBusqueda_Click(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new AlgoritmosDeBusqueda());

        }

        private void EstructurasLinealesBtn_Click(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new EstructurasLineales());

        }

        private void BtnAbrirRecursividad_Click(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new Recursividad());

        }

        private void BtnAbrirArboles_Click(object sender, RoutedEventArgs e)

        {


            this.NavigationService.Navigate(new ArbolBinario());

        }


        private void BtnAbrirGrafos_Click(object sender, RoutedEventArgs e)

        {

            this.NavigationService.Navigate(new Grafos());

        }

    }

}