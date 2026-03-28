using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualBasic;

namespace ProgramacionIII_Unidad1 
{
    public partial class AlgoritmosDeBusqueda : Page
    {
        private enum TipoVector
        {
            NoDefinido,
            Numeros,
            Letras
        }

        private class ElementoVector
        {
            public int Id { get; set; }
            public string Valor { get; set; }
        }

        private List<ElementoVector> vectorDinamico = new List<ElementoVector>();
        private int siguienteId = 1;
        private Random random = new Random();
        private TipoVector tipoVector = TipoVector.NoDefinido;
        private int capacidadMaxima = 10;

        public AlgoritmosDeBusqueda()
        {
            try
            {
                InitializeComponent();
                SolicitarCapacidadInicial();
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

        private void SolicitarCapacidadInicial()
        {
            try
            {
                string entrada = Interaction.InputBox("Ingresa el tamaño máximo del vector:", "Tamaño del vector", capacidadMaxima.ToString());

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    capacidadMaxima = 10;
                    return;
                }

                int capacidad;
                if (!int.TryParse(entrada, out capacidad) || capacidad <= 0)
                {
                    MessageBox.Show("Se usará el tamaño predeterminado de 10 porque el valor ingresado no es válido.", "Tamaño inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    capacidadMaxima = 10;
                    return;
                }

                capacidadMaxima = capacidad;
            }
            catch (Exception ex)
            {
                capacidadMaxima = 10;
                MostrarError("No se pudo establecer el tamaño inicial del vector.", ex);
            }
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

        private string NormalizarTexto(string texto)
        {
            if (texto == null)
            {
                return string.Empty;
            }

            return texto.Trim().ToUpper();
        }

        private bool EsSoloNumeros(string texto)
        {
            texto = NormalizarTexto(texto);

            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsDigit(texto[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool EsSoloLetras(string texto)
        {
            texto = NormalizarTexto(texto);

            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsLetter(texto[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private TipoVector DetectarTipo(string texto)
        {
            if (EsSoloNumeros(texto))
            {
                return TipoVector.Numeros;
            }

            if (EsSoloLetras(texto))
            {
                return TipoVector.Letras;
            }

            return TipoVector.NoDefinido;
        }

        private bool HayEspacioDisponible(int cantidadAAgregar)
        {
            if (cantidadAAgregar <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Cantidad inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (vectorDinamico.Count + cantidadAAgregar > capacidadMaxima)
            {
                MessageBox.Show("No hay suficiente espacio en el vector. Capacidad máxima: " + capacidadMaxima + ".", "Vector lleno", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool ValidarYPrepararTipo(string valor)
        {
            TipoVector tipoDetectado = DetectarTipo(valor);

            if (tipoDetectado == TipoVector.NoDefinido)
            {
                MessageBox.Show("Solo se permiten letras o números. No mezcles caracteres especiales, espacios ni ambos tipos.", "Dato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (tipoVector == TipoVector.NoDefinido)
            {
                tipoVector = tipoDetectado;
                return true;
            }

            if (tipoVector != tipoDetectado)
            {
                if (tipoVector == TipoVector.Numeros)
                {
                    MessageBox.Show("Este vector es numérico. No puedes ingresar letras.", "Tipo no permitido", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Este vector es de letras. No puedes ingresar números.", "Tipo no permitido", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                return false;
            }

            return true;
        }

        private bool ObtenerCantidadAleatoria(out int cantidad)
        {
            cantidad = 0;

            try
            {
                string entrada = Interaction.InputBox("¿Cuántos elementos quieres agregar aleatoriamente?", "Cantidad aleatoria", "1");

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    MessageBox.Show("Debes ingresar una cantidad.", "Dato requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(entrada, out cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingresa una cantidad válida mayor a cero.", "Cantidad inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo leer la cantidad aleatoria.", ex);
                return false;
            }
        }

        private bool ObtenerTipoParaAleatorio(out TipoVector tipoSeleccionado)
        {
            tipoSeleccionado = tipoVector;

            try
            {
                if (tipoVector != TipoVector.NoDefinido)
                {
                    return true;
                }

                string entrada = Interaction.InputBox("Escribe N para números o L para letras.", "Tipo de datos aleatorios", "N");
                entrada = NormalizarTexto(entrada);

                if (entrada == "N")
                {
                    tipoSeleccionado = TipoVector.Numeros;
                    return true;
                }

                if (entrada == "L")
                {
                    tipoSeleccionado = TipoVector.Letras;
                    return true;
                }

                MessageBox.Show("Debes elegir N para números o L para letras.", "Tipo inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo obtener el tipo para la carga aleatoria.", ex);
                return false;
            }
        }

        private string GenerarValorAleatorio(TipoVector tipo)
        {
            if (tipo == TipoVector.Numeros)
            {
                return random.Next(1, 101).ToString();
            }

            char letra = (char)random.Next('A', 'Z' + 1);
            return letra.ToString();
        }

        private bool ObtenerValorTexto(TextBox caja, out string valor, string mensaje)
        {
            valor = string.Empty;

            try
            {
                if (caja == null)
                {
                    MessageBox.Show("No se encontró la caja de texto.", "Dato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                valor = NormalizarTexto(caja.Text);

                if (string.IsNullOrWhiteSpace(valor))
                {
                    MessageBox.Show(mensaje, "Dato inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    caja.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al leer el dato ingresado.", ex);
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

        private List<string> CopiarValores()
        {
            try
            {
                List<string> copia = new List<string>();

                for (int i = 0; i < vectorDinamico.Count; i++)
                {
                    copia.Add(vectorDinamico[i].Valor);
                }

                return copia;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo copiar el vector.", ex);
                return new List<string>();
            }
        }

        private int CompararValores(string a, string b)
        {
            if (tipoVector == TipoVector.Numeros)
            {
                int numeroA = int.Parse(a);
                int numeroB = int.Parse(b);
                return numeroA.CompareTo(numeroB);
            }

            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
        }

        private void OrdenarAscendente(List<string> lista)
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
                        if (CompararValores(lista[j], lista[indiceMinimo]) < 0)
                        {
                            indiceMinimo = j;
                        }
                    }

                    string temp = lista[i];
                    lista[i] = lista[indiceMinimo];
                    lista[indiceMinimo] = temp;
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo ordenar el vector.", ex);
            }
        }

        private int BuscarSinCentinela(string valor)
        {
            try
            {
                int i = 0;
                bool encontrado = false;

                while (i < vectorDinamico.Count && !encontrado)
                {
                    if (vectorDinamico[i].Valor.Equals(valor, StringComparison.OrdinalIgnoreCase))
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

        private int BuscarConCentinela(string valor)
        {
            try
            {
                if (vectorDinamico.Count == 0)
                {
                    return -1;
                }

                vectorDinamico.Add(new ElementoVector { Id = siguienteId++, Valor = valor });

                int i = 0;
                while (!vectorDinamico[i].Valor.Equals(valor, StringComparison.OrdinalIgnoreCase))
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
                if (vectorDinamico.Count > 0)
                {
                    vectorDinamico.RemoveAt(vectorDinamico.Count - 1);
                }

                MostrarError("Ocurrió un error en la búsqueda lineal con centinela.", ex);
                return -1;
            }
        }

        private int BuscarBinaria(string valor, List<string> ordenado)
        {
            try
            {
                int izquierda = 0;
                int derecha = ordenado.Count - 1;

                while (izquierda <= derecha)
                {
                    int medio = (izquierda + derecha) / 2;
                    int comparacion = CompararValores(valor, ordenado[medio]);

                    if (comparacion == 0)
                    {
                        return medio;
                    }
                    else if (comparacion < 0)
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

        private int BuscarIndexada(string valor, List<string> ordenado)
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

                while (inicio < ordenado.Count && CompararValores(ordenado[fin], valor) < 0)
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
                    if (ordenado[i].Equals(valor, StringComparison.OrdinalIgnoreCase))
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

        private int BuscarHashing(string valor)
        {
            try
            {
                Dictionary<string, int> tabla = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

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

        private int ObtenerPosicionOriginal(string valor)
        {
            try
            {
                for (int i = 0; i < vectorDinamico.Count; i++)
                {
                    if (vectorDinamico[i].Valor.Equals(valor, StringComparison.OrdinalIgnoreCase))
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

        private bool ValidarBusqueda(out string valor)
        {
            valor = string.Empty;

            try
            {
                if (vectorDinamico.Count == 0)
                {
                    MessageBox.Show("Primero agrega elementos al vector.", "Vector vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!ObtenerValorTexto(TxtBuscar, out valor, "Ingresa un valor para buscar."))
                {
                    return false;
                }

                if (!ValidarYPrepararTipo(valor))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo validar la búsqueda.", ex);
                return false;
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string valor;

                if (!ObtenerValorTexto(TxtElemento, out valor, "Ingresa un dato válido para agregar."))
                {
                    return;
                }

                if (!ValidarYPrepararTipo(valor))
                {
                    TxtElemento.Focus();
                    return;
                }

                if (!HayEspacioDisponible(1))
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
                int cantidad;
                if (!ObtenerCantidadAleatoria(out cantidad))
                {
                    return;
                }

                if (!HayEspacioDisponible(cantidad))
                {
                    return;
                }

                TipoVector tipoSeleccionado;
                if (!ObtenerTipoParaAleatorio(out tipoSeleccionado))
                {
                    return;
                }

                if (tipoVector == TipoVector.NoDefinido)
                {
                    tipoVector = tipoSeleccionado;
                }

                for (int i = 0; i < cantidad; i++)
                {
                    string valor = GenerarValorAleatorio(tipoVector);
                    vectorDinamico.Add(new ElementoVector { Id = siguienteId++, Valor = valor });
                }

                ActualizarVector();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo agregar elementos aleatorios al vector.", ex);
            }
        }

        private void BtnLineal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivarBoton(BtnLineal);

                string valor;
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

                string valor;
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

                string valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                List<string> ordenado = CopiarValores();
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

                string valor;
                if (!ValidarBusqueda(out valor))
                {
                    return;
                }

                List<string> ordenado = CopiarValores();
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

                string valor;
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

                if (vectorDinamico.Count == 0)
                {
                    tipoVector = TipoVector.NoDefinido;
                }

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
