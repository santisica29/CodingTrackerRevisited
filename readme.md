# Coding Tracker
Console based CRUD application to track coding sessions.

Built with C# for the backend, 
SQLite as the database with Dapper as the ORM 
and Spectre.Console for the UI

# Requirements:
- Allow the user to log daily coding time.
- Display all data in the console using the **Spectre.Console** library.
- The application should store and retrieve data from a real database
- Organize the project with separate classes in different files.
- Enforce a specific date and time format for logging and reject any other formats.
- Create an `appsettings.json` configuration file containing the database path, connection strings.
- Implement a `CodingSession` class.
- When reading from the database do not use anonymous objects.
- Do not allow the user to input the session duration; calculate it from the start and end times.
- Use **Dapper ORM** for data access instead of ADO.NET.
- Follow the **DRY (Don’t Repeat Yourself)** principle and avoid code duplication.
- Use separation of concerns and OOP to keep your code clean and organized.

# Features
- SQLite database connection
	- The program uses a SQLite db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created on program start.
- Spectre.Console for the UI, user can anvihgate by key presses.
- CRUD DB functions
	- Users can Create, Read, Update or Delete coding sessions. 
	- Users can Read data from specific dates.
- Reporting and other data output uses ConsoleTableExt library to output in a more pleasant way
	
