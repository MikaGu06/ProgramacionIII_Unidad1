using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgramacionIII_Unidad1
{
    public partial class AlgoritmosDeBusqueda : Page
    {
        private class ElementoVector
        {
            public int Id { get; set; }
            public int Valor { get; set; }
        }

        private List<ElementoVector> vectorDinamico = new List<ElementoVector>();
        private int siguienteId = 1;
        private Random random = new Random();

        public AlgoritmosDeBusqueda()
        {
            try
            {
                InitializeComponent();
                ActualizarVector();
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al cargar la ventana de búsqueda.", ex);
            }
        }

        private void MostrarError(string mensaje, Exception ex)
        {
            MessageBox.Show(mensaje + "\n\nDetalle: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ActualizarVector()
        {
            try
            {
                ICVector.ItemsSource = null;
                ICVector.ItemsSource = vectorDinamico;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo actualizar el vector en pantalla.", ex);
            }
        }

        private bool ObtenerNumero(TextBox caja, out int valor, string mensaje)
        {
            try
            {
                if (caja == null)
                {
                    valor = 0;
                    MessageBox.Show("No se encontró la caja de texto.", "Dato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(caja.Text, out valor))
                {
                    MessageBox.Show(mensaje, "Dato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    caja.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                valor = 0;
                MostrarError("Ocurrió un error al leer el número ingresado.", ex);
                return false;
            }
        }

        private void MostrarResultado(bool encontrado, int posicion, string complejidad, string detalle)
        {
            try
            {
                TxtComplejidad.Text = complejidad;
                TxtDetalle.Text = detalle;

                if (encontrado)
                {
                    TxtResultado.Text = "✓ Encontrado en posición " + (posicion + 1);

                    ResultadoBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1ECE3"));
                    TxtResultado.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262233"));
                }
                else
                {
                    TxtResultado.Text = "✗ No encontrado";

                    ResultadoBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8A4D67"));
                    TxtResultado.Foreground = Brushes.White;
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo mostrar el resultado de la búsqueda.", ex);
            }
        }

        private void ResetBotones()
        {
            try
            {
                Button[] botones = { BtnLineal, BtnCentinela, BtnBinaria, BtnIndexada, BtnHashing };

                foreach (Button boton in botones)
                {
                    if (boton != null)
                    {
                        boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E2F2"));
                        boton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262233"));
                        boton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDD8E7"));
                        boton.BorderThickness = new Thickness(1);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo restablecer el estado de los botones.", ex);
            }
        }

        private void ActivarBoton(Button boton)
        {
            try
            {
                ResetBotones();

                if (boton != null)
                {
                    boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F73A1"));
                    boton.Foreground = Brushes.White;
                    boton.BorderBrush = Brushes.Transparent;
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo activar el botón seleccionado.", ex);
            }
        }

        private List<int> CopiarValores()
        {
            try
            {
                List<int> copia = new List<int>();

                for (int i = 0; i < vectorDinamico.Count; i++)
                {
                    copia.Add(vectorDinamico[i].Valor);
                }

                return copia;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo copiar el vector.", ex);
                return new List<int>();
            }
        }

        private void OrdenarAscendente(List<int> lista)
        {
            try
            {
                if (lista == null)
                {
                    throw new Exception("La lista a ordenar es nula.");
                }

                for (int i = 0; i < lista.Count; i++)
                {
                    int indiceMinimo = i;

                    for (int j = i + 1; j < lista.Count; j++)
                    {
                        if (lista[j] < lista[indiceMinimo])
                        {
                            indiceMinimo = j;
                        }
                    }

                    int temp = lista[i];
                    lista[i] = lista[indiceMinimo];
                    lista[indiceMinimo] = temp;
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo ordenar el vector.", ex);
            }
        }

        private int BuscarSinCentinela(int valor)
        {
            try
            {
                int i = 0;
                bool encontrado = false;

                while (i < vectorDinamico.Count && !encontrado)
                {
                    if (vectorDinamico[i].Valor == valor)
                    {
                        encontrado = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (encontrado)
                {
                    return i;
                }

                return -1;
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error en la búsqueda lineal sin centinela.", ex);
                return -1;
            }
        }

        private int BuscarConCentinela(int valor)
        {
            try
            {
                if (vectorDinamico.Count == 0)
                {
                    return -1;
                }

                vectorDinamico.Add(new ElementoVector { Id = siguienteId++, Valor = valor });

                int i = 0;
                while (vectorDinamico[i].Valor != valor)
                {
                    i++;
                }

                vectorDinamico.RemoveAt(vectorDinamico.Count - 1);

                if (i < vectorDinamico.Count)
                {
                    return i;
                }

                return -1;
            }
            catch (Exception ex)
            {
                if (vectorDinamico.Count > 0 && vectorDinamico[vectorDinamico.Count - 1].Valor == valor)
                {
                    vectorDinamico.RemoveAt(vectorDinamico.Count - 1);
                }

                MostrarError("Ocurrió un error en la búsqueda lineal con centinela.", ex);
                return -1;
            }
        }

        private int BuscarBinaria(int valor, List<int> ordenado)
        {
            try
            {
                int izquierda = 0;
                int derecha = ordenado.Count - 1;

                while (izquierda <= derecha)
                {
                    int medio = (izquierda + derecha) / 2;

                    if (ordenado[medio] == valor)
                    {
                        return medio;
                    }
                    else if (valor < ordenado[medio])
                    {
                        derecha = medio - 1;
                    }
                    else
                    {
                        izquierda = medio + 1;
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error en la búsqueda binaria.", ex);
                return -1;
            }
        }

        private int BuscarIndexada(int valor, List<int> ordenado)
        {
            try
            {
                if (ordenado.Count == 0)
                {
                    return -1;
                }

                int bloque = (int)Math.Sqrt(ordenado.Count);
                if (bloque == 0)
                {
                    bloque = 1;
                }

                int inicio = 0;
                int fin = bloque - 1;

                if (fin >= ordenado.Count)
                {
                    fin = ordenado.Count - 1;
                }

                while (inicio < ordenado.Count && ordenado[fin] < valor)
                {
                    inicio = inicio + bloque;

                    if (inicio >= ordenado.Count)
                    {
                        return -1;
                    }

                    fin = inicio + bloque - 1;
                    if (fin >= ordenado.Count)
                    {
                        fin = ordenado.Count - 1;
                    }
                }

                for (int i = inicio; i <= fin; i++)
                {
                    if (ordenado[i] == valor)
                    {
                        return i;
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error en la búsqueda indexada.", ex);
                return -1;
            }
        }

        private int BuscarHashing(int valor)
        {
            try
            {
                Dictionary<int, int> tabla = new Dictionary<int, int>();

                for (int i = 0; i < vectorDinamico.Count; i++)
                {
                    if (!tabla.ContainsKey(vectorDinamico[i].Valor))
                    {
                        tabla.Add(vectorDinamico[i].Valor, i);
                    }
                }

                if (tabla.ContainsKey(valor))
                {
                    return tabla[valor];
                }

                return -1;
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error en la búsqueda por hashing.", ex);
                return -1;
            }
        }

        private int ObtenerPosicionOriginal(int valor)
        {
            try
            {
                for (int i = 0; i < vectorDinamico.Count; i++)
                {
                    if (vectorDinamico[i].Valor == valor)
                    {
                        return i;
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo obtener la posición original del elemento.", ex);
                return -1;
            }
        }

        private bool ValidarBusqueda(out int valor)
        {
            try
            {
                if (vectorDinamico.Count == 0)
                {
                    MessageBox.Show("Primero agrega elementos al vector.", "Vector vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                    valor = 0;
                    return false;
                }

                return ObtenerNumero(TxtBuscar, out valor, "Ingresa un valor numérico para buscar.");
            }
            catch (Exception ex)
            {
                valor = 0;
                MostrarError("No se pudo validar la búsqueda.", ex);
                return false;
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int valor;

                if (!ObtenerNumero(TxtElemento, out valor, "Ingresa un número válido para agregar."))
                {
                    return;
                }

                vectorDinamico.Add(new ElementoVector { Id = siguienteId++, Valor = valor });
                TxtElemento.Clear();
                TxtElemento.Focus();
                ActualizarVector();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo agregar el elemento al vector.", ex);
            }
        }

        private void BtnAgregarAleatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int valor = random.Next(1, 101);
                vectorDinamico.Add(new ElementoVector { Id = siguienteId++, Valor = valor });
                ActualizarVector();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo agregar un elemento aleatorio al vector.", ex);
            }
        }

        private void BtnLineal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnLineal);

                int valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                int posicion = BuscarSinCentinela(valor);
                MostrarResultado(posicion >= 0, posicion, "O(n)", "Búsqueda lineal sin centinela: recorre el vector desde la posición 0 hasta encontrar el valor.");
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al ejecutar la búsqueda lineal.", ex);
            }
        }

        private void BtnCentinela_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnCentinela);

                int valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                int posicion = BuscarConCentinela(valor);
                MostrarResultado(posicion >= 0, posicion, "O(n)", "Búsqueda lineal con centinela: agrega temporalmente el valor al final y luego realiza la búsqueda.");
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al ejecutar la búsqueda con centinela.", ex);
            }
        }

        private void BtnBinaria_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnBinaria);

                int valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                List<int> ordenado = CopiarValores();
                OrdenarAscendente(ordenado);

                int posicionOrdenada = BuscarBinaria(valor, ordenado);
                int posicionOriginal = -1;

                if (posicionOrdenada >= 0)
                {
                    posicionOriginal = ObtenerPosicionOriginal(valor);
                }

                string detalle = "Búsqueda binaria: primero ordena el vector y luego divide el rango en mitades. Posición mostrada según el vector original.";
                MostrarResultado(posicionOriginal >= 0, posicionOriginal, "O(log n)", detalle);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al ejecutar la búsqueda binaria.", ex);
            }
        }

        private void BtnIndexada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnIndexada);

                int valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                List<int> ordenado = CopiarValores();
                OrdenarAscendente(ordenado);

                int posicionOrdenada = BuscarIndexada(valor, ordenado);
                int posicionOriginal = -1;

                if (posicionOrdenada >= 0)
                {
                    posicionOriginal = ObtenerPosicionOriginal(valor);
                }

                string detalle = "Búsqueda indexada: ordena el vector, revisa por bloques y luego busca dentro del bloque correspondiente. Posición mostrada según el vector original.";
                MostrarResultado(posicionOriginal >= 0, posicionOriginal, "O(√n)", detalle);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al ejecutar la búsqueda indexada.", ex);
            }
        }

        private void BtnHashing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnHashing);

                int valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                int posicion = BuscarHashing(valor);
                MostrarResultado(posicion >= 0, posicion, "O(1) promedio", "Hashing: guarda cada valor con su posición en una tabla para acceder de forma directa.");
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al ejecutar la búsqueda por hashing.", ex);
            }
        }

        private void BtnEliminarElemento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button boton = sender as Button;
                if (boton == null)
                {
                    return;
                }

                ElementoVector elemento = boton.Tag as ElementoVector;
                if (elemento == null)
                {
                    return;
                }

                vectorDinamico.Remove(elemento);
                ActualizarVector();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo eliminar el elemento seleccionado del vector.", ex);
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NavigationService != null && NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("No hay una página anterior para volver.", "Navegación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo volver a la página anterior.", ex);
            }
        }
    }
}