# Fiap Cloud Games – Sistema de Gestão de Jogos On Line 🎮

## 📌 Descrição
A Fiap Cloud Games permite o gerenciamento completo de usuários, jogos, pedidos e promoções. Desenvolvido em C# com SQL Server, ele faz parte do Tech Challenge da Fase 1 da Pós-Tech em Arquitetura de Sistemas .NET

# 👤 Níveis de Acesso e Funcionalidades

### 🧑 Usuário
- ✅ Pode criar uma conta e fazer login.
- ✅ Pode visualizar os jogos disponíveis.
- ✅ Pode fazer pedidos com promoção valida ou sem promoção.
- ✅ Pode visualizar seus próprios pedidos.

### 👨‍💼 Administrador
- ✅ Pode cadastrar, consultar, editar e excluir todos os usuários.
- ✅ Pode cadastrar, consultar, editar e excluir todos os jogos.
- ✅ Pode cadastrar, consultar, editar e excluir todas as promoções.
- ✅ Pode cadastrar, consultar, editar e excluir todos os pedidos.

## 🏗️ Tecnologias
- C#
- SQL Server

## ⚙️ Como Rodar
1. Clone este repositório.
2. Em /DB execute o script.sql em um banco de dados SqlServer para criação de tabelas.
3. Abra o Projeto no Visual Studio.
4. Configure a conexão com banco de dados em `appsetings.json`
5. Rode o projeto.

## 🗃️ Estrutura de Dados

### Usuário
- ID_Usuario
- Data de Criação
- Nome
- Email
- Senha

### Jogo
- ID_Jogo
- Data de Criação
- Nome
- Ano de Lançamento
- Preço Base
  
### Pedido
- ID_Pedido
- Data de Criação
- ID_Usuario
- ID_Jogo
- ID_Promocao
- Valor do Pedido

### Promoção
- ID_Promocao
- Data de Criação
- Nome
- Percentual de Desconto
- Data de Validade

## 📜 Regras Gerais do Sistema

O sistema segue as seguintes regras e restrições de funcionamento:

1. 🎮 **Cadastro de Usuario**
   - Todo usuario deve possuir Nome, E-mail e Senha de Acesso.
   - Não deverá conter usuários com e-mail repetido.   

1. 🎮 **Cadastro de Jogos**
   - Todo jogo deve possuir nome, descrição, ano de lançamento e preço base.
   - Jogos não podem ser cadastrados com preços negativos ou zerados.
   - O ano de lançamento do jogo não poderá ser superior a sua data de criação.

2. 🛒 **Pedidos**
   - Cada pedido está vinculado a um único jogo.
   - Todo pedido deve conter obrigatoriamente um usuário e o jogo adquirido.
   - Pode haver uma promoção (cupom de desconto) associada ao pedido, desde que sua data de validade atenda a data de criação do pedido.
   - O valor total do pedido é calculado com base no preço do jogo, aplicando o desconto da promoção, se houver.

3. 💸 **Promoções**
   - A promoção deverá conter obrigatoriamente um nome, data de validade e percentual de desconto.
   - O percentual de desconto deverá ser em numéro inteiro de 10% a 90% de desconto.
   - A promoção deverá ter um nome único entre todas as promoções existentes.
   - A data de validade da promoção deverá ser ao menos a data de inclusão da promoção.

4. 👥 **Controle de Acesso**
   - Usuários comuns podem criar usuário, fazer login, consultar jogos, realizar pedidos e visualizar seus próprios pedidos sendo seu nivel como "U"-Usuário.
   - Administradores têm acesso completo ao sistema sendo nível como "A"-Administrador. 

5. 🔐 **Segurança**   
   - O e-mail do usuário informado deverá ser bem formado: usuario@domino.xxx 
   - A senha do usuário deverá conter obrigatoriamente 8 caracteres contendo números, letras e caracteres especiais.
   - O login deverá ser realizado através de e-mail do usuário e sua respectiva senha.   

7. 🗑️ **Exclusões**
   - Toda solicitação de exclusão deve verificar se o item não está sendo referenciado por outras entidades no sistema. Conforme a seguir:
   - Não é permitido excluir um jogo que esteja vinculado a algum pedido.
   - Não é permitido excluir uma promoção que tenha sido aplicada em algum pedido.
   - Não é permitido excluir um usuário que esteja associado a um pedido já registrado.

## 🎓 Informações Acadêmicas
- Curso: Pós-Tech em Arquitetura de Sistemas .NET
- Instituição: FIAP
- Aluno: Demetrio Pupolin
- RM: 365898
