using System.Collections.Generic;

namespace BidTracker
{
    /// <summary>
    /// An interface representing a repository containing bids and items data.
    /// </summary>
    public interface IBidRepository
    {
        List<Item> Items { get; set; }

        List<Bid> Bids { get; set; }
    }

    public class BidRepository : IBidRepository
    {
        public List<Item> Items { get; set; }

        public List<Bid> Bids { get; set; }

        public BidRepository()
        {
            this.Items = new List<Item>();
            this.Bids = new List<Bid>();
        }
    }
}
