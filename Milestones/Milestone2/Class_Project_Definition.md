2018-19 Class Project Inception: Discussion Hub
=====================================

## Summary of Our Approach to Software Development

[What processes are we following?  What are we choosing to do and at what level of detail or extent?]

## Initial Vision Discussion with Stakeholders

People like to talk; people like to discuss and argue.  People like to state their opinions and read thoughts of others.  But there are real problems with how it is happening on the Web.  Just take a look at YouTube or Twitter comments.  That's not a discussion and it's not often useful, productive or even civil.  Social media is not the place to talk about things.  The Internet should be a place where people can *communicate*.  Let's make a site where people can do that!

Places like Reddit and Kialo already have a handle on general topic discussions, so we won't try to do that.  Enthusiasts often have their own sites with discussion and comment sections (e.g. Slashdot) and have formed close-knit communities that work well.  We don't want to do that either.  Here's our idea, using an example:

Let's say I just read this article on CNN, [American endurance athlete becomes the first person to cross Antarctica solo](https://www.cnn.com/2018/12/27/world/colin-obrady-antarctica-solo-trip-trnd/index.html), about a guy from Oregon who skied *across* Antarctica.  The thing is, is that he didn't and he wasn't the first.  His claims should be challenged and the article/journalist is disingenuous by not at least bringing this up.  People reading this article should know there are serious questions about it.  Someone should comment on this to point it out and spark further discussion.   There is no way to do this.  CNN does not have a comment section.  Even if it did, do I want to create an account there just so I can make a quick comment?  I'd have to do that everywhere on the web where I had a question or wanted to comment.  How would I know if my question was answered?  

We want a centralized discussion site that can be found easily and where an individual can maintain an account, build a history, expertise, level of trust, etc.  It should make it easy to create or find a discussion page about any news article, post or web site.  It should provide features for the user to follow their discussions without ever going back to the original website.  It should allow them to create and maintain their own identity that is separate from any social media identity.

## Initial Requirements Elaboration and Elicitation

### Questions

1. How do we link a discussion on the site to one or more articles/pages?

    We were thinking via URL.

2. How will users find out that there is a discussion on the site for the article/page they're currently viewing?
   
    How about a browser plug-in?  It could send the URL of the current page to our API to see if a discussion page exists and provide an easy way for them to navigate to our page.

     (Like the working principle of "honey", when you are in a shopping website, it will automatically find the coupons for you, and when you in the checkout page, it will pop out and apply the avaliable coupon.) We can make an extension of the browser like it, so when the user is in the news page, it will automatically running to search the discussion about news, when the user drag to the bottom, the extension will pop out and show all the discussion and the comment place .
    
     Or the user can copy the URL and paste it into a search bar on our site.

3. Clearly we need accounts and logins.  Our own, or do we allow logging in via 3rd party, i.e. "Log in with Google" or ...?  
    
    It depends on the different brower, if the user downloads extension from Google, he can use the google account log in and comment, if he use the microsoft edge, he can use microsoft account to log in. Because most time where the user downloads the extension shows which browser he prefer to use and stay for a long time, it's convenient to update and send new message to the user.
    
4. Do we allow people to comment anonymously?  Read anonymously? 
    I don't think people can comment anonymously, because they should take responsibility to their comment.
    In China, there is a very popular social media application named "Weibo", similar to twitter, users can document their life, publish their views about something and comment others' views. If one user's view is forward more than 500 times, then the user will be liable for any economic and legal liability incurred herefrom.
    I think users can read anoymously.
5. Do we allow people to sign up with a pseudonym or will we demand/enforce real names?
    I think the pseudonym can encourage users publish their own opinions.
    
6. What is it important to know about our users?  What data should we collect?
   
   Preference for news, what kinds of news are they like so we can recommend more for them????
   We should collect the tags of news which users always scan.
    
7. If there are news articles on multiple sites that are about the same topic should we have separate discussion pages or just one?
   I think we should keep one discussion area but I really don't know how to do this.(how can we know they are about the same topic???
   
8. What kind of discussion do we want to create? Linear traditional, chronological, ranked, or ?
   I think ranked will be ok, the hottest comment can always attract more comments and make the news interesting.
   
9. Should we allow image/video uploads and host them ourselves?
   I think images are fine, for example, sometims the meme can help express feelings a lot.
   
10. Should we allow user check others' comment history?
    I don't think we should allow them to do that, it's like the part of the social media application, we just want to focus on the comment for news, also we not allow the user can follow other users.

11. How can users record their loved comment?
    They can view the comments which they voted in the history or they can star the comment they like. 

### Interviews

### ?

## List of Needs and Features

1. A great looking landing page with info to tell the user what our site is all about and how to use it.  Include a link to and a page with more info.  Needs a page describing our company and our philosophy.
2. The ability to create a new discussion page about a given article/URL.  This discussion page needs to allow users to write comments.
3. The ability to find a discussion page.
4. User accounts
5. A user needs to be able to keep track of things they've commented on and easily go back to those discussion pages.  If someone rates or responds to their comment we need to alert them.
6. Allow users to identify fundamental questions and potential answers about the topic under discussion.  Users can then vote on answers.
7. A user can delete or edit his comment.
8. Once a user edit his comment, the vote for the original comment will be clear.
9. Once the article/news deleted by authors, all the comments will disappear but can save in users' own comment history.

## Initial Modeling


### Use Case Diagrams

### Other Modeling

## Identify Non-Functional Requirements

1. User accounts and data must be stored indefinitely.
2. Site and data must be backed up regularly and have failover redundancy that will allow the site to remain functional in the event of loss of primary web server or primary database.  We can live with 1 minute of complete downtime per event and up to 1 hour of read-only functionality before full capacity is restored.
3. Site should never return debug error pages.  Web server must never return 404's.  All server errors must be logged.  Users should receive a custom error page in that case telling them what to do.
4. Must work in all languages and countries.  English will be the default language but users can comment in their own language and we may translate it.
5. 

## Identify Functional Requirements (User Stories)

E: Epic  
F: Feature  
U: User Story  
T: Task  

1. [U] As a visitor to the site I would like to see a fantastic and modern homepage that tells me how to use the site so I can decide if I want to use this service in the future.
   1. [T] Create starter ASP dot NET MVC 5 Web Application with Individual User Accounts and no unit test project
   2. [T] Switch it over to Bootstrap 4
   3. [T] Create nice homepage: write content, customize navbar
   4. [T] Create SQL Server database on Azure and configure web app to use it. Hide credentials.
2. [U] Fully enable Individual User Accounts
   1. [T] Copy SQL schema from an existing ASP.NET Identity database and integrate it into our UP script
   2. [T] Configure web app to use our db with Identity tables in it
   3. [T] Create a user table and customize user pages to display additional data
3. [F] Allow logged in user to create new discussion page
4. [F] Allow any user to search for and find an existing discussion page
5. [E] Allow a logged in user to write a comment on an article in an existing discussion page
   1. [F]
      1. [U]
         1. [T]
         2. [T]
         3. [T]
      2. [U]
   2. [F]
   3. [F]
6. [U] As a robot I would like to be prevented from creating an account on your website so I don't ask millions of my friends to join your website and add comments about male enhancement drugs.
7. 

## Initial Architecture Envisioning

## Agile Data Modeling

## Timeline and Release Plan