# Fiap Cloud Games – Sistema de Gestão de Jogos On Line 🎮

## 📌 Descrição
A Fiap Cloud Games permite o gerenciamento completo de usuários, jogos, pedidos e promoções. Desenvolvido em C# com SQL Server, ele faz parte do Tech Challenge da Fase 1 da Pós-Tech em Arquitetura de Sistemas .NET

# 👤 Níveis de Acesso e Funcionalidades

### 🧑 Usuário Comum
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
- Nome
- Email
- Senha
- Data de Criação

### Jogo
- ID_Jogo
- Nome
- Ano de Lançamento
- Preço Base
- Data de Criação

### Pedido
- ID_Pedido
- ID_Usuario
- ID_Jogo
- ID_Promocao
- Data de Criação

### Promoção
- ID_Promocao
- Nome
- Percentual
- Data de Validade
- Data de Criação

## 📜 Regras Gerais do Sistema

Este sistema de loja de games segue as seguintes regras e restrições de funcionamento:

1. 🎮 **Cadastro de Jogos**
   - Todo jogo deve possuir nome, descrição, preço, desenvolvedor e data de lançamento.
   - Jogos não podem ser cadastrados com preços negativos ou datas futuras.

2. 🛒 **Pedidos**
   - Um pedido só pode ser realizado por usuários autenticados.
   - Cada pedido deve conter pelo menos um jogo.
   - O valor total do pedido é calculado com base nos preços dos jogos aplicando (se houver) uma promoção válida.

3. 💸 **Promoções**
   - Apenas administradores podem criar e aplicar promoções.
   - Promoções têm data de início e término.
   - Promoções expiradas não são aplicadas automaticamente.

4. 👥 **Controle de Acesso**
   - Usuários comuns podem visualizar jogos e seus próprios pedidos.
   - Administradores têm acesso completo ao sistema (cadastro de jogos, promoções, usuários, pedidos).

5. 🔐 **Segurança**
   - As senhas dos usuários são armazenadas de forma segura (criptografadas).
   - O sistema impede acesso a rotas administrativas por usuários não autorizados.

6. 🗑️ **Exclusões**
   - Ao excluir um jogo, ele não é removido de pedidos anteriores, apenas fica indisponível para novos pedidos.
   - Promoções não podem ser excluídas se estiverem associadas a pedidos já realizados.




## 🎓 Informações Acadêmicas
- Curso: Pós-Tech em Arquitetura de Sistemas .NET
- Instituição: FIAP
- Aluno: Demetrio Pupolin
- RM: 365898

