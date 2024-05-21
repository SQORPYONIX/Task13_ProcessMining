
namespace Task13_ProcessMining
{
    internal class Trace
    {
        public List<string> Activities { get; private set; }

        public Trace(IEnumerable<string> activities)
        {
            Activities = new List<string>(activities);
            Activities.Insert(0, "I");
            Activities.Add("O");
        }
        public Trace(Trace trace)
        {
            Activities = new List<string>(trace.Activities);
        }
        public IEnumerable<(string, string)> Zip()
        {
            //Делает коллекцию кортежей Активность->Активность
            return Activities.Zip(Activities.Skip(1));
        }
        public override int GetHashCode() 
        {
            //Переопределяем, чтобы при сравнении по хешу было сравнение всех активностей по хешу
            return Activities.Aggregate(0, (a, y) => a ^ y.GetHashCode());
        }
        public override bool Equals(object? obj) 
        {
            //Переопределяем, чтобы при сравнении с другим Trace было сравнение списков активностей по элементам в списках
            if (obj is not Trace trace)
                return false;
            return Activities.SequenceEqual(trace.Activities);
        }
        public override string ToString()
        {
            return string.Join("-", Activities);
        }
    }
}
