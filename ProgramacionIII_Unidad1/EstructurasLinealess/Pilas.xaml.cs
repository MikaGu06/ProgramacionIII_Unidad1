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
            TxtVacia.Text = (count == 0) ? "Sí" : "No";
            TxtLlena.Text = (tamMax > 0 && count == tamMax) ? "Sí" : "No";
            TxtCima.Text = (top != null) ? top.data.ToString() : "-";
            TxtMaximo.Text = (tamMax > 0) ? tamMax.ToString() : "∞";
        }

        private void Push(int valor)
        {
            Node nuevo = new Node();
            nuevo.data = valor;
            nuevo.link = top;
            top = nuevo;
            count++;
        }

        private void Pop()
        {
            if (top != null)
            {
                top = top.link;
                count--;
            }
        }

        private void BtnMaximo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (count > 0)
                {
                    MessageBox.Show("No puedes definir el tamaño máximo si la pila ya tiene elementos");
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
                MessageBox.Show("Ingrese un número válido para el tamaño máximo");
            }
        }

        
        private void BtnInsertar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = int.Parse(TxtNumero.Text);

                if (tamMax > 0 && count >= tamMax)
                {
                    MessageBox.Show("La pila está llena, no se pueden agregar más elementos");
                    return;
                }

                Push(num);
                TxtNumero.Clear();
                ActualizarUI();
            }
            catch
            {
                MessageBox.Show("Ingrese un número válido para insertar");
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

                if (tamMax > 0 && count >= tamMax)
                {
                    MessageBox.Show("La pila ya está llena");
                    return;
                }

                if (tamMax > 0 && count + cantidad > tamMax)
                {
                    int espacioDisponible = tamMax - count;
                    MessageBox.Show("Solo puedes agregar " + espacioDisponible + " elementos más");
                    return;
                }

                Random rnd = new Random();

                for (int i = 0; i < cantidad; i++)
                {
                    Push(rnd.Next(1, 100));
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
            ActualizarUI();
        }
    }
}