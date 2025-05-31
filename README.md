MovieApp
MovieApp is a web application that allows users to view, rate, and manage movie information on a centralized platform. Users can add films to favorites, build a watchlist, write reviews, and rate movies. Additionally, role-based access control and email verification mechanisms ensure that only authorized users can perform certain actions.

Key Features
Viewing & Searching Movies
All registered movies are listed.

Users can search by title, genre, or director.

Favorites & Watchlist
Liked movies can be added to the “Favorites” list.

Users can add movies to their “Watchlist” to watch later.

Users can remove movies from these lists at any time.

Reviews & Ratings
Each movie can be rated by users on a scale of 1–5 stars (averaged on a 10-point scale for display).

Users can write reviews for movies.

Under each review, other users can post replies or delete their own replies.

Reviews and comments are saved in real time and immediately reflected in lists.

User Authentication & Authorization
Registration, login, and logout are JWT-based.

Email verification is implemented (accounts must verify their email before full access; otherwise, a “Waiting for Email Verification” status is shown).

Role-based access control for API endpoints (Admin, Moderator, User):

Admin: Can add/edit/delete movies, manage users, approve or reject reviews/comments.

Moderator: Can approve or delete pending reviews and comments.

User: Can write/view/delete only their own reviews/comments, rate movies, and manage their own favorites/watchlist.

Protected endpoints based on roles (e.g., only Admin can add or update movies).

Email Verification & Notifications
On registration, a verification email is sent to the user’s address.

Unverified accounts attempting to log in receive a “Waiting for Email Verification” message and cannot access protected resources.

(In development) Critical operations—such as password reset requests or new review notifications—trigger email notifications.

Admin Panel
An “Admin Panel” page accessible only to Admin users:

CRUD operations for movie management.

User list and role assignment/management.

Pending review/comment queue with approve/reject functionality.

A “Moderator Panel” page:

Displays only pending reviews/comments with approve/reject controls.

Responsive Design & UI/UX
Frontend: Built with React, Tailwind CSS, and React Router as a single-page application (SPA).

UI: Responsive design for mobile and desktop. Optionally supports light/dark theme.

