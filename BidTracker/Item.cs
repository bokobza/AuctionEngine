namespace BidTracker
{
    /// <summary>
    /// Class representing an item a user can bid on.
    /// </summary>
    public class Item
    {
        public Item(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}
