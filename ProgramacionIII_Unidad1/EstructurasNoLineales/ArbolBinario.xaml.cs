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

        // --- UTILIDADES GENERALES ---
        private dynamic ObtenerValorDesdeTexto(string texto, bool requiereTextoCompleto = false)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new Exception("Ingrese un valor válido.");

            if (rbIntegers.IsChecked == true)
            {
                if (!int.TryParse(texto, out int valorEntero))
                    throw new Exception("Ingrese un número entero válido.");

                return valorEntero;
            }

            if (requiereTextoCompleto && texto.Length == 0)
                throw new Exception("Ingrese un carácter válido.");

            return texto[0];
        }

        private bool SonIguales(dynamic a, dynamic b)
        {
            return Comparer<dynamic>.Default.Compare(a, b) == 0;
        }

        private bool ExisteValor(TreeNode<dynamic> nodo, dynamic valor)
        {
            return BuscarNodoGeneral(nodo, valor) != null;
        }

        // Búsqueda general para soportar también inserción manual
        private TreeNode<dynamic> BuscarNodoGeneral(TreeNode<dynamic> nodo, dynamic valor)
        {
            if (nodo == null)
                return null;

            if (SonIguales(nodo.Value, valor))
                return nodo;

            TreeNode<dynamic> encontradoIzquierda = BuscarNodoGeneral(nodo.Left, valor);
            if (encontradoIzquierda != null)
                return encontradoIzquierda;

            return BuscarNodoGeneral(nodo.Right, valor);
        }

        private void ReasignarPadre(TreeNode<dynamic> hijo, TreeNode<dynamic> nuevoPadre)
        {
            if (hijo != null)
                hijo.Parent = nuevoPadre;
        }

        // Reutiliza la misma lógica de selección que ya usan en búsqueda/ordenamiento
        private void OrdenarAscendente(List<dynamic> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                int indiceMinimo = i;

                for (int j = i + 1; j < lista.Count; j++)
                {
                    if (Comparer<dynamic>.Default.Compare(lista[j], lista[indiceMinimo]) < 0)
                    {
                        indiceMinimo = j;
                    }
                }

                dynamic temp = lista[i];
                lista[i] = lista[indiceMinimo];
                lista[indiceMinimo] = temp;
            }
        }

        private void RecolectarValores(TreeNode<dynamic> nodo, List<dynamic> valores)
        {
            if (nodo == null)
                return;

            RecolectarValores(nodo.Left, valores);
            valores.Add(nodo.Value);
            RecolectarValores(nodo.Right, valores);
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
                dynamic valorPadre = ObtenerValorDesdeTexto(txtPadre.Text, true);
                dynamic nuevoValor = ObtenerValorDesdeTexto(txtNuevoValor.Text, true);

                TreeNode<dynamic> padre = BuscarNodoGeneral(root, valorPadre);

                if (padre == null)
                {
                    MessageBox.Show($"El nodo padre '{valorPadre}' no existe en el árbol.");
                    return;
                }

                if (ExisteValor(root, nuevoValor))
                {
                    MessageBox.Show($"El valor '{nuevoValor}' ya existe en el árbol.");
                    return;
                }

                bool izquierda = cbPosicion.SelectedIndex == 0;

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

                var nuevoNodo = new TreeNode<dynamic>(nuevoValor)
                {
                    Parent = padre
                };

                if (izquierda)
                    padre.Left = nuevoNodo;
                else
                    padre.Right = nuevoNodo;

                Redibujar();
                txtPadre.Clear();
                txtNuevoValor.Clear();
                MessageBox.Show($"Nodo '{nuevoValor}' insertado como hijo {(izquierda ? "izquierdo" : "derecho")} de '{padre.Value}'.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // --- ELIMINACIÓN GENERAL ---
        private TreeNode<dynamic> ObtenerNodoMasIzquierdo(TreeNode<dynamic> nodo)
        {
            if (nodo == null)
                return null;

            while (nodo.Left != null)
                nodo = nodo.Left;

            return nodo;
        }

        private void ReemplazarEnPadre(TreeNode<dynamic> nodo, TreeNode<dynamic> reemplazo)
        {
            if (nodo.Parent == null)
            {
                root = reemplazo;
                ReasignarPadre(reemplazo, null);
                return;
            }

            if (nodo.Parent.Left == nodo)
                nodo.Parent.Left = reemplazo;
            else if (nodo.Parent.Right == nodo)
                nodo.Parent.Right = reemplazo;

            ReasignarPadre(reemplazo, nodo.Parent);
        }

        private bool EliminarNodoGeneral(dynamic valor)
        {
            TreeNode<dynamic> nodo = BuscarNodoGeneral(root, valor);

            if (nodo == null)
                return false;

            // Caso 1: hoja
            if (nodo.Left == null && nodo.Right == null)
            {
                ReemplazarEnPadre(nodo, null);
                return true;
            }

            // Caso 2: un solo hijo
            if (nodo.Left == null)
            {
                ReemplazarEnPadre(nodo, nodo.Right);
                return true;
            }

            if (nodo.Right == null)
            {
                ReemplazarEnPadre(nodo, nodo.Left);
                return true;
            }

            // Caso 3: dos hijos
            TreeNode<dynamic> sucesor = ObtenerNodoMasIzquierdo(nodo.Right);
            nodo.Value = sucesor.Value;

            // El sucesor nunca tendrá hijo izquierdo
            if (sucesor.Right != null)
                ReemplazarEnPadre(sucesor, sucesor.Right);
            else
                ReemplazarEnPadre(sucesor, null);

            return true;
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
                dynamic valor = ObtenerValorDesdeTexto(txtNodeValue.Text, true);
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
                dynamic valor = ObtenerValorDesdeTexto(txtNodeValue.Text, true);

                bool eliminado = EliminarNodoGeneral(valor);
                if (!eliminado)
                {
                    MessageBox.Show($"El valor {valor} no se encuentra en el árbol.");
                    return;
                }

                Redibujar();
                txtNodeValue.Clear();
                txtResultado.Text = $"Nodo '{valor}' eliminado correctamente.";
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
            RecolectarValores(root, elementos);
            OrdenarAscendente(elementos);
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
                dynamic valor = ObtenerValorDesdeTexto(txtNuevoValor.Text, true);
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
