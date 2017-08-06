using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentFilter.Contracts;

namespace ContentFilter
{
    public class ProcessArticle
    {
        private readonly string _artticleFile;
        private readonly string _resultFile;
        private readonly IBaseRule _rule;
        private readonly BlockingCollection<ProcessedLine> _dataItems = new BlockingCollection<ProcessedLine>(100);

        public ProcessArticle(string artticleFile, string resultFile, IBaseRule rule)
        {
            _artticleFile = artticleFile;
            _resultFile = resultFile;
            _rule = rule;
        }

        public async Task StartProcess()
        {
            var orderedList = new SortedList<long, ProcessedLine>();

            var consumer = Consume(orderedList);
            Parallel.ForEach(File.ReadLines(_artticleFile), (line, _, lineNumber) =>
            {
                _dataItems.Add(new ProcessedLine
                {
                    Line = line,
                    LineNumber = lineNumber,
                    IsMatch = _rule.IsMatch(line)
                });
            });
            _dataItems.CompleteAdding();

            await consumer;
        }

        private Task Consume(SortedList<long, ProcessedLine> orderedList)
        {
            return Task.Run(() =>
            {
                while (!_dataItems.IsCompleted)
                {
                    ProcessedLine data = null;
                    try
                    {
                        data = _dataItems.Take();
                    }
                    catch (InvalidOperationException)
                    {
                    }

                    if (data == null) continue;
                    orderedList.Add(data.LineNumber, data);
                    var lastLineNumber = orderedList.Last().Key;
                    var firstLineNumber = orderedList.First().Key;
                    if (orderedList.Count <= 99) continue;
                    if (orderedList.Count == lastLineNumber - firstLineNumber + 1)
                    {
                        FlushResultFile(orderedList);
                    }
                }
                FlushResultFile(orderedList);
            });
        }
        private void FlushResultFile(SortedList<long, ProcessedLine> orderedList)
        {
            using (var fs = new FileStream(_resultFile, FileMode.Append, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            {
                foreach (var processLine in orderedList)
                {
                    if (processLine.Value.IsMatch)
                    {
                        sw.WriteLine($"{processLine.Key} - {processLine.Value.Line}");
                    }
                }
            }
            orderedList.Clear();
        }
    }
}
