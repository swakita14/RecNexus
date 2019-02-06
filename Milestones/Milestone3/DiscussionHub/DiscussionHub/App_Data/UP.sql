CREATE TABLE [dbo].DiscussionHubUser	
(
    [UserID]        INT IDENTITY(1,1) NOT NULL,
    [Browser]        NVARCHAR(128) NOT NULL,
    [FName]            NVARCHAR(64),
    [LName]            NVARCHAR(128),
    [Email]            NVARCHAR(64),
    [LoginPref]        NVARCHAR(128),
    [VoteTotal]        BIGINT,
    [About]            TEXT,
    [Pseudonym]        NVARCHAR(128),

    CONSTRAINT [PK_dbo.Requests] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

INSERT INTO [dbo].DiscussionHubUser(Browser,FName,LName,Email,LoginPref,VoteTotal,About,Pseudonym) VALUES
    ('Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246','Tom','Jones','tjones@yahoo.com','jonest13', 43, 'Love surfing, and particularly interested in politics. Here for heated discussions', 'Thomas Smith'),
    ('Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36','Jon', 'Snow', 'snowj@gmail.com','snowj', 435, 'Enjoys traveling north, interested in animals', 'Jonathan Outright'),
    ('Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit/601.3.9 (KHTML, like Gecko) Version/9.0.2 Safari/601.3.9', 'Tyrion', 'Lannister', 'lanistersmall@yahoo.com', 'tlansmall', 325, 'very fond of geography and marine biology', 'William Spencer'),
    ('Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36', 'Sansa', 'Stark', 'truequeen@msn.com', 'sstark', 65, 'currently learning about cryptography, 23, Recent grad @ OSU', 'Peter Reichenburg'),
    ('Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1', 'Daenerys','Targaryen', 'dragonqueen@yahoo.com', 'Dgaryen', 6001, 'very fond of reptiles, including dinasours and dragons, 25', 'Denny Target'),
    ('Roku4640X/DVP-7.70 (297.70E04154A)', 'Cersei', 'Lannister', 'queenofeverything@msn.com', 'LannisterC', 89, 'interested to meet others to talk about ancient history, mother of 2', 'Caesar Looney'),
    ('Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36', 'Jaime', 'Lannister', 'jlann14@wou.edu', 'jammieLan', 954, 'Currently studying/working on automobile engines, living in Wyoming', 'James Lanes'),
    ('Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36', 'Hordor', 'Door', 'holdthedoor@gmail.com', 'holdthedoor', 286, 'Interested in interior design and architecture, strong man', 'Hoovert Dawn'),
    ('Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36', 'Theon', 'Greyjoy', 'tgreyjoy@yahoo.com', 'notgreyjoy', 67, 'Sea is what captivates me, hold roots in the coast, 43', 'Thanos G'),
    ('Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36', 'Eddard', 'Start', 'nedstark@gmail.com', 'eddstart', 543, 'Junior at SOU, Major in Chemical Engineer and Minor in Visual Arts, Vancouver,BC', 'Nathan Sales'),
    ('Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246', 'Khal', 'Drogo', 'roughriders@yahoo.com', 'horseback', 82, 'amateur athlete for the USA triathlon team, likes to lift', 'Carl Dragic'),
    ('Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36', 'Brienne', 'Tarth', 'boftarth@gmail.com', 'tarthB', 57, 'Entrepenur, mother of 4, come visit my website at www.squarespace.com', 'Brianna Thames'),
    ('Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1', 'Grey', 'Worm', 'theunsullied@gmail.com', 'Gormm', 701, 'Currently employee at Java Crew, gathering information here to use for my thesis', 'Gretchen W'),
    ('Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus Player Build/MMB29T)','Margery', 'Tyrell', 'restlessqueen@yahoo.com', 'TMargery', 78, '26, born and raised in the meadows in Idaho, Senior @WOU', 'Tyler Marge'),
    ('Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36','Tormund', 'Guard', 'leadbyexample@gmail.com', 'torguard', 592, '6ft5, like reading articles on sports and MMA news', 'Tammond Gregory');
	
CREATE TABLE [dbo].[Discussion] (
    [DiscussionID]  INT            IDENTITY (1, 1) NOT NULL,
	[PostTime]		DATETIME		   NOT NULL,
    [VoteCount]	    BIGINT             NOT NULL,
    [UpvoteCount]	BIGINT             NOT NULL,
	[DownvoteCount]	BIGINT             NOT NULL,
    [CommentCount]	BIGINT             NOT NULL,
    [TotalViews]	BIGINT             NOT NULL,
	[Rank]		    INT	               NOT NULL,
    [ArticleLink]   NVARCHAR(MAX)	   NOT NULL,
    [Title]		    NVARCHAR(MAX)	   NOT NULL,
	[Contents]		NVARCHAR(MAX)	   NULL,
	[UserID]		INT	               NOT NULL

    CONSTRAINT [PK_dbo.Discussion] PRIMARY KEY CLUSTERED ([DiscussionID] ASC),
    CONSTRAINT [FK_dbo.DiscussionHubUser] FOREIGN KEY (UserID) REFERENCES [dbo].[DiscussionHubUser] (UserID) 
		ON DELETE CASCADE ON UPDATE CASCADE,
);



INSERT INTO [dbo].[Discussion](VoteCount, UpvoteCount, DownvoteCount,CommentCount,TotalViews,Rank,ArticleLink,Title, Contents, UserID) VALUES
    ('12/10/2018 12:00:00 AM', 345, 200, 145, 3, 400, 4, 'https://www.ancient-origins.net/history-famous-people/niall-nine-hostages-0011410', 'Niall of the Nine Hostages, One of the Most Fruitful Kings in History', 'Test article', 2),
    ('5/10/2018 12:00:00 AM', 200, 152, 48, 5, 265, 5, 'https://www.alaskapublic.org/2019/01/30/study-finds-poor-air-quality-aboard-cruise-ships/', 'Study finds poor air quality aboard cruise ships', 'Test article', 9), 
    ('10/6/2018 12:00:00 AM', 1000, 990, 10, 45, 2000, 2, 'https://www.sciencemag.org/news/2019/01/tourism-endangering-these-giant-lizards', 'Is tourism endangering these giant lizards?', 'Test article', 5), 
    ('12/26/2018 12:00:00 AM', 450, 150, 300, 10, 500, 6, 'https://www.history.com/news/knights-middle-ages', 'Eight Knights Who Changed History', 'Test article', 7), 
    ('2/15/2018 12:00:00 AM', 76, 23, 54, 5, 205, 9, 'https://www.kdlt.com/2019/01/30/local-animal-sanctuary-helping-horses-stay-warm/', 'Local Animal Sanctuary Helping Horses Stay Warm', 'Test article', 11), 
    ('9/6/2018 12:00:00 AM', 100, 50, 50, 12, 154, 8, 'https://www.express.co.uk/news/royal/1080615/the-queen-news-elizabeth-ii-prince-philip-buckingham-palace-finance-net-worth-spt', 'Royal SCANDAL: How the Queen almost LOST Buckingham Palace because of :MONEY PROBLEMS', 'Test article', 6),
    ('7/24/2018 12:00:00 AM', 5017, 5000, 17, 357, 6078, 1, 'https://www.timeslive.co.za/motoring/news/2019-01-31-at-last-bash-proof-and-pinch-proof-car-doors/' , 'At last: bash-proof and pinch-proof car doors', 'Test article', 8), 
    ('10/15/2018 12:00:00 AM', 296, 266, 30, 24, 709, 3, 'https://www.dezeen.com/2019/01/23/marc-newson-gagosian-gallery-exhibition-new-york/', 'Marc Newson exhibition in New York features swords and surfboards', 'Test article', 12), 
    ('3/11/2018 12:00:00 AM', 400, 100, 300, 35, 594, 7, 'https://www.foxnews.com/travel/disney-world-uses-secret-shade-paint-park-elements-hide', 'Disney World reportedly uses a secret shade of paint to camouflage less than exciting park elements', 'Test article', 4), 
    ('1/13/2019 12:00:00 AM', 10, 1, 9, 2, 58, 10, 'https://www.kmov.com/news/it-s-about-loyalty-st-louis-group-rooting-for-rams/article_933823f6-24e7-11e9-9caa-d757bd32399b.html', 'Its about loyalty: St. Louis group rooting for Rams in Super Bowl', 'Test article', 13); 


CREATE TABLE [dbo].Comment	
(
    [CommentID]        INT IDENTITY(1,1) NOT NULL,
    [VoteCount]        BIGINT NOT NULL,
    [UpvoteCount]      BIGINT,
    [DownvoteCount]    BIGINT,
    [GifRequest]       NVARCHAR(100),
    [ImageRequest]     NVARCHAR(100) NOT NULL,
	[UserID]		   INT NOT NULL,
	[DiscussionID]     INT NOT NULL,

    CONSTRAINT [PK_dbo.Comment] PRIMARY KEY CLUSTERED ([CommentID] ASC),
	CONSTRAINT [FK_dbo.Comment_User] FOREIGN KEY (UserID) REFERENCES [dbo].[DiscussionHubUser] (UserID),
	CONSTRAINT [FK_dbo.Comment_Discussion] FOREIGN KEY (DiscussionID) REFERENCES [dbo].[Discussion] (DiscussionID) 
	ON DELETE CASCADE ON UPDATE CASCADE,
);
