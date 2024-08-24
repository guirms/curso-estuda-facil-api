# Estuda Fácil API

Uma API RESTful projetada para ajudar estudantes a se organizarem de forma inteligente para suas avaliações, utilizando inteligência artificial.

Com base no conteúdo e na data das provas, essa API identifica os tópicos mais relevantes para estudo, permitindo a criação de um plano otimizado de acordo com o tempo disponível do aluno.

Essa solução utiliza a [API da OpenAI](https://openai.com/index/openai-api/) através de um [microsserviço em Python](https://github.com/guirms/curso-estuda-facil-ms).

## Pré-requisitos

Para executar o projeto com sucesso, você deve atender os seguintes requisitos:

- [.NET 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/) (A versão utilizada no projeto é 8.0.33)

## Criando instância da API

Para criar uma instância da API localmente, siga o passo a passo:

- Clone o projeto na sua pasta de preferência através do comando `git clone https://github.com/guirms/curso-estuda-facil-api.git`
- No terminal, dentro da pasta raíz do projeto, execute o comando `cd ./Presentation.Web`
- Por fim, crie a instância da API através do comando `dotnet run`

## Execute a aplicação através do Docker

Esta API está "dockerizada" e sua imagem está hospedada no [DockerHub](https://hub.docker.com).
Caso você deseje criar um container Docker a partir dessa imagem, siga os seguintes passos:

- Instale o [Docker](https://docs.docker.com/get-started/get-docker/) (caso não tenha)
- No terminal, execute o seguinte comando `docker run -it -p 5000:8080 guirms/estuda-facil`

## Considerações

Desenvolvi este projeto para o lançamento do meu curso .NET FOCADO NO MERCADO DE TRABALHO. Coloquei em prática diversos conceitos adquiridos durante meus cinco anos de experiência na área e busquei focar em construir uma solução escalável tanto conceitual quanto tecnicamente. Espero que todos os alunos tenham tido uma boa experiência durante o curso assim como eu tive durante o seu planejamento e desenvolvimento. Agradeço a todos os alunos e gostaria de ressaltar que é uma satisfação indescritível poder passar meu conhecimento e experiência adiante e espero que nos vejamos em breve em novos desafios (novidades por aí...😉).
