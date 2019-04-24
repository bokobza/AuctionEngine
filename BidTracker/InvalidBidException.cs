using System;

namespace BidTracker
{
    public class InvalidBidException : Exception
    {
        public InvalidBidException(string message) : base(message)
        {

        }
    }
}
