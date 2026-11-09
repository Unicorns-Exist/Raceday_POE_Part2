# RaceDay API Testing

## Testing Environment

- Application: RaceDay API
- Framework: ASP.NET Core .NET 8
- API testing tool: Swagger UI
- Automated testing framework: xUnit
- Database: SQL Server

## Manual API Tests

| Test | Endpoint | Expected result | Actual result |
|---|---|---|---|
| Register a new user | POST `/api/auth/register` | User created successfully | To be completed |
| Register duplicate email | POST `/api/auth/register` | HTTP 409 Conflict | HTTP 409 observed |
| Log in with valid details | POST `/api/auth/login` | HTTP 200 and JWT token | HTTP 200 observed |
| View current user | GET `/api/users/me` | HTTP 200 with valid token | To be completed |
| Access protected endpoint without token | GET `/api/users/me` | HTTP 401 Unauthorized | To be completed |

## Automated Tests

Record the number of tests passed and failed after running the xUnit test project.

Command:

`dotnet test`

## Notes

Only record test results that have actually been observed. Update this document as additional endpoints are tested.
