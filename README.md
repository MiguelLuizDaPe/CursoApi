## Coisas pra fazer
 - ESTUDAR LAMBDA ESPRESSÃO E LAMBDA STATEMENT
## Baixar essas coisas(não precisou pq baixei localmente no código)
 - Microsoft.AspNetCore.JsonPatch
 - Microsoft.AspNetCore.Mvc.NewtonsoftJson
 - "dotnet tool install --global dotnet-ef" pra ver depois isso

# Anotações
 - Estudar sobre os bagulho de 200, 400 de BadRequest e os caralho
 - usar o SelecMany pra pegar os endereços pelo id(eu acho)
## Regras de validação
 - ModelState contem o estado e validação do modelo e uma coleção de mensagens(eu acho) de erros
 - tools.ietf.org/html/rfc7807 contem detalhes do problema para APIs (não sem se vale muito apena ler)
 - aspnet core faz a definição automática de acordo com rfc
 - existem formatos específicos para serem retornados para o cliente
 - o que define as regras de validação é (definir, aplicar, reportar)

# Anotações Sobre patch
É preciso do (Microsoft.AspNetCore.JsonPatch) pra desbloquear a função patch no hhtp, pois ela não vem por padrão, e para ele funcinar é preciso baixar e configurar (Microsoft.AspNetCore.Mvc.NewtonsoftJson)
E pelo visto 