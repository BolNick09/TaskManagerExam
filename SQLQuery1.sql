CREATE TABLE TaskObjects
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	TOName NVARCHAR(100) NOT NULL CHECK(TOName != ''),
	TOAddress NVARCHAR(100) NOT NULL CHECK(TOAddress != ''),
	TOCadastralNumber NVARCHAR (30) NOT NULL CHECK (TOCadastralNumber != '')
)
------------------------------

create table Users
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	UFullName NVARCHAR(100) NOT NULL CHECK(UFullName != ''),
	UPassWord NVARCHAR(100) NOT NULL CHECK(UPassWord != ''),
	UType SMALLINT NOT NULL
)
------------------------------

CREATE TABLE Decisions 
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	DDescription NVARCHAR(300) NOT NULL CHECK(DDescription != ''),
	DStartDate DATE DEFAULT CAST (GETDATE() AS DATE) NOT NULL,
	DEndDate DATE NOT NULL,
	DStatus  SMALLINT NOT NULL,
)
------------------------------

CREATE TABLE Tasks
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	TDescription NVARCHAR(300) NOT NULL CHECK(TDescription != ''),
	T_FK_User_ID INT NOT NULL,
	FOREIGN KEY (T_FK_User_ID) REFERENCES Users (Id),
	T_FK_Decision_Id INT NOT NULL,
	FOREIGN KEY (T_FK_Decision_Id) REFERENCES Decisions(Id)
)
-----------------------------
CREATE TABLE Objects_Tasks
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	OT_FK_TaskObjects_Id INT NOT NULL,
	FOREIGN KEY (OT_FK_TaskObjects_Id) REFERENCES TaskObjects (Id),
	OT_FK_Tasks_Id INT NOT NULL,
	FOREIGN KEY (OT_FK_Tasks_Id) REFERENCES Tasks(Id)
)



