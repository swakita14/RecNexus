﻿CREATE TABLE [dbo].[Contact] (
    [ContactId]            INT IDENTITY(1,1) NOT NULL,
	[Username]             NVARCHAR (128)  NULL,
	[FirstName]            NVARCHAR (128)  NULL,
	[LastName]             NVARCHAR (128)  NULL,
    [Email]                NVARCHAR (256)  NOT NULL,
    [PhoneNumber]          NVARCHAR (128)  NULL,
    [Address1]             NVARCHAR (256)  NULL,
	[Address2]             NVARCHAR (256)  NULL,
	[City]				   NVARCHAR (256)  NULL,
	[State]                NVARCHAR (256)  NULL,
	[ZipCode]              NVARCHAR (256)  NULL,
    CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

CREATE TABLE [dbo].[TimePreference](
    [TimePrefID]    INT IDENTITY(1,1) NOT NULL,
    [DayOfWeek]     INT,
    [BeginTime]     TIME,
    [EndTime]       TIME,
    [ContactID]		INT	               NOT NULL,

    CONSTRAINT [PK_dbo.TimePreference] PRIMARY KEY CLUSTERED ([TimePrefID] ASC),
    CONSTRAINT [FK_dbo.Contact] FOREIGN KEY (ContactID) REFERENCES [dbo].[Contact] (ContactId) 

);

CREATE TABLE Venue
(
	VenueID int IDENTITY(1,1) NOT NULL,
	Name nvarchar(max) NOT NULL,
	Phone nvarchar(50) NULL,
	Address1 nvarchar(50) NULL,
	Address2 nvarchar(50) NULL,
	City nvarchar(50) NULL,
	State nvarchar(50) NULL,
	ZipCode nvarchar(50) NULL,
	DateUpdated datetime null,
	GooglePlaceID nvarchar(50) NULL,

	CONSTRAINT PK_Venue PRIMARY KEY (VenueID),
);

CREATE TABLE BusinessHours
(
	BusinessHoursID int IDENTITY(1,1) NOT NULL,
	DayOfWeek int NOT NULL,
	OpenTime time(7) NOT NULL,
	CloseTime time(7) NOT NULL,
	VenueID int NOT NULL,

	CONSTRAINT PK_BusinessHours PRIMARY KEY (BusinessHoursID),
	CONSTRAINT FK_BusinessHours_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)
);

CREATE TABLE Review
(
	ReviewID int IDENTITY(1,1) NOT NULL,
	Timestamp datetime NOT NULL,
	Rating int NOT NULL,
	Comments nvarchar(max) null,
	IsGoogleReview bit NOT NULL,
	GoogleAuthor nvarchar(50) null,
	ContactID int NULL,
	VenueID int NOT NULL,

	CONSTRAINT PK_Review PRIMARY KEY (ReviewID),
	CONSTRAINT FK_Review_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID),
	CONSTRAINT FK_Review_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)
);