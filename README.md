# Checkout PaymentGateway challenge solution

## General solution design
This solution was developed as a simple as possible, using a micro service approach for breaking down complexity into smaller services with a smaller set of responsibilities.

### PaymentAPI Use Cases
#### Create a payment
![alt text](https://github.com/edabreu/payment-gateway-challenge/blob/main/plantuml/process-payment.png?raw=true)

#### Retrieve a payment
![alt text](https://github.com/edabreu/payment-gateway-challenge/blob/main/plantuml/retrieve-payment.png?raw=true)

## Let's get started
Please ensure your have Docker up and running on your machine.

1. `git clone` this repository  
2. `cd` it and get inside the directory where you cloned this repository  
3. `docker-compose up --build --detach` and wait a few moments
4. Let's perform some requests. You can use [Swagger](http://localhost:5001/swagger/index.html) or you can find in the `./postman/PaymentGateway.postman_collection.json` file a couple of requests I used for my manual tests.

**With `docker-compose up` command will get you these containers running:**
|Container|Port|Description|
|----------:|:-------------:|:-------------|
|paymentgateway-api|5001|Main App|
|ckobank|5002|CKO Bank API simulator|
|cardtokenizer-api|5003|Card tokenizer API simulator|
|mongo|27017|PaymentGateway Database|
|mongo-migrations|Not binded|Initial DB seed with ans pre-existing payment|
|mongo-express|8081|A convenient way to take a look into the database|

### Thoughts and assumptions that led me to this design:
 - I've decided to containarize because it is a easy way to have it up and running independently of the OS being used.
 - I haven't developed nor setup any authentication service for simplicity sake and to focus on the payment flow it self. Thus, the header `X-merchant_id` is used to identify the merchant making the request. No validation is made on the merchant id as it would part of the authorization token.
 - I've decided to create a card tokenizer service to separate the resposibility of masking, creating and managing card tokens.

#### What could be improved
 - Exception handling to give better visibility over possible issues while processing the payment request (i.e. a merchant trying to process a second payment using a duplicate reference).
 - Request validation could be improved with more validations.
 - Implementing resilience mechanisms by introducing [Polly](https://github.com/App-vNext/Polly#polly).
 - Logging could be improved in preparation, either by adding more logging, but also setting up [Serilog](https://github.com/serilog/serilog#serilog------) for easier integration with Kibana.
 - There are a few improvements that could be made on both unit and integration tests.
 - The way mapping between dtos-to-model and model-to-dbos could be improved and abstracted, which would make developing tests easier.
 - An API client could be provided, allowing faster development and integration with the merchants services. 
 - Ensure that persisted PCI and PII information is securelly encrypted.
 - Ensure that any PCI and PII information logged is masked.

## Tech Stack
 - .NET 6
	 - FluentValidation
 - Docker
 - MongoDB
 - Mongo Express

## Debugging
- Running dependencies
    - `docker-compose up -d mongo mongo-migrations mongo-express ckobank cardtokenizer-api`
- Using **VisualStudio**
	- Open `payment-gateway-challenge.sln`
	- Run `PaymentGateway.API`, it should start at `5201`.

## Running tests
- Unit test can be run in one of two ways:
    - `dotnet test payment-gateway-challenge.sln --filter="Category=Unit" --logger:"console;verbosity=normal"`
    - `docker compose -f docker-compose-unittests.yml -f docker-compose-unittests.override.yml up --build`
- Integration tests can be run by running `docker compose up --build -d` and then `dotnet test payment-gateway-challenge.sln --filter="Category=Integration" --logger:"console;verbosity=normal"`
