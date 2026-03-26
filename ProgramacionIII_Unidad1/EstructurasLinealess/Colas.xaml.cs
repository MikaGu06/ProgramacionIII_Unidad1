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
    public partial class Colas : UserControl
    {
        private class Nodo
        {
            public int key;
            public Nodo next;

            public Nodo(int key)
            {
                this.key = key;
                this.next = null;
            }
        }

        private Nodo front = null;
        private Nodo rear = null;
        private int tamMax = 0;
        private int count = 0;

        public Colas()
        {
            InitializeComponent();
            ActualizarUI();
        }

        private void Enqueue(int key)
        {
            if (tamMax == 0)
            {
                MessageBox.Show("Primero debes definir el tamaño máximo (1 a 30)");
                return;
            }

            if (count >= tamMax)
            {
                MessageBox.Show("La cola está llena");
                return;
            }

            Nodo temp = new Nodo(key);

            if (rear == null)
            {
                front = rear = temp;
            }
            else
            {
                rear.next = temp;
                rear = temp;
            }

            count++;
        }

        private void Dequeue()
        {
            if (front == null)
            {
                MessageBox.Show("La cola está vacía");
                return;
            }

            front = front.next;
            count--;

            if (front == null)
                rear = null;
        }

        private bool IsEmpty()
        {
            return count == 0;
        }

        private bool IsFull()
        {
            return tamMax > 0 && count == tamMax;
        }

        private string TopElement()
        {
            if (IsEmpty())
            {
                return "-";
            }
            else
            {
                return front.key.ToString();
            }
        }

        private void ActualizarUI()
        {
            LstCola.Items.Clear();

            Nodo temp = front;

            while (temp != null)
            {
                LstCola.Items.Add(temp.key);
                temp = temp.next;
            }

            TxtElementos.Text = "Elementos: " + count;
        }

        // DEFINIR TAMAÑO (1 A 30)
        private void BtnMaximo_Click(object sender, RoutedEventArgs e)
        {
            if (count > 0)
            {
                MessageBox.Show("No puedes definir el tamaño si la cola tiene elementos");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtTamMax.Text))
            {
                MessageBox.Show("Debes ingresar un tamaño máximo (1 a 30)");
                return;
            }

            if (!int.TryParse(TxtTamMax.Text, out int valor))
            {
                MessageBox.Show("Ingrese un número válido");
                return;
            }

            if (valor <= 0 || valor > 30)
            {
                MessageBox.Show("El tamaño debe estar entre 1 y 30");
                return;
            }

            tamMax = valor;
            TxtMaximo.Text = tamMax.ToString();
            ActualizarUI();
        }

        private void BtnInsertar_Click(object sender, RoutedEventArgs e)
        {
            if (tamMax == 0)
            {
                MessageBox.Show("Primero define el tamaño máximo");
                return;
            }

            if (!int.TryParse(TxtCantidadAleatoria.Text, out int num))
            {
                MessageBox.Show("Ingrese un número válido");
                return;
            }

            Enqueue(num);
            TxtCantidadAleatoria.Clear();
            ActualizarUI();
        }

        private void BtnGenerarAleatorio_Click(object sender, RoutedEventArgs e)
        {
            if (tamMax == 0)
            {
                MessageBox.Show("Primero define el tamaño máximo");
                return;
            }

            if (!int.TryParse(TxtTamMax.Text, out int cantidad))
            {
                MessageBox.Show("Ingrese una cantidad válida");
                return;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0");
                return;
            }

            if (cantidad > tamMax)
            {
                MessageBox.Show("Solo puedes generar hasta " + tamMax + " elementos");
                return;
            }

            front = rear = null;
            count = 0;

            Random rnd = new Random();

            for (int i = 0; i < cantidad; i++)
            {
                Enqueue(rnd.Next(1, 100));
            }

            ActualizarUI();
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            Dequeue();
            ActualizarUI();
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            front = rear = null;
            count = 0;

            TxtVacia.Text = "-";
            TxtLlena.Text = "-";
            TxtMaximo.Text = "-";
            TxtCima.Text = "-";

            ActualizarUI();
        }

        private void BtnVacia_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty())
            {
                TxtVacia.Text = "Sí";
            }
            else
            {
                TxtVacia.Text = "No";
            }
        }

        private void BtnLlena_Click(object sender, RoutedEventArgs e)
        {
            if (IsFull())
            {
                TxtLlena.Text = "Sí";
            }
            else
            {
                TxtLlena.Text = "No";
            }
        }

        private void BtnCima_Click(object sender, RoutedEventArgs e)
        {
            TxtCima.Text = TopElement();
        }
    }
}