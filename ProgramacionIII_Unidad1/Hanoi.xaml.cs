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

                // Crear discos en TorreA con la piramide correcta:
                // Se llama la recursion PRIMERO y luego se agrega el disco actual,
                // asi el disco 1 (pequeño) queda en Children[0] (arriba visual)
                // y el disco n (grande) queda en Children[n-1] (abajo visual)
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
        // Se llama la recursion PRIMERO para que los discos pequeños
        // queden arriba (índices bajos) y los grandes abajo (índices altos).
        // Ejemplo con n=3:
        //   CrearDiscosRecursivo(3) → llama CrearDiscosRecursivo(2) → llama CrearDiscosRecursivo(1)
        //     → llama CrearDiscosRecursivo(0) → retorna
        //     → agrega disco 1 (pequeño) → Children[0] = TOP visual
        //   → agrega disco 2 → Children[1]
        // → agrega disco 3 (grande) → Children[2] = BOTTOM visual  ✓ pirámide correcta
        private void CrearDiscosRecursivo(int discoActual)
        {
            if (discoActual == 0) return;

            CrearDiscosRecursivo(discoActual - 1); // primero los discos más pequeños

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

            TorreA.Children.Add(disco); // el disco actual va abajo del anterior
        }

        // --- LIMPIAR TORRES ---
        private void LimpiarTorreRecursivo(StackPanel torre, int indice)
        {
            if (indice < 0) return;

            torre.Children.RemoveAt(indice);
            LimpiarTorreRecursivo(torre, indice - 1);
        }

        // --- HANOI RECURSIVO ---
        // Igual al algoritmo del PDF:
        //   hanoi(n, origen, destino, auxiliar)
        //   si n==1: mover disco de origen a destino
        //   si no:
        //     hanoi(n-1, origen, auxiliar, destino)   ← mover n-1 discos a auxiliar
        //     mover disco n de origen a destino
        //     hanoi(n-1, auxiliar, destino, origen)   ← mover n-1 discos al destino
        private async Task ResolverHanoiRecursivoAsync(int n, StackPanel origen, StackPanel auxiliar, StackPanel destino)
        {
            if (n == 0) return;

            await ResolverHanoiRecursivoAsync(n - 1, origen, destino, auxiliar);

            MoverDisco(origen, destino);
            await Task.Delay(1000);

            await ResolverHanoiRecursivoAsync(n - 1, auxiliar, origen, destino);
        }

        // --- MOVER DISCO ---
        // El disco del TOPE de la torre es Children[0] (el más pequeño disponible,
        // visualmente arriba). Se inserta en Children[0] del destino para que
        // quede en el tope de esa torre también.
        private void MoverDisco(StackPanel origen, StackPanel destino)
        {
            if (origen.Children.Count > 0)
            {
                // Tomar el disco del tope: Children[0] = disco más pequeño = arriba visual
                UIElement disco = origen.Children[0];

                origen.Children.RemoveAt(0);

                // Insertar en el tope del destino: Insert(0, ...) lo pone arriba
                destino.Children.Insert(0, disco);

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
