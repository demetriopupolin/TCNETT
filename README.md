
# 🎮 Fiap Cloud Games
 
## 📌 Descrição
O Fiap Cloud Games permite o gerenciamento completo de usuários, jogos, pedidos e promoções.

## 👤 Níveis de Acesso e Funcionalidades

### 🧑 Usuário
  
✅ Criar uma conta e fazer login.   
✅ Consultar os todos os jogos disponíveis.   
✅ Fazer pedidos com promoção vigente ou sem promoção.   
✅ Visualizar seus próprios pedidos.   

### 👨‍💼 Administrador

✅ Cadastrar, Consultar, Editar e Excluir (usuários, jogos, promoções e pedidos).  
✅ Cadastrar novos usuários com nivel A-Administrador.  

## ⚙️ Como Rodar
1. Clone este repositório.
2. Em /DB execute o script.sql em um banco de dados SqlServer para criação de tabelas e dados de exemplo.
3. Abra o Projeto FiapCloudGames.sln no Visual Studio.
4. Configure a conexão com banco de dados em `appsetings.json`
5. Compile e execute o projeto.

## 🗃️ Estrutura de Dados

📋 Tabela: Usuário
| Campo         | Tipo     | Chave | Not Null | Observação          |
| ------------- | -------- | ----- | -------- | ------------------- |
| ID           | int      | 🔑 PK | ✅        | Identificador do Usuário (auto incremento IDENTITY) |
| Data_Criacao | datetime |       | ✅        | Data do cadastro    |
| Nome          | varchar(100)  |       | ✅        | Nome do usuário     |
| Email         | varchar(100)  | 🔷 UK | ✅        | Deve ser único      |
| Senha         | varchar(100)  |       | ✅        | Senha                |
| Nivel         | char(1)  |       | ✅        | "A"-Administrador ou "U"-Usuário |

📋 Tabela: Jogo
| Campo           | Tipo     | Chave | Not Null | Observação        |
| --------------- | -------- | ----- | -------- | ----------------- |
| ID              | int      | 🔑 PK | ✅        | Identificador do Jogo (auto incremento IDENTITY)    |
| Data_Criacao   | datetime |       | ✅        | Cadastro do jogo  |
| Nome            | varchar(100) |       | ✅        | Nome do jogo      |
| Ano_Lancamento | int      |       | ✅        | Ano de lançamento |
| Preco_Base     | decimal  |       | ✅        | Preço original    |


📋 Tabela Promoção
| Campo                | Tipo     | Chave | Not Null | Observação                |
| -------------------- | -------- | ----- | -------- | ------------------------- |
| ID                   | int      | 🔑 PK | ✅        | Identificador da Promoção (auto incremento IDENTITY) |
| Data_Criacao         | datetime |       | ✅        | Cadastro da promoção      |
| Nome                 | varchar(100)  | 🔷 UK | ✅        | Deve ser único            |
| Desconto             | int  |       | ✅        | Valor em porcentagem      |
| Data_Validade        | datetime |       | ✅        | Até quando é válida       |

  
📋 Tabela Pedido
| Campo         | Tipo     | Chave  | Not Null | Observação                            |
| ------------- | -------- | -----  | -------- | ------------------------------------- |
| ID_Pedido     | int      | 🔑 PK | ✅        | Identificador do Pedido (auto incremento IDENTITY)             |
| Data_Criacao  | datetime |        | ✅        | Quando o pedido foi criado            |
| UsuarioID     | int      | 🔗 FK | ✅        | ID do usuário                         |
| JogoID        | int      | 🔗 FK | ✅        | ID do jogo                          |
| PromocaoID    | int      | 🔗 FK | ❌        | ID da promoção                       |
| VlPedido      | decimal(10,2)  |       | ✅        | Valor do pedido                       |
| VlDesconto    | decimal(10,2)  |       | ✅        | Valor do desconto                     |
| VlPago        | decimal(10,2)  |       | ✅        | Valor Pago                            |


🔑 = Primary Key (PK)
🔗 = Foreign Key (FK)
🔷 = Unique Key (UK)

## 📜 Regras Gerais do Sistema  

O sistema segue as seguintes regras e restrições de funcionamento:

