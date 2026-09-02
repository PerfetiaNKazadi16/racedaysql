create database raceDay;
 

DROP TABLE IF EXISTS entry;
DROP TABLE IF EXISTS Result;
DROP TABLE IF EXISTS route;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Events;
DROP TABLE IF EXISTS Participant;
DROP TABLE IF EXISTS Organiser;
DROP TABLE IF EXISTS Users;
 

create table Users (
usersID int IDENTITY(1,1), 
Email varchar(50)unique,
userName varchar(50),
role varchar(50),
AccountType varchar(20)not null,

Constraint PK_Users_usersID PRIMARY KEY(usersID),
Constraint UQ_Users_userName unique(userName),
constraint UQ_Users_Email unique(Email)
 
);

create table Organiser(
organiserID int,
organiserNumber int not null,
usersID int NOT NULL,
 

constraint PK_Organiser_organiserID PRIMARY KEY(organiserID),
constraint UK_Organiser_usersID UNIQUE(usersID),
constraint FK_Organiser_Users foreign key(usersID) references Users(usersID)
ON DELETE NO ACTION

);

create table Participant(
participantID int ,
participantName varchar(50)not null,
usersID int NOT NULL,
 
Constraint PK_Participant_participantID PRIMARY KEY(participantID),
constraint UK_Participant_usersID UNIQUE(usersID),
constraint FK_Participant_Users foreign key(usersID) references Users(usersID)
ON DELETE NO ACTION


);

create table Events(
eventID int,  
eventName varchar(50)not null,
eventDate DATE NOT NULL,
description VARCHAR(255),
location varchar(100)not null,
organiserID int not null,
 
constraint PK_Events_eventID PRIMARY KEY(eventID),
constraint FK_Events_Organiser foreign key(organiserID) references Organiser(organiserID)
ON DELETE NO ACTION

);

create table Category(
categoryID int,
categoryName varchar(50)not null,
eventID int not null,
 
 constraint PK_Category_categoryID PRIMARY KEY(categoryID)
);

CREATE TABLE Result(
 resultID INT NOT NULL,
 participantID INT NOT NULL,
 eventID INT NOT NULL,
 finishingTime TIME NOT NULL,
 position INT NOT NULL,
    CONSTRAINT PK_Result_resultID 
        PRIMARY KEY(resultID),
    CONSTRAINT FK_Result_Participant
        FOREIGN KEY(participantID)
        REFERENCES Participant(participantID),
    CONSTRAINT FK_Result_Event
        FOREIGN KEY(eventID)
        REFERENCES Events(eventID)
);
create table route(
RouteID INT IDENTITY(1,1),
EventID int not null,
RouteName varchar (100)not null,
distance decimal(6,2) not null,

 Constraint PK_route_RouteID PRIMARY KEY(RouteID),
 Constraint FK_Events_eventID foreign key(eventID) references Events(eventID)
 ON DELETE NO ACTION

);

CREATE TABLE entry(
    EntryID INT IDENTITY(1,1) NOT NULL,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    PRICE DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_Entry_EntryID 
        PRIMARY KEY(EntryID),

    CONSTRAINT UK_Entry_Participant_Events 
        UNIQUE(EventID, ParticipantID),

    CONSTRAINT FK_Entry_Participant
        FOREIGN KEY(ParticipantID)
        REFERENCES Participant(ParticipantID)
        ON DELETE NO ACTION,

    CONSTRAINT FK_Entry_Event
        FOREIGN KEY(EventID)
        REFERENCES Events(EventID)
        ON DELETE NO ACTION
);

 

  

INSERT INTO Users (Email, userName, role, AccountType)
VALUES
('alice@example.com', 'AliceRunner', 'Participant', 'Standard'),
('bob@example.com', 'BobRunner', 'Participant', 'Standard'),
('carol@example.com', 'CarolRunner', 'Participant', 'Premium'),
('david@example.com', 'DavidEvents', 'Organiser', 'Admin'),
('emma@example.com', 'EmmaEvents', 'Organiser', 'Admin'),
('frank@example.com', 'FrankRunner', 'Participant', 'Standard'),
('grace@example.com', 'GraceRunner', 'Participant', 'Premium'),
('henry@example.com', 'HenryEvents', 'Organiser', 'Admin'),
('isabel@example.com', 'IsabelRunner', 'Participant', 'Standard'),
('james@example.com', 'JamesRunner', 'Participant', 'Standard');


 

