# Deploy

## Azure ARM
	az login
	az group create --name rg-techchallenge2-dev --location eastus2
	az deployment group create --resource-group rg-techchallenge2-dev --template-file ./template.json --parameters storageAccountName=sttechchallenge2dev containerRegistryName=crtechchalleng2dev postgresServerName=psqltechchalleng2dev postgresAdminUsername=systemadmin postgresAdminPassword=root@1234

### Create RBAC
	az ad sp create-for-rbac --name "TechChallenge2Dev" --role contributor --scopes /subscriptions/00000-0000-0000-000000000/resourceGroups/rg-techchallenge2-dev/providers/Microsoft.ContainerRegistry/registries/crtechchalleng2dev --sdk-auth

### Get Json RBAC and add github secrets
	ACR_CREDENTIALS

### Get Container Registry Password 	
	az acr credential show --name crtechchalleng2dev --query "passwords[0].value" --output tsv

### Add Password in github secrets
	ACR_PASSWORD

### Add UserName in github secrets
	ACR_USERNAME=crtechchalleng2dev

### Add Registry Name in github variables
	REGISTRY_NAME=crtechchalleng2dev.azurecr.io

### Add Resource Group in github variables
	RESOURCE_GROUP=rg-techchallenge2-dev

### Postgres Flexible Server, create database and copy connectionstring
### Add connectionstring in github secrets
	CONNECTION_STRING
	BLOG_CONNECTION_STRING

### Add StorageAccount in github secrets
	STORAGECONNECTIONSTRING


# Referências

- https://learn.microsoft.com/en-us/cli/azure/
- https://github.com/dotnet-architecture/eShopOnWeb
- https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio
- https://github.com/ardalis/CleanArchitecture
- https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0#blazor-server
- https://www.mudblazor.com/docs/overview
- https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming
