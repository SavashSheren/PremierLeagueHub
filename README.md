# PremierLeagueHub

PremierLeagueHub is a modern Premier League football management platform built with **ASP.NET Core MVC**, **ASP.NET Core Web API**, **Entity Framework Core**, **SQL Server**, layered architecture, DTO-based data transfer and a premium public/admin user interface.

This project is not just a football listing website. It is designed as a complete football platform where teams, fixtures, match results, match events and league standings are managed through a Web API and reflected dynamically on the public UI.

---

## Project Preview

![Public Home Featured Match](screenshots/public-home-featured-match.png)

---

## Key Highlights

* ASP.NET Core MVC public website
* ASP.NET Core Web API backend
* Entity Framework Core with SQL Server
* Layered architecture
* DTO-based request/response structure
* Repository and service-based business flow
* Admin panel for football data management
* Public fixtures with match scores
* Fixture detail page with match timeline
* Dynamic league standings calculated from fixture results
* Match event system: goals, cards, penalties, substitutions and VAR
* External football API integration-ready foundation
* Clean responsive football-themed UI
* Swagger API documentation

---

## Architecture

The project follows a clean layered architecture.

```text
EntityLayer
    ↓
DataAccessLayer
    ↓
BusinessLayer
    ↓
WebApi
    ↓
WebUI
```

The main data flow is:

```text
Entity → DTO → Service → Web API → MVC Controller → Razor View
```

This separation keeps the project maintainable, testable and closer to real-world enterprise development practices.

---

## Tech Stack

| Layer             | Technology                    |
| ----------------- | ----------------------------- |
| Backend API       | ASP.NET Core Web API          |
| Frontend          | ASP.NET Core MVC, Razor Views |
| Database          | SQL Server                    |
| ORM               | Entity Framework Core         |
| Architecture      | N-Tier / Layered Architecture |
| API Communication | IHttpClientFactory            |
| Mapping           | AutoMapper                    |
| Documentation     | Swagger / OpenAPI             |
| UI                | HTML, CSS, Bootstrap          |

---

## Public Website Features

The public side presents a premium football platform experience with featured matches, latest results, team listings, fixtures, match detail pages and dynamic league standings.

### Public Home Page

The home page includes a featured match area, latest results and Premier League team discovery.

![Public Home Featured Match](screenshots/public-home-featured-match.png)

### Public Fixtures and Results

Fixtures are displayed with team logos, match status, match date, stadium information and final score when available.

![Public Fixtures Results](screenshots/public-fixtures-results.png)

### Fixture Detail with Match Timeline

Each fixture has a detail page that shows the match scoreboard, teams, status, stadium and timeline events such as goals, yellow cards, red cards, substitutions, penalties and VAR events.

![Public Fixture Detail Timeline](screenshots/public-fixture-detail-timeline.png)

### Dynamic League Standings

The standings table is calculated dynamically from completed fixture results. It automatically calculates played matches, wins, draws, losses, goals for, goals against, goal difference, points and recent form.

![Public Dynamic Standings](screenshots/public-dynamic-standings.png)

---

## Admin Panel Features

The admin panel is designed for managing the football platform from one central dashboard. It includes analytics, team management, fixture management, match result updates and match event management.

### Admin Dashboard Analytics

The dashboard provides a quick overview of total teams, fixtures, finished matches, match events, recent fixtures, recent events and project snapshot details.

![Admin Dashboard Analytics](screenshots/admin-dashboard-analytics.png)

### Team Management

Admins can manage Premier League clubs, team logos, city, stadium, manager, colors and active/passive status.

![Admin Team Management](screenshots/admin-team-management.png)

### Fixture Management

Admins can view fixtures, check status, see scores and navigate to result or event management screens.

![Admin Fixture Management](screenshots/admin-fixture-management.png)

### Update Match Result

Admins can update match scores and match status from the admin panel. Once updated, the score is reflected on public fixtures, fixture detail and dynamic standings.

![Admin Update Match Result](screenshots/admin-update-match-result.png)

### Match Event Management

Admins can add and delete match events such as goals, cards, substitutions, penalties and VAR events. These events are shown directly on the public fixture detail timeline.

![Admin Match Event Management](screenshots/admin-match-event-management.png)

---

## Web API and Swagger

The project exposes backend functionality through ASP.NET Core Web API endpoints. Swagger is used for endpoint testing and documentation.

![Swagger API Overview](screenshots/swagger-api-overview.png)

Main API modules include:

```text
/api/Teams
/api/Fixtures
/api/MatchEvents
/api/DevelopmentSeed
/api/ExternalFootball
```

---

## External Football API Foundation

PremierLeagueHub includes an external football API integration foundation. The system is prepared for API-based football data integration while still being able to run safely with local demo data.

API keys are intentionally not committed to the repository. When no key is configured, the system returns a safe configuration response instead of crashing.

