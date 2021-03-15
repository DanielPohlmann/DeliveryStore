### Execution

Docker

```sh
    $ docker-compose build 
    $ docker-compose up
```

### URL

http://localhost:32778

Documentation API REST

/swagger 

HealthCheck

/api/hc

/api/hc-ui

### Architecture Evolution

* Security - Identity + JWT
* LOGS/MONITORING - ElasticSearch/Serilog + Elmah
* Quality - SonarQuebe
* Testing Performance - Apache JMeter
* Messaging - MediatR + MessageBus + Rabbitmq + EventSourcing 
