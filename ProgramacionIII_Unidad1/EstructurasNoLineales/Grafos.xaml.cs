using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgramacionIII_Unidad1.EstructurasNoLineales
{
    public partial class Grafos : Page
    {
        private class Vertice
        {
            public string Nombre { get; set; }
            public List<string> Vecinos { get; } = new List<string>();
            public Point Posicion { get; set; }

            public Vertice(string nombre)
            {
                Nombre = nombre;
            }
        }

        private readonly List<Vertice> vertices = new List<Vertice>();
        private readonly List<string> aristasVisuales = new List<string>();

        public Grafos()
        {
            InitializeComponent();
            ActualizarVista();
        }

        private string NormalizarNombre(string texto)
        {
            return (texto ?? string.Empty).Trim().ToUpper();
        }

        private Vertice BuscarVertice(string nombre)
        {
            string buscado = NormalizarNombre(nombre);
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Nombre == buscado)
                {
                    return vertices[i];
                }
            }
            return null;
        }

        private bool ExisteEnLista(List<string> lista, string valor)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i] == valor)
                {
                    return true;
                }
            }
            return false;
        }

        private void AgregarVecino(Vertice vertice, string vecino)
        {
            if (!ExisteEnLista(vertice.Vecinos, vecino))
            {
                vertice.Vecinos.Add(vecino);
            }
        }

        private void EliminarVecino(Vertice vertice, string vecino)
        {
            for (int i = 0; i < vertice.Vecinos.Count; i++)
            {
                if (vertice.Vecinos[i] == vecino)
                {
                    vertice.Vecinos.RemoveAt(i);
                    return;
                }
            }
        }

        private string CrearClaveArista(string origen, string destino, bool dirigido)
        {
            if (dirigido)
            {
                return origen + "->" + destino;
            }

            if (string.Compare(origen, destino, StringComparison.Ordinal) < 0)
            {
                return origen + "-" + destino;
            }

            return destino + "-" + origen;
        }

        private void MostrarEstado(string mensaje)
        {
            txtEstado.Text = mensaje;
        }

        private void ActualizarVista()
        {
            txtTotalVertices.Text = vertices.Count.ToString();
            txtTotalAristas.Text = aristasVisuales.Count.ToString();
            txtListaAdyacencia.Text = ConstruirListaAdyacencia();
            txtMatriz.Text = ConstruirMatrizAdyacencia();
            DibujarGrafo();
        }

        private string ConstruirListaAdyacencia()
        {
            if (vertices.Count == 0)
            {
                return "Sin vertices";
            }

            string resultado = string.Empty;
            for (int i = 0; i < vertices.Count; i++)
            {
                resultado += vertices[i].Nombre + ": ";
                if (vertices[i].Vecinos.Count == 0)
                {
                    resultado += "(sin conexiones)";
                }
                else
                {
                    for (int j = 0; j < vertices[i].Vecinos.Count; j++)
                    {
                        resultado += vertices[i].Vecinos[j];
                        if (j < vertices[i].Vecinos.Count - 1)
                        {
                            resultado += ", ";
                        }
                    }
                }

                if (i < vertices.Count - 1)
                {
                    resultado += Environment.NewLine;
                }
            }
            return resultado;
        }

        private string ConstruirMatrizAdyacencia()
        {
            if (vertices.Count == 0)
            {
                return "Sin vertices";
            }

            string resultado = "    ";
            for (int i = 0; i < vertices.Count; i++)
            {
                resultado += vertices[i].Nombre.PadRight(4);
            }
            resultado += Environment.NewLine;

            for (int i = 0; i < vertices.Count; i++)
            {
                resultado += vertices[i].Nombre.PadRight(4);
                for (int j = 0; j < vertices.Count; j++)
                {
                    string valor = ExisteEnLista(vertices[i].Vecinos, vertices[j].Nombre) ? "1" : "0";
                    resultado += valor.PadRight(4);
                }
                if (i < vertices.Count - 1)
                {
                    resultado += Environment.NewLine;
                }
            }

            return resultado;
        }

        private void DistribuirVerticesEnCirculo()
        {
            double centroX = Math.Max(canvasGrafo.Width / 2, 450);
            double centroY = Math.Max(canvasGrafo.Height / 2, 300);
            double radio = Math.Min(canvasGrafo.Width, canvasGrafo.Height) / 2 - 90;

            if (vertices.Count == 1)
            {
                vertices[0].Posicion = new Point(centroX, centroY);
                return;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                double angulo = (2 * Math.PI * i / vertices.Count) - Math.PI / 2;
                double x = centroX + radio * Math.Cos(angulo);
                double y = centroY + radio * Math.Sin(angulo);
                vertices[i].Posicion = new Point(x, y);
            }
        }

        private void DibujarGrafo()
        {
            canvasGrafo.Children.Clear();
            canvasGrafo.Width = Math.Max(900, vertices.Count * 120);
            canvasGrafo.Height = Math.Max(600, vertices.Count * 90);

            if (vertices.Count == 0)
            {
                return;
            }

            DistribuirVerticesEnCirculo();

            for (int i = 0; i < aristasVisuales.Count; i++)
            {
                DibujarArista(aristasVisuales[i]);
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                DibujarVertice(vertices[i]);
            }
        }

        private void DibujarArista(string clave)
        {
            bool dirigido = clave.Contains("->");
            string separador = dirigido ? "->" : "-";
            string[] partes = clave.Split(new[] { separador }, StringSplitOptions.None);
            if (partes.Length != 2) return;

            Vertice origen = BuscarVertice(partes[0]);
            Vertice destino = BuscarVertice(partes[1]);
            if (origen == null || destino == null) return;

            Line linea = new Line
            {
                X1 = origen.Posicion.X,
                Y1 = origen.Posicion.Y,
                X2 = destino.Posicion.X,
                Y2 = destino.Posicion.Y,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9B8DBE")),
                StrokeThickness = 2.5
            };
            canvasGrafo.Children.Add(linea);

            if (dirigido)
            {
                DibujarFlecha(origen.Posicion, destino.Posicion);
            }
        }

        private void DibujarFlecha(Point inicio, Point fin)
        {
            Vector direccion = inicio - fin;
            direccion.Normalize();
            Vector perpendicular = new Vector(-direccion.Y, direccion.X);

            Point punta = fin + (direccion * 28);
            Point lado1 = punta + (direccion * 14) + (perpendicular * 8);
            Point lado2 = punta + (direccion * 14) - (perpendicular * 8);

            Polygon flecha = new Polygon
            {
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9B8DBE")),
                Points = new PointCollection { punta, lado1, lado2 }
            };
            canvasGrafo.Children.Add(flecha);
        }

        private void DibujarVertice(Vertice vertice)
        {
            const double radio = 24;
            Ellipse circulo = new Ellipse
            {
                Width = radio * 2,
                Height = radio * 2,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B5BD6")),
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262233")),
                StrokeThickness = 2
            };

            Canvas.SetLeft(circulo, vertice.Posicion.X - radio);
            Canvas.SetTop(circulo, vertice.Posicion.Y - radio);
            canvasGrafo.Children.Add(circulo);

            TextBlock texto = new TextBlock
            {
                Text = vertice.Nombre,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Width = radio * 2,
                TextAlignment = TextAlignment.Center
            };
            Canvas.SetLeft(texto, vertice.Posicion.X - radio);
            Canvas.SetTop(texto, vertice.Posicion.Y - 10);
            canvasGrafo.Children.Add(texto);
        }

        private List<string> RecorridoBfs(string inicio)
        {
            List<string> resultado = new List<string>();
            Vertice inicial = BuscarVertice(inicio);
            if (inicial == null) return resultado;

            List<string> visitados = new List<string>();
            List<string> cola = new List<string>();
            int frente = 0;

            cola.Add(inicial.Nombre);
            visitados.Add(inicial.Nombre);

            while (frente < cola.Count)
            {
                string actual = cola[frente++];
                resultado.Add(actual);
                Vertice verticeActual = BuscarVertice(actual);

                for (int i = 0; i < verticeActual.Vecinos.Count; i++)
                {
                    string vecino = verticeActual.Vecinos[i];
                    if (!ExisteEnLista(visitados, vecino))
                    {
                        visitados.Add(vecino);
                        cola.Add(vecino);
                    }
                }
            }

            return resultado;
        }

        private void RecorridoDfsRecursivo(string actual, List<string> visitados, List<string> resultado)
        {
            visitados.Add(actual);
            resultado.Add(actual);

            Vertice verticeActual = BuscarVertice(actual);
            for (int i = 0; i < verticeActual.Vecinos.Count; i++)
            {
                string vecino = verticeActual.Vecinos[i];
                if (!ExisteEnLista(visitados, vecino))
                {
                    RecorridoDfsRecursivo(vecino, visitados, resultado);
                }
            }
        }

        private List<string> RecorridoDfs(string inicio)
        {
            List<string> resultado = new List<string>();
            Vertice inicial = BuscarVertice(inicio);
            if (inicial == null) return resultado;

            List<string> visitados = new List<string>();
            RecorridoDfsRecursivo(inicial.Nombre, visitados, resultado);
            return resultado;
        }

        private void BtnAgregarVertice_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NormalizarNombre(txtVertice.Text);
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarEstado("Ingresa un nombre valido para el vertice.");
                return;
            }

            if (BuscarVertice(nombre) != null)
            {
                MostrarEstado("Ese vertice ya existe.");
                return;
            }

            vertices.Add(new Vertice(nombre));
            ActualizarVista();
            MostrarEstado("Vertice agregado: " + nombre);
            txtVertice.Clear();
        }

        private void BtnEliminarVertice_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NormalizarNombre(txtVertice.Text);
            Vertice vertice = BuscarVertice(nombre);
            if (vertice == null)
            {
                MostrarEstado("No se encontro el vertice a eliminar.");
                return;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                EliminarVecino(vertices[i], nombre);
            }

            for (int i = aristasVisuales.Count - 1; i >= 0; i--)
            {
                if (aristasVisuales[i].Contains(nombre + "->") || aristasVisuales[i].Contains("->" + nombre) ||
                    aristasVisuales[i].Contains(nombre + "-") || aristasVisuales[i].EndsWith("-" + nombre))
                {
                    aristasVisuales.RemoveAt(i);
                }
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Nombre == nombre)
                {
                    vertices.RemoveAt(i);
                    break;
                }
            }

            ActualizarVista();
            MostrarEstado("Vertice eliminado: " + nombre);
            txtVertice.Clear();
        }

        private void BtnAgregarArista_Click(object sender, RoutedEventArgs e)
        {
            string origen = NormalizarNombre(txtOrigen.Text);
            string destino = NormalizarNombre(txtDestino.Text);
            bool dirigido = chkDirigido.IsChecked == true;

            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino))
            {
                MostrarEstado("Ingresa origen y destino.");
                return;
            }

            Vertice vOrigen = BuscarVertice(origen);
            Vertice vDestino = BuscarVertice(destino);
            if (vOrigen == null || vDestino == null)
            {
                MostrarEstado("Ambos vertices deben existir antes de crear la arista.");
                return;
            }

            string clave = CrearClaveArista(origen, destino, dirigido);
            if (ExisteEnLista(aristasVisuales, clave))
            {
                MostrarEstado("La arista ya existe.");
                return;
            }

            AgregarVecino(vOrigen, destino);
            if (!dirigido)
            {
                AgregarVecino(vDestino, origen);
            }

            aristasVisuales.Add(clave);
            ActualizarVista();
            MostrarEstado("Arista agregada: " + clave);
            txtOrigen.Clear();
            txtDestino.Clear();
        }

        private void BtnEliminarArista_Click(object sender, RoutedEventArgs e)
        {
            string origen = NormalizarNombre(txtOrigen.Text);
            string destino = NormalizarNombre(txtDestino.Text);
            bool dirigido = chkDirigido.IsChecked == true;
            string clave = CrearClaveArista(origen, destino, dirigido);

            if (!ExisteEnLista(aristasVisuales, clave))
            {
                MostrarEstado("La arista indicada no existe con el tipo seleccionado.");
                return;
            }

            Vertice vOrigen = BuscarVertice(origen);
            Vertice vDestino = BuscarVertice(destino);
            if (vOrigen != null) EliminarVecino(vOrigen, destino);
            if (!dirigido && vDestino != null) EliminarVecino(vDestino, origen);

            for (int i = 0; i < aristasVisuales.Count; i++)
            {
                if (aristasVisuales[i] == clave)
                {
                    aristasVisuales.RemoveAt(i);
                    break;
                }
            }

            ActualizarVista();
            MostrarEstado("Arista eliminada: " + clave);
        }

        private void BtnBfs_Click(object sender, RoutedEventArgs e)
        {
            string inicio = NormalizarNombre(txtInicioRecorrido.Text);
            List<string> recorrido = RecorridoBfs(inicio);
            if (recorrido.Count == 0)
            {
                MostrarEstado("No se pudo ejecutar BFS. Verifica el vertice inicial.");
                return;
            }

            txtRecorridoResultado.Text = "BFS: " + string.Join(" -> ", recorrido);
            MostrarEstado("Recorrido BFS ejecutado correctamente.");
        }

        private void BtnDfs_Click(object sender, RoutedEventArgs e)
        {
            string inicio = NormalizarNombre(txtInicioRecorrido.Text);
            List<string> recorrido = RecorridoDfs(inicio);
            if (recorrido.Count == 0)
            {
                MostrarEstado("No se pudo ejecutar DFS. Verifica el vertice inicial.");
                return;
            }

            txtRecorridoResultado.Text = "DFS: " + string.Join(" -> ", recorrido);
            MostrarEstado("Recorrido DFS ejecutado correctamente.");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            vertices.Clear();
            aristasVisuales.Clear();
            txtVertice.Clear();
            txtOrigen.Clear();
            txtDestino.Clear();
            txtInicioRecorrido.Clear();
            txtRecorridoResultado.Text = "Sin ejecutar";
            MostrarEstado("Grafo reiniciado correctamente.");
            ActualizarVista();
        }
    }
}
