# RaceDay API

## Project Overview

RaceDay is an ASP.NET Core Web API developed in C# using .NET 8. The system is designed to manage race events, categories, participant enrolments and race results.

The API supports two user roles:
- **Organiser:** Can create and manage events and categories, and manage event enrolments and results.
- **Participant:** Can view events, manage their profile, enrol in events and view their own enrolments and results.

## Technologies Used

- C#
- ASP.NET Core Web API
- .NET 8
- Entity Framework Core 8
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- xUnit (unit testing)

## Main Features

- User registration and login.
- JWT-based authentication.
- Role-based access control.
- Event management.
- Category management.
- Participant enrolments.
- Race result management.
- API documentation through Swagger.

## Getting Started

### Requirements

- Visual Studio 2022
- .NET 8 SDK
- SQL Server / SQL Server LocalDB

### Running the Project

1. Clone or download the repository.
2. Open `Raceday_POE2.sln` in Visual Studio.
3. Configure the database connection string for your own SQL Server instance.
4. Configure the JWT settings securely.
5. Build the solution.
6. Run the `Raceday_POE2` project.
7. Open the Swagger page at:

`https://localhost:7055/swagger/index.html`

The actual Swagger address may differ depending on the launch settings.

## Authentication

The API uses JSON Web Tokens (JWT).

Users must log in through:

`POST /api/auth/login`

A successful login returns a JWT token. The token must be supplied as a Bearer token when accessing protected endpoints.

Do not commit JWT signing keys, passwords, access tokens or other secrets to GitHub.

## User Roles

| Role | Permissions |
|---|---|
| Organiser | Manage events and categories, view event enrolments, submit results |
| Participant | View events and categories, enrol in events, view own enrolments and results |

## API Endpoints

| Method | Endpoint | Access |
|---|---|---|
| POST | `/api/auth/register` | Public |
| POST | `/api/auth/login` | Public |
| GET | `/api/users/me` | Organiser / Participant |
| PUT | `/api/users/me` | Organiser / Participant |
| GET | `/api/events` | Organiser / Participant |
| GET | `/api/events/{id}` | Organiser / Participant |
| POST | `/api/events` | Organiser |
| PUT | `/api/events/{id}` | Organiser |
| DELETE | `/api/events/{id}` | Organiser |
| GET | `/api/categories` | Organiser / Participant |
| POST | `/api/categories` | Organiser |
| POST | `/api/enrolments` | Participant |
| GET | `/api/enrolments/my` | Participant |
| GET | `/api/events/{eventId}/enrolments` | Organiser |
| POST | `/api/results` | Organiser |
| GET | `/api/results/my` | Participant |

## Database

The project uses Entity Framework Core and SQL Server.

The main entities are:
- User
- Event
- Category
- EventCategory
- Enrolment
- Result

The database uses relationships between these entities to connect organisers, events, categories, participants, enrolments and results.

## Testing

The project includes an xUnit test project named `Raceday_POE2.Tests`.

Run tests using Visual Studio Test Explorer or the .NET CLI:

`dotnet test`

Add the final test results here after running the tests.

## Project Structure

- `Controllers/` – API endpoint controllers.
- `Models/` – Database entity models.
- `Models/RequestModels/` – Request models used by the API.
- `Data/` – Database context and configuration (adjust this path if your context is stored elsewhere).
- `Raceday_POE2.Tests/` – Automated tests.

## Security Notes

Connection strings and JWT signing keys should be configured locally and should not contain personal credentials in the public repository.
