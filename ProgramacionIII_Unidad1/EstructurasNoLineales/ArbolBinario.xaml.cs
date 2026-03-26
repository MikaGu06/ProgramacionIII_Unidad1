using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgramacionIII_Unidad1.EstructurasNoLineales
{
    public partial class ArbolBinario : UserControl
    {
        private TreeNode<dynamic> root;
        private double yOffset = 70;
        private const double NODO_RADIO = 22;

        public ArbolBinario()
        {
            InitializeComponent();
            ActualizarEstadisticas();
        }

        // --- CLASE NODO ---
        public class TreeNode<T>
        {
            public T Value { get; set; }
            public TreeNode<T> Left { get; set; }
            public TreeNode<T> Right { get; set; }
            public TreeNode<T> Parent { get; set; }

            public TreeNode(T value)
            {
                Value = value;
                Left = null;
                Right = null;
                Parent = null;
            }
        }

        // --- INSERCIÓN AUTOMÁTICA (BST) ---
        private TreeNode<dynamic> InsertNode(TreeNode<dynamic> node, dynamic value, TreeNode<dynamic> parent = null)
        {
            if (node == null)
            {
                var newNode = new TreeNode<dynamic>(value);
                newNode.Parent = parent;
                return newNode;
            }

            int comparacion = Comparer<dynamic>.Default.Compare(value, node.Value);

            if (comparacion < 0)
                node.Left = InsertNode(node.Left, value, node);
            else if (comparacion > 0)
                node.Right = InsertNode(node.Right, value, node);
            else
                MessageBox.Show($"El valor {value} ya existe en el árbol.");

            return node;
        }

        // --- INSERCIÓN MANUAL ---
        private void BtnInsertarManual_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPadre.Text) || string.IsNullOrWhiteSpace(txtNuevoValor.Text))
            {
                MessageBox.Show("Ingrese el valor del padre y el nuevo valor.");
                return;
            }

            try
            {
                dynamic valorPadre, nuevoValor;

                if (rbIntegers.IsChecked == true)
                {
                    if (!int.TryParse(txtPadre.Text, out int padreInt) || !int.TryParse(txtNuevoValor.Text, out int nuevoInt))
                    {
                        MessageBox.Show("Ingrese números enteros válidos.");
                        return;
                    }
                    valorPadre = padreInt;
                    nuevoValor = nuevoInt;
                }
                else
                {
                    if (txtPadre.Text.Length == 0 || txtNuevoValor.Text.Length == 0)
                    {
                        MessageBox.Show("Ingrese caracteres válidos.");
                        return;
                    }
                    valorPadre = txtPadre.Text[0];
                    nuevoValor = txtNuevoValor.Text[0];
                }

                TreeNode<dynamic> padre = BuscarNodo(root, valorPadre);

                if (padre == null)
                {
                    MessageBox.Show($"El nodo padre '{valorPadre}' no existe en el árbol.");
                    return;
                }

                bool izquierda = (cbPosicion.SelectedIndex == 0);

                if (izquierda && padre.Left != null)
                {
                    MessageBox.Show($"El nodo {padre.Value} ya tiene un hijo izquierdo.");
                    return;
                }

                if (!izquierda && padre.Right != null)
                {
                    MessageBox.Show($"El nodo {padre.Value} ya tiene un hijo derecho.");
                    return;
                }

                var nuevoNodo = new TreeNode<dynamic>(nuevoValor);
                nuevoNodo.Parent = padre;

                if (izquierda)
                    padre.Left = nuevoNodo;
                else
                    padre.Right = nuevoNodo;

                Redibujar();
                txtPadre.Clear();
                txtNuevoValor.Clear();
                MessageBox.Show($"Nodo '{nuevoValor}' insertado como hijo {(izquierda ? "izquierdo" : "derecho")} de '{padre.Value}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // --- ELIMINACIÓN ---
        private TreeNode<dynamic> EliminarNodo(TreeNode<dynamic> nodo, dynamic valor)
        {
            if (nodo == null)
            {
                MessageBox.Show($"El valor {valor} no se encuentra en el árbol.");
                return null;
            }

            int comp = Comparer<dynamic>.Default.Compare(valor, nodo.Value);

            if (comp < 0)
                nodo.Left = EliminarNodo(nodo.Left, valor);
            else if (comp > 0)
                nodo.Right = EliminarNodo(nodo.Right, valor);
            else
            {
                if (nodo.Left == null && nodo.Right == null)
                {
                    MessageBox.Show($"Nodo '{valor}' eliminado (hoja).");
                    return null;
                }

                if (nodo.Left == null)
                {
                    if (nodo.Right != null)
                        nodo.Right.Parent = nodo.Parent;
                    MessageBox.Show($"Nodo '{valor}' eliminado (tiene hijo derecho).");
                    return nodo.Right;
                }

                if (nodo.Right == null)
                {
                    if (nodo.Left != null)
                        nodo.Left.Parent = nodo.Parent;
                    MessageBox.Show($"Nodo '{valor}' eliminado (tiene hijo izquierdo).");
                    return nodo.Left;
                }

                dynamic sucesor = MinimoValor(nodo.Right);
                nodo.Value = sucesor;
                nodo.Right = EliminarNodo(nodo.Right, sucesor);
                MessageBox.Show($"Nodo '{valor}' reemplazado por su sucesor '{sucesor}'.");
            }
            return nodo;
        }

        private dynamic MinimoValor(TreeNode<dynamic> nodo)
        {
            while (nodo.Left != null)
                nodo = nodo.Left;
            return nodo.Value;
        }

        // --- BÚSQUEDA ---
        private TreeNode<dynamic> BuscarNodo(TreeNode<dynamic> nodo, dynamic valor)
        {
            if (nodo == null)
                return null;

            int comp = Comparer<dynamic>.Default.Compare(valor, nodo.Value);

            if (comp == 0)
                return nodo;
            else if (comp < 0)
                return BuscarNodo(nodo.Left, valor);
            else
                return BuscarNodo(nodo.Right, valor);
        }

        // --- PROPIEDADES DEL ÁRBOL ---
        private int ContarNodos(TreeNode<dynamic> node)
        {
            if (node == null) return 0;
            return 1 + ContarNodos(node.Left) + ContarNodos(node.Right);
        }

        private int CalcAltura(TreeNode<dynamic> node)
        {
            if (node == null) return 0;
            return Math.Max(CalcAltura(node.Left), CalcAltura(node.Right)) + 1;
        }

        private bool EsArbolCompleto(TreeNode<dynamic> node, int indice, int totalNodos)
        {
            if (node == null) return true;
            if (indice >= totalNodos) return false;
            return EsArbolCompleto(node.Left, 2 * indice + 1, totalNodos) &&
                   EsArbolCompleto(node.Right, 2 * indice + 2, totalNodos);
        }

        private bool EsArbolPerfecto(TreeNode<dynamic> node, int altura, int nivel = 0)
        {
            if (node == null) return true;
            if (node.Left == null && node.Right == null)
                return nivel == altura - 1;
            if (node.Left == null || node.Right == null)
                return false;
            return EsArbolPerfecto(node.Left, altura, nivel + 1) &&
                   EsArbolPerfecto(node.Right, altura, nivel + 1);
        }

        // --- RECORRIDOS ---
        private void Inorden(TreeNode<dynamic> node, List<dynamic> result)
        {
            if (node == null) return;
            Inorden(node.Left, result);
            result.Add(node.Value);
            Inorden(node.Right, result);
        }

        private void Preorden(TreeNode<dynamic> node, List<dynamic> result)
        {
            if (node == null) return;
            result.Add(node.Value);
            Preorden(node.Left, result);
            Preorden(node.Right, result);
        }

        private void Postorden(TreeNode<dynamic> node, List<dynamic> result)
        {
            if (node == null) return;
            Postorden(node.Left, result);
            Postorden(node.Right, result);
            result.Add(node.Value);
        }

        // --- BALANCEO ---
        private TreeNode<dynamic> BalancearArbol(List<dynamic> elementos, int inicio, int fin, TreeNode<dynamic> parent = null)
        {
            if (inicio > fin) return null;
            int medio = (inicio + fin) / 2;
            TreeNode<dynamic> nodo = new TreeNode<dynamic>(elementos[medio]);
            nodo.Parent = parent;
            nodo.Left = BalancearArbol(elementos, inicio, medio - 1, nodo);
            nodo.Right = BalancearArbol(elementos, medio + 1, fin, nodo);
            return nodo;
        }

        // --- DIBUJO ---
        private void Redibujar()
        {
            canvas.Children.Clear();
            ActualizarEstadisticas();

            if (root == null) return;

            int altura = CalcAltura(root);
            double anchoMinimo = Math.Pow(2, altura - 1) * (NODO_RADIO * 2.5);
            canvas.Width = Math.Max(1200, anchoMinimo);
            canvas.Height = Math.Max(600, altura * yOffset + 100);

            DibujarArbol(root, canvas.Width / 2, 40, 1);
        }

        private void DibujarArbol(TreeNode<dynamic> node, double x, double y, int nivel)
        {
            if (node == null) return;

            double espaciadoHorizontal = canvas.Width / Math.Pow(2, nivel + 1);
            double espaciadoVertical = yOffset;

            if (node.Left != null)
            {
                double xHijo = x - espaciadoHorizontal;
                double yHijo = y + espaciadoVertical;
                DibujarLinea(x, y, xHijo, yHijo);
                DibujarArbol(node.Left, xHijo, yHijo, nivel + 1);
            }

            if (node.Right != null)
            {
                double xHijo = x + espaciadoHorizontal;
                double yHijo = y + espaciadoVertical;
                DibujarLinea(x, y, xHijo, yHijo);
                DibujarArbol(node.Right, xHijo, yHijo, nivel + 1);
            }

            DibujarNodo(node, x, y);
        }

        private void DibujarNodo(TreeNode<dynamic> node, double x, double y)
        {
            Ellipse nodoVisual = new Ellipse
            {
                Width = NODO_RADIO * 2,
                Height = NODO_RADIO * 2,
                Fill = (SolidColorBrush)FindResource("ColorPrimario"),
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            Canvas.SetLeft(nodoVisual, x - NODO_RADIO);
            Canvas.SetTop(nodoVisual, y - NODO_RADIO);
            canvas.Children.Add(nodoVisual);

            TextBlock txt = new TextBlock
            {
                Text = node.Value.ToString(),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 12
            };
            Canvas.SetLeft(txt, x - (node.Value.ToString().Length * 3));
            Canvas.SetTop(txt, y - 7);
            canvas.Children.Add(txt);
        }

        private void DibujarLinea(double x1, double y1, double x2, double y2)
        {
            Line linea = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                StrokeThickness = 1.5
            };
            canvas.Children.Add(linea);
        }

        // --- ACTUALIZACIÓN DE ESTADÍSTICAS ---
        private void ActualizarEstadisticas()
        {
            int totalNodos = ContarNodos(root);
            int altura = CalcAltura(root);

            TxtNodos.Text = $"Nodos: {totalNodos}";
            TxtAltura.Text = $"Altura: {altura}";

            if (root != null)
            {
                bool completo = EsArbolCompleto(root, 0, totalNodos);
                TxtEsCompleto.Text = completo ? "Sí" : "No";

                bool perfecto = EsArbolPerfecto(root, altura);
                TxtEsPerfecto.Text = perfecto ? "Sí" : "No";
            }
            else
            {
                TxtEsCompleto.Text = "N/A";
                TxtEsPerfecto.Text = "N/A";
            }
        }

        // --- EVENTOS ---
        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNodeValue.Text))
            {
                MessageBox.Show("Ingrese un valor.");
                return;
            }

            try
            {
                dynamic valor;
                if (rbIntegers.IsChecked == true)
                {
                    if (!int.TryParse(txtNodeValue.Text, out int intValue))
                    {
                        MessageBox.Show("Ingrese un número entero válido.");
                        return;
                    }
                    valor = intValue;
                }
                else
                {
                    valor = txtNodeValue.Text[0];
                }

                root = InsertNode(root, valor);
                Redibujar();
                txtNodeValue.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                MessageBox.Show("El árbol está vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNodeValue.Text))
            {
                MessageBox.Show("Ingrese el valor a eliminar.");
                return;
            }

            try
            {
                dynamic valor;
                if (rbIntegers.IsChecked == true)
                {
                    if (!int.TryParse(txtNodeValue.Text, out int intValue))
                    {
                        MessageBox.Show("Ingrese un número entero válido.");
                        return;
                    }
                    valor = intValue;
                }
                else
                {
                    valor = txtNodeValue.Text[0];
                }

                root = EliminarNodo(root, valor);
                Redibujar();
                txtNodeValue.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}");
            }
        }

        private void Balancear_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                MessageBox.Show("El árbol está vacío.");
                return;
            }

            List<dynamic> elementos = new List<dynamic>();
            Inorden(root, elementos);
            root = BalancearArbol(elementos, 0, elementos.Count - 1);
            Redibujar();
            MessageBox.Show("Árbol balanceado correctamente.");
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de limpiar el árbol?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                root = null;
                canvas.Children.Clear();
                txtResultado.Text = "Árbol limpio.";
                ActualizarEstadisticas();
            }
        }

        private void BtnInsertarRaiz_Click(object sender, RoutedEventArgs e)
        {
            if (root != null)
            {
                MessageBox.Show("Ya existe una raíz.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNuevoValor.Text))
            {
                MessageBox.Show("Ingrese un valor para la raíz.");
                return;
            }

            try
            {
                dynamic valor;
                if (rbIntegers.IsChecked == true)
                {
                    if (!int.TryParse(txtNuevoValor.Text, out int intValue))
                    {
                        MessageBox.Show("Ingrese un número entero válido.");
                        return;
                    }
                    valor = intValue;
                }
                else
                {
                    valor = txtNuevoValor.Text[0];
                }

                root = new TreeNode<dynamic>(valor);
                Redibujar();
                txtNuevoValor.Clear();
                txtResultado.Text = $"Raíz '{valor}' creada.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnEsCompleto_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                MessageBox.Show("El árbol está vacío.");
                return;
            }

            int totalNodos = ContarNodos(root);
            bool completo = EsArbolCompleto(root, 0, totalNodos);
            MessageBox.Show(completo ? "El árbol es completo." : "El árbol no es completo.");
        }

        private void BtnEsPerfecto_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                MessageBox.Show("El árbol está vacío.");
                return;
            }

            int altura = CalcAltura(root);
            bool perfecto = EsArbolPerfecto(root, altura);
            MessageBox.Show(perfecto ? "El árbol es perfecto." : "El árbol no es perfecto.");
        }

        private void BtnNodoRaiz_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                MessageBox.Show("El árbol está vacío.");
                return;
            }

            MessageBox.Show($"El nodo raíz es: {root.Value}");
        }

        private void Preorden_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                txtResultado.Text = "El árbol está vacío.";
                return;
            }
            var r = new List<dynamic>();
            Preorden(root, r);
            txtResultado.Text = "Preorden: " + string.Join(" → ", r);
        }

        private void Inorden_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                txtResultado.Text = "El árbol está vacío.";
                return;
            }
            var r = new List<dynamic>();
            Inorden(root, r);
            txtResultado.Text = "Inorden: " + string.Join(" → ", r);
        }

        private void Postorden_Click(object sender, RoutedEventArgs e)
        {
            if (root == null)
            {
                txtResultado.Text = "El árbol está vacío.";
                return;
            }
            var r = new List<dynamic>();
            Postorden(root, r);
            txtResultado.Text = "Postorden: " + string.Join(" → ", r);
        }
    }
}