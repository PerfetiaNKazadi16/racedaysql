

RaceDay – Event Management System

RaceDay is a web-based event management system designed to simplify the organisation and participation of road running, walking, and cycling events in South Africa.

The system provides a central platform where organisers can create and manage events, categories, routes, participant entries, and race results, while participants can browse upcoming events, register for races, and track their performance history.

Main Features

* User registration and authentication
* Role-based access for Organisers and Participants
* Event creation and management
* Event categories and race routes
* Participant event registration
* Race result management
* Participant performance history
* Upcoming event browsing
* Secure database management

Technologies Used

* C#
* ASP.NET Core
* SQL Server
* HTML / CSS / JavaScript
* RESTful Web API

Database

The RaceDay database is designed around several related entities, including:

* Users
* Organiser
* Participant
* Events
* Category
* Route
* Entry
* Result

The database uses primary keys, foreign keys, unique constraints, and relational connections to maintain data integrity.

Project Purpose

RaceDay aims to replace traditional paper-based registration, spreadsheets, and disconnected communication with a more organised digital solution. The system provides organisers with better tools for managing events and gives participants an easier way to discover, enter, and track road events.

This project was developed as part of an academic Software Development project and demonstrates skills in database design, REST API development, C# programming, and web application development.
RaceDay API — Part 2
Overview
RaceDay is an event management API for running, walking, and cycling events. This Part 2 project provides the backend API for organisers and participants.
Features
Register and log in as an Organiser or Participant.
Store passwords as hashes.
Keep the signed-in user and role in a session.
View and manage events.
View and manage event categories and routes.
Let Participants enrol in events.
Let Organisers view enrolments for their events.
Let Organisers record results and Participants view their own results.
View and update the signed-in user’s profile.
Explore and try API endpoints through Swagger.
Technology
ASP.NET Core Web API
Entity Framework Core
SQL Server
Swagger / OpenAPI
xUnit
GitHub Actions
Getting Started
Open the RaceDay solution in Visual Studio.
Set up a local SQL Server connection string in your local configuration.
Apply the Entity Framework migrations to create or update the database.
Set the RaceDay API project as the startup project and run it.
Open the Swagger URL shown in the browser.
Register and log in through Swagger before using endpoints that require an Organiser or Participant session.
Main API Endpoints


Area
Endpoint
Purpose
Authentication
POST /api/Auth/register
Register an account
Authentication
POST /api/Auth/login
Log in
Authentication
POST /api/Auth/logout
Log out
Events
GET /api/Events
View events
Events
GET /api/Events/{id}
View one event
Events
POST /api/Events
Create an event as an Organiser
Events
PUT /api/Events/{id}
Update an event as an Organiser
Events
DELETE /api/Events/{id}
Delete an event as an Organiser
Categories
/api/Category
View and manage event categories
Routes
GET /api/Route/event/{eventId}
View routes for an event
Enrolments
POST /api/Entry
Enrol as a Participant
Enrolments
GET /api/Entry/my-events
View an Organiser’s event enrolments
Results
POST /api/Result
Record a result as an Organiser
Results
GET /api/Result/my-results
View a Participant’s results
Profile
GET /api/Users/profile
View your profile
Profile
PUT /api/Users/profile
Update your profile
Use Swagger for the full request and response details.
Tests
Run the test suite from the solution folder:
dotnet test
The tests cover authentication, role checks, event creation, and participant enrolment.
GitHub Actions
GitHub Actions builds the solution and runs the tests when changes are pushed. A successful run is shown below.
