# dotnet8-aot-lambda-serveless

Meu primeiro projeto em dotnet 8, usando AOT, para AWS Lambda.

## Disclaimer
Isso não é um código para ir para produção. Não há injeções de dependencias, os métodos async não possuem await (por limitação do List<Product>) e bons controles. 

## Como replicar do zero

Instalar os templates da aws lambda:
```bash
dotnet new --install Amazon.Lambda.Templates
```

Rodar o seguinte comando: 
```bash
dotnet new serverless.NativeAOT --name Bossolani.Products --output .
```

Isso irá criar o projeto na pasta raiz com o nome Bossolani.Products

## Como testar

De um play pelo Visual Studio ou pela linha de comando:
```
cd src
dotnet run .\Bossolani.Products.csproj
```

## Build da aplicação

```
sam build
```

## Subindo na AWS

Primeiramente, garanta que o arquivo MakeFile e o arquivo template.yaml estão corretos.

```
sam deploy
```