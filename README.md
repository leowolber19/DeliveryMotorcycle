# DeliveryMotorcycle

Este projeto é uma API desenvolvida em .NET 8 que oferece funcionalidades essenciais para o aluguel de motocicletas e o gerenciamento de entregadores.
A solução foi construída com foco em boas práticas de arquitetura, autenticação/autorização, persistência de dados em PostgreSQL e mensageria com RabbitMQ.


## Como executar o projeto localmente:

 1 - Clone o repositório para o seu ambiente local.
 
 2 - Acesse o terminal na pasta do projeto.
 
 3 - Suba os containers necessários com o comando: docker-compose up

Após esse comando, os conteineres do PostgreSQL e RabbitMQ serão iniciados automaticamente, permitindo que a API rode corretamente.


## 🙍‍♂️ User

- ### Registrar usuário 

#### **POST** /User/Register

Parâmetros:
```
"userName": "string",
"password": "string",
"role": "string"
```

**Obs.:** Para criar um usuário administrador, é necessário informar explicitamente o parâmetro Role = "Admin".
Caso o parâmetro não seja informado, o sistema atribuirá automaticamente o perfil Role = "User".

Retorno:
```
{
  "success": true,
  "message": "User registered!"
}
```

- ### Realizar Login

#### **POST** /User/Login

Parâmetros:
```
"userName": "string",
"password": "string"
```

Retorno:
```
{
  "token": "string",
  "role": "string"
}
```

## 🔑 Autenticação

O esquema de autenticação Bearer utiliza um token de acesso enviado no cabeçalho Authorization da requisição HTTP. Neste projeto, foi configurado para que o prefixo "Bearer" seja adicionado automaticamente, portanto não é necessário digitá-lo manualmente — basta informar apenas o token.

```
Authorization: <seu_token_aqui>
```

## 🛵 Motorcycles

- ### Cadastrar uma moto

**Obs.:** Eu como usuário Admin, irei ter acesso para os métodos da Motorcycle.

#### **POST** /Motorcycles/Create

Body:
```
{
  "year": int,
  "model": "string",
  "plate": "string"
}
```

Retorno:
```
{
  "success": true,
  "message": "Success to create motorcycle!"
}
```

- ### Consultar uma moto pela Placa

#### **GET** /Motorcycles/GetByPlate

Parâmetros:
```
"plate": "string"
```

Retorno:
```
{
  "year": int,
  "model": "string",
  "plate": "string",
  "status": int,
  "id": Guid,
  "createdAt": "2025-09-11T21:25:52.036898Z",
  "deleteAt": "2025-09-11T21:25:52.036898Z",
  "excluded": bool
}
```

- ### Editar a placa de uma moto por Id

#### **PUT** /Motorcycles/Edit/{id}/Plate

Parâmetros:
```
"id": Guid
```

Body:
```
{
  "plate": "string"
}
```

Retorno:
```
{
  "success": true,
  "message": "Success to edit motorcycle!"
}
```

- ### Consultar motos por Id

#### **GET** /Motorcycles/{Id}

Parâmetros:
```
"id": Guid
```

Retorno:
```
{
  "year": int,
  "model": "string",
  "plate": "string",
  "status": int,
  "id": Guid,
  "createdAt": "2025-09-11T21:25:52.036898Z",
  "deleteAt": "2025-09-11T21:25:52.036898Z",
  "excluded": bool
}
```

- ### Deletar motos por Id

#### **DELETE** /Motorcycles/Delete/{Id}

Parâmetros:
```
"id": Guid
```

Retorno:
```
{
  "success": true,
  "message": "Success to delete motorcycle!"
}
```

## 📧 Notifications

- ### Consultar motos que são de 2024

**Obs.:** Eu como usuário Admin, irei ter acesso para os métodos da Notifications.

#### **GET** /Notificartions/Get

Parâmetros:
```
No parameters
```

Retorno:
```
[
  {
    "model": "string",
    "plate": "string",
    "year": int,
    "createdAt": "2025-09-11T21:43:42.002713Z"
  }
]
```

## 🏍 DeliveryMan

- ### Cadastrar um entregador
 
**Obs.:** Eu com usuário autenticado, seja Admin ou User, irei ter acesso para os métodos da DeliveryMan.

#### **POST** /DeliveryMan/Create

Body:
```
{
  "name": "string",
  "cnpj": "string",
  "birthDate": "2025-09-11T21:50:13.473Z",
  "cnhNumber": "string",
  "cnhType": int
}
```

cnhType:

```
0 = Habilitação A
1 = Habilitação B
2 = Habilitação A e B
```

Retorno:
```
{
  "success": true,
  "message": "Success to create delivery man!"
}
```

- ### Atualizar a CNH do entregador

#### **PUT** /DeliveryMan/UpdateCnh

Parâmetros:
```
File: FILE
```

Retorno:
```
{
  "success": true,
  "message": "Success updating driver's license!"
}
```

## 📍 Rental

- ### Consultar planos de locações

**Obs.:** Eu com usuário autenticado, seja Admin ou User, irei ter acesso para os métodos da Rental.

#### **GET** /Rental/Plans

Parâmetros:
```
No parameters
```

Retorno:
```
[
  {
    "days": int,
    "value": decimal,
    "percentage": double,
    "id": Guid,
    "createdAt": "2025-09-11T20:28:01.433291Z",
    "deleteAt": "2025-09-11T20:28:01.433291Z",
    "excluded": bool
  }
]
```

- ### Criar uma locação

#### **POST** /Rental/Create

Parâmetros:
```
{
  "days": int,
  "expectedEndDate": "2025-09-11T22:14:26.231Z"
}
```

days:

```
7 = R$30,00 por dia
15 = R$28,00 por dia
30 = R$22,00 por dia
45 = R$20,00 por dia
50 = R$18,00 por dia
```

Retorno:
```
{
  "success": true,
  "message": "Success to create rental! RentalID: {{Guid}}"
}
```

- ### Consultar locações por Id

#### **GET** /Rental/{Id}

Parâmetros:
```
"id": Guid
```

Retorno:
```
{
  "motorcycle": "string",
  "deliveryMan": "string",
  "plan": int,
  "rentalValue": double,
  "startDate": "2025-09-12T22:16:36.466904Z",
  "endDate": "2025-09-18T22:16:36.467057Z",
  "expectedEndDate": "2025-09-18T22:16:23.522Z",
  "returnDate": "2025-09-18T22:16:23.522Z"
}
```

- ### Informar a data de devolução e calcular o valor da locação

#### **PUT** /Rental/{id}/Return

Parâmetros:
```
"id": Guid
```

Body:
```
{
  "returnDate": "2025-09-11T22:21:11.215Z"
}
```

Retorno:
```
{
  "success": true,
  "message": "Success saving return date! Value rental: R$ 0,00"
}
```
