using CsvHelper.Configuration.Attributes;

namespace Task13_ProcessMining
{
    internal class Record
    {
        [Index(0)] public string Id { get; set; }
        [Index(1)] public string Action { get; set; }
        [Index(2)] public DateTime Time { get; set; }
        public override string ToString()
        {
            return $"{Id}, {Action}, {Time}";
        }
    }
}
