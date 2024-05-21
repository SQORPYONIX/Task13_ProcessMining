using QuikGraph;

namespace Task13_ProcessMining
{
    internal class DFGraph
    {
        public int MaxTraceFrequency { get; private set; }
        public int MaxActivityFrequency { get; private set; }
        public int MaxArcFrequency { get; private set; }
        public int VarFilter { get; private set; }
        public int ActFilter { get; private set; }
        public int FdFilter { get; private set; }

        private Dictionary<Trace, int> traceFrequency;
        private Dictionary<Trace, int> varFilteredTraceFrequency;
        private Dictionary<Trace, int> actFilteredTraceFrequency;
        private Dictionary<string, int> activityFrequency;
        private Dictionary<string, int> filteredActivityFrequency;
        private Dictionary<(string, string), int> arcFrequency;
        private Dictionary<(string, string), int> filteredArcFrequency;

        public DFGraph(IEnumerable<Record> records)
        {
            /* Сортируем по времени
             * Группируем по Id (Группировка создаёт коллекцию типа IGrouping<TKey, TElement>, в данном случае TKey - Id, а TElement - Record)
             * Проецируем в коллекцию Trace. Для создания Trace проецируем группу в список активностей и передаём конструктору
             * Группируем все виды трасс по самим трассам (Будет коллекция IGrouping<Trace, Trace>)
             * Создаём словарь, где TKey - трасса, TValue - частота трассы
             */
            traceFrequency = records
                    .OrderBy(x => x.Time)
                    .GroupBy(x => x.Id)
                    .Select(g => new Trace(g.Select(x => x.Action)))
                    .GroupBy(x => x)
                    .ToDictionary(g => g.Key, g => g.Count());

            //Сохраняем максимальное значения частоты трассы
            MaxTraceFrequency = traceFrequency.Max(x => x.Value);
        }
        public int GetActivityFrequency(string activity)
        {
            if (activityFrequency.ContainsKey(activity))
                return activityFrequency[activity];
            else 
                return -1;
        }
        public AdjacencyGraph<string, TaggedEdge<string, int>> GetQuikGraph()
        {
            //Преобразовываем в граф из библиотеки QuikGraph
            AdjacencyGraph<string, TaggedEdge<string, int>> graph = new AdjacencyGraph<string, TaggedEdge<string, int>>();
            foreach (var item in filteredActivityFrequency)
            {
                graph.AddVertex(item.Key);
            }
            foreach (var item in filteredArcFrequency)
            {
                (string from, string to) = item.Key;

                graph.AddVerticesAndEdge(new TaggedEdge<string, int>(from, to, item.Value));
            }
            return graph;
        }
        public void Compute(int varFilter, int actFilter, int fdFilter)
        {
            //Вычисляем DFG с использованием новых фильтров.
            //Чем глубже фильтр, тем меньше необходимо вычислить 
            if (VarFilter != varFilter)
            {
                VarFilter = varFilter;
                ActFilter = actFilter;
                FdFilter = fdFilter;
                CalculateVarFilteredTraceFrequency();
                CalculateActFilteredTraceFrequency();
                CalculateFdFilteredArcFrequency();
            }
            else if (ActFilter != actFilter)
            {
                ActFilter = actFilter;
                FdFilter = fdFilter;
                CalculateActFilteredTraceFrequency();
                CalculateFdFilteredArcFrequency();
            }
            else if (FdFilter != fdFilter)
            {
                FdFilter = fdFilter;
                CalculateFdFilteredArcFrequency();
            }
        }
        private void CalculateVarFilteredTraceFrequency()
        {
            // Из словаря трасса-частота берём подходящие по фильтру трассы
            varFilteredTraceFrequency = traceFrequency
                .Where(x => x.Value >= VarFilter)
                .ToDictionary(x => x.Key, x => x.Value);

            /* Отфильтрованный словарь проецируем в коллекцию KeyValuePair, где TKey - активность, TElement - частота трассы
             * В результате из разных трасс будут одинаковые активности с разными частотами трасс
             * Группируем в коллекцию IGrouping<string, int>, где ключ-активность соответвует коллекции частот разных трасс
             * Создаём словарь где TKey - активность, TValue - частота активности (частоту получаем как сумму коллекции частот разных трасс)
             */
            activityFrequency = varFilteredTraceFrequency
                .SelectMany(x => x.Key.Activities.Select(act => KeyValuePair.Create(act, x.Value)))
                .GroupBy(x => x.Key, x => x.Value)
                .ToDictionary(g => g.Key, g => g.Sum());

            //Сохраняем максимальное значения частоты активности (Активности I O не учитываются)
            //Если фильтр по частоте активностей был больше, то снижаем до нового максимального
            MaxActivityFrequency = activityFrequency.Where(x => x.Key != "I" && x.Key != "O").Max(x => x.Value);
            if (ActFilter > MaxActivityFrequency)
                ActFilter = MaxActivityFrequency;
        }
        private void CalculateActFilteredTraceFrequency()
        {
            actFilteredTraceFrequency = new Dictionary<Trace, int>();

            //Фильтруем по частоте активностей уже отфильтрованный по частоте трасс словарь
            foreach (KeyValuePair<Trace, int> item in varFilteredTraceFrequency)
            {
                Trace trace = new Trace(item.Key);
                List<string> activities = trace.Activities;
                for (int i = activities.Count - 2; i >= 1; i--) //Начало и конец цикла не учитывают I O
                {
                    if (activityFrequency[activities[i]] < ActFilter)
                        activities.RemoveAt(i);
                }
                if (activities.Count <= 2) //Если осталось 2 активности, то это I->O, не добавляем
                    continue;
                if (!actFilteredTraceFrequency.TryAdd(trace, item.Value)) //Если словарь уже содержит такую трассу, то добавляем к ней частоту
                    actFilteredTraceFrequency[trace] += item.Value;
            }

            /* Отфильтрованный по частоте активностей словарь трасса-частота проецируем в коллекцию KeyValuePair<(string, string), int>
             * (string, string) - это кортёж, в данном случае служит последовательностю (для краткости ДУГА) из одной активности в другую
             * Группируем в коллекцию IGrouping<(string, string), int>, где ключ-дуга соответвует коллекции частот разных трасс
             * Создаём словарь где TKey - дуга, TValue - частота дуги (частоту получаем как сумму коллекции частот разных трасс)
             */
            arcFrequency = actFilteredTraceFrequency
                .SelectMany(x => x.Key.Zip().Select(tuple => KeyValuePair.Create(tuple, x.Value)))
                .GroupBy(x => x.Key, x => x.Value)
                .ToDictionary(g => g.Key, g => g.Sum());

            //Сохраняем максимальное значения частоты дуги
            //Если фильтр по частоте дуги был больше, то снижаем до нового максимального
            MaxArcFrequency = arcFrequency.Max(x => x.Value);
            if (FdFilter > MaxArcFrequency)
                FdFilter = MaxArcFrequency;

            //Из словаря активность-частота берём подходящие по фильтру активности (Активности I O не учитываются)
            filteredActivityFrequency = activityFrequency
                .Where(x => x.Value >= ActFilter || x.Key == "I" || x.Key == "O")
                .ToDictionary(x => x.Key, x => x.Value);
        }
        private void CalculateFdFilteredArcFrequency()
        {
            // Из словаря дуга-частота берём подходящие по фильтру дуги
            filteredArcFrequency = arcFrequency
                .Where(x => x.Value >= FdFilter)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
