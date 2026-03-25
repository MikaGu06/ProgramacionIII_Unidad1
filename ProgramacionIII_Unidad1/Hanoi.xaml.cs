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
    public partial class Hanoi : UserControl
    {
        private int contadorPasos = 0;

        public Hanoi()
        {
            InitializeComponent();
        }

        private async void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtDiscos.Text, out int numeroDeDiscos) && numeroDeDiscos > 0 && numeroDeDiscos <= 8)
            {
                // Limpiar torres
                LimpiarTorreRecursivo(TorreA, TorreA.Children.Count - 1);
                LimpiarTorreRecursivo(TorreB, TorreB.Children.Count - 1);
                LimpiarTorreRecursivo(TorreC, TorreC.Children.Count - 1);

                contadorPasos = 0;
                lblPasos.Text = "Pasos realizados: 0";

                // Crear discos
                CrearDiscosRecursivo(numeroDeDiscos);

                // Resolver Hanoi
                await ResolverHanoiRecursivoAsync(numeroDeDiscos, TorreA, TorreB, TorreC);

                MessageBox.Show("¡Rompecabezas completado con " + contadorPasos + " movimientos!");
            }
            else
            {
                MessageBox.Show("Ingresa un número entre 1 y 8");
            }
        }

        // --- CREAR DISCOS ---
        private void CrearDiscosRecursivo(int discoActual)
        {
            if (discoActual == 0) return;

            Border disco = new Border
            {
                Height = 20,
                Width = 40 + (discoActual * 25),
                Background = ConvertirNumeroAColor(discoActual),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 2, 0, 0),
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1)
            };

            TorreA.Children.Add(disco);

            CrearDiscosRecursivo(discoActual - 1);
        }

        // --- LIMPIAR TORRES ---
        private void LimpiarTorreRecursivo(StackPanel torre, int indice)
        {
            if (indice < 0) return;

            torre.Children.RemoveAt(indice);
            LimpiarTorreRecursivo(torre, indice - 1);
        }

        // --- HANOI ---
        private async Task ResolverHanoiRecursivoAsync(int n, StackPanel origen, StackPanel auxiliar, StackPanel destino)
        {
            if (n == 0) return;

            await ResolverHanoiRecursivoAsync(n - 1, origen, destino, auxiliar);

            MoverDisco(origen, destino);
            await Task.Delay(1000); // velocidad más lenta

            await ResolverHanoiRecursivoAsync(n - 1, auxiliar, origen, destino);
        }

        private void MoverDisco(StackPanel origen, StackPanel destino)
        {
            if (origen.Children.Count > 0)
            {
                int indice = origen.Children.Count - 1;
                UIElement disco = origen.Children[indice];

                origen.Children.RemoveAt(indice);
                destino.Children.Add(disco);

                contadorPasos++;
                lblPasos.Text = "Pasos realizados: " + contadorPasos;
            }
        }

        // --- COLORES PASTEL ---
        private SolidColorBrush ConvertirNumeroAColor(int n)
        {
            switch (n % 6)
            {
                case 0: return new SolidColorBrush(Color.FromRgb(255, 179, 186));
                case 1: return new SolidColorBrush(Color.FromRgb(255, 223, 186));
                case 2: return new SolidColorBrush(Color.FromRgb(255, 255, 186));
                case 3: return new SolidColorBrush(Color.FromRgb(186, 255, 201));
                case 4: return new SolidColorBrush(Color.FromRgb(186, 225, 255));
                default: return new SolidColorBrush(Color.FromRgb(218, 191, 255));
            }
        }
    }
}