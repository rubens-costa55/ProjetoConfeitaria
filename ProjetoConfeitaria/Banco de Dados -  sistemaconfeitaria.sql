CREATE DATABASE sistemaconfeitaria;
USE sistemaconfeitaria;

-- ========================
-- TABELA: login
-- ========================
CREATE TABLE login (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  cpf VARCHAR(20) UNIQUE,
  senha VARCHAR(100) NOT NULL
);

INSERT INTO login (id_usuario, cpf, senha) VALUES
(1, '123456789', '1234');

-- ========================
-- TABELA: categorias
-- ========================
CREATE TABLE categorias (
  id_categoria INT AUTO_INCREMENT PRIMARY KEY,
  nome_categoria VARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO categorias (id_categoria, nome_categoria) VALUES
(1,'Bento Cakes'),
(2,'Bolos (Sabores Especiais)'),
(3,'Bolos (Sabores)'),
(4,'Docinhos de Festa'),
(5,'Itens Individuais e Personalizados'),
(6,'Kits Festa Individuais');

-- ========================
-- TABELA: produtos
-- ========================
CREATE TABLE produtos (
  id_produto INT AUTO_INCREMENT PRIMARY KEY,
  NomeProduto VARCHAR(100) NOT NULL,
  PrecoProduto DECIMAL(10,2) NOT NULL,
  id_categoria INT NOT NULL,
  FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria)
);

-- ========================
-- TABELA: pedidos
-- ========================
CREATE TABLE pedidos (
  id_pedido INT AUTO_INCREMENT PRIMARY KEY,
  NomeCliente VARCHAR(100) NOT NULL,
  TelefoneCliente VARCHAR(20) NOT NULL,
  DataHoraEntrega VARCHAR(50) NOT NULL,
  ValorTotal DECIMAL(10,2) NOT NULL,
  Status VARCHAR(30)
);

-- ========================
-- TABELA: itens_pedido
-- ========================
CREATE TABLE itens_pedido (
  id_item INT AUTO_INCREMENT PRIMARY KEY,
  id_pedido INT NOT NULL,
  id_produto INT NOT NULL,
  nome_produto VARCHAR(100) NOT NULL,
  Quantidade INT NOT NULL,
  ValorUnitario DECIMAL(10,2) NOT NULL,
  ValorItem DECIMAL(10,2) NOT NULL,
  FOREIGN KEY (id_pedido) REFERENCES pedidos(id_pedido),
  FOREIGN KEY (id_produto) REFERENCES produtos(id_produto)
);

-- ========================
-- TABELA: historico
-- ========================
CREATE TABLE historico (
  id_historico INT AUTO_INCREMENT PRIMARY KEY,
  NomeCliente VARCHAR(100) NOT NULL,
  TelefoneCliente VARCHAR(20) NOT NULL,
  DataHoraEntrega VARCHAR (50) NOT NULL,
  Produto VARCHAR(100) NOT NULL,
  Quantidade INT NOT NULL,
  Valor DECIMAL(10,2) NOT NULL
);


-- Categoria 1: Bento Cakes
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Bento Cake Chocolate', 45.00, 1),
('Bento Cake Morango', 47.00, 1);

-- Categoria 2: Bolos (Sabores Especiais)
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Bolo Red Velvet', 120.00, 2),
('Bolo Ninho com Nutella', 135.00, 2);

-- Categoria 3: Bolos (Sabores)
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Bolo de Chocolate', 95.00, 3),
('Bolo de Cenoura com Chocolate', 90.00, 3);

-- Categoria 4: Docinhos de Festa
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Brigadeiro Tradicional', 2.50, 4),
('Beijinho de Coco', 2.50, 4);

-- Categoria 5: Itens Individuais e Personalizados
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Cupcake Personalizado', 8.00, 5),
('Pirulito de Chocolate Personalizado', 6.50, 5);

-- Categoria 6: Kits Festa Individuais
INSERT INTO produtos (NomeProduto, PrecoProduto, id_categoria) VALUES
('Kit Festa Simples', 150.00, 6),
('Kit Festa Premium', 220.00, 6);


