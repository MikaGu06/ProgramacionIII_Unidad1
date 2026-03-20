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
    /// Lógica de interacción para ListasCircularesDobles.xaml
    /// </summary>
    public partial class ListasCircularesDobles : UserControl
    {
        private int tamañoMaximo = 0;
        private Random rnd = new Random();
        private Nodo inicio;
        private Nodo fin;

        private class Nodo
        {
            public int Dato { get; set; }
            public Nodo Siguiente { get; set; }
            public Nodo Anterior { get; set; }

            public Nodo(int dato)
            {
                Dato = dato;
                Siguiente = null;
                Anterior = null;
            }
        }

        public ListasCircularesDobles()
        {
            InitializeComponent();
            ActualizarTodo();
        }

        private void AgregarInicio(int valor)
        {
            if (!DefinirTamañoSiHaceFalta())
                return;

            if (EstaLlena())
            {
                MessageBox.Show("La lista está llena.");
                return;
            }

            Nodo nuevoNodo = new Nodo(valor);

            if (inicio == null)
            {
                inicio = nuevoNodo;
                fin = nuevoNodo;
                inicio.Siguiente = inicio;
                inicio.Anterior = inicio;
            }
            else
            {
                nuevoNodo.Siguiente = inicio;
                nuevoNodo.Anterior = fin;
                inicio.Anterior = nuevoNodo;
                fin.Siguiente = nuevoNodo;
                inicio = nuevoNodo;
            }
        }

        private void AgregarFinal(int valor)
        {
            if (!DefinirTamañoSiHaceFalta())
                return;

            if (EstaLlena())
            {
                MessageBox.Show("La lista está llena.");
                return;
            }

            Nodo nuevoNodo = new Nodo(valor);

            if (inicio == null)
            {
                inicio = nuevoNodo;
                fin = nuevoNodo;
                inicio.Siguiente = inicio;
                inicio.Anterior = inicio;
            }
            else
            {
                nuevoNodo.Anterior = fin;
                nuevoNodo.Siguiente = inicio;
                fin.Siguiente = nuevoNodo;
                inicio.Anterior = nuevoNodo;
                fin = nuevoNodo;
            }
        }

        private void EliminarInicio()
        {
            if (inicio == null)
            {
                return;
            }

            if (inicio == fin)
            {
                inicio = null;
                fin = null;
            }
            else
            {
                inicio = inicio.Siguiente;
                inicio.Anterior = fin;
                fin.Siguiente = inicio;
            }
        }

        private void EliminarFinal()
        {
            if (fin == null)
            {
                return;
            }

            if (inicio == fin)
            {
                inicio = null;
                fin = null;
            }
            else
            {
                fin = fin.Anterior;
                fin.Siguiente = inicio;
                inicio.Anterior = fin;
            }
        }
        /*
        private int BuscarElemento(int valor)
        {
            if (inicio == null)
                return -1; // no encontrado

            Nodo actual = inicio;
            int posicion = 0;

            do
            {
                if (actual.Dato == valor)
                {
                    return posicion;
                }

                actual = actual.Siguiente;
                posicion++;

            } while (actual != inicio);

            return -1; // no encontrado
        }
        */
        private void ActualizarLista()
        {
            tbLista.Text = "";

            if (inicio == null)
            {
                tbLista.Text = "Esperando valores";
                return;
            }

            Nodo actual = inicio;

            do
            {
                tbLista.Text += actual.Dato.ToString();
                actual = actual.Siguiente;

                if (actual != inicio)
                {
                    tbLista.Text += ",  ";
                }

            } while (actual != inicio);
        }

        //Para el textblock de contador
        private int ContarElementos()
        {
            if (inicio == null)
                return 0;

            int contador = 0;
            Nodo actual = inicio;

            do
            {
                contador++;
                actual = actual.Siguiente;

            } while (actual != inicio);

            return contador;
        }

        private void ActualizarContador()
        {
            TxtElementos.Text = "Elementos: " + ContarElementos();
        }

        //si no se ingresa tamaño maximo
        private bool DefinirTamañoSiHaceFalta()
        {
            if (tamañoMaximo > 0)
                return true;

            MessageBox.Show("Primero presione el botón 'Definir tamaño'.");
            return false;
        }

        //sincronizando datos
        private void ActualizarTodo()
        {
            ActualizarLista();
            ActualizarContador();

            BtnAgregarInicio.IsEnabled = !EstaLlena();
            BtnAgregarFinal.IsEnabled = !EstaLlena();
        }

        //generando vector automatico
        private void GenerarAutomatico()
        {
            if (tamañoMaximo <= 0)
            {
                MessageBox.Show("Primero defina un tamaño máximo.");
                return;
            }

            inicio = null;
            fin = null;
            BordeResultadoBusqueda.Visibility = Visibility.Hidden;
            TxtResultadoBusqueda.Text = "";

            for (int i = 0; i < tamañoMaximo; i++)
            {
                int num = rnd.Next(1, 100);
                AgregarFinal(num);
            }

            ActualizarTodo();
        }

        //validación de longitud numeral
        private bool ValidarRango(int valor, int minimo, int maximo)
        {
            if (valor < minimo || valor > maximo)
            {
                MessageBox.Show("El valor debe estar entre " + minimo + " y " + maximo);
                return false;
            }

            return true;
        }

        //CARD INFERIOR
        //metodos 
        private bool EstaVacia()
        {
            return inicio == null;
        }

        private bool EstaLlena()
        {
            return tamañoMaximo > 0 && ContarElementos() >= tamañoMaximo;
        }

        private string CalcularModa()
        {
            if (inicio == null)
                return "-";

            Nodo actual = inicio;
            int maxFrecuencia = 0;

            //1) encontrar frecuencia máxima
            do
            {
                int frecuencia = 0;
                Nodo aux = inicio;

                do
                {
                    if (aux.Dato == actual.Dato)
                        frecuencia++;

                    aux = aux.Siguiente;

                } while (aux != inicio);

                if (frecuencia > maxFrecuencia)
                    maxFrecuencia = frecuencia;

                actual = actual.Siguiente;

            } while (actual != inicio);

            //Si todos aparecen una sola vez → no hay moda
            if (maxFrecuencia <= 1)
                return "No hay moda";

            //2) obtener todas las modas
            string resultado = "";
            actual = inicio;

            do
            {
                int frecuencia = 0;
                Nodo aux = inicio;

                do
                {
                    if (aux.Dato == actual.Dato)
                        frecuencia++;

                    aux = aux.Siguiente;

                } while (aux != inicio);

                //evitar repetir números en el resultado
                if (frecuencia == maxFrecuencia && !resultado.Contains(actual.Dato.ToString()))
                {
                    if (resultado != "")
                        resultado += ", ";

                    resultado += actual.Dato.ToString();
                }

                actual = actual.Siguiente;

            } while (actual != inicio);

            return resultado;
        }

        private int CalcularSumaTotal()
        {
            if (inicio == null)
                return 0;

            int suma = 0;
            Nodo actual = inicio;

            do
            {
                suma += actual.Dato;
                actual = actual.Siguiente;

            } while (actual != inicio);

            return suma;
        }

        private int ObtenerMayor()
        {
            if (inicio == null)
                return 0;

            int mayor = inicio.Dato;
            Nodo actual = inicio.Siguiente;

            while (actual != inicio)
            {
                if (actual.Dato > mayor)
                {
                    mayor = actual.Dato;
                }

                actual = actual.Siguiente;
            }

            return mayor;
        }

        private int ObtenerMenor()
        {
            if (inicio == null)
                return 0;

            int menor = inicio.Dato;
            Nodo actual = inicio.Siguiente;

            while (actual != inicio)
            {
                if (actual.Dato < menor)
                {
                    menor = actual.Dato;
                }

                actual = actual.Siguiente;
            }

            return menor;
        }

        //ordenamiento merge sort
        private Nodo ObtenerMitad(Nodo cabeza)
        {
            if (cabeza == null)
                return null;

            Nodo lento = cabeza;
            Nodo rapido = cabeza.Siguiente;

            while (rapido != null && rapido.Siguiente != null)
            {
                lento = lento.Siguiente;
                rapido = rapido.Siguiente.Siguiente;
            }

            return lento;
        }

        private Nodo MezclarListas(Nodo izquierda, Nodo derecha, bool ascendente)
        {
            if (izquierda == null)
                return derecha;

            if (derecha == null)
                return izquierda;

            Nodo resultado;

            if (ascendente)
            {
                if (izquierda.Dato <= derecha.Dato)
                {
                    resultado = izquierda;
                    resultado.Siguiente = MezclarListas(izquierda.Siguiente, derecha, ascendente);

                    if (resultado.Siguiente != null)
                    {
                        resultado.Siguiente.Anterior = resultado;
                    }
                }
                else
                {
                    resultado = derecha;
                    resultado.Siguiente = MezclarListas(izquierda, derecha.Siguiente, ascendente);

                    if (resultado.Siguiente != null)
                    {
                        resultado.Siguiente.Anterior = resultado;
                    }
                }
            }
            else
            {
                if (izquierda.Dato >= derecha.Dato)
                {
                    resultado = izquierda;
                    resultado.Siguiente = MezclarListas(izquierda.Siguiente, derecha, ascendente);

                    if (resultado.Siguiente != null)
                    {
                        resultado.Siguiente.Anterior = resultado;
                    }
                }
                else
                {
                    resultado = derecha;
                    resultado.Siguiente = MezclarListas(izquierda, derecha.Siguiente, ascendente);

                    if (resultado.Siguiente != null)
                    {
                        resultado.Siguiente.Anterior = resultado;
                    }
                }
            }

            resultado.Anterior = null;
            return resultado;
        }

        private Nodo MergeSort(Nodo cabeza, bool ascendente)
        {
            if (cabeza == null || cabeza.Siguiente == null)
                return cabeza;

            Nodo mitad = ObtenerMitad(cabeza);
            Nodo segundaMitad = mitad.Siguiente;
            mitad.Siguiente = null;

            if (segundaMitad != null)
            {
                segundaMitad.Anterior = null;
            }

            Nodo izquierda = MergeSort(cabeza, ascendente);
            Nodo derecha = MergeSort(segundaMitad, ascendente);

            return MezclarListas(izquierda, derecha, ascendente);
        }

        private void ConvertirACircular()
        {
            fin = inicio;

            if (fin == null)
                return;

            while (fin.Siguiente != null)
            {
                fin = fin.Siguiente;
            }

            fin.Siguiente = inicio;
            inicio.Anterior = fin;
        }

        //BOTONES CARD SUPERIOR
        private void BtnAgregarInicio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int valor = int.Parse(TxtNumero.Text);

                if (!ValidarRango(valor, 0, 999))
                {
                    TxtNumero.Clear();
                    return;
                }

                AgregarInicio(valor);
                ActualizarTodo();
                TxtNumero.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese un valor entero válido.");
                TxtNumero.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error al agregar el elemento.");
            }
        }

        private void BtnAgregarFinal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int valor = int.Parse(TxtNumero.Text);

                if (!ValidarRango(valor, 0, 999))
                {
                    TxtNumero.Clear();
                    return;
                }

                AgregarFinal(valor);
                ActualizarTodo();
                TxtNumero.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese un valor entero válido.");
                TxtNumero.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error al agregar el elemento.");
            }
        }

        private void BtnEliminarInicio_Click(object sender, RoutedEventArgs e)
        {
            EliminarInicio();
            ActualizarTodo();
        }

        private void BtnEliminarFinal_Click(object sender, RoutedEventArgs e)
        {
            EliminarFinal();
            ActualizarTodo();
        }
        /*
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int valor = int.Parse(txtBuscar.Text);

                if (!ValidarRango(valor, 0, 999))
                {
                    txtBuscar.Clear();
                    return;
                }

                int posicion = BuscarElemento(valor);
                BordeResultadoBusqueda.Visibility = Visibility.Visible;

                if (posicion != -1)
                {
                    TxtResultadoBusqueda.Text = "Resultado encontrado en la posición " + posicion;
                    txtBuscar.Clear();
                }
                else
                {
                    TxtResultadoBusqueda.Text = "El elemento no se encuentra en la lista ";
                    txtBuscar.Clear();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese un valor entero válido.");
                txtBuscar.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al buscar el elemento.");
            }
        }*/

        private void BtnGenerarAleatorio_Click(object sender, RoutedEventArgs e)
        {
            GenerarAutomatico();
        }

        private void BtnDefinirTam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tam = int.Parse(TxtCantidadAleatoria.Text);

                if (!ValidarRango(tam, 1, 30))
                {
                    TxtCantidadAleatoria.Clear();
                    return;
                }

                tamañoMaximo = tam;
                MessageBox.Show("Tamaño máximo definido correctamente.");
                ActualizarTodo();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese un tamaño válido.");
                TxtCantidadAleatoria.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al definir el tamaño.");
            }
        }

        //botones
        private void BtnEstaVacia_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
            {
                TxtEstaVacia.Text = "Sí";
            }
            else
            {
                TxtEstaVacia.Text = "No";
            }
        }

        private void BtnEstaLlena_Click(object sender, RoutedEventArgs e)
        {
            if (EstaLlena())
            {
                TxtEstaLlena.Text = "Sí";
            }
            else
            {
                TxtEstaLlena.Text = "No";
            }
        }

        private void BtnModa_Click(object sender, RoutedEventArgs e)
        {
            TxtModa.Text = CalcularModa();
        }

        private void BtnSumaTotal_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
            {
                TxtSumaTotal.Text = "-";
                return;
            }

            TxtSumaTotal.Text = CalcularSumaTotal().ToString();
        }

        private void BtnMayor_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
            {
                TxtMayor.Text = "-";
                return;
            }

            TxtMayor.Text = ObtenerMayor().ToString();
        }

        private void BtnMenor_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
            {
                TxtMenor.Text = "-";
                return;
            }

            TxtMenor.Text = ObtenerMenor().ToString();
        }

        private void BtnLimpiarListaCompleta_Click(object sender, RoutedEventArgs e)
        {
            inicio = null;
            fin = null;
            tamañoMaximo = 0;

            TxtCantidadAleatoria.Clear();
            TxtNumero.Clear();
            txtBuscar.Clear();

            TxtEstaVacia.Text = "-";
            TxtEstaLlena.Text = "-";
            TxtModa.Text = "-";
            TxtSumaTotal.Text = "Calcular";
            TxtMayor.Text = "Calcular";
            TxtMenor.Text = "Calcular";

            BordeResultadoBusqueda.Visibility = Visibility.Hidden;
            TxtResultadoBusqueda.Text = "";

            ActualizarTodo();
        }

        private void BtnOrdenarLista_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
            {
                MessageBox.Show("La lista está vacía.");
                return;
            }

            fin.Siguiente = null;
            inicio.Anterior = null;

            if (RbAscendente.IsChecked == true)
            {
                inicio = MergeSort(inicio, true);
            }

            if (RbDescendente.IsChecked == true)
            {
                inicio = MergeSort(inicio, false);
            }

            ConvertirACircular();
            ActualizarTodo();
        }
    }
}
