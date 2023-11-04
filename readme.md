# Documentação dos Componentes Azure para a Aplicação

Este documento descreve os componentes Azure necessários para a aplicação, explicando sua finalidade e como interagem entre si com base nas informações atualizadas sobre restrições de acesso.

## 1. **Azure Database para PostgreSQL**

Azure Database para PostgreSQL para armazendo dos dados.

## 2. **Storage Account**

Azure Storage Account, para armazenamento de imagens.

## 3. **WebApp Service - Blog**

Azure WebApp Service com Docker para o Blog.

## 4. **Application Insights**

Azure Application Insights monitoramento.

## 5. **WebApp Service - API**

Azure WebApp Service com Docker para a API.

## **Interação Entre Componentes**:
- O **serviço de API** interage com o **PostgreSQL Database** para recuperar e armazenar dados.
- **Application Insights** coleta e analisa dados de telemetria de ambos os serviços **API** e **Blog** para fornecer insights sobre o desempenho e uso da aplicação.
- A **Storage Account** é acessada pelo **serviço de Blog** para recuperar e armazenar dados não estruturados ou arquivos.
- Ambos os **WebApp Services** (API e Blog) podem ser configurados para enviar logs e métricas para **Application Insights** para monitoramento e análise centralizados.

# GitHub Action: Build, Test, and Deploy to Azure

## Environment Variables

- `REGISTRY_NAME`: Nome do Azure Container Registry.
- `API_SERVICE`: Endpoint do serviço para a API.
- `RESOURCE_GROUP`: Nome do Grupo de Recursos do Azure.

## Secrets

- `StorageConnectionString`: String de conexão para o Azure Blob Storage.
- `CONNECTION_STRING`: String de conexão para o banco de dados usado na API.
- `BLOG_CONNECTION_STRING`: String de conexão para o banco de dados usado no Blog.
- `AZURE_CREDENTIALS`: Credenciais para login no Azure.
- `ACR_USERNAME`: Nome de usuário para o Azure Container Registry.
- `ACR_PASSWORD`: Senha para o Azure Container Registry.
- `AZURE_WEBAPP_PUBLISH_PROFILE_API`: Perfil de publicação para o Azure Web App para a API.
- `AZURE_WEBAPP_PUBLISH_PROFILE_BLOG`: Perfil de publicação para o Azure Web App para o Blog.

## Jobs

### 1. Unit Testing

- **OS**: Ubuntu-latest
- **Passos**:
  - Checkout do código do repositório.
  - Executar testes unitários usando o comando dotnet.

### 2. Integration Testing

- **OS**: Ubuntu-latest
- **Passos**:
  - Checkout do código do repositório.
  - Executar testes de integração usando o comando dotnet.

### 3. Build and Push

- **Condição**: Sucesso dos jobs anteriores.
- **Depende De**: unit_tests, integration_tests
- **OS**: Ubuntu-latest
- **Passos**:
  - Checkout do código do repositório.
  - Definir vários valores de propriedades nos arquivos appsettings.json.
  - Login no Azure e no Azure Container Registry.
  - Construir e enviar imagens Docker para a API e Blog para o Azure Container Registry.

### 4. Publish WebApp API

- **Condição**: Sucesso dos jobs anteriores.
- **Depende De**: build_and_push
- **OS**: Ubuntu-latest
- **Passos**:
  - Implementar a imagem Docker da API no Azure WebApp.

### 5. Publish WebApp Blog

- **Condição**: Sucesso dos jobs anteriores.
- **Depende De**: build_and_push
- **OS**: Ubuntu-latest
- **Passos**:
  - Implementar a imagem Docker do Blog no Azure WebApp.


# Referências

- https://learn.microsoft.com/en-us/cli/azure/
- https://github.com/Azure/actions-workflow-samples/blob/master/assets/create-secrets-for-GitHub-workflows.md
- https://github.com/dotnet-architecture/eShopOnWeb
- https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio
- https://github.com/ardalis/CleanArchitecture
- https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0#blazor-server
- https://www.mudblazor.com/docs/overview
- https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming
- https://www.youtube.com/watch?v=ha-wbrrrRnk