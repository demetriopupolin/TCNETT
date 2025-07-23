# Fiap Cloud Games – Sistema de Gestão de Jogos On Line 🎮
# Tech Challenge da Fase 1 da Pós-Tech em Arquitetura de Sistemas .NET
 
## 📌 Descrição
A Fiap Cloud Games permite o gerenciamento completo de usuários, jogos, pedidos e promoções.

# 👤 Níveis de Acesso e Funcionalidades

### 🧑 Usuário
  
✅ Criar uma conta e fazer login.   
✅ Consultar os todos os jogos disponíveis.   
✅ Fazer pedidos com promoção valida ou sem promoção.   
✅ Visualizar seus próprios pedidos.   

### 👨‍💼 Administrador

✅ Cadastrar, Consultar, Editar e Excluir (usuários, jogos, promoções e pedidos).  
✅ Inclusive cadastrar novos usuários com nivel A-Administrador.  

## 🔧 Tecnologias Utilizadas

| Camada / Recurso        | Tecnologia                         | Descrição                                                                 |
|-------------------------|-------------------------------------|---------------------------------------------------------------------------|
| 💻 Linguagem            | C#                                  | Linguagem principal do projeto                                            |
| 🌐 Framework Web        | ASP.NET Core                        | Framework para construção da API REST                                    |
| 📚 Documentação API     | Swagger (Swashbuckle.AspNetCore)    | Geração automática e visualização da documentação da API                 |
| 🛢️ Banco de Dados       | SQL Server                          | Sistema de gerenciamento de banco relacional                             |
| 📦 ORM (mapeamento)     | Entity Framework Core               | ORM para facilitar acesso e manipulação do banco de dados                |
| 🔐 Autenticação         | JWT (JSON Web Token)                | Segurança da API via autenticação baseada em tokens                      |
| 📁 DTOs                 | Data Transfer Objects               | Objetos para transportar dados entre as camadas da aplicação             |
| 🧪 Testes               | Swagger UI                          | Interface gráfica para testar e validar os endpoints da API              |

## ⚙️ Como Rodar
1. Clone este repositório.
2. Em /DB execute o script.sql em um banco de dados SqlServer para criação de tabelas.
3. Abra o Projeto no Visual Studio.
4. Configure a conexão com banco de dados em `appsetings.json`
5. Rode o projeto.

## 🗃️ Estrutura de Dados

Usuario
| Campo         | Tipo     | Chave | Not Null | Observação          |
| ------------- | -------- | ----- | -------- | ------------------- |
| ID_Usuario   | int      | 🔑 PK | ✅        | Identificador do Usuário |
| Data_Criacao | datetime |       | ✅        | Data do cadastro    |
| Nome          | varchar  |       | ✅        | Nome do usuário     |
| Email         | varchar  | 🔷 UK | ✅        | Deve ser único      |
| Senha         | varchar  |       | ✅        | Criptografada       |

Jogo
| Campo           | Tipo     | Chave | Not Null | Observação        |
| --------------- | -------- | ----- | -------- | ----------------- |
| ID_Jogo        | int      | 🔑 PK | ✅        | Identificador do Jogo     |
| Data_Criacao   | datetime |       | ✅        | Cadastro do jogo  |
| Nome            | varchar  |       | ✅        | Nome do jogo      |
| Ano_Lancamento | int      |       | ✅        | Ano de lançamento |
| Preco_Base     | decimal  |       | ✅        | Preço original    |

  
Pedido
| Campo         | Tipo     | Chave  | Not Null | Observação                            |
| ------------- | -------- | -----  | -------- | ------------------------------------- |
| ID_Pedido     | int      | 🔑 PK | ✅        | Identificador do Pedido               |
| Data_Criacao  | datetime |        | ✅        | Quando o pedido foi criado            |
| ID_Usuario    | int      | 🔗 FK | ✅        | Ref. ao usuário                       |
| ID_Jogo       | int      | 🔗 FK | ✅        | Ref. ao jogo                          |
| ID_Promocao   | int      | 🔗 FK | ❌        | Pode ou não estar presente            |
| VlPedido      | decimal  |       | ✅        | Valor do pedido                       |
| VlDesconto    | decimal  |       | ✅        | Valor do desconto                     |
| VlPago        | decimal  |       | ✅        | Valor Pago                            |


