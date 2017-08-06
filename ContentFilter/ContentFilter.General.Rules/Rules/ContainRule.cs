namespace ContentFilter.General.Rules.Rules
{
    public class ContainRule : BaseRule
    {
        
        public override bool IsMatch(string text)
        {
            var upperText = text.ToUpper();
            var upperContent = Content.ToUpper();
            var b = upperText.Contains(upperContent);
            return b;
        }
    }
}
