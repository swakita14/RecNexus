CREATE TABLE [dbo].[Discussions] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [VoteCount]	    BIGINT             NOT NULL,
    [UpvoteCount]	BIGINT             NOT NULL,
	[DownvoteCount]	BIGINT             NOT NULL,
    [CommentCount]	BIGINT             NOT NULL,
    [TotalViews]	BIGINT             NOT NULL,
	[Rank]		    INT	               NOT NULL,
    [UserID]		INT	               NOT NULL,
    [LINK]		    NVARCHAR (128)	   NULL,
    [Title]		    NVARCHAR (128)	   NOT NULL,

    CONSTRAINT [PK_dbo.Discussions] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.Users] FOREIGN KEY (UserID) REFERENCES [dbo].[Users] (UserID) 
		ON DELETE CASCADE ON UPDATE CASCADE,
	
);



INSERT INTO [dbo].[Discussions](VoteCount, UpvoteCount, DownvoteCount,CommentCount,TotalViews,Rank,UserID,LINK,Title) VALUES
    (345, 200, 145, 3, 400, 4, 2, 'https://www.ancient-origins.net/history-famous-people/niall-nine-hostages-0011410', 'Niall of the Nine Hostages, One of the Most Fruitful Kings in History'),
    (200, 152, 48, 5, 265, 5, 9, 'https://www.alaskapublic.org/2019/01/30/study-finds-poor-air-quality-aboard-cruise-ships/', 'Study finds poor air quality aboard cruise ships'), 
    (1000, 990, 10, 45, 2000, 2, 5, 'https://www.sciencemag.org/news/2019/01/tourism-endangering-these-giant-lizards', 'Is tourism endangering these giant lizards?'), 
    (450, 150, 300, 10, 500, 6, 7, 'https://www.history.com/news/knights-middle-ages', 'Eight Knights Who Changed History'), 
    (76, 23, 54, 5, 205, 9, 11, 'https://www.kdlt.com/2019/01/30/local-animal-sanctuary-helping-horses-stay-warm/', 'Local Animal Sanctuary Helping Horses Stay Warm'), 
    (100, 50, 50, 12, 154, 8, 6, 'https://www.express.co.uk/news/royal/1080615/the-queen-news-elizabeth-ii-prince-philip-buckingham-palace-finance-net-worth-spt', 'Royal SCANDAL: How the Queen almost LOST Buckingham Palace because of :MONEY PROBLEMS'),
    (5017, 5000, 17, 357, 6078, 1, 8, 'https://www.timeslive.co.za/motoring/news/2019-01-31-at-last-bash-proof-and-pinch-proof-car-doors/' , 'At last: bash-proof and pinch-proof car doors'), 
    (296, 266, 30, 24, 709, 3, 12, 'https://www.dezeen.com/2019/01/23/marc-newson-gagosian-gallery-exhibition-new-york/', 'Marc Newson exhibition in New York features swords and surfboards'), 
    (400, 100, 300, 35, 594, 7, 4, 'https://www.foxnews.com/travel/disney-world-uses-secret-shade-paint-park-elements-hide', 'Disney World reportedly uses a secret shade of paint to camouflage less than exciting park elements'), 
    (10, 1, 9, 2, 58, 10, 13, 'https://www.kmov.com/news/it-s-about-loyalty-st-louis-group-rooting-for-rams/article_933823f6-24e7-11e9-9caa-d757bd32399b.html', 'Its about loyalty: St. Louis group rooting for Rams in Super Bowl'); 
    





