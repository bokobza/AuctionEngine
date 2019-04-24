using System.Collections.Generic;

namespace BidTracker
{
    /// <summary>
    /// An interface providing operations for bids on items.
    /// </summary>
    public interface IBidTracker
    {
        /// <summary>
        /// Records a user's bid on an item.
        /// </summary>
        /// <remarks>Each new bid must be at a higher price than before.</remarks>
        /// <returns>The newly created bid.</returns>
        Bid AddBid(string itemId, decimal bidAmount, string userId);

        /// <summary>
        /// Gets the current winning bid for an item.
        /// </summary>
        /// <returns>The winning bid for an item.</returns>
        Bid GetWinningBid(string itemId);

        /// <summary>
        /// Gets all the bids for an item.
        /// </summary>
        /// <returns>The list of bids for an item.</returns>
        IEnumerable<Bid> GetBids(string itemId);

        /// <summary>
        /// Gets all the items on which a user has bid.
        /// </summary>
        /// <returns>The bids on which the user has bid.</returns>
        IEnumerable<Item> GetItemsBidOn(string userId);
    }
}
