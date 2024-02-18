# dotnet8-aot-lambda-serveless
Meu primeiro projeto em dotnet 8, usando AOT, para AWS Lambda.

Instalar os templates da aws lambda:
```bash
dotnet new --install Amazon.Lambda.Templates
```

Rodar o seguinte comando: 
```bash
dotnet new serverless.NativeAOT --name Bossolani.Products --output .
```

Isso irá criar o projeto na pasta raiz com o nome Bossolani.Products