INSERT INTO Organiser (organiserID, organiserNumber, usersID)
VALUES
(1, 1001, 4),
(2, 1002, 5),
(3, 1003, 8);


 
INSERT INTO Participant (participantID, participantName, usersID)
VALUES
(1, 'Alice Johnson', 1),
(2, 'Bob Smith', 2),
(3, 'Carol Williams', 3),
(4, 'Frank Brown', 6),
(5, 'Grace Wilson', 7),
(6, 'Isabel Davis', 9),
(7, 'James Miller', 10);


 

INSERT INTO Events
(eventID, eventName, eventDate, description, location, organiserID)
VALUES
(1, 'Spring City Run', '2026-09-20',
 'A community road running event for all fitness levels.',
 'Central Park', 1),
(2, 'Summer Beach Race', '2026-10-11',
 'A beach running event featuring multiple race distances.',
 'Brighton Beach', 1),
(3, 'Autumn Mountain Challenge', '2026-10-25',
 'A challenging trail running event through the Highlands.',
 'Highlands', 2),
(4, 'Winter Charity Run', '2026-06-14',
 'A charity running event supporting local community projects.',
 'City Stadium', 3),
(5, 'Cape Fitness Race', '2026-11-08',
 'A fitness-focused road race for recreational and competitive runners.',
 'Green Point', 2),
(6, 'New Year Road Race', '2027-01-10',
 'A new year road race welcoming runners of different abilities.',
 'Main Street', 1);

 
INSERT INTO Category (categoryID, categoryName, eventID)
VALUES
-- Spring City Run
(1, '5K Fun Run', 1),
(2, '10K Race', 1),
(3, '21K Half Marathon', 1),

-- Summer Beach Race
(4, '5K Beach Run', 2),
(5, '10K Beach Race', 2),
(6, 'Half Marathon', 2),

-- Autumn Mountain Challenge
(7, '5K Mountain Trail', 3),
(8, '10K Mountain Trail', 3),
(9, '15K Mountain Challenge', 3),

-- Winter Charity Run
(10, '5K Charity Run', 4),
(11, '10K Charity Run', 4),
(12, '21K Charity Run', 4),

-- Cape Fitness Race
(13, '5K Fitness Run', 5),
(14, '10K Fitness Race', 5),
(15, '15K Fitness Race', 5),

-- New Year Road Race
(16, '5K Road Race', 6),
(17, '10K Road Race', 6),
(18, '21K New Year Race', 6);

 

INSERT INTO route (EventID, RouteName, distance)
VALUES
(1, 'Central Park 5K Route', 5.00),
(1, 'Central Park 10K Route', 10.00),
(2, 'Brighton Beach Half Marathon Route', 21.10),
(3, 'Highlands Mountain Trail', 15.50),
(4, 'City Stadium Charity Route', 10.00),
(5, 'Green Point Fitness Route', 15.00),
(6, 'Main Street Road Race Route', 5.00);


 
INSERT INTO entry (ParticipantID, EventID, PRICE)
VALUES
(1, 1, 150.00),
(2, 1, 150.00),
(3, 1, 150.00),
(4, 1, 150.00),
(1, 2, 250.00),
(2, 2, 250.00),
(3, 2, 250.00),
(5, 2, 250.00);
 
INSERT INTO Result
(resultID, participantID, eventID, finishingTime, position)
VALUES
(1, 1, 1, '00:28:35', 1),
(2, 2, 1, '00:31:42', 2),
(3, 3, 1, '00:34:18', 3),
(4, 1, 2, '01:52:20', 1),
(5, 2, 2, '02:01:45', 2),
(6, 3, 2, '02:08:30', 3);


 
SELECT * FROM Users;
SELECT * FROM Organiser;
SELECT * FROM Participant;
SELECT * FROM Events;
SELECT * FROM Category;
SELECT * FROM Result;
SELECT * FROM entry;
SELECT * FROM route;

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';