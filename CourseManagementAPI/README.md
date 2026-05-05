# Course Management System API

A RESTful API built with ASP.NET Core 8 and Entity Framework Core for managing courses, instructors, students, and enrollments. Implements JWT authentication, role-based authorization, DTO validation, and optimized LINQ queries.

---

## Technologies Used

| Technology | Description |
|---|---|
| **ASP.NET Core 8** | Cross-platform framework for building Web APIs. Provides the HTTP pipeline, routing, middleware, and dependency injection container. |
| **Entity Framework Core 8** | ORM (Object-Relational Mapper) that maps C# classes to database tables. Used for all database operations and migrations. |
| **SQL Server / LocalDB** | Relational database engine. LocalDB is used in development; can be swapped for full SQL Server in production. |
| **JWT (JSON Web Tokens)** | Stateless authentication standard. Tokens are signed with HMAC-SHA256 and carry user identity and role claims. |
| **BCrypt.Net-Next** | Password hashing library. Passwords are never stored in plain text — BCrypt applies a salt and cost factor before storing. |
| **Swashbuckle / Swagger** | Auto-generates interactive API documentation from controller attributes. Allows testing endpoints directly from the browser. |
| **Microsoft.AspNetCore.Authentication.JwtBearer** | ASP.NET Core middleware that validates incoming JWT tokens on protected routes. |
| **System.IdentityModel.Tokens.Jwt** | Library for creating and reading JWT tokens, including claim management and signature verification. |

---

## How to Run the Project

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio) **or** a full SQL Server instance
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension

### Step 1 — Clone the Repository
```bash
git clone <your-repo-url>
cd CourseManagementAPI
```

### Step 2 — Configure the Database Connection
Open `appsettings.json` and update the connection string if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CourseManagementDb;Trusted_Connection=True;"
}
```
For a full SQL Server instance, replace with:
```
Server=YOUR_SERVER;Database=CourseManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;
```

### Step 3 — Apply EF Core Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This creates the database schema and seeds a default Admin account.

### Step 4 — Run the Application
```bash
dotnet run
```
The API will start at `https://localhost:5001` (or `http://localhost:5000`).  
Swagger UI is available at the root URL: `https://localhost:5001/`

---

## Default Seeded Admin Account

| Field | Value |
|---|---|
| Email | `admin@cms.com` |
| Password | `Admin@123` |
| Role | `Admin` |

---

## API Endpoints

### Authentication
| Method | Endpoint | Access | Description |
|---|---|---|---|
| POST | `/api/auth/login` | Public | Login and receive a JWT token |

**Login body:**
```json
{
  "email": "admin@cms.com",
  "password": "Admin@123",
  "role": "Admin"
}
```
Role must be one of: `Admin`, `Instructor`, `Student`

---

### Instructors
| Method | Endpoint | Roles | Description |
|---|---|---|---|
| GET | `/api/instructors` | Admin | Get all instructors |
| GET | `/api/instructors/{id}` | Admin, Instructor | Get instructor by ID |
| POST | `/api/instructors` | Admin | Create instructor |
| PUT | `/api/instructors/{id}` | Admin | Update instructor |
| DELETE | `/api/instructors/{id}` | Admin | Delete instructor |
| POST | `/api/instructors/{id}/profile` | Admin, Instructor | Add profile to instructor |
| PUT | `/api/instructors/{id}/profile` | Admin, Instructor | Update instructor profile |

---

### Students
| Method | Endpoint | Roles | Description |
|---|---|---|---|
| GET | `/api/students` | Admin | Get all students |
| GET | `/api/students/{id}` | Admin, Student | Get student by ID |
| POST | `/api/students` | **Public** | Register a new student |
| PUT | `/api/students/{id}` | Admin | Update student |
| DELETE | `/api/students/{id}` | Admin | Delete student |
| GET | `/api/students/{id}/courses` | Admin, Instructor, Student | Get enrolled courses |

---

### Courses
| Method | Endpoint | Roles | Description |
|---|---|---|---|
| GET | `/api/courses` | Admin, Instructor, Student | Get all courses |
| GET | `/api/courses/{id}` | Admin, Instructor, Student | Get course by ID |
| POST | `/api/courses` | Admin, Instructor | Create course |
| PUT | `/api/courses/{id}` | Admin, Instructor | Update course |
| DELETE | `/api/courses/{id}` | Admin | Delete course |
| GET | `/api/courses/{id}/students` | Admin, Instructor | Get enrolled students |

