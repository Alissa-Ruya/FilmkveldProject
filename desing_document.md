# Filmkveld Planlegger - Project Plan

## What Do We Aim to Achieve?
In the **Filmkveld Planlegger** project, users will:

- Create movie nights, invite their friends, and vote on which movie to watch.
- Join multiple movie nights as a participant.
- Own a movie night as an organizer.
- Use SQLite as the primary data source to manage relationships between users and movie nights.
- The frontend will display different UI elements depending on whether the user is the owner or an invitee of a movie night.

## Data Handling Requirements

- User data (name and email) must be anonymized before storage.
- User passwords will only be stored in a hashed format and never in plaintext.
- Users will receive a token upon login, allowing for session persistence.
- Users can create new accounts and log in.
- The API will ensure secure data transmission using HTTPS.

## Features and Data Models

### Filmkveld (Movie Night):
- **Name**
- **Date and time**
- **Genre**
- **Location**
- **Maximum number of participants**
- **Owner**
- **List of votes**

### User:
- **Name (anonymized)**
- **Email**
- **Password (hashed)**

### Votes:
- **Relationship between user and the selected movie**

## CRUD Operations

### Users:
- **Create:** Create new accounts.
- **Read:** Log in to view movie nights.
- **Update:** Update their passwords.
- **Delete:** Delete their accounts.

### Movie Nights:
- **Create:** Create new movie nights.
- **Read:** List all movie nights.
- **Update:** Update details of a movie night (only by the owner).
- **Delete:** Delete a movie night (only by the owner).

### Votes:
- **Create:** Add a vote to a movie.
- **Read:** View the list of votes.

## Backend: .NET and SQLite

### Entity Framework Core:
- **Models:** User, Filmkveld, Vote.
- **Migrations and database updates** will be managed using Entity Framework.

### Controller Structure:
- **FilmkveldController:** Manages all operations related to movie nights.
- **UserController:** Manages user operations and login functionality.

### DTO (Data Transfer Objects):
- **UserDTO:** For transferring user-related data.
- **FilmkveldDTO:** For transferring movie night-related data.
- **VoteDTO:** For transferring vote-related data.

### Services:
- **Singleton services:**
  - **DatabaseService:** Manages database connections.
  - **LoginService:** Handles user authentication.

## Frontend: HTML, CSS, and JavaScript

### General Layout:
- If the user is not logged in, only public movie nights will be displayed.
- Logged-in users will see the movie nights they own or are invited to.

### Page Structure:
- **Home Page:** Displays a list of movie nights based on the user's login status.
- **Login Page:** Contains forms for user login and registration.
- **Movie Night Details Page:** Provides detailed information about the selected movie night and allows voting.

### Dynamic Data Processing:
- Backend data will be used to generate dynamic content.
- `.map()` and `Object.keys()` will be used to dynamically render lists from the data.

### Login Status:
- A "Log In" button will appear for users who are not logged in.
- Logged-in users will see their username displayed in the navigation bar.

### Forms:
- HTML forms will send user login data via `POST` to the `LoginController`.
- Upon successful login, the user will be redirected to the home page.

## Technologies Used

### Backend:
- **ASP.NET Core**
- **Entity Framework Core**
- **SQLite**
- **JWT (Json Web Token):** For session management.

### Frontend:
- **HTML, CSS, JavaScript**
- **Modern CSS styles** for responsive design.








### Filmkveld Planlegger -Sekvensdiagrammer and Routing

#### **Sekvensdiagrammer**

1. **Default Route to API, standard /GET:**
```mermaid
sequenceDiagram
    httpRequest ->>+ ApiDefaultRoute: /GET
    ApiDefaultRoute ->>+ StaticFolder: Find index.html
    StaticFolder -->>- httpRequest: Serve Index.html
```

2. **Sequence for existing login token:**
```mermaid
sequenceDiagram
    actor User
    participant View(Index)
    participant LoginController
    participant LoginService
    participant UserDatabase

    User ->>+ View(Index): "Enters Index View"
    View(Index) ->>+ LoginService: "Checks for existing token in cookies"
    LoginService ->>+ UserDatabase: "Validates token"
    UserDatabase -->>- LoginService: "Returns validation result"
    LoginService -->>- View(Index): "Sends validation status (valid/invalid)"
    alt Token is valid
        View(Index) -->> User: "Displays authenticated view"
    else Token is invalid or absent
        View(Index) ->>+ User: "Display UnAuthenticated(anonymous) view"
    end
```

