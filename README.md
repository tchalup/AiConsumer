# AI Consumer API

Esta é uma API ASP.NET Core que permite aos usuários fazer upload de um arquivo e fazer perguntas sobre ele usando a API do Google Gemini.

## Documentation

- [Class Diagram](./docs/class-diagram.md)
- [Sequence Diagram](./docs/sequence-diagram.md)

## Funcionalidades

-   Faz upload de um arquivo e o armazena em um banco de dados em memória.
-   Recebe um prompt de texto e o conteúdo de um arquivo para fazer uma pergunta à API do Gemini.
-   Retorna a resposta de texto gerada pelo Gemini.

## Configuração e Execução

1.  **Clone o repositório:**
    ```bash
    git clone <URL_DO_REPOSITORIO>
    cd <NOME_DO_REPOSITORIO>
    ```

2.  **Configure a Chave da API:**
    -   Abra o arquivo `src/AiConsumer.Api/appsettings.json`.
    -   Substitua o valor de `SUA_CHAVE_API_AQUI` pela sua chave de API do Google Gemini.
    ```json
    "Gemini": {
      "ApiKey": "SUA_CHAVE_API_AQUI",
      "BaseUrl": "https://generativelanguage.googleapis.com/"
    }
    ```

3.  **Execute a API:**
    Navegue até a raiz do projeto (onde o arquivo `.sln` está localizado) e execute o seguinte comando:
    ```bash
    dotnet run --project src/AiConsumer.Api/AiConsumer.Api.csproj
    ```
    A API estará em execução em `https://localhost:7199` (a porta pode variar).

## Endpoints

### 1. Upload de Arquivo

Este endpoint faz o upload de um arquivo para o servidor. Ele retorna um ID exclusivo (`Guid`) que pode ser usado para se referir ao arquivo em chamadas subsequentes.

-   **Método:** `POST`
-   **URL:** `/api/files`
-   **Corpo:** `multipart/form-data`

**Exemplo de Requisição com `curl`:**

Substitua `caminho/para/seu/arquivo.png` pelo caminho real do arquivo que você deseja enviar.

```bash
curl -X POST "https://localhost:7199/api/files" -F "file=@caminho/para/seu/arquivo.png" -k
```
- A flag `-k` é usada para ignorar a validação do certificado de desenvolvimento local.

**Resposta de Sucesso (Exemplo):**

```json
"a1b2c3d4-e5f6-7890-1234-567890abcdef"
```

### 2. Fazer uma Pergunta sobre o Arquivo

Este endpoint envia o arquivo (referenciado pelo seu ID) e um prompt de texto para a API do Gemini para análise.

-   **Método:** `POST`
-   **URL:** `/api/files/{id}/ask`
-   **Corpo:** `application/json`

**Exemplo de Requisição com `curl`:**

Substitua `{id-do-arquivo}` pelo ID retornado pelo endpoint de upload.

```bash
curl -X POST "https://localhost:7199/api/files/{id-do-arquivo}/ask" \
-H "Content-Type: application/json" \
-d '{"prompt": "O que há nesta imagem?"}' \
-k
```

**Resposta de Sucesso (Exemplo):**

A resposta será o texto gerado pela API do Gemini com base no arquivo e no prompt.

```json
"Esta imagem contém um belo pôr do sol na praia, com o céu em tons de laranja e roxo."
```