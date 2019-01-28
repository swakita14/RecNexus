CREATE TABLE [dbo].[Users]
(
    [UserID]        INT IDENTITY(1,1) NOT NULL,
    [Browser]        NVARCHAR(128) NOT NULL,
    [FName]            NVARCHAR(64),
    [LName]            NVARCHAR(128),
    [Email]            NVARCHAR(64),
    [LoginPref]        NVARCHAR(128) NOT NULL,
    [VoteTotal]        BIGINT NOT NULL,
    [About]            TEXT,
    [Pseudonym]        NVARCHAR(128),

    CONSTRAINT [PK_dbo.Requests] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

INSERT INTO [dbo].[Users](Browser,FName,LName,Email,LoginPref,VoteTotal,About,Pseudonym) VALUES
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
