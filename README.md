To execute the program, in this root directory you can do:
dotnet run --project ./app/lantek.csproj


1. How long did you spend on the coding test? What would you add to your solution if you had more
time? If you didn't spend much time on the coding test then use this as an opportunity to explain what
you would add.

Around 2 hours. I can't think of new functionality, but if I had to say sohething maybe a CRUD API and use mappers for the responses. 

2. What was the most useful feature added to the latest version of your chosen language? Please include
a snippet of code that shows how you've used it.

I didn't use a feature added to the latest version, but one thing I used and for me is very useful is the SendAsync, and how to deserialize the model, very clean:

```
var result = await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("url")));
var resultContent = result.Content.ReadAsStringAsync().Result;
machines = JsonSerializer.Deserialize<List<CuttingMachine>>(resultContent);
```

3. How would you track down a performance issue in production? Have you ever had to do this?

I usually use prometheus to track metrics and detect a performance issue or a code issue, like a lag consumption in kafka consumer groups, time spend to process a kafka message, time spend to do some http request to other integrations, etc.

4. How would you improve the Lantek API that you just used?

The lantek API has only a GET endpoint. One thing I would add is pagination, because if we have a huge amount of machines it's better to paginate it, instead of listing all. Also, I would add a memory cache to the endpoint in case of there are no changes in the machines, so the process don't execute a query to the database and return directly the results saved in memory cache.