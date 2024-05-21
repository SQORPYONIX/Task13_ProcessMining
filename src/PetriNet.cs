using System.Diagnostics.CodeAnalysis;
using QuikGraph;

namespace Task13_ProcessMining
{
    public enum Relation
    {
        Independency,
        Succession,
        Predecession,
        Parallelism
    }
    public class PetriNet
    {
        public int PlacesCount { get; private set; }

        private Relation[,] relationMatrix;
        private List<string> activities = new List<string>();
        private Dictionary<string, int> activityIndices = new Dictionary<string, int>();
        private List<string> startActivities = new List<string>();
        private List<string> endActivities = new List<string>();
        private IProgress<int>? progress;
        private struct ActivitySet //Данный тип служит обёрткой для переопределения GetHashCode и Equals
        {
            public HashSet<string> Activities;
            public ActivitySet(HashSet<string> activities)
            {
                Activities = new HashSet<string>(activities);
            }
            public override int GetHashCode() 
            {
                //Переопределяем, чтобы при сравнении по хешу было сравнение всех активностей по хешу
                return Activities.Aggregate(0, (a, y) => a ^ y.GetHashCode());
            }
            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                //Переопределяем, чтобы при сравнении с другим ActivitySet было сравнение списков активностей по элементам в списках
                if (obj is not ActivitySet actSet)
                    return false;

