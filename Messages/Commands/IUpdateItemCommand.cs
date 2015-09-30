namespace Messages.Commands
{
    public interface IUpdateItemCommand
    {
        string ItemId { get; set; }
        int NewValue { get; set; }
    }
}
