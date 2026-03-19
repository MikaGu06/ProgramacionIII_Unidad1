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
    /// Lógica de interacción para ListasSimplementeEnlazadas.xaml
    /// </summary>
    public partial class ListasSimplementeEnlazadas : UserControl
    {
        private int tamañoMaximo = 0;
        private Random rnd = new Random();
        private Nodo inicio;

        private class Nodo
        {
            public int Dato { get; set; }
            public Nodo Siguiente { get; set; }
            public Nodo(int dato)
            {
                Dato = dato;
                Siguiente = null;
            }
        }
        public ListasSimplementeEnlazadas()
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
            nuevoNodo.Siguiente = inicio;
            inicio = nuevoNodo;
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
                return;
            }

            Nodo ultimo = inicio;
            while (ultimo.Siguiente != null)
            {
                ultimo = ultimo.Siguiente;
            }
            ultimo.Siguiente = nuevoNodo;
        }

        private void EliminarInicio()
        {
            if (inicio != null)
            {
                inicio = inicio.Siguiente;
            }
        }
        private void EliminarFinal()
        {
            if (inicio == null || inicio.Siguiente == null)
            {
                inicio = null;
                return;
            }
            Nodo penultimo = inicio;
            while (penultimo.Siguiente.Siguiente != null)
            {
                penultimo = penultimo.Siguiente;
            }
            penultimo.Siguiente = null;
        }
        private int BuscarElemento(int valor)
        {
            Nodo actual = inicio;
            int posicion = 0;

            while (actual != null)
            {
                if (actual.Dato == valor)
                {
                    return posicion; 
                }

                actual = actual.Siguiente;
                posicion++;
            }

            return -1; // no encontrado
        }
        private void ActualizarLista()
        {
            tbLista.Text = "";
            Nodo actual = inicio;

            while (actual != null)
            {
                tbLista.Text += actual.Dato.ToString();

                if (actual.Siguiente != null)
                {
                    tbLista.Text += ", ";
                }

                actual = actual.Siguiente;
            }

            if (inicio == null)
            {
                tbLista.Text = "Esperando valores";
            }
        }

        //Para el textblock de contador
        private int ContarElementos()
        {
            int contador = 0;
            Nodo actual = inicio;

            while (actual != null)
            {
                contador++;
                actual = actual.Siguiente;
            }

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
                MessageBox.Show("El valor debe estar entre " + minimo + " y " + maximo );
                return false;
            }

            return true;
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
        }
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
            while (actual != null)
            {
                int frecuencia = 0;
                Nodo aux = inicio;

                while (aux != null)
                {
                    if (aux.Dato == actual.Dato)
                        frecuencia++;

                    aux = aux.Siguiente;
                }

                if (frecuencia > maxFrecuencia)
                    maxFrecuencia = frecuencia;

                actual = actual.Siguiente;
            }

            //Si todos aparecen una sola vez → no hay moda
            if (maxFrecuencia <= 1)
                return "No hay moda";

            //2) obtener todas las modas
            string resultado = "";
            actual = inicio;

            while (actual != null)
            {
                int frecuencia = 0;
                Nodo aux = inicio;

                while (aux != null)
                {
                    if (aux.Dato == actual.Dato)
                        frecuencia++;

                    aux = aux.Siguiente;
                }

                //evitar repetir números en el resultado
                if (frecuencia == maxFrecuencia && !resultado.Contains(actual.Dato.ToString()))
                {
                    if (resultado != "")
                        resultado += ", ";

                    resultado += actual.Dato.ToString();
                }

                actual = actual.Siguiente;
            }

            return resultado;
        }
        private int CalcularSumaTotal()
        {
            int suma = 0;
            Nodo actual = inicio;

            while (actual != null)
            {
                suma += actual.Dato;
                actual = actual.Siguiente;
            }

            return suma;
        }

        private int ObtenerMayor()
        {
            if (inicio == null)
                return 0;

            int mayor = inicio.Dato;
            Nodo actual = inicio;

            while (actual != null)
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
            Nodo actual = inicio;

            while (actual != null)
            {
                if (actual.Dato < menor)
                {
                    menor = actual.Dato;
                }

                actual = actual.Siguiente;
            }

            return menor;
        }

        private void MetodoBurbuja(bool ascendente)
        {
            int t;
            Nodo a = inicio;

            while (a != null)
            {
                Nodo b = inicio;

                while (b.Siguiente != null)
                {
                    if (ascendente)
                    {
                        if (b.Dato > b.Siguiente.Dato)
                        {
                            t = b.Dato;
                            b.Dato = b.Siguiente.Dato;
                            b.Siguiente.Dato = t;
                        }
                    }
                    else
                    {
                        if (b.Dato < b.Siguiente.Dato)
                        {
                            t = b.Dato;
                            b.Dato = b.Siguiente.Dato;
                            b.Siguiente.Dato = t;
                        }
                    }

                    b = b.Siguiente;
                }

                a = a.Siguiente;
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

            bool ascendente = RbAscendente.IsChecked == true;

            MetodoBurbuja(ascendente);
            ActualizarTodo();
        }
    }
}
