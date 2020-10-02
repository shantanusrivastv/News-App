# Pressford News Application #

## Setup Instruction ##

 ### Client App ###
 1. Open terminal/Command line 
 2. Go to the folder location
 3. run npm install
 4. Wait for dependencies to be installed
 5. run npm start
 6. By default app will run on http://localhost:3000/
 

 ### Service App ### 
 1. Open the solution inside backend-service in Visual Studio
 2. Open Package Manager Console
 3. run update-database
 4. Run the application (ctrl + F5)
 5. By default appliaction will run in localhost:63961
 6. The same address is used in client app  
  **Optional** Change the connection string in appsettings.json to Instance of Sql Server.
   By default it will run using localdb

 ### Login Information ##
 The databse is seeded with the following Login
##  Publisher Role:
1.      username: adminUser@pressford.com
        password: admin 

2.       username: w.Pressford@pressford.com
         password: admin 

## Non-admins:
        username: normalUser@pressford.com
        password: user 