
CREATE DATABASE confeitaria; 

use confeitaria;
CREATE TABLE agendamento (
	id int PRIMARY KEY not null,
    NomeCliente VARCHAR(30) not null,
    DataeHoraEntrega VARCHAR(20),
    Produto VARCHAR(20),
    Quantidade int,
	Valor VARCHAR (15));

SELECT * FROM agendamento;