                return Activities.SequenceEqual(actSet.Activities);
            }
        }
        public PetriNet(AdjacencyGraph<string, TaggedEdge<string, int>> dfGraph)
        {
            //Копируем граф, чтобы не изменять оригинал
            AdjacencyGraph<string, Edge<string>> graph = new AdjacencyGraph<string, Edge<string>>();
            foreach (TaggedEdge<string, int> edge in dfGraph.Edges)
            {
                graph.AddVerticesAndEdge(edge);
            }
            //Сохраняем начальные и конечные активности графа и удаляем I O
            startActivities = graph.Edges.Where(x => x.Source == "I").Select(x => x.Target).ToList();
            endActivities = graph.Edges.Where(x => x.Target == "O").Select(x => x.Source).ToList();
            graph.RemoveVertexIf(x => x == "I" || x == "O");

            relationMatrix = new Relation[graph.VertexCount, graph.VertexCount];
            int i = 0;
            //Заполняем список активностей и словарь активность-индекс для удобного обращения
            foreach (string activity in graph.Vertices)
            {
                activities.Add(activity);
                activityIndices.Add(activity, i++);
            }

            //Заполняем матрицу отношений
            //Сначала добавляем все рёбра как Последовательное отношение
            foreach (IEdge<string> edge in graph.Edges)
            {
                int sourceIndex = activityIndices[edge.Source];
                int targetIndex = activityIndices[edge.Target];

                relationMatrix[sourceIndex, targetIndex] = Relation.Succession;
            }
            //Теперь можно проверить каждое отношение
            for (i = 0; i < activities.Count; i++)
            {
                for (int j = 0; j < activities.Count; j++)
                {
                    //Если a->b и b->a значит a||b и b||a
                    if (relationMatrix[i, j] == Relation.Succession && relationMatrix[j, i] == Relation.Succession)
                        relationMatrix[i, j] = relationMatrix[j, i] = Relation.Parallelism;

                    //Если a#b и b->a значит a<-b
                    if (relationMatrix[i, j] == Relation.Independency && relationMatrix[j, i] == Relation.Succession)
                        relationMatrix[i, j] = Relation.Predecession;
                }
            }
        }
        public AdjacencyGraph<string, Edge<string>> AlphaAlgorithm(HashSet<HashSet<string>> independentSets, IProgress<int> progress, CancellationToken token)
        {
            this.progress = progress;
            HashSet<(HashSet<string>, HashSet<string>)> setsAB = FindMaximalIndependentSetsAB(independentSets, token);

            AdjacencyGraph<string, Edge<string>> graph = new AdjacencyGraph<string, Edge<string>>();

            //Добавляем Place'ы на каждуый setAB
            int placeId = 1;
            foreach ((HashSet<string>, HashSet<string>) setAB in setsAB)
            {
                foreach (string activityA in setAB.Item1)
                {
                    graph.AddVerticesAndEdge(new Edge<string>(activityA, "P" + placeId));
                    //Если это стартовая активность, то соединяем её с I
                    if (startActivities.Contains(activityA))
                    {
                        graph.AddVerticesAndEdge(new Edge<string>("I", activityA));
                        startActivities.Remove(activityA);
                    }
                }
                foreach (string activityB in setAB.Item2)
                {
                    graph.AddVerticesAndEdge(new Edge<string>("P" + placeId, activityB));
                    //Если это конечная активность, то соединяем её с O
                    if (endActivities.Contains(activityB))
                    {
                        graph.AddVerticesAndEdge(new Edge<string>(activityB, "O"));
                        endActivities.Remove(activityB);
                    }
                }
                placeId++;
            }
            PlacesCount = placeId - 1;

            return graph;
        }
        public HashSet<HashSet<string>> FindIndependentSets() //Находим все возможные независимые сеты
        {
            int size = activities.Count;
            HashSet<ActivitySet> independentSets = new HashSet<ActivitySet>();

            //Проходим каждый элемент матрицы отношений, начиная сет с активности из строки и сравниваем с каждой активностью из столбца,
            //начиная каждый проход всё правее, чтобы проверить все возможные варианты сетов
            //После полного прохода переходим к следующему элементу матрицы
            for (int i = 0; i < size; i++)
            {
                if (relationMatrix[i, i] != Relation.Independency)
                    continue;

                HashSet<string> set = new HashSet<string>() { activities[i] };
                independentSets.Add(new ActivitySet(set));

                for (int j = 0; j < size; j++)
                {
                    set.Clear();
                    set.Add(activities[i]);
                    for (int k = j; k < size; k++)
                    {
                        if (relationMatrix[i, k] != Relation.Independency || i == k)
                            continue;

                        bool allIndependent = true;
                        //Потенциальную активность для сета сравниваем со всеми уже добавленными в сет активностями на полную независимость
                        foreach (string activity in set)
                        {
                            int m = activityIndices[activity];
                            if (relationMatrix[m, k] != Relation.Independency)
                            {
                                allIndependent = false;
                                break;
                            }
                        }
                        if (allIndependent)
                        {
                            //Если все активности независимы, добавляем в текущий сет
                            set.Add(activities[k]);
                            //Также добавляем в итоговую коллекцию. Если такой сет уже был добавлен,
                            //то дубликат не появится (особенность HashSet, для которой и пришлось делать обёртку ActivitySet)
                            independentSets.Add(new ActivitySet(set));
                        }
                    }
                }
            }
            return independentSets.Select(x => new HashSet<string>(x.Activities)).ToHashSet();
        }
        private HashSet<(HashSet<string>, HashSet<string>)> FindMaximalIndependentSetsAB(HashSet<HashSet<string>> independentSets, CancellationToken token)
        {
            //setsAB - коллекция кортежей (setA, setB)
            var setsAB = new HashSet<(HashSet<string>, HashSet<string>)>();

            int progressCounter = 0;
            //Каждый возможный setA сравнивается с каждым возможным setB на полную последовательность
            foreach (HashSet<string> setA in independentSets)
            {
                progress?.Report(++progressCounter); //вызов события прогресса
                token.ThrowIfCancellationRequested(); //Проверка, если было решено отменить выполнение алгоритма
                foreach (HashSet<string> setB in independentSets)
                {
                    bool invalidSet = false;
                    foreach (string activityA in setA)
                    {
                        foreach (string activityB in setB)
                        {
                            if (relationMatrix[activityIndices[activityA], activityIndices[activityB]] != Relation.Succession)
                            {
                                invalidSet = true;
                                break;
                            }
                        }
                        if (invalidSet)
                            break;
                    }
                    if (invalidSet)
                        continue;

                    bool isBiggerSet = false;
                    var setsToRemove = new HashSet<(HashSet<string>, HashSet<string>)>();

                    foreach (var setAB in setsAB)
                    {
                        //Если в итоговом setsAB есть сеты меньше, чем новый, то готовимся их удалить
                        if (setAB.Item1.IsSubsetOf(setA) && setAB.Item2.IsSubsetOf(setB))
                        {
                            isBiggerSet = true;
                            setsToRemove.Add(setAB);
                        }
                    }
                    setsAB.ExceptWith(setsToRemove);

                    //Если это наибольший сет или же минимальный сет, которого в результате проверки
                    //нет в итоговой коллекции, то добавляем в итоговую коллекцию
                    if (isBiggerSet || (setA.Count == 1 && setB.Count == 1))
                    {
                        setsAB.Add((setA, setB));
                    }
                }
            }
            return setsAB;
        }
    }
}
