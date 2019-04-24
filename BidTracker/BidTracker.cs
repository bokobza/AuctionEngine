using System;
using System.Collections.Generic;
using System.Linq;

namespace BidTracker
{
    public class BidTracker : IBidTracker
    {
        private readonly IBidRepository repository;

        public BidTracker(IBidRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            this.repository = repository;
        }

        // <inheritdoc />
        public Bid AddBid(string itemId, decimal bidAmount, string userId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException(nameof(itemId), $"An item id must be specified.");
            }

            if (bidAmount <= 0)
            {
                throw new ArgumentException($"The bid amount must be greater than 0.", nameof(bidAmount));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), $"A user id must be specified.");
            }

            if (!this.repository.Items.Any(i => i.Id == itemId))
            {
                throw new InvalidBidException($"An item with id '{itemId}' does not exist.");
            }

            // Get all the bids for the item.
            IEnumerable<Bid> bids = this.repository.Bids.Where(b => b.ItemId == itemId);

            if (bids.Any())
            {
                decimal highestBid = bids.Max(b => b.Amount);

                if (highestBid >= bidAmount)
                {
                    throw new InvalidBidException($"Bid too low. The current highest bid is {highestBid}.");
                }
            }

            Bid bid = new Bid(this.GenerateUniqueId(), itemId, userId, bidAmount);

            this.repository.Bids.Add(bid);
            return bid;
        }

        // <inheritdoc />
        public IEnumerable<Bid> GetBids(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException(nameof(itemId), $"An item id must be specified.");
            }

            if (!this.repository.Items.Any(i => i.Id == itemId))
            {
                throw new InvalidBidException($"An item with id '{itemId}' does not exist.");
            }

            return this.repository.Bids.Where(b => b.ItemId == itemId);
        }

        // <inheritdoc />
        public IEnumerable<Item> GetItemsBidOn(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), $"A user id must be specified.");
            }

            // Get all the bids made by the user.
            IEnumerable<Bid> bids = this.repository.Bids.Where(b => b.UserId == userId);

            if (!bids.Any())
            {
                return Enumerable.Empty<Item>();
            }

            // Get the ids of the items on which the user has bid.
            IEnumerable<string> itemIds = bids.Select(i => i.ItemId).Distinct();

            IEnumerable<Item> items = this.repository.Items.Where(i => itemIds.Contains(i.Id));

            return items;
        }

        // <inheritdoc />
        public Bid GetWinningBid(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException(nameof(itemId), $"An item id must be specified.");
            }

            if (!this.repository.Items.Any(i => i.Id == itemId))
            {
                throw new InvalidBidException($"An item with id '{itemId}' does not exist.");
            }

            IEnumerable<Bid> bids = this.repository.Bids.Where(b => b.ItemId == itemId);

            if (!bids.Any())
            {
                return null;
            }

            // Return the highest bid.
            return bids.OrderByDescending(b => b.Amount).First();
        }

        /// <summary>
        /// Generates a unique identifier.
        /// </summary>
        /// <returns>The unique identifier.</returns>
        private string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
