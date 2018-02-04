# 1: nuget: Awaken.Utils.Widgets

# 2: nuget: Awaken.Utils.Cache
** base StackExchange.Redis **
### How to use:
-- 1: add to "appsettings.json" of project
`` "Redis": "139.196.148.7:6379,allowAdmin=true,password=3W1e#r&f,abortConnect=false", //abortConnect=false ``
-- 2: add to Startup.cs > Methods ConfigureServices() , DI Singleton:"IRedisCache"
``
services.AddRedisCache(options =>
        {
            options.ConnectionString = Configuration.GetValue<string>("Redis");
        });
``
-- 3: insert and read 
``
  [Route("[controller]")]
  public class DemoController: Controller{  
    private readonly IRedisCache _cache;    
    public Demo(IRedisCache cache)
    {
        _cache = cache;       
    }
    [HttpGet("{id}", Name = "get")]
    public async Task<string> Get(){
      var success=_cache.Set("foo","Hello World");
      
      return _cache.Get("foo")
    }
  }
``

