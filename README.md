# TaskFlow

**TaskFlow** is an ASP.NET Core 8 MVC application for managing small
projects and tasks.  
The solution follows an **N-tier architecture** with clear separation of
concerns and uses **Entity Framework Core** and **ASP.NET Identity** for
persistence and authentication.

This project was built without scaffolding -- login, registration, and
role management are implemented manually, following professional coding
practices.

------------------------------------------------------------------------

## ✨ Features

-   **Authentication & Authorization**
    -   Implemented with **ASP.NET Identity**
    -   Roles: **Administrator**, **Project Manager**, **Developer**
    -   Custom login & registration (no scaffolding)
    -   **Email confirmation flow implemented**:
        -   After registration, a confirmation link is sent to the user's email
        -   Users cannot log in until they confirm their email
        -   Ensures system integrity and prevents fake accounts
        - **Added rate limiting for email confirmation resend:**
            - Max **5 requests per 10 minutes per user**
            - Redirects to **TooManyRequests** page if limit exceeded
            - Prevents abuse, spam, and brute-force attacks on email system
    -   **Password reset flow**:
        -   Forgot password form sends a secure reset link to the user's email
        -   Reset link opens a form where the user sets a new password
        -   Ensures secure recovery and prevents unauthorized access
        - **Added rate limiting for password reset requests:**
            -   Prevents a single user from sending more than 5 reset requests in 10 minutes
            -   Protects the app from spam, brute-force, and denial-of-service attacks
            -   Implemented using IMemoryCache for lightweight and in-memory request tracking
            -   Users exceeding the limit are redirected to a dedicated “Too Many Requests” page with a friendly message
            -   Enhances both security and user experience
-   **Projects**
    -   Each project has a unique code and a name
    -   Must have an assigned **Project Manager**
    -   Progress is automatically calculated from associated tasks
-   **Tasks**
    -   Belong to a single project
    -   Attributes: title, description, deadline, estimated time,
        status, progress (0--100)
    -   Can be assigned to a developer or remain unassigned
    -   Developers cannot hold more than **3 active tasks**
-   **Role-based rules**
    -   **Administrator**
        -   Full CRUD on projects, tasks, and users
        -   Assigns Project Managers to projects
        -   Assigns/unassigns tasks to developers
    -   **Project Manager**
        -   Creates projects and tasks
        -   Assigns tasks to developers
        -   Can update assignee, status, progress, deadline, description
        -   Has visibility over all projects, tasks, and users
    -   **Developer**
        -   Sees tasks assigned to them or unassigned tasks
        -   Can update their own tasks (status, progress, description)
        -   Limited to 3 active tasks maximum
-   **Comments**
    -   Tasks and projects support comments
    -   All users can see comments for projects and tasks they are
        assigned to
    -   PMs and Admins can comment on any project or task

## ⏰ Background Jobs with Hangfire

-   Integrated **Hangfire** for background job scheduling and execution  
-   Implemented **ProjectEndReminderJob**:
    -   Automatically schedules an email reminder **5 days before the project deadline**
    -   Reminder is sent to the assigned **Project Manager**
    -   Job tracking fields (`ReminderJobId`, `ReminderSent`) are stored in the `Project` entity
-   Configured **Hangfire Dashboard** (`/hangfire`) to monitor, reschedule, or cancel jobs  
-   Ensures reliable notifications without blocking the main request pipeline

## 🧪 Unit Testing

- Implemented **unit tests** for repositories, controllers, and background jobs using **xUnit** and **Moq**
- Used **EF Core InMemory Database** for testing repository operations without touching the production database
- Mocked services like **UserManager** and **IEmailService** to test business logic independently
- Covered CRUD operations, business rules, and Hangfire job scheduling logic
- Ensures **robustness and reliability** of application logic while enabling safe refactoring

------------------------------------------------------------------------

## 🛠️ Tech Stack & Architecture

-   **ASP.NET Core 8 MVC** -- web framework  
-   **Entity Framework Core** -- ORM  
-   **ASP.NET Identity** -- authentication & role management  
-   **SQL Server** -- database  
-   **Bootstrap 5** -- UI styling  
-   **N-tier architecture**
    -   `TaskFlow` -- MVC layer (controllers, views, startup)  
    -   `TaskFlow.Data` -- database context & migrations  
    -   `TaskFlow.Models` -- domain models (Project, TaskItem, User,
        Comment, etc.)  
    -   `TaskFlow.Utility` -- repositories, interfaces, services,
        seeders

This separation ensures maintainability, testability, and scalability.

------------------------------------------------------------------------

## 🚀 Usage Scenarios

-   **Admin**
    -   Creates projects and assigns a PM
    -   Manages all tasks and users
-   **PM**
    -   Manages projects and tasks
    -   Assigns tasks to developers
    -   Tracks overall project progress
-   **Developer**
    -   Works only on assigned tasks (max 3 at a time)
    -   Updates their own task's progress and status

------------------------------------------------------------------------

## 🔐 Security Enhancements

- Manual **email confirmation flow** before login  
- Manual **password reset implementation** (no scaffolding)  
- **Rate limiting for password reset requests** (5 requests / 10 minutes per user)  
- **Rate limiting for email confirmation resend** (5 requests / 10 minutes per user)  
- Users exceeding the limit are redirected to **TooManyRequests** view  
- All flows return generic responses to avoid information disclosure  
  (system does not reveal if email exists or not)

------------------------------------------------------------------------

## ▶️ Getting Started

1.  Clone the repository

    ``` bash
    git clone https://github.com/your-username/TaskFlow.git
    ```

2.  Update the connection string in `appsettings.json`

3.  Apply migrations

    ``` bash
    dotnet ef database update
    ```

4.  Run the app

    ``` bash
    dotnet run
    ```

------------------------------------------------------------------------

## 📂 Solution Structure

    TaskFlow.sln
     ┣ TaskFlow                → MVC layer (Controllers, Views, Startup)
     ┣ TaskFlow.Data           → EF DbContext & Migrations
     ┣ TaskFlow.Models         → Domain models (Entities & ViewModels)
     ┣ TaskFlow.Utility        → Repository pattern, Interfaces, Services, Seeders

------------------------------------------------------------------------

## 📌 Notes

This project was implemented with a focus on professional standards:  
- Repository pattern & Dependency Injection  
- N-tier architecture  
- Clean separation of concerns  
- Identity integration without scaffolding  
- Email confirmation before login implemented for better security and reliability 
- Password reset implemented manually for improved security and reliability
- Extensible design for future features (email, tests, UI improvements)

------------------------------------------------------------------------
