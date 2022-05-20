# PhotoSi

![.NET](https://github.com/FedeC87p/PhotoSi/workflows/.NET/badge.svg)

## Project Documentation

For the project I chose to follow the architecture I told you about in the previous call, in order to be able to give a demonstration of the things said.

I therefore used a Clean Architecture, in order to divide the various parts of the project into Domain, Application Core and Infrastracture.

In the domain I modeled all the entities (Product, Category Option, Order with their children).

I also tried to follow a DDD approach, through a factory that receives a Dto and validation rules (which are identified as a dependency in the configuration phase) and I create all the Entities of the domain. In this way I make sure that every object created is always consistent, so I tried to treat the Dto as Mutable objects while the Entity classes are "Immutable" even if they have some public functions that allow us to modify some properties anyway.

For the validation I used a Validator Pattern (even if here the solutions would have been many), I was happy to use it because it allows me to insert validation logic that is complex or that requires access to the repository, trying not to bring the Repository dependencies inside my domain class (which in theory shouldn't have any references). The advantage of this approach is that it allows me to add or remove validation rules without the risk of breaking other points in the code and to have very distinct validation functions so that each one deals with only one task (I tried to follow the principles of SOLID programming)

I also used the Specification Patterns (using the implementation of https://gunnarpeipman.com/) with all the advantages / disadvantages that it entails, but in my case having a very simple logic of CRUD I think there are more advantages.

As for database access I used EFCore with a sqlite database so that I can use it without any additional installation. Actually the database (like other configurations) are editable in the appSetting.json so that the dependencies are loaded directly to the startup. Both Read and Write access was done using EFCore, but I split the Command part from Query, so that in the future we can use Dapper in case of optimizing some queries.

In the Command I manage all access to the repository and the saving of data, I would also have thought of inserting some logics (for example for now I do not do any checks to verify that the ids arrived are correct, and other simpler logics that I have not entered for give more space to other things).

The database is created and populated automatically at startup. At each start, if there are no products, 6 default products are created, if there are no orders a default is created.

The database name is configurable from appSetting.json, and whether it is created or not is also a boolean in "Databae: UseMigrationScript"

As for the tests, I have included some examples that show the creation of an Entity starting from a Dto, both with and without validation rules (here maybe we can deepen orally, because in fact I believe that the validation rules must be in a Distinct Unity). There is also a Unit Test that shows the different responses of the controller depending on whether the requested data is found or not.

I have modeled the application so that once the order is placed (therefore after the commit) an event is generated intercepted by a subscriber who will have the task of calling other services (in this case an interface to send the emails which is implemented by a class that pretends to send the mail)

An order has a relationship with N products, which in turn can have N options with a string value. If two different products have the usual option, two different entities (each linked to the product) will still be saved. Example:

I have created n Knitted Products that can have a Size and Color option so that when ordering you can tell the color and the size. For other Shoe Products I have entered Color and Size.



## Tools and Frameworks Used
* [.NET 5.0]
* [ASP.NET Core]
* [EF Core]
* [Swashbuckle.AspNetCore]
* [xUnit] and [Moq]
* [SQLite] 
* [Mediator] 
* [Serilog] 
* [AutoMapper] 


## Configure, Deploy and Run the project
* 1. Aprire la solution che si trova in \PhotoSi\PhotoSi.sln 
* 2. Impostare come progetto di startup "WebAPI"
* 3. Far partire la solution facendo play
* 4. Al primo avviso verrà creato automaticamente il database (vedi configurazioni in appSetting.json)
* 5. ATTENZIONE: Ad ogni avvio se non sono presenti prodotti nel database verrano creati automaticamente. La stessa logica è applicata per gli ordini.


## Acluni esempi di chiamate rest REST CALL
* 0: Swagger si trova a questo indirizzo http://localhost:5000/swagger/index.html
* 1: Get all products: GET http://localhost:5000/Products
* 2: Get Product: GET http://localhost:5000/Products/1
* 5: Creare Product: POST http://localhost:5000/Products
	Body Message
```javascript
    {
    "name": "NomeProd",
    "description": "Descr",
    "categoryId": 1,
    "optionsId": [
       2,
       3
    ]
}
```
* 6: Creare Valid Order: POST http://localhost:5000/Orders
	Body Message
```javascript
{
    "code": "ProvaOrdine",
    "productItems": [
        {
            "productId": 1,
            "quantity": 1,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Blu"
                }
            ]
        },
        {
            "productId": 2,
            "quantity": 10,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Celeste"
                },
                {
                    "optionId": 2,
                    "value": "XL"
                }
            ]
        }
    ]
}
```
* 7: Creare InvalidValid Category Order: POST http://localhost:5000/Orders
	Body Message
```javascript
{
    "code": "ProvaOrdine",
    "productItems": [
        {
            "productId": 1,
            "quantity": 1,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Blu"
                }
            ]
        },
        {
            "productId": 3,
            "quantity": 10,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Celeste"
                },
                {
                    "optionId": 2,
                    "value": "XL"
                }
            ]
        }
    ]
}
```
* 8: Creare InvalidValid Option For Product Order: POST http://localhost:5000/Orders
	Body Message
```javascript
{
    "code": "ProvaOrdine",
    "productItems": [
        {
            "productId": 1,
            "quantity": 1,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Blu"
                }
            ]
        },
        {
            "productId": 2,
            "quantity": 10,
            "optionItems": [
                {
                    "optionId": 1,
                    "value": "Celeste"
                },
                {
                    "optionId": 4,
                    "value": "XL"
                }
            ]
        }
    ]
}
```
