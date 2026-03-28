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
        List<int> vectorOrdenadoAscendente = new List<int>();
        List<int> vectorOrdenadoDescendente = new List<int>();

        Random rand = new Random();

        public AlgoritmosDeOrdenamiento()
        {
            InitializeComponent();
        }

        
        private void BtnAgregar_Click(object sender, RoutedEventArgs e) 
        {

            try
            {

                if (string.IsNullOrWhiteSpace(TxtBxIngresarVector.Text))
                {
                    throw new FormatException("El campo de texto está vacío.");
                }


                int valor = int.Parse(TxtBxIngresarVector.Text);

                if (valor < 0)
                {

                    throw new ArgumentException("No se permiten números negativos. Ingresa solo valores positivos.");
                }

                vectorDinamico.Add(valor);


                ICVectorOriginal.ItemsSource = null;
                ICVectorOriginal.ItemsSource = vectorDinamico;


                TxtBxIngresarVector.Text = "";
                TxtBxIngresarVector.Focus();
            }
            catch (FormatException)
            {

                MessageBox.Show("Por favor, ingresa un número entero válido. No se permiten letras, decimales ni espacios vacíos.",
                                "Error de Formato", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (OverflowException)
            {

                MessageBox.Show("El número ingresado es demasiado grande. Por favor, ingresa un número más pequeño (menor a 2,147,483,647).",
                                "Número Fuera de Límite", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (ArgumentException arg)
            {

                MessageBox.Show(arg.Message, "Valor Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}",
                                "Error Desconocido", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void BtnSeleccion_Click(object sender, RoutedEventArgs e) 
        {
            // Algoritmo de selección
            for (int i = 0; i < vectorDinamico.Count; i++)
            {
                int indiceMinimo = i;
                for (int j = i + 1; j < vectorDinamico.Count; j++)
                {

                    if (radBtnAsc.IsChecked == true)
                    {
                        if (vectorDinamico[j] < vectorDinamico[indiceMinimo])
                        {
                            indiceMinimo = j;
                        }
                    }
                    else
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

            vectorOrdenadoAscendente.Clear();
            vectorOrdenadoAscendente.AddRange(vectorDinamico);


            ICVectorOrdenado.ItemsSource = null;
            ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

            TxtComplexity.Text = "Complejidad: O(n²)";

        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void BtnInsercion_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < vectorDinamico.Count; i++)
            {
                int valorActual = vectorDinamico[i];
                int j = i -1;

                if (radBtnAsc.IsChecked == true)
                {
                     while (j >= 0 && vectorDinamico[j] > valorActual)
                     {
                        vectorDinamico[j + 1] = vectorDinamico[j];
                        j--;
                     }
                }
                else
                {
                    while (j >= 0 && vectorDinamico[j] < valorActual)
                    {
                        vectorDinamico[j + 1] = vectorDinamico[j];
                        j--;
                    }
                }
               
                vectorDinamico[j + 1] = valorActual;
            }
            vectorOrdenadoAscendente.Clear();
            vectorOrdenadoAscendente.AddRange(vectorDinamico);

            ICVectorOrdenado.ItemsSource = null;
            ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

            TxtComplexity.Text = "Complejidad: O(n²)";

        }

        private void BtnIntercambio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBurbuja_Click(object sender, RoutedEventArgs e)
        {
            int t;
            for (int i = 1; i < vectorDinamico.Count; i++)
                for (int j = vectorDinamico.Count - 1; j >= i; j--)
                {
                    if (radBtnAsc.IsChecked == true)
                    {
                        if (vectorDinamico[j - 1] > vectorDinamico[j])
                        {
                            t = vectorDinamico[j - 1];
                            vectorDinamico[j - 1] = vectorDinamico[j];
                            vectorDinamico[j] = t;
                        }

                    }
                    else
                    {
                        if (vectorDinamico[j - 1] < vectorDinamico[j])
                        {
                            t = vectorDinamico[j - 1];
                            vectorDinamico[j - 1] = vectorDinamico[j];
                            vectorDinamico[j] = t;
                        }

                    }

                }
            vectorOrdenadoAscendente.Clear();
            vectorOrdenadoAscendente.AddRange(vectorDinamico);

            ICVectorOrdenado.ItemsSource = null;
            ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

            TxtComplexity.Text = "Complejidad: O(n²)";

        }

        private void BtnQuickSort_Click(object sender, RoutedEventArgs e)
        {
            // Validacion
            if (vectorDinamico.Count > 0)
            {

                int[] arr = vectorDinamico.ToArray();

                // método
                quicksort(arr, 0, arr.Length - 1);


                vectorOrdenadoAscendente.Clear();
                vectorOrdenadoAscendente.AddRange(arr);


                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(n log n)";
            }
        }


        private void quicksort(int[] vector, int primero, int ultimo)
        {
            int i, j, central;
            double pivote;
            central = (primero + ultimo) / 2;
            pivote = vector[central];
            i = primero;
            j = ultimo;

            do
            {

                if (radBtnAsc.IsChecked == true)
                {
                    while (vector[i] < pivote) i++;
                    while (vector[j] > pivote) j--;
                }
                else
                {
                    while (vector[i] > pivote) i++;
                    while (vector[j] < pivote) j--;
                }

                if (i <= j)
                {
                    int temp;
                    temp = vector[i];
                    vector[i] = vector[j];
                    vector[j] = temp;
                    i++;
                    j--;
                }
            } while (i <= j);

            if (primero < j)
            {
                quicksort(vector, primero, j);
            }
            if (i < ultimo)
            {
                quicksort(vector, i, ultimo);
            }

        }

        //MERGE SORT

        private void BtnMergeSort_Click(object sender, RoutedEventArgs e)
        {
            if (vectorDinamico.Count > 0)
            {
                int[] arr = vectorDinamico.ToArray();

                mergesort(arr, 0, arr.Length - 1);

                vectorOrdenadoAscendente.Clear();
                vectorOrdenadoAscendente.AddRange(arr);

                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(n log n)";
            }
        }

        // MÉTODO RECURSIVO PARA DIVIDIR
        private void mergesort(int[] vector, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int medio = (inicio + fin) / 2;

                mergesort(vector, inicio, medio);
                mergesort(vector, medio + 1, fin);

                merge(vector, inicio, medio, fin);
            }
        }
            
            private void merge(int[] vector, int inicio, int medio, int fin)
            {   
                int n1 = medio - inicio + 1;
                int n2 = fin - medio;

                int[] izq = new int[n1];
                int[] der = new int[n2];

                for (int i = 0; i < n1; i++) izq[i] = vector[inicio + i];
                for (int j = 0; j < n2; j++) der[j] = vector[medio + 1 + j];

                int i_izq = 0, j_der = 0;
                int k = inicio;

                while (i_izq < n1 && j_der < n2)
            {

                if (radBtnAsc.IsChecked == true)
                {
                    if (izq[i_izq] <= der[j_der])
                    {
                        vector[k] = izq[i_izq];
                        i_izq++;
                    }
                    else
                    {
                        vector[k] = der[j_der];
                        j_der++;
                    }
                }
                else
                {
                    if (izq[i_izq] >= der[j_der])
                    {
                        vector[k] = izq[i_izq];
                        i_izq++;
                    }
                    else
                    {
                        vector[k] = der[j_der];
                        j_der++;
                    }
                }
                k++;
            }

            // Copiar los elementos restantes
            while (i_izq < n1)
            {
                vector[k] = izq[i_izq];
                i_izq++;
                k++;
            }

            while (j_der < n2)
            {
                vector[k] = der[j_der];
                j_der++;
                k++;
            }
        }

        private void BtnRadixSort_Click(object sender, RoutedEventArgs e)
        {
            if (vectorDinamico.Count > 0)
            {
                int[] arr = vectorDinamico.ToArray();
                int n = arr.Length;

                // Llamada al método
                radixsort(arr, n);

                vectorOrdenadoAscendente.Clear();
                vectorOrdenadoAscendente.AddRange(arr);

                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(nk)";

            }
        }

        // MÉTODO PARA OBTENER EL VALOR MÁXIMO
        private int obtenerMax(int[] arr, int n)
        {
            int mx = arr[0];
            for (int i = 1; i < n; i++)
                if (arr[i] > mx) mx = arr[i];
            return mx;
        }

        // MÉTODO PRINCIPAL RADIX SORT
        private void radixsort(int[] arr, int n)
        {
            int m = obtenerMax(arr, n);

            // Se ejecuta el conteo por cada dígito
            // exp es 10^i donde i es el dígito actual
            for (int exp = 1; m / exp > 0; exp *= 10)
            {
                conteoParaRadix(arr, n, exp);
            }
        }

        // MÉTODO DE CONTEO
        private void conteoParaRadix(int[] arr, int n, int exp)
        {
            int[] salida = new int[n];
            int[] conteo = new int[10];
            int i;

            // Almacenar el conteo de ocurrencias de los dígitos
            for (i = 0; i < n; i++)
            {
                int digito = (arr[i] / exp) % 10;


                if (radBtnAsc.IsChecked == false)
                {
                    digito = 9 - digito;
                }

                conteo[digito]++;
            }

            for (i = 1; i < 10; i++)
            {
                conteo[i] += conteo[i - 1];
            }

            for (i = n - 1; i >= 0; i--)
            {
                int digito = (arr[i] / exp) % 10;

                if (radBtnAsc.IsChecked == false)
                {
                    digito = 9 - digito;
                }

                salida[conteo[digito] - 1] = arr[i];
                conteo[digito]--;
            }

            for (i = 0; i < n; i++)
            {
                arr[i] = salida[i];
            }
        }

        private void BtnBucketSort_Click(object sender, RoutedEventArgs e)
        {
            if (vectorDinamico.Count > 0)
            {
                //máximo y mínimo
                int valorMax = vectorDinamico[0];
                int valorMin = vectorDinamico[0];
                foreach (int n in vectorDinamico)
                {
                    if (n > valorMax) valorMax = n;
                    if (n < valorMin) valorMin = n;
                }

                // Buckets = Numero de elementos
                int numeroDeCubetas = vectorDinamico.Count;
                List<int>[] cubetas = new List<int>[numeroDeCubetas];

                for (int i = 0; i < numeroDeCubetas; i++)
                {
                    cubetas[i] = new List<int>();
                }

                // Distribución
                double rango = (double)(valorMax - valorMin + 1) / numeroDeCubetas;
                foreach (int num in vectorDinamico)
                {
                    int indiceCubeta = (int)((num - valorMin) / rango);
                    if (indiceCubeta >= numeroDeCubetas) indiceCubeta = numeroDeCubetas - 1;
                    cubetas[indiceCubeta].Add(num);
                }


                vectorOrdenadoAscendente.Clear();

                if (radBtnAsc.IsChecked == true)
                {

                    for (int i = 0; i < numeroDeCubetas; i++)
                    {
                        cubetas[i].Sort();
                        vectorOrdenadoAscendente.AddRange(cubetas[i]);
                    }
                }
                else
                {
                    for (int i = numeroDeCubetas - 1; i >= 0; i--)
                    {
                        cubetas[i].Sort();
                        cubetas[i].Reverse();
                        vectorOrdenadoAscendente.AddRange(cubetas[i]);
                    }
                }

                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(n + k)";

            }

        }

        private void BtnShell_Click(object sender, RoutedEventArgs e)
        {
            if (vectorDinamico.Count > 0)
            {
                int n = vectorDinamico.Count;

                // gap = espacio
                for (int gap = n / 2; gap > 0; gap /= 2)
                {
                    // insercion al gap
                    for (int i = gap; i < n; i++)
                    {
                        int temp = vectorDinamico[i];
                        int j = i;

                        
                        if (radBtnAsc.IsChecked == true)
                        {
                            while (j >= gap && vectorDinamico[j - gap] > temp)
                            {
                                vectorDinamico[j] = vectorDinamico[j - gap];
                                j -= gap;
                            }
                        }
                        else
                        {
                            while (j >= gap && vectorDinamico[j - gap] < temp)
                            {
                                vectorDinamico[j] = vectorDinamico[j - gap];
                                j -= gap;
                            }
                        }

                        //temp
                        vectorDinamico[j] = temp;
                    }
                }

                vectorOrdenadoAscendente.Clear();
                vectorOrdenadoAscendente.AddRange(vectorDinamico);

                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(n²)";

            }

        }

        private void BtnMonticulo_Click(object sender, RoutedEventArgs e)
        {
            if (vectorDinamico.Count > 0)
            {
                int[] arr = vectorDinamico.ToArray();
                int n = arr.Length;

                // montículo
                for (int i = n / 2 - 1; i >= 0; i--)
                {
                    Heapify(arr, n, i);
                }

                for (int i = n - 1; i > 0; i--)
                {
                    // Movemos la raíz actual al final
                    int temp = arr[0];
                    arr[0] = arr[i];
                    arr[i] = temp;

                    Heapify(arr, i, 0);
                }

                vectorOrdenadoAscendente.Clear();
                vectorOrdenadoAscendente.AddRange(arr);

                ICVectorOrdenado.ItemsSource = null;
                ICVectorOrdenado.ItemsSource = vectorOrdenadoAscendente;

                TxtComplexity.Text = "Complejidad: O(n log n)";

            }
        }
        private void Heapify(int[] arr, int n, int i)
        {
            int extremo = i; 
            int izq = 2 * i + 1; // hijo izquierdo
            int der = 2 * i + 2; // hijo derecho

            if (radBtnAsc.IsChecked == true)
            {
                if (izq < n && arr[izq] > arr[extremo])
                    extremo = izq;

                if (der < n && arr[der] > arr[extremo])
                    extremo = der;
            }
            else
            {
                if (izq < n && arr[izq] < arr[extremo])
                    extremo = izq;

                if (der < n && arr[der] < arr[extremo])
                    extremo = der;
            }

            if (extremo != i)
            {
                int swap = arr[i];
                arr[i] = arr[extremo];
                arr[extremo] = swap;

                Heapify(arr, n, extremo);
            }
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            int numeroAleatorio = rand.Next(0, 1000);

            // Agrega el número al vector principal
            vectorDinamico.Add(numeroAleatorio);

            // Actualiza la interfaz gráfica para que aparezca la cajita con el número
            ICVectorOriginal.ItemsSource = null;
            ICVectorOriginal.ItemsSource = vectorDinamico;
        }
    }
}
