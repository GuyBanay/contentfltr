namespace ContentFilter
{
    internal class ProcessedLine
    {
        public long LineNumber { get; set; }
        public string Line { get; set; }
        public bool IsMatch{get; set; }
    }
}