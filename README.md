# Event-driven vending machine
Example of using CQRS pattern and event-driven microservice architecture for a simple vending machine model implementation

## Description
Vending machine microservice is an API has three parts:
 - Controllers and CQRS command, queries and domain event handlers
 - Domain aggregates and events
 - Infrastructure layer (Repositories and Unit Of Work used for access to Redis cache store)

Due to small amount of data I've made my choice to use Redis cache for both main Storage and Read-only views.

## Domain model
There are two main aggregates: Product and Wallet. 
Wallet contains cashbox and deposit. There are two main commands: add coins onto deposit and return it to the buyer.
Product is a tuple of name, price, and available amount. Main command here is check out a product.

There are two important queries: get deposit and get the list of available products. That's why this short information in string representation for end user is stored in the read only views.

Read only views are updated once one of two domain events occurs: deposit changed or product run out. In those two cases both caches are invalidated. 

Since for the purchase command both decreasing of product amount and sending coins to cashbox and the change back to user happens, there is a unit of work implemented for both repositories, having shared transaction. Once transaction is commited, domain events are dispatched and handled by domain event handlers.

## Deployment
Use docker compose to run vending machine microservice and a web app with a simple UI

![](https://i.ibb.co/kqXk2Gy/image.png)
