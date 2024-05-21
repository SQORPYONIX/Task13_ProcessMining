using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using QuikGraph;
using QuikGraph.MSAGL;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace Task13_ProcessMining
{
    public partial class MainForm : Form //Класс является partial, другая его часть генерируется дизайнером и находится в другом файле
    {
        private DFGraph dfGraph;
        private AdjacencyGraph<string, TaggedEdge<string, int>> graph;
        private const string formDFGName = "DFG Viewer";
        private const string formPetriNetName = "PetriNet Graph Viewer";
        private bool computingDFG = false;
        private bool computingPetriNet = false;
        CancellationTokenSource? cancelTokenSource = null;
        public MainForm()
        {
            InitializeComponent();
            openDFGButton.Enabled = false;
            openPetriNetButton.Enabled = false;
            comboBox.DisplayMember = "Text";
            comboBox.ValueMember = "Value";
            //Предоставленные примеры были в кодировке ANSI, поэтому добавлен выбор кодировки, для правильного отображения кириллицы на нужной кодировке.
            comboBox.DataSource = new[]
            {
                new { Text = Encoding.GetEncoding(1251).EncodingName, Value = Encoding.GetEncoding(1251) },
                new { Text = Encoding.UTF8.EncodingName, Value = Encoding.UTF8 }
            };
        }
        private void SetEnabledParametersUI(bool enabled)
        {
            selectFileButton.Enabled = enabled;
            flowLayoutPanel4.Enabled = enabled;
        }
        private void ConfigurateGraphNodesIO(Graph graph)
        {
            Node node = graph.FindNode("I");
            if (node != null)
            {
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                node.Attr.Shape = Shape.Circle;
            }
            node = graph.FindNode("O");
            if (node != null)
            {
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Orange;
                node.Attr.Shape = Shape.Circle;
            }
        }
        private void ShowGraphForm(Graph graph, string name)
        {
            //Если форма с графом открыта, то обнавляем граф на форме, иначе открываем новую форму
            foreach (Form openedForm in Application.OpenForms)
            {
                if (openedForm.Name == name)
                {
                    if (openedForm.Controls[0] is GViewer viewer)
                    {
                        viewer.Graph = graph;
                    }
                    return;
                }
            }
            GViewer newViewer = new GViewer();
            newViewer.Dock = DockStyle.Fill;
            newViewer.Graph = graph;

            Form form = new Form();
            form.SuspendLayout();
            form.Name = name;
            form.Text = name;
            form.ClientSize = new Size(600, 600);
            form.Controls.Add(newViewer);
            form.ResumeLayout();
            form.Show();
        }
        private void ComputeDFG()
        {
            computingDFG = true;
            dfGraph.Compute((int)varFilterNUD.Value, (int)actFilterNUD.Value, (int)fdFilterNUD.Value);
            graph = dfGraph.GetQuikGraph();

            actFilterNUD.Maximum = dfGraph.MaxActivityFrequency;
            fdFilterNUD.Maximum = dfGraph.MaxArcFrequency;

            varMaxFilterLabel.Text = "/" + (int)varFilterNUD.Maximum;
            actMaxFilterLabel.Text = "/" + (int)actFilterNUD.Maximum;
            fdMaxFilterLabel.Text = "/" + (int)fdFilterNUD.Maximum;

            computingDFG = false;

            //Открытую форму DFG графа обновляем. Открытый PetriNet закрываем
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form openedForm = Application.OpenForms[i]!;
                if (openedForm.Name == formDFGName)
                    ShowDFG();
                else if (openedForm.Name == formPetriNetName)
                    openedForm.Close();
            }
        }
        private void ShowDFG()
        {
            Graph dfGraphMSAGL = graph.ToMsaglGraph(
                x => x,
                (sender, args) => { args.Node.LabelText += $" ({dfGraph.GetActivityFrequency(args.Vertex)})"; },
                (sender, args) => { args.MsaglEdge.LabelText = args.Edge.Tag.ToString(); });

            ConfigurateGraphNodesIO(dfGraphMSAGL);
            ShowGraphForm(dfGraphMSAGL, formDFGName);
        }
        private async Task ShowPetriNetAsync(CancellationToken token)
        {
            PetriNet petriNet = new PetriNet(graph);
            HashSet<HashSet<string>> independentSets = petriNet.FindIndependentSets();

            progressBar.Maximum = independentSets.Count();
            Progress<int> progress = new Progress<int>((value) => { progressBar.Value = value; });

            AdjacencyGraph<string, Edge<string>> graphPetriNet = await Task.Run(() => petriNet.AlphaAlgorithm(independentSets, progress, token));

            if (graphPetriNet.Vertices.Count() == 0)
            {
                MessageBox.Show("Impossible to build a PetriNet");
                return;
            }

            Graph petriNetGraphMSAGL = graphPetriNet.ToMsaglGraph(x => x);
            ConfigurateGraphNodesIO(petriNetGraphMSAGL);
            for (int i = 1; i <= petriNet.PlacesCount; i++)
            {
                Node node = petriNetGraphMSAGL.FindNode("P" + i);
                node.LabelText = string.Empty;
                node.Attr.Shape = Shape.Circle;
            }
            ShowGraphForm(petriNetGraphMSAGL, formPetriNetName);
        }

        //Ниже идут подписчики на ивенты UI объектов (подписка происходит в сгенерированном дизайнером коде)
        private void filterNUD_ValueChanged(object sender, EventArgs e)
        {
            //В процессе вычисления DFG изменяются максимально возможные значения фильтров, которые сразу выставляются в NumericUpDown как ограничение
            //Однако если до этого значение NumericUpDown было больше, чем новое максимальное значение, то оно меняется на максимальное и вызывает ивент ValueChanged
            //Поэтому во время вычисления стоит ставится заглушка на ивент
            if (computingDFG)
                return;
            Enabled = false;
            ComputeDFG();
            Enabled = true;
        }
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    Enabled = true;
                    return;
                }

                Stream fileStream = openFileDialog.OpenFile();
                Encoding encoding = (Encoding)comboBox.SelectedValue!;
                using StreamReader reader = new StreamReader(fileStream, encoding, true);
                CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                };
                //Читаем данные с собственно заданным типом Record
                using CsvReader csvReader = new CsvReader(reader, config);
                csvReader.Read(); //Заголовок не нужен

                IEnumerable<Record> records = csvReader.GetRecords<Record>();

                dfGraph = new DFGraph(records);
            }

            varFilterNUD.Maximum = dfGraph.MaxTraceFrequency;
            ComputeDFG();
            openDFGButton.Enabled = true;
            openPetriNetButton.Enabled = true;
            Enabled = true;
        }
        private void openDFGButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            ShowDFG();
            Enabled = true;
        }
        private async void openPetriNetButton_Click(object sender, EventArgs e)
        {
            //Так как PetriNet может считаться очень долго, то рассчёт был сделан асинхронным с возможностью его отмены
            if (!computingPetriNet)
            {
                SetEnabledParametersUI(false);
                computingPetriNet = true;
                openPetriNetButton.Text = "Cancel";
                cancelTokenSource = new CancellationTokenSource();
                try
                {
                    await ShowPetriNetAsync(cancelTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    progressBar.Value = 0;
                }
                finally
                {
                    SetEnabledParametersUI(true);
                    openPetriNetButton.Text = "Open PetriNet";
                    computingPetriNet = false;
                    cancelTokenSource = null;
                }
            }
            else
            {
                openPetriNetButton.Text = "Waiting to cancel...";
                cancelTokenSource!.Cancel();
            }
        }
    }
}