![External Football API Status](screenshots/external-football-api-status.png)

This design allows two usage modes:

```text
1. External API mode
   A valid football API key can be configured through user-secrets.

2. Local demo mode
   The project can run with seeded teams, fixtures, results and match events without any external dependency.
```

---

## Demo Data and Seed System

The project includes development seed endpoints for preparing demo football data.

Seed endpoints can create:

* Premier League teams
* Week 34 fixtures
* Demo match results
* Demo match events
* Clean demo football data

Example seed flow:

```text
POST /api/DevelopmentSeed/premier-league-teams
POST /api/DevelopmentSeed/week-34-fixtures
POST /api/DevelopmentSeed/demo-results-and-events
```

The demo results and events seed fills the project with realistic match data so that public fixtures, match detail timeline and dynamic standings look complete.

---

## Core Modules

### Teams Module

* Team CRUD
* Logo URL
* City
* Stadium
* Manager
* Club colors
* Active/passive status

### Fixtures Module

* Home team / away team
* Match date
* Week number
* Season
* Stadium
* Top match flag
* Match status
* Score fields

### Match Result Module

* Home score
* Away score
* Scheduled / Live / Finished status
* Public score display
* Admin result update workflow

### Match Events Module

Supported event types:

```text
Goal
Penalty
YellowCard
RedCard
Substitution
VAR
```

Each event can include:

* Fixture
* Team
* Minute
* Event type
* Player name
* Assist player
* Related player
* Description

### Dynamic Standings Module

The league table is calculated from fixture results.

Calculated fields:

* Played
* Won
* Drawn
* Lost
* Goals For
* Goals Against
* Goal Difference
* Points
* Form

---

## What Makes This Project Strong

PremierLeagueHub is stronger than a basic CRUD football project because it combines public football presentation with admin-driven football operations.

Main strengths:

* It uses Web API and MVC as separate layers.
* It has admin-managed fixtures, scores and match events.
* Public fixture detail pages display real timeline-style football events.
* League standings are calculated from actual fixture results.
* The dashboard summarizes football data with analytics.
* It includes external football API readiness without making the project dependent on external services.
* The UI is designed as a premium football platform, not a default template project.

---

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/PremierLeagueHub.git
cd PremierLeagueHub
```

### 2. Configure Database Connection

Update the connection string in:

```text
PremierLeagueHub.WebApi/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=PremierLeagueHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply Migrations

Run the following command from the solution directory:

```bash
dotnet ef database update --project PremierLeagueHub.DataAccessLayer --startup-project PremierLeagueHub.WebApi
```

### 4. Run Both Projects

Start both projects:

```text
PremierLeagueHub.WebApi
PremierLeagueHub.WebUI
```

Expected local addresses:

```text
Web API: https://localhost:7147
Web UI : https://localhost:7243
```

### 5. Seed Demo Data

Open Swagger:

```text
https://localhost:7147/swagger/index.html
```

Run seed endpoints:

```text
POST /api/DevelopmentSeed/premier-league-teams
POST /api/DevelopmentSeed/week-34-fixtures
POST /api/DevelopmentSeed/demo-results-and-events
```

Then open:

```text
https://localhost:7243
```

---

## External API Configuration

If you want to test external football API integration, configure the API key using user-secrets.

```bash
dotnet user-secrets init --project PremierLeagueHub.WebApi
dotnet user-secrets set "ExternalFootballApi:ApiKey" "YOUR_API_KEY" --project PremierLeagueHub.WebApi
```

The API key should not be written directly into `appsettings.json`.

---

## Important Pages

| Page               | URL                                           |
| ------------------ | --------------------------------------------- |
| Public Home        | `/`                                           |
| Teams              | `/Home/Index`                                 |
| Fixtures           | `/Fixtures/Index`                             |
| Fixture Detail     | `/Fixtures/Detail/{id}`                       |
| Standings          | `/Standings/Index`                            |
| Admin Dashboard    | `/Admin/Dashboard/Index`                      |
| Admin Teams        | `/Admin/AdminTeam/Index`                      |
| Admin Fixtures     | `/Admin/AdminFixture/Index`                   |
| Admin Match Events | `/Admin/AdminMatchEvent/Index?fixtureId={id}` |
| Swagger            | `/swagger/index.html`                         |

---

## Project Status

Completed modules:

* Team management
* Fixture management
* Match result management
* Match event management
* Public fixture detail timeline
* Dynamic standings calculation
* Admin dashboard analytics
* Public home featured match section
* External football API foundation
* Demo data seed system

---

## Future Improvements

Potential improvements:

* Real football API synchronization
* Player statistics module
* Match statistics module
* News module
* Authentication and role-based authorization
* Pagination and advanced filtering
* Live match status updates
* Deployment to cloud hosting

---

## Disclaimer

This project is developed for educational and portfolio purposes. It is not affiliated with, endorsed by or officially connected to the Premier League or any football club.
