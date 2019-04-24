using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BidTracker.Tests
{
    public class BidTrackerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void BidAddedWithoutItemIdThrowsException(string itemId)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentNullException>(() => tracker.AddBid(itemId, 1, "user1"));
            Assert.Contains("An item id must be specified.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void BidAddedWithoutUserIdThrowsException(string userId)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentNullException>(() => tracker.AddBid("item1", 1, userId));
            Assert.Contains("A user id must be specified.", ex.Message);
        }

        [Theory]
        [InlineData(-3)]
        [InlineData(0)]
        public void BidAddedWithInvalidAmountThrowsException(decimal bidAmount)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentException>(() => tracker.AddBid("item1", bidAmount, "user1"));
            Assert.Contains("The bid amount must be greater than 0.", ex.Message);
        }

        [Fact]
        public void BidAddedToNonExistingItemThrowsException()
        { 
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            InvalidBidException exception = Assert.Throws<InvalidBidException>(() => tracker.AddBid("item1", 1, "user1"));
            Assert.Equal("An item with id 'item1' does not exist.", exception.Message);
        }

        [Fact]
        public void BidLowerThanWinningBidThrowsException()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 3));

            BidTracker tracker = new BidTracker(repo);

            InvalidBidException exception = Assert.Throws<InvalidBidException>(() => tracker.AddBid("item1", 1, "user3"));
            Assert.Equal("Bid too low. The current highest bid is 3.", exception.Message);
        }

        [Fact]
        public void UserCanCreateBidSuccessfully()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 3));

            BidTracker tracker = new BidTracker(repo);

            Bid bid = tracker.AddBid("item1", 5, "user3");

            Assert.Equal(3, repo.Bids.Count);

            // Retrieve the newly created bid and check its values.
            Bid newBid = repo.Bids.Single(b => b.Id == bid.Id);
            Assert.Equal(5, newBid.Amount);
            Assert.Equal("item1", newBid.ItemId);
            Assert.Equal("user3", newBid.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetBidsForItemWithoutItemIdThrowsException(string itemId)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentNullException>(() => tracker.GetBids(itemId));
            Assert.Contains("An item id must be specified.", ex.Message);
        }

        [Fact]
        public void GetBidsForNonExistingItemThrowsException()
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            InvalidBidException exception = Assert.Throws<InvalidBidException>(() => tracker.GetBids("item1"));
            Assert.Equal("An item with id 'item1' does not exist.", exception.Message);
        }

        [Fact]
        public void GetBidsForItemReturnBidsSuccessfully()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 3));

            BidTracker tracker = new BidTracker(repo);

            List<Bid> bids = tracker.GetBids("item1").ToList();
            Assert.Equal(2, bids.Count());

            Bid bid1 = bids.SingleOrDefault(b => b.Id == "bid1");
            Assert.NotNull(bid1);
            Assert.Equal(2, bid1.Amount);
            Assert.Equal("item1", bid1.ItemId);
            Assert.Equal("user1", bid1.UserId);

            Bid bid2 = bids.SingleOrDefault(b => b.Id == "bid2");
            Assert.NotNull(bid2);
            Assert.Equal(3, bid2.Amount);
            Assert.Equal("item1", bid2.ItemId);
            Assert.Equal("user2", bid2.UserId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetItemsBidOnByUserWithoutUserIdThrowsException(string userId)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentNullException>(() => tracker.GetItemsBidOn(userId));
            Assert.Contains("A user id must be specified.", ex.Message);
        }

        [Fact]
        public void GetItemsBidOnByUserWhenUserHasNotBidReturnsNoItems()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 3));

            BidTracker tracker = new BidTracker(repo);

            IEnumerable<Item> items = tracker.GetItemsBidOn("user3");
            Assert.Empty(items);
        }

        [Fact]
        public void GetItemsBidOnByUserWhenUserHasBidReturnsAllItems()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Items.Add(new Item("item2", "name2"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 3));
            repo.Bids.Add(new Bid("bid3", "item1", "user3", 4));
            repo.Bids.Add(new Bid("bid4", "item2", "user3", 3));

            BidTracker tracker = new BidTracker(repo);

            IEnumerable<Item> items = tracker.GetItemsBidOn("user3");
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count());

            Item item1 = items.SingleOrDefault(i => i.Id == "item1");
            Assert.NotNull(item1);
            Assert.Equal("name1", item1.Name);

            Item item2 = items.SingleOrDefault(i => i.Id == "item2");
            Assert.NotNull(item2);
            Assert.Equal("name2", item2.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetWinningBidForItemWithoutItemIdThrowsException(string itemId)
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            var ex = Assert.Throws<ArgumentNullException>(() => tracker.GetWinningBid(itemId));
            Assert.Contains("An item id must be specified.", ex.Message);
        }

        [Fact]
        public void GetWinnigBidForNonExistingItemThrowsException()
        {
            BidRepository repo = new BidRepository();
            BidTracker tracker = new BidTracker(repo);

            InvalidBidException exception = Assert.Throws<InvalidBidException>(() => tracker.GetWinningBid("item1"));
            Assert.Equal("An item with id 'item1' does not exist.", exception.Message);
        }

        [Fact]
        public void GetWinningBidWhenThereAreNoBidsReturnsNull()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));

            BidTracker tracker = new BidTracker(repo);

            Bid bid = tracker.GetWinningBid("item1");
            Assert.Null(bid);
        }

        [Fact]
        public void GetWinningBidReturnsSuccessfully()
        {
            BidRepository repo = new BidRepository();
            repo.Items.Add(new Item("item1", "name1"));
            repo.Bids.Add(new Bid("bid1", "item1", "user1", 2));
            repo.Bids.Add(new Bid("bid2", "item1", "user2", 4));
            repo.Bids.Add(new Bid("bid3", "item1", "user3", 3));

            BidTracker tracker = new BidTracker(repo);

            Bid bid = tracker.GetWinningBid("item1");
            Assert.NotNull(bid);
            Assert.Equal("item1", bid.ItemId);
            Assert.Equal(4, bid.Amount);
            Assert.Equal("user2", bid.UserId);
            Assert.Equal("bid2", bid.Id);
        }
    }
}
