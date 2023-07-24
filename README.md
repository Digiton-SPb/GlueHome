# GlueHome - Platform Technical Task

#### How to run

##### Prerequisites

- .Net 6
- Entity Framework Core 6

##### Run Web API
```
dotnet run --project "src/Web/TT.Deliveries.Web.Api/TT.Deliveries.Web.Api.csproj"
```

##### Run Web API in Docker
```
docker-compose build
docker-compose up -d
```
*Open browser at port 8080. For example: "http://localhost:8080/swagger/index.html"*

##### Run Unit Test
```
dotnet test "src/Tests/TT.Deliveries.Tests/TT.Deliveries.Tests.csproj"
```

Collect code coverage:
```
dotnet test --collect:"XPlat Code Coverage"
// Replace guid below with given after 1st command
reportgenerator -reports:"src/Tests/TT.Deliveries.Tests/TestResults/9ba2a4ee-f7e4-422c-b205-dbf9a63ec82c/coverage.cobertura.xml" -targetdir:"TestCoverage" -reporttypes:Html
```

Code coverage report:
```
src/Tests/TestsCoverage/index.html
```

##### Testing with Swagger

Using Bearer token:

1. Get Bearer token
    - To test API under Recipient, get Bearer token by authorizing with user 'dmitry' on /api/Auth/User that filed to db at startup (or create another user first via POST /api/Auth/User).
    - To test API under Partner, get Bearer token by authorizing with any username. Partner api started with prefix 'partner-api'.
2. Authorize with Bearer token using built-in Authorize feature in Swagger
3. Bearer token used for methods to update Delivery state by Recipient or by Partner.

Other methods do not require authorization.

#### Tasks

##### Planning

- Define scope and clarify requirements
- Choose stack and tools for code implementation and documentation
- Taskify scope into tasks for execution

##### Execution

1. Implement project structure
2. Define models and repositories contracts
3. Implement common executor for logging of performance and exceptions
4. Implement base CRUD repository
5. Implement Deliveries, Recipients, Orders repositories and context
6. Add unit tests for repositories
7. Implement WebAPI controllers
8. Implement Delivery State service
9. Implement HostedService to automatically mark deliveries Expired
10. Implement WebAPI auth
11. Implement Docker and Docker compose

#### Implementation details

1. Domain Driven Design used for Architecture of Web Api

2. Used patterns
    - Inversion of Controls
    - Repository

3. Used external packages
    - EFCore provider for SQLite
    - Serilog for logging
    - xunit and Moq for unit testing
    - NCrontab for allow crontab like scheduling for repeated tasks

4. Recipient entity
    - A separate entity has been created for the Recipient to provide scalability for support of real user profiles in the future.  
    - For demo purposes, recipients can be created or deleted without any authorization.  
    - For demo purposes added Bearer authentication based on the username of an Recipient without any password. Added auth support to Swagger config.

5. Implementation Details
    - Guid used as a primiry key for all entities
    - One api for Recipients and Partners. Partners "sub" api started with prefix "partners-api". For real app, I suggest to use different Web APIs for Recipients and Partners.
    - SQLite db used as a storage, one initial migration added. Db seeded with initial data if db does not have any Deliveries.
    - Swagger used both on Dev and Prod invironments for demo purposes.
    - Serilog logger added and setuped to log to Console and File. Log files stored in "./logs".
    - Dto objects used for some API methods. For real app I suggest to use Dto objects for all API methods. 
    - Automapper used to map Dto to Domain models.
    - Used single classes for Domain Model and Data Model. For complex models, separation may be required, but should be used carefully, as it requires additional resources to develop and maintain.
    - AccessWindow object stored inside Deliveries by using option ".OwnsOne(p => p.AccessWindow)", which allows to have AccessWindow as an object on Domain level, and on db level it is stored as 2 fields in Deliveries. Other possible approach is to have separation between Domain Model and Data Model.
    - For demo and testing purposes all entities have Controlles with CRUD logic. In real app, some of them should be hiden.
    - Backgound task for periodically check and set Expired state in Deliveries implemented as a HostedService in WebAPI app.

6. Containerization details
    - Created Dockerfile and docker-compose.yml
    - Mapped binded volumes to host fs for database and logs
        - ./db/deliveries.db => /app/db/deliveries.db
        - ./logs => /app/logs
        - where "." is a project sources root directory

7. Testing
    - XUnit used for testing
    - Added unit tests for repositories
    - Added unit tests for controller: Deliveries and DeliveriesPartner
    - Contollers tests includes testing scenarios for update state of Delivery py Recipient and Partner
    - Code coverage calcualeted, report saved in "src/Tests/TestsCoverage/index.html"
    - For real projects, it is also important to have a range of tests on different levels including unit, integration, and e2e, and track and monitor test coverage of files and flows to ensure all functionality is covered with tests.

8. Documentation
    - For test assignment, Swagger is chosen for auto documentation of OpenApi.
    - Comments to Controllers methods exposed to Swagger
    - For real projects Documentation should be well structured, clear and up-to-date, core systems should have architecture diagrams, non-automated coding guidelines, tools, and resources for troubleshooting should be also documented.

#### Suggestions

- Separation between Domain Model and Data Model can be used in the case of complex objects.
- Unit of Work pattern should be used in the complex model to change few related entities and save changes simultaneously.

#### Product quality

Product quality depends on: Documentation, Testing, Reliability, Performance, Security, QA, Product analytics, Tech Debt. Some basic features are implemented as part of task assignment for auto documentation, testing, reliability, performance, and security.

#### Reliability

For test assignment logging of exceptions is implemented. For real projects reliability includes logging, monitoring, and alerting. I would choose to use Elastic stack and Kibana, as reliability metrics should be defined and tracked in dashboards. Errors should be easily visible to prioritize and debug.

#### Performance

For test assignment basic logging of core endpoints response time is implemented. For real projects, performance metrics should be defined, tracked and goaled in dashboards.

#### Security

For test assigment Bearer authentification is implemented and also added to Swagger.



## Task Descriptiion and Requirements

### Background

Your company has decided to create a delivery system to connect users from the consumer market to partners from the logistics business sector.

You are responsible for building a Web API that will be used by partners and users to create, manage and execute deliveries.

### Business Requirements

* The API should support all CRUD operations (create, read, update, delete).
* A delivery must handle 5 different states: `created`, `approved`, `completed`, `cancelled`, `expired`
* Users may `approve` a delivery before it starts.
* Partner may `complete` a delivery, that is already in `approved` state.
* If a delivery is not `completed` during the access window period, then it should expire. 
* Either the partner or the user should be able to cancel a pending delivery (in state `created` or `approved`).

A delivery should respect the following structrure:

```json
{
    "state": "created",
    "accessWindow": {
        "startTime": "2019-12-13T09:00:00Z",
        "endTime": "2019-12-13T11:00:00Z"
    },
    "recipient": {
        "name": "John Doe",
        "address": "Merchant Road, London",
        "email": "john.doe@mail.com",
        "phoneNumber": "+44123123123"
    },
    "order": {
        "orderNumber": "12209667",
        "sender": "Ikea"
    }
}
```

### Technical Requirements

* All code should be written in C# and target the .NET core, or .NET framework library version 4.5+.
* Please check all code into a public accessible repository on GitHub and send us a link to your repository.

Feel free to make any assumptions whenever you are not certain about the requirements, but make sure your assumptions are made clear either through the design or additional documentation.

### Bonus
* Application logging
* Documentation
* Containerization
* Authentication
* Testing
* Data storage
* Partner facing Pub/Sub API for receiving state updates.
* Anything else you feel may benefit your solution from a technical perspective.
