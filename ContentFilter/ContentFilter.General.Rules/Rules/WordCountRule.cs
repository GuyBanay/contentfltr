using System;


namespace ContentFilter.General.Rules.Rules
{
    public class WordCountRule : BaseRule
    {
        public override bool IsMatch(string text)
        {
            if (!int.TryParse(Content, out int content))
                throw new Exception($"WordCount content must be a number {Content}");
            var wordCount = text.Split(' ').Length;
            return wordCount == content;
        }
    }
}
