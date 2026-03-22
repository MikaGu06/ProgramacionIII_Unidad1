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
    public partial class CalculadoraConPilas : UserControl
    {
        public CalculadoraConPilas()
        {
            InitializeComponent();
        }
        private void AgregarTexto(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (TxtPantalla.Text == "Error")
                TxtPantalla.Text = "";

            TxtPantalla.Text += btn.Content.ToString();
        }

        private void Limpiar(object sender, RoutedEventArgs e)
        {
            TxtPantalla.Text = "";
        }

        private void BorrarUno(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtPantalla.Text))
            {
                TxtPantalla.Text = TxtPantalla.Text.Substring(0, TxtPantalla.Text.Length - 1);
            }
        }

        private void Calcular(object sender, RoutedEventArgs e)
        {
            try
            {
                string infija = TxtPantalla.Text;
                string postfija = InfijaAPostfija(infija);
                double resultado = EvaluarPostfija(postfija);

                TxtPantalla.Text = resultado.ToString();
            }
            catch
            {
                TxtPantalla.Text = "Error";
            }
        }

        private string InfijaAPostfija(string infija)
        {
            Stack<char> pila = new Stack<char>();
            string salida = "";

            foreach (char c in infija)
            {
                if (char.IsDigit(c))
                {
                    salida += c;
                }
                else if (c == '(')
                {
                    pila.Push(c);
                }
                else if (c == ')')
                {
                    while (pila.Peek() != '(')
                        salida += pila.Pop();

                    pila.Pop();
                }
                else
                {
                    while (pila.Count > 0 && Prioridad(pila.Peek()) >= Prioridad(c))
                        salida += pila.Pop();

                    pila.Push(c);
                }
            }

            while (pila.Count > 0)
                salida += pila.Pop();

            return salida;
        }
        private int Prioridad(char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            return 0;
        }

        private double EvaluarPostfija(string expr)
        {
            Stack<double> pila = new Stack<double>();

            foreach (char c in expr)
            {
                if (char.IsDigit(c))
                {
                    pila.Push(Convert.ToDouble(c.ToString()));
                }
                else
                {
                    double b = pila.Pop();
                    double a = pila.Pop();

                    switch (c)
                    {
                        case '+': pila.Push(a + b); break;
                        case '-': pila.Push(a - b); break;
                        case '*': pila.Push(a * b); break;
                        case '/': pila.Push(a / b); break;
                    }
                }
            }

            return pila.Pop();
        }
    }
}
