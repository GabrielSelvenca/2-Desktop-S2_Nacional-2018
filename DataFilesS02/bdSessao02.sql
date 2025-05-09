

use master
go
create database dbTarefas
go
use dbTarefas
go
create table Usuario(
	Codigo int identity(1,1) primary key,
	Nome varchar(80) not null,
	Email varchar(80) not null,
	Usuario varchar(20) not null,
	Senha varchar(30)  null,
	Telefone varchar(20) not null,
	Foto varbinary(max) not null,
)

CREATE TABLE Projeto(
	Codigo int identity(1,1) primary key,
	Nome varchar(60) not null,
	CodUsuario int not null,
	NaoPertube bit not null DEFAULT(0),
	foreign key (CodUsuario) references Usuario(Codigo)
)

CREATE TABLE Projeto_Tarefas(
	Codigo int identity(1,1) primary key,
	CodProjeto int not null,
	Descricao varchar(300) not null,
	isConcluida bit,	
	foreign key (CodProjeto) references Projeto(Codigo)
)

CREATE TABLE Notificacoes(
	Codigo int identity(1,1) primary key,
	Texto varchar(300) not null,
	Data datetime not null,
	CodProjeto int,
	CodUsuario int not null,
	isFechada bit,
	foreign key (CodUsuario) references Usuario(Codigo),
	foreign key (CodProjeto) references Projeto(Codigo)
)

CREATE TABLE Projeto_Membros(	
	CodMembro int not null,
	CodProjeto int not null ,	
	foreign key (CodMembro) references Usuario(Codigo),
	foreign key (CodProjeto) references Projeto(Codigo),
	Constraint PK_ProjetoMembro Primary Key (CodMembro,CodProjeto)
)



CREATE TABLE Items_Favoritos(	
	CodUsuario int not null ,
	CodTarefa int not null ,
	foreign key (CodUsuario) references Usuario(Codigo),
	foreign key (CodTarefa) references Projeto_Tarefas(Codigo),
	Constraint PK_ItemFavorito Primary Key (CodUsuario,CodTarefa)
)