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
             
            if (TxtPantalla.Text == "Error" || TxtPantalla.Text == "Indefinido" || TxtPantalla.Text == "Indeterminado")
                TxtPantalla.Text = "";

            TxtPantalla.Text += btn.Content.ToString();
        }

        private void Limpiar(object sender, RoutedEventArgs e)
        {
            TxtPantalla.Text = "";
        }

        private void BorrarUno(object sender, RoutedEventArgs e)
        {
            if (TxtPantalla.Text == "Error" || TxtPantalla.Text == "Indefinido" || TxtPantalla.Text == "Indeterminado")
            {
                TxtPantalla.Text = "";
                return;
            }
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
            catch (DivideByZeroException)
            {
                TxtPantalla.Text = "Indefinido";
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Indeterminado")
                    TxtPantalla.Text = "Indeterminado";
                else
                    TxtPantalla.Text = "Error";
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
            string numero = "";
            char anterior = '\0';

            foreach (char c in infija)
            {
                // multiplicación implícita: 8(4) o (2+3)(4) o (2+3)4
                if ((c == '(' && (char.IsDigit(anterior) || anterior == ')')) ||
                    (char.IsDigit(c) && anterior == ')'))
                {
                    if (numero != "")
                    {
                        salida += numero + " ";
                        numero = "";
                    }

                    while (pila.Count > 0 && Prioridad(pila.Peek()) >= Prioridad('*'))
                        salida += pila.Pop() + " ";

                    pila.Push('*');
                }

                if (char.IsDigit(c))
                {
                    numero += c;
                }
                else
                {
                    if (numero != "")
                    {
                        salida += numero + " ";
                        numero = "";
                    }

                    if (c == '(')
                    {
                        pila.Push(c);
                    }
                    else if (c == ')')
                    {
                        while (pila.Peek() != '(')
                            salida += pila.Pop() + " ";

                        pila.Pop();
                    }
                    else
                    {
                        while (pila.Count > 0 && Prioridad(pila.Peek()) >= Prioridad(c))
                            salida += pila.Pop() + " ";

                        pila.Push(c);
                    }
                }

                anterior = c;
            }

            if (numero != "")
                salida += numero + " ";

            while (pila.Count > 0)
                salida += pila.Pop() + " ";

            return salida.Trim();
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
            string[] tokens = expr.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double numero))
                {
                    pila.Push(numero);
                }
                else
                {
                    double b = pila.Pop();
                    double a = pila.Pop();

                    switch (token)
                    {
                        case "+": pila.Push(a + b); break;
                        case "-": pila.Push(a - b); break;
                        case "*": pila.Push(a * b); break;

                        case "/":
                            if (b == 0)
                            {
                                if (a == 0)
                                    throw new InvalidOperationException("Indeterminado");
                                else
                                    throw new DivideByZeroException();
                            }
                            pila.Push(a / b);
                            break;
                    }
                }
            }

            return pila.Pop();
        }
    }
}