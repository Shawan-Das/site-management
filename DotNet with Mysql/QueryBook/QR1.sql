Create TABLE clients(
	id INT NOT	NULL PRIMARY KEY IDENTITY,
	name VARCHAR(100) NOT NULL,
	email VARCHAR(150) NOT NULL UNIQUE,
	phone VARCHAR(100) NULL,
	address VARCHAR(100) NULL,
	createdAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO clients(name, email,phone,address)
VALUES
('user1', 'user1@gmail.com', '+8801234', 'NY, USA'),
('user2', 'user2@gmail.com', '+8805678', 'DH, BDA'),
('user3', 'user3@gmail.com', '+8809101', 'CH, BDA')
