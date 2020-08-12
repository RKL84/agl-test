# AGL Developer Test

## Solution Overview
The application is a cross-platform **.NET Core 3.1 Console application** which can be executed on both Windows and Linux operating systems.  
The solution comprises of the following projects - 

- **Agl.Client** - the main executable program (exe)
- **Agl.Core** - the library project which includes dto, business logic, and infrastructure concerns. 
- **Agl.UnitTest** - unit tests.   

The application uses typed HttpClient **PeopleService**. This class is registered with DI and can be injected where required in the application.  
Polly library is used to implement Retry policy.

## Frameworks and Libraries
- [NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-3.1?view=aspnetcore-3.1)
- [Polly Framework](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly) (for resilience and transient-fault handling)
- [Serilog](https://serilog.net/)
- [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [Moq](https://github.com/moq/moq4)

## How to execute the program

First, install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1).  
Then, open the terminal or command prompt at the root path of the project (```~/src/Agl.Client```) and run the following commands in sequence:

```
dotnet restore
dotnet run
```

![sample output](../master/images/image.png)

## How to run unit tests

First, install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1).  
Then, open the terminal or command prompt at the root path of the project (```~/test/Agl.UnitTest```) and run the following commands in sequence:

```
dotnet restore
dotnet test
```