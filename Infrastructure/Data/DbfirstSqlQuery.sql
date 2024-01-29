DROP TABLE OrderRows
DROP TABLE Orders
DROP TABLE Products
DROP TABLE Manufacturers
DROP TABLE Categories

CREATE TABLE Categories
(
	Id int identity primary key,
	Name nvarchar(50) not null unique
)
CREATE TABLE Manufacturers
(
	Id int identity primary key,
	Name nvarchar(50) not null unique
)
CREATE TABLE Products
(
	Id int identity primary key,
	Title nvarchar(100) not null,
	Description nvarchar(max) not null,
	Price money not null,
	CategoryId int not null references Categories(Id),
	ManufacturerId int not null references Manufacturers(Id)
)
CREATE TABLE Orders
(
	Id int identity primary key,
	UserId integer not null
)
CREATE TABLE OrderRows
(
	OrderId int references Orders(Id),
	ProductId int references Products(Id),
	Amount int not null
	PRIMARY KEY (OrderId, ProductId)
)


-- Sql test kod för att se att allt harmonerar
INSERT INTO Categories VALUES('Test Category')
INSERT INTO Manufacturers VALUES('Test Manufacturers')
INSERT INTO Products VALUES('Test title1', 'Test Description1', 100, 1, 1)
INSERT INTO Products VALUES('Test title2', 'Test Description2', 1000, 1, 1)
INSERT INTO Orders VALUES(1)
INSERT INTO OrderRows VALUES(1, 1, 3)
INSERT INTO OrderRows VALUES(1, 2, 2)

SELECT 

    o.Id, o.UserId,
    p.Title, p.Price,
    r.Amount, r.ProductId,
    m.Name,
    c.Name

FROM OrderRows r
JOIN Orders o on o.Id = r.OrderId
JOIN Products p on p.Id = r.ProductId
JOIN Categories c ON c.Id = p.CategoryId
JOIN Manufacturers m on m.Id = p.ManufacturerId