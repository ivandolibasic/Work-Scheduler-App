create database WorkScheduler;

create table AccessLevel(
Id UNIQUEIDENTIFIER primary key,
AccessLevel varchar(10) not null
);
CREATE TABLE Account
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Username VARCHAR(20) not null unique,
    Password VARCHAR(30) not null,
    AccessLevelId UNIQUEIDENTIFIER not null,
    DateCreated datetime not null,
    DateUpdated datetime,
    CreatedByUser UNIQUEIDENTIFIER,
    UpdatedByUser UNIQUEIDENTIFIER,
    FOREIGN KEY (AccessLevelId) REFERENCES AccessLevel(Id),
    FOREIGN KEY (CreatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (UpdatedByUser) REFERENCES Account(Id)
);
CREATE TABLE WorkPosition
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	PositionName varchar(70) not null,
	Description VARCHAR(200),
	DateCreated datetime not null,
	DateUpdated datetime,
	CreatedByUser UNIQUEIDENTIFIER,
	UpdatedByUser UNIQUEIDENTIFIER,
	FOREIGN KEY (CreatedByUser) REFERENCES Account(Id),
	FOREIGN KEY (UpdatedByUser) REFERENCES Account(Id)
);
CREATE TABLE Worker
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName VARCHAR(20),
    LastName VARCHAR(25),
    PositionId UNIQUEIDENTIFIER not null,
    DateCreated datetime not null,
    DateUpdated datetime,
    CreatedByUser UNIQUEIDENTIFIER,
    UpdatedByUser UNIQUEIDENTIFIER,
    FOREIGN KEY (CreatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (UpdatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (Id) REFERENCES Account(Id),
    FOREIGN KEY (PositionId) REFERENCES WorkPosition(Id)
);
CREATE TABLE WorkerStatus
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Status varchar(20) not  null
);
CREATE TABLE Request
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    RequestStatus VARCHAR(8),
    StartDate DATETIME not null,
    EndDate DATETIME not null,
    Description varchar(200),
    WorkerStatusId UNIQUEIDENTIFIER not null,
    DateCreated datetime not null,
    DateUpdated datetime,
    CreatedByUser UNIQUEIDENTIFIER,
    UpdatedByUser UNIQUEIDENTIFIER,
    FOREIGN KEY (CreatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (UpdatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (WorkerStatusId) REFERENCES WorkerStatus(Id),
    CONSTRAINT CHK_RequestStatus CHECK(RequestStatus = 'Pending' OR RequestStatus = 'Denied' OR RequestStatus = 'Accepted')
);

CREATE TABLE WorkerAvailability
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    WorkerId UNIQUEIDENTIFIER not null,
    RequestId UNIQUEIDENTIFIER null,
    StartDate datetime not null,
    EndDate datetime not null,
    FOREIGN KEY (RequestId) REFERENCES Request(Id) ON DELETE CASCADE,
    FOREIGN KEY (WorkerId) REFERENCES Worker(Id) ON DELETE CASCADE
);

Create table TaskStatus
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Status varchar(15)
);
Create table Task
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Description varchar(150),
    TotalHoursNeeded int,
    TaskStatusId UniqueIdentifier not null,
    DateCreated datetime not null,
    DateUpdated datetime,
    CreatedByUser UNIQUEIDENTIFIER,
    UpdatedByUser UNIQUEIDENTIFIER,
    FOREIGN KEY (CreatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (UpdatedByUser) REFERENCES Account(Id),
    FOREIGN KEY (TaskStatusId) REFERENCES TaskStatus(Id)
);
Create table ScheduleTaskWorker(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TaskId UNIQUEIDENTIFIER,
    WorkerAvailabilityId UNIQUEIDENTIFIER,
    StartDateTime datetime not null,
    EndDateTime datetime not null,
    TaskDuration int not null,
    FOREIGN KEY (TaskId) REFERENCES Task(Id),
    FOREIGN KEY (WorkerAvailabilityId) REFERENCES WorkerAvailability(Id)
);

INSERT INTO AccessLevel VALUES (NEWID(), 'SuperAdmin');
INSERT INTO AccessLevel VALUES (NEWID(), 'Admin');
INSERT INTO AccessLevel VALUES (NEWID(), 'User');

INSERT INTO TaskStatus VALUES (NEWID(), 'InProgress');
INSERT INTO TaskStatus VALUES (NEWID(), 'Completed');

INSERT INTO WorkerStatus VALUES (NEWID(), 'SickDays');
INSERT INTO WorkerStatus VALUES (NEWID(), 'Holidays');
INSERT INTO WorkerStatus VALUES (NEWID(), 'PaidLeave');
INSERT INTO WorkerStatus VALUES (NEWID(), 'Available');
INSERT INTO WorkerStatus VALUES (NEWID(), 'UnpaidLeave');
INSERT INTO WorkerStatus VALUES (NEWID(), 'OnTask');