---

### Enrollments
| Method | Endpoint | Roles | Description |
|---|---|---|---|
| GET | `/api/enrollments` | Admin | Get all enrollments |
| GET | `/api/enrollments/{id}` | Admin, Instructor | Get enrollment by ID |
| POST | `/api/enrollments` | Admin | Enroll student in course |
| PUT | `/api/enrollments/{id}` | Admin, Instructor | Update student grade |
| DELETE | `/api/enrollments/{id}` | Admin | Remove enrollment |

---

## How to Authenticate in Swagger

1. Call `POST /api/auth/login` with valid credentials to receive a token.
2. Click the **Authorize** button (🔒) at the top of the Swagger UI.
3. Enter: `Bearer <your_token_here>`
4. Click **Authorize** — all subsequent requests will include the token.

---

## Entity Relationships

```
Instructor ──(1:1)──► InstructorProfile
Instructor ──(1:N)──► Course
Student    ──(M:N)──► Course   (via Enrollment join table)
```

---

## Why HTTP-Only Cookies Are an Industry Standard for Authentication Security

When building authentication systems, there are two common approaches for storing tokens on the client: **localStorage/sessionStorage** and **HTTP-only cookies**.

**HTTP-only cookies are the preferred industry standard** for the following reasons:

### 1. Protection Against XSS (Cross-Site Scripting)
Tokens stored in `localStorage` or `sessionStorage` are accessible via JavaScript (`document.cookie` / `localStorage.getItem`). If an attacker injects malicious JavaScript into your page — even through a third-party library — they can steal the token instantly.

HTTP-only cookies **cannot be read by JavaScript at all**. The browser automatically attaches them to requests but never exposes their value to any script. This makes XSS attacks unable to steal the authentication token.

### 2. Automatic Transmission by the Browser
The browser automatically sends HTTP-only cookies with every matching request. Developers don't need to manually attach the token in the `Authorization` header, which reduces the chance of implementation errors.

### 3. Secure Flag + SameSite Protection
HTTP-only cookies can be combined with:
- `Secure` flag — ensures cookies are only transmitted over HTTPS
- `SameSite=Strict` or `SameSite=Lax` — protects against CSRF (Cross-Site Request Forgery) attacks by restricting which cross-origin requests can include the cookie

### 4. Server-Side Expiry Control
With HTTP-only cookies, the server can set an expiry and revoke sessions server-side. With localStorage tokens, the client controls whether the token persists.

### Summary

| | localStorage / sessionStorage | HTTP-Only Cookie |
|---|---|---|
| Accessible by JS | ✅ Yes (vulnerable to XSS) | ❌ No (safe) |
| Auto-sent by browser | ❌ No (manual) | ✅ Yes |
| CSRF protection | ✅ Not an issue | ⚠️ Needs SameSite |
| HTTPS enforcement | Manual | Via `Secure` flag |
| Industry standard | Not recommended for tokens | ✅ Recommended |

> **Note:** This project uses JWT via the `Authorization: Bearer` header for simplicity and Swagger compatibility, which is common in API-first or mobile app contexts. HTTP-only cookies are the preferred approach for browser-based web applications.

---

## Project Structure

```
CourseManagementAPI/
├── Controllers/
│   ├── AuthController.cs
│   ├── InstructorsController.cs
│   ├── StudentsController.cs
│   ├── CoursesController.cs
│   └── EnrollmentsController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── Request/
│   │   ├── InstructorDtos.cs
│   │   ├── InstructorProfileDtos.cs
│   │   ├── StudentDtos.cs
│   │   ├── CourseDtos.cs
│   │   └── EnrollmentDtos.cs (includes LoginDto)
│   └── Response/
│       └── ResponseDtos.cs
├── Helpers/
│   └── JwtHelper.cs
├── Models/
│   ├── Admin.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   ├── Instructor.cs
│   ├── InstructorProfile.cs
│   └── Student.cs
├── Services/
│   ├── Interfaces/
│   │   └── IServices.cs
│   ├── AuthService.cs
│   ├── CourseService.cs
│   ├── EnrollmentService.cs
│   ├── InstructorService.cs
│   └── StudentService.cs
├── appsettings.json
├── appsettings.Development.json
└── Program.cs
```
