1.	PROJECT SUMMARY
MovieApp enables users with different permissions (Admin, Moderator, User) to perform secure and interactive operations on a movie database. After email verification, users can search for movies; add or remove titles from their favorites and watchlists; and submit ratings and comments. Moderator and Admin roles have additional privileges such as content approval, content management, and user management.
2.	TECHNOLOGY STACK
Frontend:
•	React (v18+)
•	Tailwind CSS
•	React Router v6
•	Axios
•	React Hook Form
•	Vite
Backend:
•	ASP.NET Core 6
•	Entity Framework Core
•	Microsoft SQL Server (SQLite supported in development)
•	JWT-based authentication
•	SendGrid/SMTP for email verification
•	Serilog for logging
Other Tools:
•	Docker
•	GitHub Actions (CI/CD)
3.	ARCHITECTURE
Layered architecture is employed:
•	Frontend SPA (React) →
•	API Layer (ASP.NET Core Controllers) →
•	Business Rules Layer (Business Services) →
•	Data Access Layer (EF Core Repositories) →
•	Database (SQL Server)
Data flows sequentially from incoming HTTP requests through Controller, Service, Repository, and DbContext.
4.	SETUP AND RUNNING
Prerequisites: .NET 6 SDK, Node.js, Docker (optional), SQL Server or SQLite
Step 1: Clone the repo
git clone https://github.com/Parzival51/MovieApp.git  
cd MovieApp
Step 2: Prepare the Backend
cd Backend/MovieApp.DataAccess  
dotnet restore  
dotnet ef migrations add InitialCreate  
dotnet ef database update  
cd ../MovieApp.API  
dotnet run  
The API will run at https://localhost:7176.
Step 3: Prepare the Frontend
cd ../../Frontend  
npm install  
npm run dev  
The application opens at http://localhost:5173.
5.	FEATURES AND USAGE
5.1. Registration & Email Verification
1.	User navigates to the “Sign Up” page.
2.	They enter email, password, and full name, then click “Sign Up.”
3.	On successful registration, a “Verification email sent” message appears.
4.	User clicks the link in the email to verify their account.
5.	Until verification, login attempts return “Email verification pending.”
5.2. Login
1.	User goes to the “Login” page.
2.	They enter verified email and password.
3.	On success, a JWT token is stored in the browser and the user is redirected to the home page.
4.	On invalid credentials, “Email or password incorrect” is shown.
5.3. Home Page: Movie Listing & Search
1.	After logging in, a paged list of movies appears.
2.	The search bar filters by title, director, or genre.
3.	Type-ahead suggestions appear; users can select from them.
4.	“Next” / “Previous” paging controls navigate through the list.
5.4. Movie Details Page
1.	Click a movie from the list or search results.
2.	The details page shows:
o	Poster, title, description
o	Director(s), cast, genre, language, runtime, release date
o	Average rating and total reviews count
o	“Add to Favorites” and “Add to Watchlist” buttons
o	Review and comment section
5.5. Favorites
1.	Click the heart icon on the movie details page to add to favorites.
2.	In the menu, “Favorites” shows a paged list of added movies.
3.	Click the heart icon again in the favorites list to remove.
5.6. Watchlist
1.	Click “Add to Watchlist” on the movie details page.
2.	“My Watchlist” in the menu lists all added movies.
3.	Click the trash icon on a watchlist card to remove.
5.7. Reviews & Ratings
1.	On the movie details page, the “Write a Review” form appears (1–5 stars and text).
2.	Select stars, enter text, click “Submit.”
3.	On success, the form clears and the new review appears.
4.	Each review includes “Reply” buttons; users can delete their own replies.
5.	Only approved reviews are shown on the details page.
5.8. MODERATOR PANEL
1.	Select “Moderator Panel” from the menu (visible only to Moderators).
2.	The “Pending Reviews” list appears; each review has “Approve” or “Reject” buttons.
3.	Approved reviews go to the general list; rejected ones are deleted or archived.
5.9. ADMIN PANEL
1.	“Admin Panel” appears in the menu (visible only to Admins).
2.	Movie Management:
o	“Add New Movie” form (title, description, genre, director, cast, etc.)
o	List, update, or delete existing movies
3.	User Management:
o	View full user list with email and verification status
o	Role assignment dropdown (User, Moderator, Admin)
o	Temporarily ban or delete users
4.	Review & Comment Management:
o	List all approved and pending content
o	Bulk approve/reject support
5.10. ADVANCED FEATURES
•	“Recommended for You”: algorithm based on user history
•	“Top Rated”: highest average scores
•	“Similar Movies”: suggestions by genre/category
•	“Activity Log”: paged listing of all user actions
•	Mobile-friendly responsive design; light/dark theme support
6.	USER ROLES & PERMISSIONS
User:
•	Browse and search movies
•	Manage favorites and watchlist
•	Add or delete own ratings and reviews
Moderator:
•	Access pending reviews list
•	Approve or reject reviews
Admin:
•	Add, update, delete movies
•	View user list and assign roles
•	Approve or delete any review
7.	CORE ENTITY TABLES
•	Users (Id, Email, PasswordHash, FullName, EmailConfirmed)
•	Roles (Id, Name)
•	Movies (Id, Title, ReleaseDate, PosterUrl)
•	Actors (Id, Name, ExternalId)
•	Directors (Id, Name, ExternalId)
Join Tables
•	UserRoles (UserId → Users, RoleId → Roles)
•	MovieGenres (MovieId → Movies, GenreId → Genres) (optional)
Interaction Tables
•	Reviews (Id, UserId → Users, MovieId → Movies, Rating, Comment, CreatedAt, IsApproved)
•	Comments (Id, ReviewId → Reviews, UserId → Users, Text, CreatedAt)
•	Favorites (UserId → Users, MovieId → Movies)
•	Watchlists (UserId → Users, MovieId → Movies)
8.	API ROUTING
See API_Dokümantasyonu.pdf for detailed endpoint definitions.
9.	TESTING & QUALITY ASSURANCE
  There are no automated tests. Manual testing steps:
•	Verify registration and email flow
•	Test movie search and filtering
•	Test adding/removing favorites and watchlist
•	Test review submission and Moderator approval/rejection
10.	 DEPLOYMENT & CI/CD
   With GitHub Actions on every push:
•	Backend: dotnet restore, dotnet build, dotnet test
•	Frontend: npm install, npm run build
•	Build production Docker image
•	Push to Docker Hub and auto-deploy to remote server
11.	 CONTRIBUTING
•	Fork → create a branch → submit a pull request
•	Use commit message prefixes like “feat:”, “fix:”
•	Do not merge without code review approval
12.	 AUTHORS
•	Bayram Külter (Frontend Development)
•	Yusuf Emre Gözcü (Backend Development)
•	Rojhat Çetin (Business Logic & DTO Management)










