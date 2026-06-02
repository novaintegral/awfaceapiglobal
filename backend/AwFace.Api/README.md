# AWFace API

Backend .NET 9 para centralizar as chamadas da aplicacao Angular para a Certiface.

## Configuracao

As configuracoes ficam na secao `Certiface` e podem ser sobrescritas por variaveis de ambiente:

- `Certiface__BaseUrl`
- `Certiface__Login`
- `Certiface__Password`
- `Certiface__TimeoutSeconds`

Nao grave credenciais reais em `appsettings*.json`. Use variaveis de ambiente, user-secrets ou o cofre de segredos do ambiente de deploy.

## Execucao local

```powershell
dotnet run --project backend/AwFace.Api/AwFace.Api.csproj
```

O Angular deve chamar `/api/facecaptcha/service/captcha/...`. Em desenvolvimento, use o proxy do Angular para redirecionar `/api` para a porta do backend.
