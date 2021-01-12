# PhotoSi


## Project Documentation

Per il progetto ho scelto di seguire l'architetture di cui vi avevo parlato nella precedente call, in modo da poter dare una dimostrazione delle cose dette.

Ho quindi utilizzato una Clean Architecture, in modo da divedere la varie parti del progetto in Domain, Application Core ed Infrastracture.

Nel domain ho modellato tutte le entità (Product, Category Option, Order con i relativi figli).

Ho cercato di seguire anche un approccio DDD, tramite una factory che riceve in ingresso un Dto e delle regole di validazione (che sono ingnettate come dipendenza in fase di configurazione) e creo tutte le Entity del dominio.
In questo modo mi assicuro che ogni oggetto creato sia sempre consistente, quindi ho cercato di trattare i Dto come oggetti Mutable mentre le classi Entity sono "Immutable" anche se hanno delle funzioni public che ci consentono di modificare comunque alcune proprietà.

Per la validazione ho utilizzato un Validator Pattern (anche se qua le soluzioni sarebbero state molteplici), io mi sono trovato bene ad utilizzarlo perchè mi permette di inserire logica di validazione sia complessa oppure che richede l'accesso anche al repository, cercando di non portare le dipenze del Repository dentro la mia classe di dominio (che in teoria non dovrebbe avere nessun riferimento).
Il vantaggio di questo approccio è che mi permette di aggiungere o rimuovere regole di validazione senza rischiare di rompere altri punti del codice e di avere funzioni di validazione ben distinte in modo che ognuna si occupi di un solo compito (ho cercato di seguire i principi della programmazione SOLID)

Ho utilizzato anche gli Specification Pattern (utilizzando l'implementazione di https://gunnarpeipman.com/) con tutti vantaggio/svantaggi che comporta, ma nel mio caso avendo una logia molto semplice di CRUD credo che siano più i vantaggi.

Per quanto riguarda l'accesso al database ho utilizzato EFCore con un database sqlite in modo da poterlo utilizzare senza nessuna installazione aggiuntiva.
In realtà il database (come altre configurazioni) sono modificabili nel appSetting.json in modo ch le dipendenze vengano caricate direttamente allo startup.
Sia l'accesso in Lettura che Scrittura è stato fatto utilizzando EFCore, però ho diviso la parte di Command da Query, in modo che in futuro si possa utilizzare Dapper in caso di ottimizzazione di alcune query.

Nel Command gestisco tutto l'accesso al repository ed il salvataggio dei dati, avrei inoltre pensato di inserire alcune logiche di validazione utilizzate nei Validator Pattern.

Il database si crea e si popola in automatico all'avvio. Ad ogni avviso se non sono presenti prodotti vengono creati 6 prodotti di default, se non ci sono ordini ne viene creato uno di default.

Il nome del database è configurabile da appSetting.json, ed anche il fatto che venga creato oppure no il database è un booleano in "Databae:UseMigrationScript"

Per quanto riguarda i test ho inserito alcuni esempio che mostrano la creazione di un Entity a partire da un Dto, sia con che senza regole di validazione (qui magari possiamo approfindire a voce, perchè in effetti le regole di validazione credo che debbano stare in un Unity distinto).
Inoltre è presente un Unit Test che mostra le differenti risposte del controller a seconda che sia trovato o meno il dato richiesto.

Ho modellato l'applicazione che in modo che una volta effettuato l'ordine (quindi dopo la commit) sia generato un event intercettato da un subscriber che avrà il compito di  chiamare altri servizi (in questo caso un'interfaccia per inviare le mail)

Un ordine ha una relazione con N prodotti, che a loro volta potranno avere N opzioni con un valore string. Se due prodotti diversi avranno la solita opzione, saranno comunque salvate due entità diverse (ognuna legata al prodotto). 



## Tools and Frameworks Used
* [.NET 5.0]
* [ASP.NET Core]
* [EF Core]
* [Swashbuckle.AspNetCore]
* [xUnit] and [Moq]
* [SQLite] 
* [Mediator] 
* [Serilog] 


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
