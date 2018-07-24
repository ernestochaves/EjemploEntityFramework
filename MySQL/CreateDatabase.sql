CREATE Database Demo;


Use Demo
CREATE TABLE Items 
(
	Id int NOT NULL Primary Key auto_increment, 
    Name VARCHAR(100),
	Description VARCHAR(200)	
);