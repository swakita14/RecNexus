CREATE TABLE [dbo].[Contacts] (
    [ContactId]                   NVARCHAR (128) NOT NULL,
	[Username]                   NVARCHAR (128) NOT NULL,
	[FirstName]                   NVARCHAR (128) NOT NULL,
	[LastName]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NOT NULL,
    [PhoneNumber]          NVARCHAR (128) NOT NULL,
    [Address1]             NVARCHAR (256) NOT NULL,
	[Address2]             NVARCHAR (256) NOT NULL,
	[City]             NVARCHAR (256) NOT NULL,
	[State]             NVARCHAR (256) NOT NULL,
	[ZipCode]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);