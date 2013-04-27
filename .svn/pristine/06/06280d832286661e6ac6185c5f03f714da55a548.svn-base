CIS726_Assignment3 - Readme File

Russell Feldhausen
212 Nichols Hall
russfeld@ksu.edu
http://people.cis.ksu.edu/~russfeld

Notes:
  * This project was developed in Visual Studio 2012. It should work in Visual Studio 2010, but I have not tested it myself.
  * This project uses SQL Server LocalDB as the storage engine. For those of you using Visual Studio 2010, you may have to install that seperately for it to work correctly.
  * For parts that may need updated when building or deploying, search the files for "//todo"
  * For parts with special comments, search the files for "@russfeld"
  * The seed data may not be completely correct; it was the best I could do at the time. So, you may want to update the seed data to more closely match what you are wanting to do.
  * Anywhere that code/js/css was borrowed/adapted from online sources should be marked with the URL of the source. Mad Props to those websites.

First Run:
  * Open Visual Studio, and build the project. If NuGet is not configured to automatically download needed packages on Build, you may get an error describing how to turn that on. (In VS 2012, go to Tools > Options > Package Manager and checkmark "Allow NuGet to download missing packages during build".) If all goes well, it should successfully build.
  * Close Visual Studio.
  * Do ONE of the following:
    * Copy the files from CIS726_Assignment2\App_Data_Good to CIS726_Assignment2\App_Data
    * Open the project and run "update-database" from the package manager console. WARNING! This takes about 20 minutes to run on my machine.
  * Open the project and run it. Everything should be there.
  
Models:
  * The models all must implement the IModel class stored in Models > IModel.cs in order to use the Repository framework (more on that below). Specifically, they MUST use the ID field from that interface as their primary key.
  * Each many-to-many relationship in the database is represented by its own class acting as the join table.
  * See "database_schema.bmp" for a better picture of the database schema and relations.
  
Controllers:
  * The controllers should be pretty self explanatory. They aren't bulletproof in all cases, but they should function well. The edit method in the DegreeProgramsController is particularly interesting as it deals with two variable length lists representing the Required Courses and Elective Courses. 
  
Repositories:
  * For the purposes of Unit Testing, this project uses a Repository framework to abstract the database connection. The code for those classes is well documented, and in each class is a link to the references used to generate them. In general, as long as any added model classes implement the IModel interface (and use the ID field as their primary key in the database), you should be able to use these classes just as I use them without modification.
  
Scripts:
  * All the custom code for the site is included in Scripts > site.js. It isn't pretty, but it is function. When depolying the project, you may have to update the URLs in the site.js file to match those used on the production server. If you deploy to the webroot (so your application is at http://localhost) then it shouldn't be a problem.
  
Views:
  * The views for some pages make heavy use of Partials, with each partial view filename ending in "Partial" so they are easy to find.
  * Also, some files make use of shared DisplayTemplates to display data for an entire class (which are displayed using the @Html.DisplayFor() function). They are stored in Views > Shared > DisplayTemplates. 
  
Unit Testing: 
  * Most of the code has Unit Tests that check basic functionality, but I can't say they are 100% thorough nor exhaustive. 
  * The Unit Tests make use of the FakeStorageContext, stored in Fakes > FakeStorageContext.cs. It uses a generic List class to mimic the functions of a database. 
  * The FakeStorageContext WILL NOT update related fields in your database, so if you update one side of a many-to-many relationship you will have to manually update the other side yourself. 
  * The Initialization function in DegreeProgramsControllerTest.cs should give you a good idea of how that works (basically, you need to add the links between the classes yourself, not just the ID fields).