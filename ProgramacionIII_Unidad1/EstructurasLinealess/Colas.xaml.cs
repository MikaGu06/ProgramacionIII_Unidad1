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
            if (tamMax > 0 && count >= tamMax)
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
            {
                rear = null;
            }
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
                return "-";

            return front.key.ToString(); 
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
            TxtVacia.Text = IsEmpty() ? "Sí" : "No";
            TxtLlena.Text = IsFull() ? "Sí" : "No";
            TxtCima.Text = TopElement();
            TxtMaximo.Text = tamMax > 0 ? tamMax.ToString() : "∞";
        }

        // 🔹 Tamaño máximo
        private void BtnMaximo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (count > 0)
                {
                    MessageBox.Show("No puedes definir el tamaño máximo si la cola ya tiene elementos");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(TxtTamMax.Text))
                {
                    int valor = int.Parse(TxtTamMax.Text);

                    if (valor <= 0)
                    {
                        MessageBox.Show("El tamaño máximo debe ser mayor a 0");
                        return;
                    }

                    tamMax = valor;
                }
                else
                {
                    tamMax = 0;
                }

                ActualizarUI();
            }
            catch
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        private void BtnInsertar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = int.Parse(TxtNumero.Text);

                Enqueue(num);
                TxtNumero.Clear();

                ActualizarUI();
            }
            catch
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        private void BtnGenerarAleatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cantidad = int.Parse(TxtCantidadAleatoria.Text);

                if (cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0");
                    return;
                }

                if (IsFull())
                {
                    MessageBox.Show("La cola ya está llena");
                    return;
                }

                if (tamMax > 0 && count + cantidad > tamMax)
                {
                    int espacio = tamMax - count;
                    MessageBox.Show("Solo puedes agregar " + espacio + " elementos más");
                    return;
                }

                Random rnd = new Random();

                for (int i = 0; i < cantidad; i++)
                {
                    Enqueue(rnd.Next(1, 100));
                }

                ActualizarUI();
            }
            catch
            {
                MessageBox.Show("Ingrese una cantidad válida");
            }
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
            ActualizarUI();
        }
    }
}