using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgramacionIII_Unidad1
{
    public partial class Hanoi : UserControl
    {
        private int contadorPasos = 0;
        private bool ejecutando = false;

        public Hanoi()
        {
            InitializeComponent();
        }

        private int ObtenerVelocidad()
        {
            if (rbLento.IsChecked == true)
                return 1000;  
            else if (rbRapido.IsChecked == true)
                return 100;
            else
                return 500;  
        }

        private async void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (ejecutando)
            {
                MessageBox.Show("Ya hay una animación en progreso. Espera a que termine.",
                              "En progreso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (int.TryParse(txtDiscos.Text, out int numeroDeDiscos) && numeroDeDiscos > 0 && numeroDeDiscos <= 8)
            {
                ejecutando = true;

                // Limpiar torres recursivamente
                LimpiarTorreRecursivo(TorreA, TorreA.Children.Count - 1);
                LimpiarTorreRecursivo(TorreB, TorreB.Children.Count - 1);
                LimpiarTorreRecursivo(TorreC, TorreC.Children.Count - 1);

                contadorPasos = 0;
                lblPasos.Text = "Pasos: 0";

                // Crear discos recursivamente en TorreA
                CrearDiscosRecursivo(numeroDeDiscos);

                // Limpiar el TextBox al empezar
                txtDiscos.Text = string.Empty;

                // Resolver Hanoi
                await ResolverHanoiRecursivoAsync(numeroDeDiscos, TorreA, TorreB, TorreC);

                MessageBox.Show($"¡Rompecabezas completado con {contadorPasos} movimientos!",
                                "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                ejecutando = false;

                // Limpiar el TextBox al terminar
                txtDiscos.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Ingresa un número entre 1 y 8", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                // Limpiar el TextBox si hay error
                txtDiscos.Text = string.Empty;
            }
        }

        private void CrearDiscosRecursivo(int discoActual)
        {
            if (discoActual == 0) return;

            CrearDiscosRecursivo(discoActual - 1);

            Border disco = new Border
            {
                Height = 22,
                Width = 40 + (discoActual * 20),
                Background = ConvertirNumeroAColor(discoActual),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 2, 0, 0),
                BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                BorderThickness = new Thickness(1)
            };

            TorreA.Children.Add(disco);
        }

        private void LimpiarTorreRecursivo(StackPanel torre, int indice)
        {
            if (indice < 0) return;

            torre.Children.RemoveAt(indice);
            LimpiarTorreRecursivo(torre, indice - 1);
        }

        private async Task ResolverHanoiRecursivoAsync(int n, StackPanel origen, StackPanel auxiliar, StackPanel destino)
        {
            if (n == 0) return;

            await ResolverHanoiRecursivoAsync(n - 1, origen, destino, auxiliar);

            MoverDisco(origen, destino);

            // Leer la velocidad actual en cada movimiento
            await Task.Delay(ObtenerVelocidad());

            await ResolverHanoiRecursivoAsync(n - 1, auxiliar, origen, destino);
        }

        private void MoverDisco(StackPanel origen, StackPanel destino)
        {
            if (origen.Children.Count > 0)
            {
                UIElement disco = origen.Children[0];
                origen.Children.RemoveAt(0);
                destino.Children.Insert(0, disco);

                contadorPasos++;
                lblPasos.Text = $"Pasos: {contadorPasos}";
            }
        }

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