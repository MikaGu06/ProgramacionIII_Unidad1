using System;
using System.Collections.Generic;
using System.Linq;
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
            public Point Posicion { get; set; }

            public Vertice(string nombre)
            {
                Nombre = nombre;
            }
        }

        private class Arista
        {
            public string Origen { get; set; }
            public string Destino { get; set; }
            public int Peso { get; set; }
            public bool Dirigido { get; set; }

            public string Clave
            {
                get
                {
                    if (Dirigido)
                        return $"{Origen}->{Destino}";

                    return string.Compare(Origen, Destino, StringComparison.Ordinal) <= 0
                        ? $"{Origen}-{Destino}"
                        : $"{Destino}-{Origen}";
                }
            }
        }

        private readonly List<Vertice> vertices = new List<Vertice>();
        private readonly List<Arista> aristas = new List<Arista>();
        private bool modoDirigidoPorDefecto = true;

        public Grafos()
        {
            InitializeComponent();

            if (chkDirigido != null)
            {
                chkDirigido.IsChecked = true;
            }

            if (txtModoActual != null)
            {
                txtModoActual.Text = modoDirigidoPorDefecto ? "Dirigido" : "No dirigido";
            }

            ActualizarVista();
        }

        private string NormalizarNombre(string texto)
        {
            return (texto ?? string.Empty).Trim().ToUpper();
        }

        private Vertice BuscarVertice(string nombre)
        {
            string buscado = NormalizarNombre(nombre);
            return vertices.FirstOrDefault(v => v.Nombre == buscado);
        }

        private bool ExisteArista(string origen, string destino, bool dirigido)
        {
            origen = NormalizarNombre(origen);
            destino = NormalizarNombre(destino);

            return aristas.Any(a =>
                a.Dirigido == dirigido &&
                (
                    dirigido
                        ? a.Origen == origen && a.Destino == destino
                        : ((a.Origen == origen && a.Destino == destino) || (a.Origen == destino && a.Destino == origen))
                )
            );
        }

        private Arista BuscarArista(string origen, string destino, bool dirigido)
        {
            origen = NormalizarNombre(origen);
            destino = NormalizarNombre(destino);

            return aristas.FirstOrDefault(a =>
                a.Dirigido == dirigido &&
                (
                    dirigido
                        ? a.Origen == origen && a.Destino == destino
                        : ((a.Origen == origen && a.Destino == destino) || (a.Origen == destino && a.Destino == origen))
                )
            );
        }

        private List<(string Destino, int Peso)> ObtenerVecinos(string nombre)
        {
            string actual = NormalizarNombre(nombre);
            List<(string Destino, int Peso)> vecinos = new List<(string Destino, int Peso)>();

            for (int i = 0; i < aristas.Count; i++)
            {
                Arista a = aristas[i];

                if (a.Dirigido)
                {
                    if (a.Origen == actual)
                    {
                        vecinos.Add((a.Destino, a.Peso));
                    }
                }
                else
                {
                    if (a.Origen == actual)
                    {
                        vecinos.Add((a.Destino, a.Peso));
                    }
                    else if (a.Destino == actual)
                    {
                        vecinos.Add((a.Origen, a.Peso));
                    }
                }
            }

            return vecinos;
        }

        private void MostrarEstado(string mensaje)
        {
            if (txtEstado != null)
            {
                txtEstado.Text = mensaje;
            }
        }

        private void ActualizarVista()
        {
            if (txtTotalVertices != null)
                txtTotalVertices.Text = vertices.Count.ToString();

            if (txtTotalAristas != null)
                txtTotalAristas.Text = aristas.Count.ToString();

            DibujarGrafo();
        }

        private string ConstruirListaAdyacencia()
        {
            if (vertices.Count == 0)
                return "Sin vertices.";

            List<string> lineas = new List<string>();

            for (int i = 0; i < vertices.Count; i++)
            {
                string nombre = vertices[i].Nombre;
                List<(string Destino, int Peso)> vecinos = ObtenerVecinos(nombre);

                if (vecinos.Count == 0)
                {
                    lineas.Add($"{nombre}: (sin conexiones)");
                }
                else
                {
                    string contenido = string.Join(", ", vecinos.Select(v => $"{v.Destino}({v.Peso})"));
                    lineas.Add($"{nombre}: {contenido}");
                }
            }

            return string.Join(Environment.NewLine, lineas);
        }

        private string ConstruirMatrizAdyacencia()
        {
            if (vertices.Count == 0)
                return "Sin vertices.";

            List<string> nombres = vertices.Select(v => v.Nombre).ToList();
            string resultado = "     ";

            for (int i = 0; i < nombres.Count; i++)
            {
                resultado += nombres[i].PadRight(6);
            }

            resultado += Environment.NewLine;

            for (int i = 0; i < nombres.Count; i++)
            {
                resultado += nombres[i].PadRight(5);

                for (int j = 0; j < nombres.Count; j++)
                {
                    int valor = ObtenerPesoEntre(nombres[i], nombres[j]);
                    resultado += valor.ToString().PadRight(6);
                }

                if (i < nombres.Count - 1)
                    resultado += Environment.NewLine;
            }

            return resultado;
        }

        private int ObtenerPesoEntre(string origen, string destino)
        {
            for (int i = 0; i < aristas.Count; i++)
            {
                Arista a = aristas[i];

                if (a.Dirigido)
                {
                    if (a.Origen == origen && a.Destino == destino)
                        return a.Peso;
                }
                else
                {
                    if ((a.Origen == origen && a.Destino == destino) ||
                        (a.Origen == destino && a.Destino == origen))
                        return a.Peso;
                }
            }

            return 0;
        }

        private void DistribuirVerticesEnCirculo()
        {
            double centroX = Math.Max(canvasGrafo.Width / 2, 450);
            double centroY = Math.Max(canvasGrafo.Height / 2, 300);
            double radio = Math.Min(canvasGrafo.Width, canvasGrafo.Height) / 2 - 100;

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
            if (canvasGrafo == null)
                return;

            canvasGrafo.Children.Clear();
            canvasGrafo.Width = Math.Max(900, vertices.Count * 130);
            canvasGrafo.Height = Math.Max(600, vertices.Count * 100);

            if (vertices.Count == 0)
                return;

            DistribuirVerticesEnCirculo();

            for (int i = 0; i < aristas.Count; i++)
            {
                DibujarArista(aristas[i]);
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                DibujarVertice(vertices[i]);
            }
        }

        private void DibujarArista(Arista arista)
        {
            Vertice origen = BuscarVertice(arista.Origen);
            Vertice destino = BuscarVertice(arista.Destino);

            if (origen == null || destino == null)
                return;

            if (origen.Nombre == destino.Nombre)
            {
                DibujarLazo(origen, arista.Peso, arista.Dirigido);
                return;
            }

            Line linea = new Line
            {
                X1 = origen.Posicion.X,
                Y1 = origen.Posicion.Y,
                X2 = destino.Posicion.X,
                Y2 = destino.Posicion.Y,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8E5E")),
                StrokeThickness = 2.5
            };
            canvasGrafo.Children.Add(linea);

            if (arista.Dirigido)
            {
                DibujarFlecha(origen.Posicion, destino.Posicion);
            }

            DibujarEtiquetaPeso(origen.Posicion, destino.Posicion, arista.Peso);
        }

        private void DibujarLazo(Vertice vertice, int peso, bool dirigido)
        {
            Ellipse lazo = new Ellipse
            {
                Width = 34,
                Height = 34,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8E5E")),
                StrokeThickness = 2.5,
                Fill = Brushes.Transparent
            };

            Canvas.SetLeft(lazo, vertice.Posicion.X + 10);
            Canvas.SetTop(lazo, vertice.Posicion.Y - 40);
            canvasGrafo.Children.Add(lazo);

            TextBlock txtPesoLazo = new TextBlock
            {
                Text = peso.ToString(),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E3D28")),
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBFCF7"))
            };

            Canvas.SetLeft(txtPesoLazo, vertice.Posicion.X + 20);
            Canvas.SetTop(txtPesoLazo, vertice.Posicion.Y - 48);
            canvasGrafo.Children.Add(txtPesoLazo);

            if (dirigido)
            {
                Polygon flecha = new Polygon
                {
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8E5E")),
                    Points = new PointCollection
                    {
                        new Point(vertice.Posicion.X + 35, vertice.Posicion.Y - 10),
                        new Point(vertice.Posicion.X + 25, vertice.Posicion.Y - 6),
                        new Point(vertice.Posicion.X + 30, vertice.Posicion.Y - 18)
                    }
                };

                canvasGrafo.Children.Add(flecha);
            }
        }

        private void DibujarFlecha(Point inicio, Point fin)
        {
            Vector direccion = inicio - fin;
            if (direccion.Length == 0)
                return;

            direccion.Normalize();
            Vector perpendicular = new Vector(-direccion.Y, direccion.X);

            Point punta = fin + (direccion * 28);
            Point lado1 = punta + (direccion * 14) + (perpendicular * 8);
            Point lado2 = punta + (direccion * 14) - (perpendicular * 8);

            Polygon flecha = new Polygon
            {
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B8E5E")),
                Points = new PointCollection { punta, lado1, lado2 }
            };

            canvasGrafo.Children.Add(flecha);
        }

        private void DibujarEtiquetaPeso(Point inicio, Point fin, int peso)
        {
            double medioX = (inicio.X + fin.X) / 2;
            double medioY = (inicio.Y + fin.Y) / 2;

            Border contenedor = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBFCF7")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8DFC9")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(6, 2, 6, 2)
            };

            TextBlock texto = new TextBlock
            {
                Text = peso.ToString(),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E3D28")),
                FontWeight = FontWeights.Bold
            };

            contenedor.Child = texto;

            Canvas.SetLeft(contenedor, medioX - 12);
            Canvas.SetTop(contenedor, medioY - 12);
            canvasGrafo.Children.Add(contenedor);
        }

        private void DibujarVertice(Vertice vertice)
        {
            const double radio = 24;

            Ellipse circulo = new Ellipse
            {
                Width = radio * 2,
                Height = radio * 2,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F7A3D")),
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E3D28")),
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

            if (inicial == null)
                return resultado;

            HashSet<string> visitados = new HashSet<string>();
            Queue<string> cola = new Queue<string>();

            cola.Enqueue(inicial.Nombre);
            visitados.Add(inicial.Nombre);

            while (cola.Count > 0)
            {
                string actual = cola.Dequeue();
                resultado.Add(actual);

                List<(string Destino, int Peso)> vecinos = ObtenerVecinos(actual);

                for (int i = 0; i < vecinos.Count; i++)
                {
                    string vecino = vecinos[i].Destino;

                    if (!visitados.Contains(vecino))
                    {
                        visitados.Add(vecino);
                        cola.Enqueue(vecino);
                    }
                }
            }

            return resultado;
        }

        private void RecorridoDfsRecursivo(string actual, HashSet<string> visitados, List<string> resultado)
        {
            visitados.Add(actual);
            resultado.Add(actual);

            List<(string Destino, int Peso)> vecinos = ObtenerVecinos(actual);

            for (int i = 0; i < vecinos.Count; i++)
            {
                string vecino = vecinos[i].Destino;

                if (!visitados.Contains(vecino))
                {
                    RecorridoDfsRecursivo(vecino, visitados, resultado);
                }
            }
        }

        private List<string> RecorridoDfs(string inicio)
        {
            List<string> resultado = new List<string>();
            Vertice inicial = BuscarVertice(inicio);

            if (inicial == null)
                return resultado;

            HashSet<string> visitados = new HashSet<string>();
            RecorridoDfsRecursivo(inicial.Nombre, visitados, resultado);
            return resultado;
        }

        private Dictionary<string, int> EjecutarDijkstra(string inicio)
        {
            Dictionary<string, int> distancias = new Dictionary<string, int>();
            HashSet<string> visitados = new HashSet<string>();

            for (int i = 0; i < vertices.Count; i++)
            {
                distancias[vertices[i].Nombre] = int.MaxValue;
            }

            if (!distancias.ContainsKey(inicio))
                return distancias;

            distancias[inicio] = 0;

            while (visitados.Count < vertices.Count)
            {
                string actual = null;
                int menor = int.MaxValue;

                foreach (var par in distancias)
                {
                    if (!visitados.Contains(par.Key) && par.Value < menor)
                    {
                        menor = par.Value;
                        actual = par.Key;
                    }
                }

                if (actual == null)
                    break;

                visitados.Add(actual);

                List<(string Destino, int Peso)> vecinos = ObtenerVecinos(actual);

                for (int i = 0; i < vecinos.Count; i++)
                {
                    string vecino = vecinos[i].Destino;
                    int peso = vecinos[i].Peso;

                    if (distancias[actual] != int.MaxValue &&
                        distancias[actual] + peso < distancias[vecino])
                    {
                        distancias[vecino] = distancias[actual] + peso;
                    }
                }
            }

            return distancias;
        }

        private void BtnAgregarVertice_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NormalizarNombre(txtVertice.Text);

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarEstado("Ingresa un nombre valido para el vertice.");
                return;
            }

            if (nombre.Length > 20)
            {
                MostrarEstado("El nombre del vertice no puede superar 20 caracteres.");
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

            aristas.RemoveAll(a => a.Origen == nombre || a.Destino == nombre);
            vertices.Remove(vertice);

            txtRecorridoResultado.Text = "Sin ejecutar";
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

            if (BuscarVertice(origen) == null || BuscarVertice(destino) == null)
            {
                MostrarEstado("Ambos vertices deben existir antes de crear la arista.");
                return;
            }

            if (!int.TryParse(txtPeso.Text, out int peso) || peso <= 0)
            {
                MostrarEstado("Ingresa un peso valido mayor a 0.");
                return;
            }

            if (ExisteArista(origen, destino, dirigido))
            {
                MostrarEstado("La arista ya existe.");
                return;
            }

            aristas.Add(new Arista
            {
                Origen = origen,
                Destino = destino,
                Peso = peso,
                Dirigido = dirigido
            });

            ActualizarVista();
            MostrarEstado($"Arista agregada: {origen} {(dirigido ? "->" : "-")} {destino} (Peso {peso})");

            txtOrigen.Clear();
            txtDestino.Clear();
            txtPeso.Clear();
        }

        private void BtnAgregarLazo_Click(object sender, RoutedEventArgs e)
        {
            string vertice = NormalizarNombre(txtOrigen.Text);

            if (string.IsNullOrWhiteSpace(vertice))
            {
                MostrarEstado("Ingresa el vertice en el campo Origen para crear el lazo.");
                return;
            }

            if (BuscarVertice(vertice) == null)
            {
                MostrarEstado("El vertice indicado no existe.");
                return;
            }

            if (!int.TryParse(txtPeso.Text, out int peso) || peso <= 0)
            {
                MostrarEstado("Ingresa un peso valido mayor a 0 para el lazo.");
                return;
            }

            bool dirigido = chkDirigido.IsChecked == true;

            if (ExisteArista(vertice, vertice, dirigido))
            {
                MostrarEstado("Ese lazo ya existe.");
                return;
            }

            aristas.Add(new Arista
            {
                Origen = vertice,
                Destino = vertice,
                Peso = peso,
                Dirigido = dirigido
            });

            ActualizarVista();
            MostrarEstado($"Lazo agregado en {vertice} con peso {peso}.");
            txtPeso.Clear();
        }

        private void BtnEliminarArista_Click(object sender, RoutedEventArgs e)
        {
            string origen = NormalizarNombre(txtOrigen.Text);
            string destino = NormalizarNombre(txtDestino.Text);
            bool dirigido = chkDirigido.IsChecked == true;

            Arista arista = BuscarArista(origen, destino, dirigido);

            if (arista == null)
            {
                MostrarEstado("La arista indicada no existe con el tipo seleccionado.");
                return;
            }

            aristas.Remove(arista);
            ActualizarVista();
            MostrarEstado("Arista eliminada correctamente.");
        }

        private void BtnModificarPeso_Click(object sender, RoutedEventArgs e)
        {
            string origen = NormalizarNombre(txtOrigen.Text);
            string destino = NormalizarNombre(txtDestino.Text);
            bool dirigido = chkDirigido.IsChecked == true;

            if (!int.TryParse(txtNuevoPeso.Text, out int nuevoPeso) || nuevoPeso <= 0)
            {
                MostrarEstado("Ingresa un nuevo peso valido mayor a 0.");
                return;
            }

            Arista arista = BuscarArista(origen, destino, dirigido);

            if (arista == null)
            {
                MostrarEstado("No se encontro la arista para modificar.");
                return;
            }

            arista.Peso = nuevoPeso;
            ActualizarVista();
            MostrarEstado($"Peso actualizado a {nuevoPeso}.");
            txtNuevoPeso.Clear();
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

        private void BtnDijkstra_Click(object sender, RoutedEventArgs e)
        {
            string inicio = NormalizarNombre(txtInicioRecorrido.Text);

            if (BuscarVertice(inicio) == null)
            {
                MostrarEstado("Verifica el vertice inicial para Dijkstra.");
                return;
            }

            for (int i = 0; i < aristas.Count; i++)
            {
                if (aristas[i].Peso <= 0)
                {
                    MostrarEstado("Dijkstra requiere pesos mayores a 0.");
                    return;
                }
            }

            Dictionary<string, int> distancias = EjecutarDijkstra(inicio);
            List<string> lineas = new List<string>();

            foreach (var item in distancias.OrderBy(x => x.Key))
            {
                string valor = item.Value == int.MaxValue ? "Inalcanzable" : item.Value.ToString();
                lineas.Add($"{inicio} -> {item.Key} = {valor}");
            }

            txtRecorridoResultado.Text = "Dijkstra:" + Environment.NewLine + string.Join(Environment.NewLine, lineas);
            MostrarEstado("Dijkstra ejecutado correctamente.");
        }

        private void BtnLista_Click(object sender, RoutedEventArgs e)
        {
            txtRecorridoResultado.Text = "Lista de adyacencia:" + Environment.NewLine + ConstruirListaAdyacencia();
            MostrarEstado("Lista de adyacencia generada.");
        }

        private void BtnMatriz_Click(object sender, RoutedEventArgs e)
        {
            txtRecorridoResultado.Text = "Matriz de adyacencia:" + Environment.NewLine + ConstruirMatrizAdyacencia();
            MostrarEstado("Matriz de adyacencia generada.");
        }

        private void BtnToggleDir_Click(object sender, RoutedEventArgs e)
        {
            modoDirigidoPorDefecto = !modoDirigidoPorDefecto;
            chkDirigido.IsChecked = modoDirigidoPorDefecto;

            if (txtModoActual != null)
            {
                txtModoActual.Text = modoDirigidoPorDefecto ? "Dirigido" : "No dirigido";
            }

            MostrarEstado(modoDirigidoPorDefecto
                ? "Modo dirigido activado."
                : "Modo no dirigido activado.");
        }

        private void BtnDemo_Click(object sender, RoutedEventArgs e)
        {
            vertices.Clear();
            aristas.Clear();

            vertices.Add(new Vertice("A"));
            vertices.Add(new Vertice("B"));
            vertices.Add(new Vertice("C"));
            vertices.Add(new Vertice("D"));
            vertices.Add(new Vertice("E"));

            aristas.Add(new Arista { Origen = "A", Destino = "B", Peso = 4, Dirigido = true });
            aristas.Add(new Arista { Origen = "A", Destino = "C", Peso = 2, Dirigido = true });
            aristas.Add(new Arista { Origen = "B", Destino = "C", Peso = 1, Dirigido = true });
            aristas.Add(new Arista { Origen = "B", Destino = "D", Peso = 5, Dirigido = true });
            aristas.Add(new Arista { Origen = "C", Destino = "D", Peso = 8, Dirigido = true });
            aristas.Add(new Arista { Origen = "C", Destino = "E", Peso = 10, Dirigido = true });
            aristas.Add(new Arista { Origen = "D", Destino = "E", Peso = 2, Dirigido = true });
            aristas.Add(new Arista { Origen = "E", Destino = "E", Peso = 3, Dirigido = true });

            chkDirigido.IsChecked = true;
            modoDirigidoPorDefecto = true;

            if (txtModoActual != null)
            {
                txtModoActual.Text = "Dirigido";
            }

            txtRecorridoResultado.Text = "Demo cargada.";
            ActualizarVista();
            MostrarEstado("Grafo de ejemplo cargado correctamente.");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            vertices.Clear();
            aristas.Clear();

            txtVertice.Clear();
            txtOrigen.Clear();
            txtDestino.Clear();
            txtPeso.Clear();
            txtNuevoPeso.Clear();
            txtInicioRecorrido.Clear();

            txtRecorridoResultado.Text = "Sin ejecutar";

            chkDirigido.IsChecked = true;
            modoDirigidoPorDefecto = true;

            if (txtModoActual != null)
            {
                txtModoActual.Text = "Dirigido";
            }

            ActualizarVista();
            MostrarEstado("Grafo reiniciado correctamente.");
        }

        private void RegresarInicio_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Inicio());
        }
    }
}