1. 🧑‍💻  **Cadastro de Usuário**  
   ✅ Todo usuario deve possuir Nome, E-mail e Senha de Acesso.  
   ✅ Não deverá conter usuários com e-mail repetido.  
   ✅ A senha deverá conter no mínimo de 8 caracteres com números, letras e caracteres especiais.

1. 🎮 **Cadastro de Jogos**  
   ✅ Todo jogo deve possuir nome, ano de lançamento e preço base sendo o valor maior que zero.  
   ✅ O ano de lançamento do jogo não poderá ser inferior a sua data de criação.  
   ✅ O ano de lançamento do jogo não poderá ser superior ao ano corrente.     

3. 💸 **Promoções**  
   ✅ A promoção deverá conter obrigatoriamente um nome, data de validade e percentual de desconto.  
   ✅ O percentual de desconto deverá ser em numéro inteiro de 10% a 90% de desconto.  
   ✅ A promoção deverá ter um nome único entre todas as promoções existentes.  
   ✅ A data de validade da promoção deverá ser ao menos a data de inclusão da promoção.  

4. 🛒 **Pedidos**  
   ✅ Cada pedido está vinculado a um único jogo.  
   ✅ Todo pedido deve conter obrigatoriamente um usuário e o jogo adquirido.  
   ✅ O pedido pode ou não conter uma promoção (cupom de desconto), desde que sua data de validade atenda a data de criação do pedido.  
   ✅ O valor pago do pedido é calculado com base no preço base do jogo, aplicando o desconto da promoção, se houver.  

5. 👥 **Controle de Acesso**  
   ✅ Usuários comuns podem criar usuário, fazer login, consultar jogos, realizar pedidos e visualizar seus próprios pedidos sendo seu nivel como "U"-Usuário.  
   ✅ Administradores têm acesso completo ao sistema sendo nível como "A"-Administrador.  

6. 🔐 **Segurança**   
   ✅ O e-mail do usuário informado deverá ser informado corretamente. 
   ✅ A senha do usuário deverá conter obrigatoriamente 8 caracteres contendo números, letras e caracteres especiais.  
   ✅ O login deverá ser realizado através de e-mail do usuário e sua respectiva senha.  

7. 🗑️ **Exclusões**  
   ✅ Toda exclusão deve verificar se o item não está sendo referenciado por outras entidades no sistema. Conforme a seguir:  
   ✅ Não é permitido excluir um jogo que esteja vinculado em algum pedido.  
   ✅ Não é permitido excluir uma promoção que esteja vinculada em algum pedido.  
   ✅ Não é permitido excluir um usuário que esteja vinculado em algum pedido.



## 🔐 Autenticação

### POST `/auth/login`

Autentica o usuário e retorna um token JWT que pode ser usado nas próximas requisições autenticadas.

#### Níveis Permitidos  
✅ Usuario   
✅ Administrador  

#### 📥 Requisição  
json
{
  "email": "usuario@email.com",
  "senha": "123456"
}

| Campo | Tipo   | Obrigatório | Descrição         |
| ----- | ------ | ----------- | ----------------- |
| email | string | ✅           | E-mail do usuário |
| senha | string | ✅           | Senha do usuário  |

📤 Resposta (200 OK)  
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI..."
}
```


⚠️ Possíveis Erros
| Código | Mensagem                          | Motivo                       |
| ------ | --------------------------------- | ---------------------------- |
| 400    | "Email inválido."                 | Formato de e-mail incorreto  |
| 401    | "Usuário {email} não encontrado." | E-mail não cadastrado        |
| 401    | "Senha incorreta."                | Senha divergente do cadastro |



## 🎮 Jogos

### GET `/jogo`

Retorna todos os jogos cadastrados.

#### Níveis Permitidos  
✅ Usuario   
✅ Administrador  

📤 **Resposta (200 OK)**
```json
[
  {
    "id": 1,
    "dataCriacao": "2025-01-01T12:00:00",
    "nome": "MORTAL KOMBAT",
    "anoLancamento": 2025,
    "precoBase": 300.0,
    "pedidos": []
  }
]
```


   
## 🎓 Informações Acadêmicas
- Curso: Pós-Tech em Arquitetura de Sistemas .NET  
- Instituição: FIAP  
- Aluno: Demetrio Pupolin  
- E-mail: pupolin@gmail.com  
- RM: 365898  
