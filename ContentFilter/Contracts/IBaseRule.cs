using System;

namespace ContentFilter.Contracts
{
    public interface IBaseRule:IDisposable
    {
        string Name { get; set; }
        bool IsMatch(string text);
        void SetContent(string content);
    }
}