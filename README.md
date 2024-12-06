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

## Still In Preview Mode!
> [!IMPORTANT]
> This project is based on packages that are in evaluation therefore we will not be creating an official nuget package.  
> In the meantime, add a folder to your project and copy the three files in the repo: HybridOutputCacheOptionsSetup.cs, HybridOutputCacheStore.cs, and OutputHybridCacheServiceExtensions.cs 
> Other warnings from Microsoft:
> [HybridCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0) is currently still in preview but will be fully released after .NET 9.0 in a future minor release of .NET Extensions.
> [Remove Cache By Tag](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0#remove-cache-entries-by-tag)

## Usage
Add your IDistriubtedCache service: 
  ```csharp
builder
      .Services.AddDistributedSqlServerCache(options =>
      {
          options.ConnectionString = builder.Configuration.GetValue<string>(CacheSettings.ConnectionString);
          options.SchemaName = CacheSettings.SchemaName;
          options.TableName = CacheSettings.TableName;
      })
  ```

Add HybridCache (preview warning):
  ```csharp
  #pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder
    .Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions() { Flags = Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryFlags.DisableLocalCache };
});
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

  ```

Add HybridOutputCache:
  ```csharp
builder.Services.AddHybridOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromMinutes(10)));

});
  ```

Use OutputCache as you normally would after you build your services:
  ```csharp
app.UseOutputCache();
  ```

Finally attach the CacheOutput() to your endpoint:
 ```csharp
  app.MapRemoteBffApiEndpoint("/catalog", builder.Configuration.GetValue<string>(HttpClientBaseAddresses.CatalogApi) + "/api/catalog")
  .CacheOutput();
 ```