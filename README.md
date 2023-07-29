# These two projects show how to implement Many-to-Many relations in EntityFrameworkCore, and How to test WebApiController in XUnit.

In **OnlineOrderApi** project, **Student** and **Grade** are the many-to-many relation.  
We use Sqlite/EntityFrameworkCore 7/NET 7/Moq to implement it.

In **OnlineOrderApi.Tests** project, we use XUnit to test it.  
Please remember to restore the project first.

```
dotnet restore
```

## For running testings, you can run the command like,

```
dotnet test
```

or, you can press the **"Debug test"**(for debug when running testings) from the hint of the testing methods if you have **NetCore Test Exploroer** in VSCode extension.
