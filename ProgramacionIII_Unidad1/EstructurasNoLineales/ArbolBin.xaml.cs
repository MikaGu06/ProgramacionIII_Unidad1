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
        private Nodo raiz;

        // Mapa de posiciones inorden para el dibujo (algoritmo propio)
        private readonly Dictionary<Nodo, int> _posicionesInorden = new Dictionary<Nodo, int>();

        public ArbolBinario()
        {
            InitializeComponent();
            ActualizarVistaCompleta();
        }

        // ──────────────────────────────────────────────
        #region Modelo
        // ──────────────────────────────────────────────
        private class Nodo
        {
            public string Valor { get; set; }
            public Nodo Izquierda { get; set; }
            public Nodo Derecha { get; set; }

            public Nodo(string valor)
            {
                Valor = valor;
            }
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Helpers de tipo y comparación (algoritmos propios)
        // ──────────────────────────────────────────────
        private bool EsModoEntero()
        {
            return rbIntegers.IsChecked == true;
        }

        private bool ValidarValor(string texto, out string valorNormalizado)
        {
            valorNormalizado = string.Empty;
            texto = (texto ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(texto))
                return false;

            if (EsModoEntero())
            {
                if (int.TryParse(texto, out int numero))
                {
                    valorNormalizado = numero.ToString();
                    return true;
                }
                return false;
            }

            if (texto.Length == 1)
            {
                valorNormalizado = texto.ToUpper();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Comparacion propia: implementa la logica directamente para enteros
        /// y caracteres sin delegar a metodos nativos de ordenamiento.
        /// </summary>
        private int CompararValores(string a, string b)
        {
            if (EsModoEntero())
            {
                int va = int.Parse(a);
                int vb = int.Parse(b);
                if (va < vb) return -1;
                if (va > vb) return 1;
                return 0;
            }

            char ca = char.ToUpper(a[0]);
            char cb = char.ToUpper(b[0]);
            if (ca < cb) return -1;
            if (ca > cb) return 1;
            return 0;
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Insercion BST (algoritmo propio recursivo)
        // ──────────────────────────────────────────────
        private Nodo InsertarBST(Nodo nodo, string valor)
        {
            if (nodo == null)
                return new Nodo(valor);

            int cmp = CompararValores(valor, nodo.Valor);

            if (cmp < 0)
                nodo.Izquierda = InsertarBST(nodo.Izquierda, valor);
            else if (cmp > 0)
                nodo.Derecha = InsertarBST(nodo.Derecha, valor);

            return nodo;
        }

        private bool ExisteValor(Nodo nodo, string valor)
        {
            if (nodo == null) return false;

            int cmp = CompararValores(valor, nodo.Valor);
            if (cmp == 0) return true;
            if (cmp < 0) return ExisteValor(nodo.Izquierda, valor);
            return ExisteValor(nodo.Derecha, valor);
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Insercion manual
        // ──────────────────────────────────────────────
        private Nodo BuscarNodo(Nodo nodo, string valor)
        {
            if (nodo == null) return null;

            if (string.Equals(nodo.Valor, valor, StringComparison.OrdinalIgnoreCase))
                return nodo;

            Nodo izq = BuscarNodo(nodo.Izquierda, valor);
            if (izq != null) return izq;

            return BuscarNodo(nodo.Derecha, valor);
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Eliminacion BST (algoritmo propio)
        // ──────────────────────────────────────────────
        private Nodo EliminarNodo(Nodo nodo, string valor)
        {
            if (nodo == null) return null;

            int cmp = CompararValores(valor, nodo.Valor);

            if (cmp < 0)
            {
                nodo.Izquierda = EliminarNodo(nodo.Izquierda, valor);
            }
            else if (cmp > 0)
            {
                nodo.Derecha = EliminarNodo(nodo.Derecha, valor);
            }
            else
            {
                // Hoja
                if (nodo.Izquierda == null && nodo.Derecha == null)
                    return null;

                // Un solo hijo
                if (nodo.Izquierda == null) return nodo.Derecha;
                if (nodo.Derecha == null) return nodo.Izquierda;

                // Dos hijos: sucesor inorden (minimo del subarbol derecho)
                Nodo sucesor = ObtenerMenor(nodo.Derecha);
                nodo.Valor = sucesor.Valor;
                nodo.Derecha = EliminarNodo(nodo.Derecha, sucesor.Valor);
            }

            return nodo;
        }

        /// <summary>
        /// Recorre hacia la izquierda iterativamente para encontrar el menor valor.
        /// Algoritmo propio, sin LINQ.
        /// </summary>
        private Nodo ObtenerMenor(Nodo nodo)
        {
            while (nodo != null && nodo.Izquierda != null)
                nodo = nodo.Izquierda;
            return nodo;
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Recorridos (algoritmos propios)
        // ──────────────────────────────────────────────
        private void Preorden(Nodo nodo, List<string> resultado)
        {
            if (nodo == null) return;
            resultado.Add(nodo.Valor);
            Preorden(nodo.Izquierda, resultado);
            Preorden(nodo.Derecha, resultado);
        }

        private void Inorden(Nodo nodo, List<string> resultado)
        {
            if (nodo == null) return;
            Inorden(nodo.Izquierda, resultado);
            resultado.Add(nodo.Valor);
            Inorden(nodo.Derecha, resultado);
        }

        private void Postorden(Nodo nodo, List<string> resultado)
        {
            if (nodo == null) return;
            Postorden(nodo.Izquierda, resultado);
            Postorden(nodo.Derecha, resultado);
            resultado.Add(nodo.Valor);
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Propiedades del arbol (algoritmos propios)
        // ──────────────────────────────────────────────
        private int ContarNodos(Nodo nodo)
        {
            if (nodo == null) return 0;
            return 1 + ContarNodos(nodo.Izquierda) + ContarNodos(nodo.Derecha);
        }

        private int CalcularAltura(Nodo nodo)
        {
            if (nodo == null) return 0;
            int altIzq = CalcularAltura(nodo.Izquierda);
            int altDer = CalcularAltura(nodo.Derecha);
            return 1 + (altIzq > altDer ? altIzq : altDer);
        }

        /// <summary>
        /// Arbol completo: todos los niveles llenos excepto quiza el ultimo,
        /// que se llena de izquierda a derecha.
        /// Implementado con lista como cola (sin Queue nativa de .NET).
        /// </summary>
        private bool EsCompleto(Nodo nodo)
        {
            if (nodo == null) return true;

            List<Nodo> cola = new List<Nodo>();
            int frente = 0;
            cola.Add(nodo);
            bool seEncontroHueco = false;

            while (frente < cola.Count)
            {
                Nodo actual = cola[frente++];

                if (actual.Izquierda != null)
                {
                    if (seEncontroHueco) return false;
                    cola.Add(actual.Izquierda);
                }
                else
                {
                    seEncontroHueco = true;
                }

                if (actual.Derecha != null)
                {
                    if (seEncontroHueco) return false;
                    cola.Add(actual.Derecha);
                }
                else
                {
                    seEncontroHueco = true;
                }
            }

            return true;
        }

        private bool EsPerfecto(Nodo nodo)
        {
            int profundidad = ObtenerProfundidadIzquierda(nodo);
            return EsPerfectoRec(nodo, 1, profundidad);
        }

        private int ObtenerProfundidadIzquierda(Nodo nodo)
        {
            int d = 0;
            while (nodo != null)
            {
                d++;
                nodo = nodo.Izquierda;
            }
            return d;
        }

        private bool EsPerfectoRec(Nodo nodo, int nivel, int profundidad)
        {
            if (nodo == null) return true;

            if (nodo.Izquierda == null && nodo.Derecha == null)
                return nivel == profundidad;

            if (nodo.Izquierda == null || nodo.Derecha == null)
                return false;

            return EsPerfectoRec(nodo.Izquierda, nivel + 1, profundidad) &&
                   EsPerfectoRec(nodo.Derecha, nivel + 1, profundidad);
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Balanceo (Inorden + burbuja + reconstruccion, algoritmos propios)
        // ──────────────────────────────────────────────
        private void RecolectarValoresInorden(Nodo nodo, List<string> valores)
        {
            if (nodo == null) return;
            RecolectarValoresInorden(nodo.Izquierda, valores);
            valores.Add(nodo.Valor);
            RecolectarValoresInorden(nodo.Derecha, valores);
        }

        /// <summary>
        /// Ordenamiento burbuja propio — no usa Array.Sort ni LINQ.
        /// </summary>
        private void OrdenarBurbuja(List<string> valores)
        {
            int n = valores.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (CompararValores(valores[j], valores[j + 1]) > 0)
                    {
                        string tmp = valores[j];
                        valores[j] = valores[j + 1];
                        valores[j + 1] = tmp;
                    }
                }
            }
        }

        private Nodo ConstruirBalanceado(List<string> valores, int ini, int fin)
        {
            if (ini > fin) return null;

            int medio = (ini + fin) / 2;
            Nodo nodo = new Nodo(valores[medio]);
            nodo.Izquierda = ConstruirBalanceado(valores, ini, medio - 1);
            nodo.Derecha = ConstruirBalanceado(valores, medio + 1, fin);
            return nodo;
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Dibujo — layout por posicion inorden (algoritmo propio)
        // ──────────────────────────────────────────────

        /// <summary>
        /// Asigna a cada nodo un indice inorden unico (0, 1, 2, ..., N-1).
        /// Garantia: en el eje X ningun par de nodos comparte indice,
        /// por lo que NUNCA se superpondran sin importar la altura ni la forma
        /// del arbol. El canvas se redimensiona para mostrar el arbol completo.
        /// </summary>
        private void AsignarPosicionesInorden(Nodo nodo, ref int contador)
        {
            if (nodo == null) return;
            AsignarPosicionesInorden(nodo.Izquierda, ref contador);
            _posicionesInorden[nodo] = contador++;
            AsignarPosicionesInorden(nodo.Derecha, ref contador);
        }

        private void DibujarArbol()
        {
            canvas.Children.Clear();
            _posicionesInorden.Clear();

            if (raiz == null) return;

            // Calcular dimensiones minimas necesarias para mostrar el arbol completo
            int totalNodos = ContarNodos(raiz);
            int altura = CalcularAltura(raiz);

            const double espH = 72;   // px por ranura horizontal
            const double espV = 90;   // px por nivel vertical
            const double radio = 24;
            const double margenH = 56;
            const double margenV = 50;

            double anchoNecesario = Math.Max(totalNodos * espH + margenH * 2, 900);
            double altoNecesario = Math.Max(altura * espV + margenV * 2, 560);

            canvas.Width = anchoNecesario;
            canvas.Height = altoNecesario;

            // Asignar posicion inorden a cada nodo
            int contador = 0;
            AsignarPosicionesInorden(raiz, ref contador);

            // Dibujar el arbol desde la raiz
            DibujarNodo(raiz, espH, espV, radio, margenH, margenV, 0);
        }

        /// <summary>
        /// Dibuja primero las lineas hacia hijos y luego el circulo del nodo,
        /// para que los circulos queden sobre las lineas visualmente.
        /// </summary>
        private void DibujarNodo(Nodo nodo, double espH, double espV, double radio,
                                  double margenH, double margenV, int profundidad)
        {
            if (nodo == null) return;

            double x = _posicionesInorden[nodo] * espH + espH / 2.0 + margenH;
            double y = profundidad * espV + margenV;

            // Lineas a hijos (se dibujan antes que el circulo)
            if (nodo.Izquierda != null)
            {
                double xH = _posicionesInorden[nodo.Izquierda] * espH + espH / 2.0 + margenH;
                double yH = (profundidad + 1) * espV + margenV;
                DibujarLinea(x, y, xH, yH);
                DibujarNodo(nodo.Izquierda, espH, espV, radio, margenH, margenV, profundidad + 1);
            }

            if (nodo.Derecha != null)
            {
                double xH = _posicionesInorden[nodo.Derecha] * espH + espH / 2.0 + margenH;
                double yH = (profundidad + 1) * espV + margenV;
                DibujarLinea(x, y, xH, yH);
                DibujarNodo(nodo.Derecha, espH, espV, radio, margenH, margenV, profundidad + 1);
            }

            // Circulo del nodo
            Ellipse circulo = new Ellipse
            {
                Width = radio * 2,
                Height = radio * 2,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F73A1")),
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262233")),
                StrokeThickness = 2
            };
            Canvas.SetLeft(circulo, x - radio);
            Canvas.SetTop(circulo, y - radio);
            canvas.Children.Add(circulo);

            // Etiqueta del valor centrada en el nodo
            TextBlock texto = new TextBlock
            {
                Text = nodo.Valor,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Width = radio * 2,
                TextAlignment = TextAlignment.Center
            };
            Canvas.SetLeft(texto, x - radio);
            Canvas.SetTop(texto, y - 9);
            canvas.Children.Add(texto);
        }

        private void DibujarLinea(double x1, double y1, double x2, double y2)
        {
            Line linea = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9B8DBE")),
                StrokeThickness = 2
            };
            canvas.Children.Add(linea);
        }
        #endregion

        // ──────────────────────────────────────────────
        #region UI
        // ──────────────────────────────────────────────
        private void ActualizarIndicadores()
        {
            TxtNodos.Text = ContarNodos(raiz).ToString();
            TxtAltura.Text = CalcularAltura(raiz).ToString();
        }

        private void ActualizarVistaCompleta()
        {
            ActualizarIndicadores();
            DibujarArbol();
        }

        private void LimpiarEntradas()
        {
            txtNodeValue.Clear();
            txtPadre.Clear();
            txtNuevoValor.Clear();
            txtNodeValue.Focus();
        }

        private void MostrarMensaje(string mensaje)
        {
            txtResultado.Text = mensaje;
        }
        #endregion

        // ──────────────────────────────────────────────
        #region Eventos de botones
        // ──────────────────────────────────────────────
        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarValor(txtNodeValue.Text, out string valor))
            {
                MostrarMensaje(EsModoEntero()
                    ? "Ingresa un numero entero valido."
                    : "Ingresa un solo caracter valido.");
                return;
            }

            if (ExisteValor(raiz, valor))
            {
                MostrarMensaje("Ese valor ya existe en el arbol.");
                return;
            }

            raiz = InsertarBST(raiz, valor);
            ActualizarVistaCompleta();
            MostrarMensaje($"Nodo '{valor}' insertado en modo BST.");
            LimpiarEntradas();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarValor(txtNodeValue.Text, out string valor))
            {
                MostrarMensaje("Ingresa un valor valido para eliminar.");
                return;
            }

            if (!ExisteValor(raiz, valor))
            {
                MostrarMensaje("El valor que deseas eliminar no existe en el arbol.");
                return;
            }

            raiz = EliminarNodo(raiz, valor);
            ActualizarVistaCompleta();
            MostrarMensaje($"Nodo '{valor}' eliminado correctamente.");
            LimpiarEntradas();
        }

        private void Balancear_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null)
            {
                MostrarMensaje("No hay nodos para balancear.");
                return;
            }

            List<string> valores = new List<string>();
            RecolectarValoresInorden(raiz, valores);
            OrdenarBurbuja(valores);
            raiz = ConstruirBalanceado(valores, 0, valores.Count - 1);

            ActualizarVistaCompleta();
            MostrarMensaje("Arbol balanceado correctamente.");
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            raiz = null;
            canvas.Children.Clear();
            TxtEsCompleto.Text = "Verificar";
            TxtEsPerfecto.Text = "Verificar";
            ActualizarVistaCompleta();
            MostrarMensaje("Arbol reiniciado correctamente.");
            LimpiarEntradas();
        }

        private void BtnInsertarRaiz_Click(object sender, RoutedEventArgs e)
        {
            if (raiz != null)
            {
                MostrarMensaje("La raiz ya existe. Usa insercion BST o manual.");
                return;
            }

            if (!ValidarValor(txtNuevoValor.Text, out string valorRaiz))
            {
                MostrarMensaje("Ingresa un valor valido para la raiz.");
                return;
            }

            raiz = new Nodo(valorRaiz);
            ActualizarVistaCompleta();
            MostrarMensaje($"Raiz '{valorRaiz}' insertada correctamente.");
            LimpiarEntradas();
        }

        private void BtnInsertarManual_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null)
            {
                MostrarMensaje("Primero debes insertar la raiz.");
                return;
            }

            if (!ValidarValor(txtPadre.Text, out string padreValor) ||
                !ValidarValor(txtNuevoValor.Text, out string nuevoValor))
            {
                MostrarMensaje("Revisa el valor del padre y el nuevo valor.");
                return;
            }

            if (BuscarNodo(raiz, nuevoValor) != null)
            {
                MostrarMensaje("El nuevo valor ya existe en el arbol.");
                return;
            }

            Nodo padre = BuscarNodo(raiz, padreValor);
            if (padre == null)
            {
                MostrarMensaje("No se encontro el nodo padre indicado.");
                return;
            }

            bool esIzquierda = cbPosicion.SelectedIndex == 0;

            if (esIzquierda)
            {
                if (padre.Izquierda != null)
                {
                    MostrarMensaje("El hijo izquierdo ya esta ocupado.");
                    return;
                }
                padre.Izquierda = new Nodo(nuevoValor);
            }
            else
            {
                if (padre.Derecha != null)
                {
                    MostrarMensaje("El hijo derecho ya esta ocupado.");
                    return;
                }
                padre.Derecha = new Nodo(nuevoValor);
            }

            ActualizarVistaCompleta();
            MostrarMensaje($"Nodo '{nuevoValor}' insertado debajo de '{padreValor}'.");
            LimpiarEntradas();
        }

        private void Preorden_Click(object sender, RoutedEventArgs e)
        {
            List<string> r = new List<string>();
            Preorden(raiz, r);
            MostrarMensaje(r.Count == 0
                ? "El arbol esta vacio."
                : "PreOrden: " + string.Join(" -> ", r));
        }

        private void Inorden_Click(object sender, RoutedEventArgs e)
        {
            List<string> r = new List<string>();
            Inorden(raiz, r);
            MostrarMensaje(r.Count == 0
                ? "El arbol esta vacio."
                : "InOrden: " + string.Join(" -> ", r));
        }

        private void Postorden_Click(object sender, RoutedEventArgs e)
        {
            List<string> r = new List<string>();
            Postorden(raiz, r);
            MostrarMensaje(r.Count == 0
                ? "El arbol esta vacio."
                : "PostOrden: " + string.Join(" -> ", r));
        }

        private void BtnEsCompleto_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) { MostrarMensaje("El arbol esta vacio."); return; }
            bool completo = EsCompleto(raiz);
            TxtEsCompleto.Text = completo ? "Si" : "No";
            MostrarMensaje(completo ? "El arbol es completo." : "El arbol no es completo.");
        }

        private void BtnEsPerfecto_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) { MostrarMensaje("El arbol esta vacio."); return; }
            bool perfecto = EsPerfecto(raiz);
            TxtEsPerfecto.Text = perfecto ? "Si" : "No";
            MostrarMensaje(perfecto ? "El arbol es perfecto." : "El arbol no es perfecto.");
        }

        private void BtnNodoRaiz_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje(raiz == null
                ? "El arbol esta vacio."
                : "Nodo raiz: " + raiz.Valor);
        }
        #endregion
    }
}