Technical Architecture & Technologies
Backend (MovieApp.API)
Platform: ASP.NET Core 6 (C#)

Database: Microsoft SQL Server (or SQLite during development)

ORM: Entity Framework Core

Authentication & Authorization
JWT (JSON Web Token) for user session management

Email Verification via SendGrid or SMTP

Identity-based user and role management

Layered Architecture
MovieApp.API

Controllers (API endpoints)

DTOs (Data Transfer Objects)

Mapping Profiles (AutoMapper)

Filters (global or attribute-based)

Program.cs, Startup configuration, appsettings.json

MovieApp.Business

Service layer (business rules, managers)

MovieApp.DataAccess

Repositories (data access)

DbContext definitions

EF Core migrations

MovieApp.DTO

Shared DTOs (if reused across multiple projects)

MovieApp.Entity

Entity classes (Movie, User, Review, Comment, etc.)

Mapping
AutoMapper profiles to convert between Entities and DTOs.

Email Integration
SendGrid or standard SMTP for sending verification and notification emails.

Logging & Error Handling
Global exception handling middleware in ASP.NET Core.

Optional in-app logging using Serilog or similar.

Frontend (MovieApp-Frontend)
Platform: React (v18+)

Package Manager: npm or Yarn

HTTP Requests: Axios

Routing: React Router v6

State Management
Context API (to store JWT, user info, and application-wide state)

Local component state via useState / useReducer for smaller scopes

UI & Styling
Tailwind CSS (utility-first approach)

Custom reusable components (Button, Input, Card, DataTable, etc.)

Form Handling
React Hook Form (optional, easily extensible for validation)

Notifications
react-toastify or a similar library for toast notifications

Build Configuration
Vite (vite.config.js) or Create React App with traditional Webpack (webpack.config.js)

Project Structure
graphql
Copy
Edit
MovieApp/
├─ Backend/
│  ├─ MovieApp.API/             # ASP.NET Core Web API project
│  │    ├─ Controllers/         # API endpoint controllers
│  │    ├─ DTO/                 # Data Transfer Objects
│  │    ├─ Mapping/             # AutoMapper profiles
│  │    ├─ Filters/             # Global filters or attributes
│  │    ├─ Program.cs
│  │    ├─ appsettings.json
│  │    └─ …                     
│  ├─ MovieApp.Business/        # Business layer (services, managers)
│  ├─ MovieApp.DataAccess/      # Repositories, DbContext, migrations
│  ├─ MovieApp.DTO/             # Shared DTOs (if used by multiple projects)
│  ├─ MovieApp.Entity/          # Entity definitions (Movie, User, Review, Comment, etc.)
│  └─ MovieApp.DataAccess.sln   # Visual Studio solution file
│
└─ Frontend/
   ├─ public/                   # static files (index.html, favicon, etc.)
   ├─ src/
   │   ├─ components/           # Reusable React components (Button, Card, DataTable…)
   │   ├─ context/              # React Context providers (AuthContext, NotificationContext…)
   │   ├─ hooks/                # Custom hooks (useFetch, useAuth, useFormValidator…)
   │   ├─ pages/                # Route-level pages (HomePage, MovieDetailPage…)
   │   ├─ services/             # API service modules (actors.js, movies.js…)
   │   ├─ utils/                # Helper utilities (dateFormat, tokenHelpers…)
   │   ├─ App.jsx                # React Router setup
   │   ├─ index.jsx              # Application entry point
   │   └─ tailwind.config.js
   ├─ package.json
   ├─ vite.config.js (or webpack.config.js)
   └─ README.md                   # Frontend-specific documentation
Installation & Running
1. Backend
Clone the repository

bash
Copy
Edit
git clone https://github.com/Parzival51/MovieApp.git
cd MovieApp/Backend/MovieApp.API
Restore dependencies

Visual Studio: Open MovieApp.API.sln and restore NuGet packages.

Command Line (CLI):

bash
Copy
Edit
cd MovieApp.API
dotnet restore
Update Database Connection

In appsettings.json, set your "DefaultConnection" string (SQL Server or SQLite).

Apply Migrations & Create Database

bash
Copy
Edit
cd ../MovieApp.DataAccess
dotnet ef migrations add InitialCreate
dotnet ef database update
Run the API

bash
Copy
Edit
cd ../MovieApp.API
dotnet run
The API will typically run at https://localhost:7176.

2. Frontend
Install dependencies & start development server

bash
Copy
Edit
cd MovieApp/Frontend
npm install
npm run dev
If you used Vite, use npm run dev; if Create React App, use npm start.

The app will open at http://localhost:5173 (or another port based on your configuration).

Environment Variables

Create a .env.local (or .env) file in the Frontend folder with:

bash
Copy
Edit
VITE_API_BASE_URL=https://localhost:7176/api
The frontend code uses import.meta.env.VITE_API_BASE_URL to send requests to the backend.

User Registration & Login

On first run, navigate to the registration page in your browser.

Register a new user and check your email for a verification link (if email verification is active).

Click the link in your email to activate your account.

Roles & Responsibilities
Frontend Development
Bayram

Build the React-based user interface (UI).

Implement responsive design with Tailwind CSS.

Make API calls via Axios.

Implement form validation (using React Hook Form or equivalent).

Backend Development
Yusuf

Develop ASP.NET Core Web APIs (Controllers, Services, Repositories).

Implement JWT-based authentication and role-based authorization (Admin/Moderator/User).

Integrate EF Core for database access and handle migrations.

Integrate email verification and notification workflows (e.g., using SendGrid or SMTP).

Rojhat

Implement the Business layer (core business logic and services).

Define DTOs and configure AutoMapper profiles.

Create moderator-specific endpoints for approving/deleting reviews and comments.

Prepare a test database with seeding of initial sample data.

Summary
MovieApp is a full-stack movie management system that includes:

Comprehensive CRUD for movies, reviews, comments, and user management

Real-time reviews and replies

Role-based access control (Admin, Moderator, User)

Email verification and basic notification support

Responsive React frontend using Tailwind CSS, React Router, and Axios

Layered ASP.NET Core backend with EF Core, AutoMapper, and clean separation of concerns

JWT-based authentication and Identity-driven user/role management

By following this guide, you can set up both the backend API and the frontend SPA, register new users, verify emails, and start managing your movie database in a secure, role-aware environment.
