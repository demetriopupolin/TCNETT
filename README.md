
# 🎮 Fiap Cloud Games
 
## 📌 Descrição
O Fiap Cloud Games permite o gerenciamento completo de usuários, jogos, pedidos e promoções.

## 👤 Níveis de Acesso e Funcionalidades

### 🧑 Usuário
  
✅ Criar uma conta e fazer login.   
✅ Consultar os todos os jogos disponíveis.   
✅ Fazer pedidos com promoção valida ou sem promoção.   
✅ Visualizar seus próprios pedidos.   

### 👨‍💼 Administrador

✅ Cadastrar, Consultar, Editar e Excluir (usuários, jogos, promoções e pedidos).  
✅ Cadastrar novos usuários com nivel A-Administrador.  

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






✅ Casos de Teste – Sistema de Jogos

Este documento descreve os casos de testes funcionais para o sistema de cadastro de usuários, jogos, promoções, pedidos, controle de acesso, segurança e exclusões.

---

🧑‍💻 Cadastro de Usuário

| ID      | Descrição                                 | Entrada                                          | Resultado Esperado                       |
|---------|-------------------------------------------|--------------------------------------------------|-------------------------------------------|
| CTU001  | Cadastro de usuario com dados válidos     | Nome: João Silva, Email: joao@email.com, Senha: Senha123! | Usuário cadastrado com sucesso            |
| CTU002  | E-mail já cadastrado                      | Email: joao@email.com                           | Erro: e-mail duplicado                    |
| CTU003  | Senha sem caracteres especiais            | Senha: Senha123                                 | Erro: senha inválida                      |
| CTU004  | Senha com menos de 8 caracteres           | Senha: R@1a                                     | Erro: senha muito curta                   |
| CTU004  | Cadastro de usuário com nível Administrador | Nome: Pedro Lucas, Email: pedroo@email.com, Senha: Pass123!  |Usuário cadastrado com sucesso                   |


---

🎮 Cadastro de Jogos

| ID      | Descrição                                           | Entrada                                              | Resultado Esperado                         |
|---------|-----------------------------------------------------|------------------------------------------------------|---------------------------------------------|
| CTJ001  | Cadastro com dados válidos                          | Nome: Game X, Ano: 2023, Preço: 199.90               | Jogo cadastrado com sucesso                 |
| CTJ002  | Preço zero, nulo ou negativo                        | Preço: 0.00 ou -1.00 ou NULL                         | Erro: preço inválido                        |
| CTJ003  | Ano lançamento maior que ano atual                 | Ano: 2026 (atual: 2025)                               | Erro: ano de lançamento superior ao ano atual  |
| CTJ004  | Ano lançamento menor que ano da data de criação    | Ano lançamento: 2024 (Data de Criação 01/07/2025)     | Erro: ano de lançamento inferior ao ano da data de criação  |

---

💸 Promoções

| ID      | Descrição                              | Entrada                                                        | Resultado Esperado                         |
|---------|----------------------------------------|----------------------------------------------------------------|---------------------------------------------|
| CTP001  | Promoção válida                        | Nome: Promoção Verão, Validade: 2025-08-01, Desconto: 20%     | Promoção cadastrada com sucesso             |
| CTP002  | Desconto fora da faixa obrigatórioa    | Desconto: Inferior a 10% ou superior a 90%                    | Erro: desconto fora do intervalo            |
| CTP003  | Nome da promoção duplicado             | Nome: Black Friday (já existente)                             | Erro: nome duplicado                        |
| CTP004  | Validade anterior à data atual         | Validade: 10/07/2025 ( atual: 01/07/2025)                      | Erro: validade inválida                     |

---

🛒 Pedidos

| ID       | Descrição                                   | Entrada                                                         | Resultado Esperado                        |
|----------|---------------------------------------------|------------------------------------------------------------------|--------------------------------------------|
| CTPD001  | Pedido com promoção válida                  | Jogo: Game X, Usuário: João, Promoção: Promoção Verão           | Pedido criado com desconto aplicado        |
| CTPD002  | Pedido com promoção vencida                 | Promoção expirada                                               | Erro: promoção inválida                    |
| CTPD003  | Pedido sem promoção                         | Jogo: Game X, Usuário: João                                     | Pedido criado com valor cheio              |
| CTPD004  | Pedido sem usuário                          | Usuário: null                                                   | Erro: usuário obrigatório                   |

---

👥 Controle de Acesso

| ID       | Descrição                                             | Entrada             | Resultado Esperado                        |
|----------|-------------------------------------------------------|---------------------|--------------------------------------------|
| CTAC001  | Acesso de usuário comum às funções permitidas         | Usuário nível "U"   | Acesso permitido às funcionalidades        |
| CTAC002  | Usuário comum tenta acessar r              | Usuário nível "U"   | Acesso negado                              |
| CTAC003  | Administrador acessa o sistema completo               | Usuário nível "A"   | Acesso total permitido                     |

---

🔐 Segurança

| ID      | Descrição                      | Entrada                                          | Resultado Esperado                         |
|---------|--------------------------------|--------------------------------------------------|---------------------------------------------|
| CTS001  | Login válido                   | E-mail: joao@email.com, Senha: Senha123!         | Login realizado com sucesso                 |
| CTS002  | Login com senha incorreta      | Senha inválida                                   | Erro: senha incorreta                       |
| CTS003  | E-mail inválido no login       | E-mail: joaoemail.com                            | Erro: e-mail inválido                       |

---

🗑️ Exclusões

| ID       | Descrição                                   | Pré-condição                                            | Resultado Esperado                         |
|----------|---------------------------------------------|---------------------------------------------------------|---------------------------------------------|
| CTE001   | Excluir jogo vinculado a pedido             | Jogo referenciado em pedido                             | Erro: exclusão não permitida                |
| CTE002   | Excluir promoção vinculada a pedido         | Promoção referenciada                                   | Erro: exclusão não permitida                |
| CTE003   | Excluir usuário com pedidos                 | Usuário possui pedidos                                  | Erro: exclusão não permitida                |
| CTE004   | Excluir item sem vínculos                   | Nenhuma referência existente                            | Exclusão permitida                          |

---








   

## 🎓 Informações Acadêmicas
- Curso: Pós-Tech em Arquitetura de Sistemas .NET  
- Instituição: FIAP  
- Aluno: Demetrio Pupolin  
- E-mail: pupolin@gmail.com  
- RM: 365898  
