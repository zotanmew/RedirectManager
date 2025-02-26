# RedirectManager
RedirectManager is a simple URL shortener written in ASP.NET Core, using SQLite for data storage & environment variables for configuration. It features a simple Blazor SSR admin dashboard under `/admin`.

## Example systemd service definition
```systemd
[Unit]
Description=RedirectManager daemon
After=network.target

[Service]
User=app
Group=app
WorkingDirectory=/home/app/RedirectManager/publish
ExecStart=/home/app/RedirectManager/publish/RedirectManager
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS='http://127.0.0.1:5000'
Environment=AUTH_TOKEN=<admin token here>

[Install]
WantedBy=multi-user.target
```

## Example reverse proxy config
```caddyfile
link.example.org {
    reverse_proxy :5000
}
```