Promoção
| Campo                | Tipo     | Chave | Not Null | Observação                |
| -------------------- | -------- | ----- | -------- | ------------------------- |
| ID\_Promocao         | int      | 🔑 PK | ✅        | Identificador da Promoção |
| Data\_Criacao        | datetime |       | ✅        | Cadastro da promoção      |
| Nome                 | varchar  | 🔷 UK | ✅        | Deve ser único            |
| Percentual\_Desconto | decimal  |       | ✅        | Valor em porcentagem      |
| Data\_Validade       | datetime |       | ✅        | Até quando é válida       |

🔑 = Primary Key (PK)
🔗 = Foreign Key (FK)
🔷 = Unique Key (UK)
⚠️	= Not Null

## 📜 Regras Gerais do Sistema  

O sistema segue as seguintes regras e restrições de funcionamento:

1. 🎮 **Cadastro de Usuário**  
   ✅ Todo usuario deve possuir Nome, E-mail e Senha de Acesso.  
   ✅ Não deverá conter usuários com e-mail repetido.   

1. 🎮 **Cadastro de Jogos**  
   ✅ Todo jogo deve possuir nome, descrição, ano de lançamento e preço base.  
   ✅ Jogos não podem ser cadastrados com preços negativos ou zerados.  
   ✅ O ano de lançamento do jogo não poderá ser superior a sua data de criação.  
   ✅ O ano de lançamento do jogo não poderá ser superior ao ano corrente.     

3. 🛒 **Pedidos**  
   ✅ Cada pedido está vinculado a um único jogo.  
   ✅ Todo pedido deve conter obrigatoriamente um usuário e o jogo adquirido.  
   ✅ Pode haver uma promoção (cupom de desconto) associada ao pedido, desde que sua data de validade atenda a data de criação do pedido.  
   ✅ O valor total do pedido é calculado com base no preço do jogo, aplicando o desconto da promoção, se houver.  

4. 💸 **Promoções**  
   ✅ A promoção deverá conter obrigatoriamente um nome, data de validade e percentual de desconto.  
   ✅ O percentual de desconto deverá ser em numéro inteiro de 10% a 90% de desconto.  
   ✅ A promoção deverá ter um nome único entre todas as promoções existentes.  
   ✅ A data de validade da promoção deverá ser ao menos a data de inclusão da promoção.  

5. 👥 **Controle de Acesso**  
   ✅ Usuários comuns podem criar usuário, fazer login, consultar jogos, realizar pedidos e visualizar seus próprios pedidos sendo seu nivel como "U"-Usuário.  
   ✅ Administradores têm acesso completo ao sistema sendo nível como "A"-Administrador.  

6. 🔐 **Segurança**   
   ✅ O e-mail do usuário informado deverá ser bem formado: usuario@domino.xxx  
   ✅ A senha do usuário deverá conter obrigatoriamente 8 caracteres contendo números, letras e caracteres especiais.  
   ✅ O login deverá ser realizado através de e-mail do usuário e sua respectiva senha.  

7. 🗑️ **Exclusões**  
   ✅ Toda solicitação de exclusão deve verificar se o item não está sendo referenciado por outras entidades no sistema. Conforme a seguir:  
   ✅ Não é permitido excluir um jogo que esteja vinculado a algum pedido.  
   ✅ Não é permitido excluir uma promoção que tenha sido aplicada em algum pedido.  
   ✅ Não é permitido excluir um usuário que esteja associado a um pedido já registrado.  

## 🎓 Informações Acadêmicas
- Curso: Pós-Tech em Arquitetura de Sistemas .NET  
- Instituição: FIAP  
- Aluno: Demetrio Pupolin  
- E-mail: pupolin@gmail.com  
- RM: 365898  
