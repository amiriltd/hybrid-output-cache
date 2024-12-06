# Hybrid Output Cache
An implementation of a Hybrid Output Cache store using HybridCache plus more to support dependency injection (DI) 

## About HybridOutputCache
This repository allows the implementation of IDistributedCache services ie 
[Sql Server Cache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-9.0#distributed-sql-server-cache), 
[NCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-9.0#distributed-ncache-cache), 
and [Azure CosmosDb](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-9.0#distributed-azure-cosmosdb-cache) 
be used for cache storage and [Distributed Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-9.0) in a production environment using the new 
[HybridCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0) library currently in preview mode. 
Prior to this, developers would have to invest in a [Redis Cache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-9.0#redis-cache) to provide consistent shared access between server nodes in a distributed architecture. 
The new HybridCache looks to bridge the gap between IMemoryCache and RedisCache.
