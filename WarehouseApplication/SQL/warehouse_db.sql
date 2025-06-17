CREATE TABLE Contractors (
    Id SERIAL PRIMARY KEY,
    Symbol VARCHAR(50) NOT NULL,
    Name VARCHAR(255) NOT NULL
);

CREATE TABLE Documents (
    Id SERIAL PRIMARY KEY,
    Date DATE NOT NULL,
    Symbol VARCHAR(100) NOT NULL,
    ContractorId INT NOT NULL,
    CONSTRAINT fk_contractor FOREIGN KEY (ContractorId)
        REFERENCES Contractors (Id)
        ON DELETE CASCADE
);

CREATE TABLE Document_Items (
    Id SERIAL PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Unit VARCHAR(50) NOT NULL,
    Quantity NUMERIC(12, 2) NOT NULL,
    DocumentId INT NOT NULL,
    CONSTRAINT fk_document FOREIGN KEY (DocumentId)
        REFERENCES Documents (Id)
        ON DELETE CASCADE
);


INSERT INTO Contractors (Symbol, Name) VALUES
  ('KON001', 'Firma Alpha Sp. z o.o.'),
  ('KON002', 'Hurtownia Beta SA');

INSERT INTO Documents (Date, Symbol, ContractorId) VALUES
  ('2024-06-01', 'PZ/1/2024', 1),
  ('2024-06-02', 'PZ/2/2024', 2);

INSERT INTO DocumentItems (ProductName, Unit, Quantity, DocumentId) VALUES
  ('Monitor 24"', 'szt', 10.00, 1),
  ('Klawiatura', 'szt', 25.00, 1),
  ('Dysk SSD 1TB', 'szt', 15.00, 2),
  ('Mysz optyczna', 'szt', 40.00, 2);