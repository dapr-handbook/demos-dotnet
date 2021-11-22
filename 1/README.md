```
dapr run --app-id WebApi1 --app-port 5000 --dapr-http-port 3500 dotnet run
```
```
dapr invoke --app-id WebApi1 --method WeatherForecast --verb GET
```

```
curl http://localhost:3500/v1.0/invoke/WebApi1/method/WeatherForecast
```