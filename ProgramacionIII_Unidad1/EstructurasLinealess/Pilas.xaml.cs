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
    public partial class Pilas : UserControl
    {
        private class Node
        {
            public int data;
            public Node link;
        }

        private Node top = null;
        private int tamMax = 0;
        private int count = 0;

        public Pilas()
        {
            InitializeComponent();
            ActualizarUI();
        }

        private void ActualizarUI()
        {
            LstPila.Items.Clear();

            Node temp = top;
            while (temp != null)
            {
                LstPila.Items.Add(temp.data);
                temp = temp.link;
            }

            TxtElementos.Text = "Elementos: " + count;
        }

        private void Push(int valor)
        {
            if (tamMax == 0)
            {
                MessageBox.Show("Primero debes definir el tamaño máximo (1 a 30)");
                return;
            }

            if (count >= tamMax)
            {
                MessageBox.Show("La pila está llena");
                return;
            }

            Node nuevo = new Node();
            nuevo.data = valor;
            nuevo.link = top;
            top = nuevo;
            count++;
        }

        private void Pop()
        {
            if (top == null)
                return;

            top = top.link;
            count--;
        }

        private bool EstaVacia()
        {
            return count == 0;
        }

        private bool EstaLlena()
        {
            return tamMax > 0 && count == tamMax;
        }

        private string Cima()
        {
            if (top != null)
            {
                return top.data.ToString();
            }
            else
            {
                return "-";
            }
        }

        // DEFINIR TAMAÑO (1 A 30)
        private void BtnMaximo_Click(object sender, RoutedEventArgs e)
        {
            if (count > 0)
            {
                MessageBox.Show("No puedes definir el tamaño si la pila tiene elementos");
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

            Push(num);
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

            top = null;
            count = 0;

            Random rnd = new Random();

            for (int i = 0; i < cantidad; i++)
            {
                Push(rnd.Next(1, 100));
            }
            ActualizarUI();
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (count == 0)
            {
                MessageBox.Show("La pila está vacía, no hay elementos para quitar");
                return;
            }

            Pop();
            ActualizarUI();
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            top = null;
            count = 0;

            TxtVacia.Text = "-";
            TxtLlena.Text = "-";
            TxtMaximo.Text = "-";
            TxtCima.Text = "-";

            TxtCantidadAleatoria.Clear();
            TxtTamMax.Clear();

            ActualizarUI();
        }

        private void BtnVacia_Click(object sender, RoutedEventArgs e)
        {
            if (EstaVacia())
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
            if (EstaLlena())
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
            TxtCima.Text = Cima();
        }
    }
}