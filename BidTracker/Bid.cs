namespace BidTracker
{
    /// <summary>
    /// Class representing a bid made by a user on an item.
    /// </summary>
    public class Bid
    {
        public Bid(string id, string itemId, string userId, decimal amount)
        {
            this.Id = id;
            this.ItemId = itemId;
            this.UserId = userId;
            this.Amount = amount;
        }

        public string Id { get; }

        public string ItemId { get; }

        public string UserId { get; }

        public decimal Amount { get; }
    }
}
