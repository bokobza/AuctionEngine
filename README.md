# Exercise: Auction Engine

You have been tasked with building part of a simple online auction system, which will allow users to bid on items for sale.

Provide a bid tracker interface and concrete implementation with the following functionality:

1. Record a user's bid on an item, each new bid must be at a higher price than before
2. Get the current winning bid for an item
3. Get all the bids for an item
4. Get all the items on which a user has bid

Please implement your answer in C#, Java, C++, Kotlin or Scala and provide adequate test coverage. You are not required to implement a GUI (or CLI) or persistent storage. 

------

## Some design decisions
- For simplicity, there is no check on whether the user placing a bid exists or not.
- There is no check on whether the `Bid` or `Item` objects are constructed properly. This could have been done but I wanted to keep the submission short and sweet.
- I used an interface for the data repository because it felt natural to do so, even though the exercise didn't necessarily called for one.