3. **Sequence for login when token does not exist:**
```mermaid
sequenceDiagram
    actor User
    participant View(Index)
    participant View(Login)
    participant LoginController
    participant LoginService
    participant UserDatabase

    User ->>+ View(Index): "Clicks Login"
    View(Index) ->>+ View(Login): "Redirects to Login View"
    View(Login) ->>+ LoginController: "Submits username and password /POST"
    LoginController ->>+ LoginService: "Triggers login action"
    LoginService ->>+ UserDatabase: "Validates credentials (hashed formData)"
    UserDatabase -->>- LoginService: "Returns validation result"
    alt Credentials are valid
        LoginService -->> LoginController: "Returns success with token"
        LoginController -->> View(Login): "Responds with success and token"
        View(Login) -->> User: "Displays success message and authenticated view"
    else Credentials are invalid
        LoginService -->>- LoginController: "Returns error"
        LoginController -->>- View(Login): "Responds with error"
        View(Login) -->>- User: "Displays error feedback"
    end
```

4. **Sequence for creating a new user:**
```mermaid
sequenceDiagram
    actor AnonymousUser
    participant LoginController
    participant LoginService
    participant UserDatabase

    AnonymousUser ->>+ LoginController: /POST /New (formData)
    LoginController ->>+ LoginService: Validate existence of required fields, and hash.
    alt validation succeeds
        LoginService ->> UserDatabase: Insert new hashed user data into the database
        UserDatabase -->> LoginService: OK (User created)
        LoginService -->> LoginController: Success response
        LoginController -->> AnonymousUser: HTTP 201 Created (New user created)
    else validation fails
        LoginService -->>- LoginController: Validation error
        LoginController -->>- AnonymousUser: HTTP 400 Bad Request with error message
    end
```

5. **Sequence for fetching movie night data on homepage load:**
```mermaid
sequenceDiagram
    actor User
    participant FilmkveldController
    participant DatabaseContext
    participant FilmkveldDatabase

    User ->>+ FilmkveldController: /GET request
    FilmkveldController ->>+ DatabaseContext: Trigger fetch for public movie nights
    DatabaseContext ->>+ FilmkveldDatabase: Fetch events marked as public
    FilmkveldDatabase -->>- DatabaseContext: Return public events

    alt User is authenticated
        FilmkveldController ->> DatabaseContext: Trigger fetch for user-specific movie nights
        DatabaseContext ->> FilmkveldDatabase: Fetch events where user is owner
        DatabaseContext ->> FilmkveldDatabase: Fetch events where user is participant
        FilmkveldDatabase -->> DatabaseContext: Return owner events
        FilmkveldDatabase -->> DatabaseContext: Return participant events
        DatabaseContext -->> FilmkveldController: Combine public and user-specific events
        FilmkveldController -->> User: Return JSON (DTO with public and user-specific events)
    else User is anonymous
        DatabaseContext -->>- FilmkveldController: Return public events only
        FilmkveldController -->>- User: Return JSON (DTO with public events)
    end
```

6. **Sequence for creating a new movie night:**
```mermaid
sequenceDiagram
    actor User
    participant FilmkveldController
    participant DtoConstructor
    participant DatabaseContext
    participant FilmkveldDatabase

    User ->>+ FilmkveldController: /POST /(jsonData)
    FilmkveldController ->>+ DtoConstructor: Validate and construct DTO from jsonData
    alt DTO validation succeeds
        DtoConstructor ->> DatabaseContext: Pass DTO with User as Owner
        DatabaseContext ->> FilmkveldDatabase: Insert movie night into database
        DatabaseContext ->> FilmkveldDatabase: Update relation table (Owner, Movie Night)
        FilmkveldDatabase -->> DatabaseContext: OK (Movie night created)
        FilmkveldDatabase -->> DatabaseContext: OK (Relation updated)
        DatabaseContext -->> FilmkveldController: Success response
        FilmkveldController -->> User: HTTP 201 Created with location of new movie night
    else DTO validation fails
        DtoConstructor -->>- FilmkveldController: Validation error
        FilmkveldController -->>- User: HTTP 400 Bad Request with error message
    end
```

#### **Entitetsrelasjonsdiagram (ERD):**
```mermaid
erDiagram
    User {
        int UserId
        string UserName
        string Email
        string HashPassword
    }
    Filmkveld {
        int FilmkveldId
        string Title
        DateTime EventDate
        int MaxParticipants
        bool Public
        int OwnerId
    }
    Votes {
        int VoteId
        int UserId
        int FilmkveldId
        string MovieTitle
    }
    User ||--o| Filmkveld: "Owns"
    User }|--|{ Filmkveld: "Participates in"
    User }|--|{ Votes: "Votes in"
    Filmkveld ||--o| Votes: "Has"
```

#### **Routing Principles:**
We follow the routing principle of `/Area/Controller/Action/Parameter`. For example:
- **Login-related routes:**
  - `/api/Login/Authenticate`
  - `/api/Login/Register`
- **Movie night-related routes:**
  - `/api/Filmkveld/Create`
  - `/api/Filmkveld/GetAll`
  - `/api/Filmkveld/Vote`


