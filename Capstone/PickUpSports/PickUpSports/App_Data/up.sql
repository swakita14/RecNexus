CREATE TABLE [dbo].[Contact] (
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
    [DayOfWeek]     TINYINT,
    [BeginTime]     DATETIME,
    [EndTime]       DATETIME,
    [ContactID]		INT	               NOT NULL,

    CONSTRAINT [PK_dbo.TimePreference] PRIMARY KEY CLUSTERED ([TimePrefID] ASC),
    CONSTRAINT [FK_dbo.Contact] FOREIGN KEY (ContactID) REFERENCES [dbo].[Contact] (ContactId) 

);