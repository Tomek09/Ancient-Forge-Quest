namespace AncientForgeQuest.Instances
{
    public class TooltipContent
    {
        public readonly string Header;
        public readonly string Description;

        public TooltipContent()
        {
            Header = string.Empty;
            Description = string.Empty;
        }
        
        public TooltipContent(string header, string description)
        {
            Header = header;
            Description = description;
        }
    }
}
