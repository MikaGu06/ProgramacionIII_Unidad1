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
    /// Interaction logic for AlgoritmosDeOrdenamiento.xaml
    /// </summary>
    public partial class AlgoritmosDeOrdenamiento : Page
    {
        List<int> vectorDinamico = new List<int>();
        List<int> vectorOrdenado = new List<int>();

        public AlgoritmosDeOrdenamiento()
        {
            InitializeComponent();
        }



        private void BtnGenerar_Click(object sender, RoutedEventArgs e) 
        {
            int n = int.Parse(TxtBxSizeArreglo.Text);
            Random rnd = new Random();
            vectorDinamico.Clear();

            for (int i = 0; i < n; i++)
            {
                vectorDinamico.Add(rnd.Next(1, 100));
            }

            ICVectorOriginal.ItemsSource = null;
            ICVectorOriginal.ItemsSource = vectorDinamico;
        }
        private void BtnAgregar_Click(object sender, RoutedEventArgs e) 
        {
            int valor = int.Parse(TxtBxIngresarVector.Text);

            vectorDinamico.Add(valor);

            ICVectorOriginal.ItemsSource = null;
            ICVectorOriginal.ItemsSource = vectorDinamico;

            TxtBxIngresarVector.Text = "";

        }
        private void BtnSeleccion_Click(object sender, RoutedEventArgs e) 
        {
            // Algoritmo de selección
            for (int i = 0; i < vectorDinamico.Count; i++)
            {
                int indiceMinimo = i;
                for (int j = i + 1; j < vectorDinamico.Count; j++)
                {
                    // Lógica para decidir si es Ascendente o Descendente según el RadioButton
                    if (radBtnAsc.IsChecked == true)
                    {
                        if (vectorDinamico[j] < vectorDinamico[indiceMinimo])
                        {
                            indiceMinimo = j;
                        }
                    }
                    else // Caso Descendente
                    {
                        if (vectorDinamico[j] > vectorDinamico[indiceMinimo])
                        {
                            indiceMinimo = j;
                        }
                    }
                }
                int temp = vectorDinamico[i];
                vectorDinamico[i] = vectorDinamico[indiceMinimo];
                vectorDinamico[indiceMinimo] = temp;
            }
            // Transferir resultados al vector ordenado
            vectorOrdenado.Clear();
            vectorOrdenado.AddRange(vectorDinamico);

            // Actualizar la interfaz (Vector Ordenado)
            ICVectorOrdenado.ItemsSource = null;
            ICVectorOrdenado.ItemsSource = vectorOrdenado;

            // Mostrar el panel de resultados
            PanelResultado.Visibility = Visibility.Visible;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
