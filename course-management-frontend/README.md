# Course Management System — Frontend

A React frontend application connected to the Course Management API backend.

## Technologies Used

| Technology | Description |
|---|---|
| **React 18** | UI library for building component-based interfaces |
| **React Router v6** | Client-side routing between pages |
| **Axios** | HTTP client for communicating with the backend API |
| **Vite** | Fast development build tool |

## Setup Instructions

### Prerequisites
- Node.js 18+
- The backend API must be running on `http://localhost:5000`

### Run the Frontend

```bash
npm install
npm run dev
```

Open `http://localhost:3000` in your browser.

### Default Login
- Email: `admin@cms.com`
- Password: `Admin@123`
- Role: `Admin`

## Project Structure

```
src/
├── components/
│   └── Navbar.jsx          Navigation bar
├── pages/
│   ├── LoginPage.jsx        Login form
│   ├── HomePage.jsx         Dashboard with stats
│   ├── InstructorsPage.jsx  List, create, delete instructors
│   ├── StudentsPage.jsx     List, create, delete students
│   ├── CoursesPage.jsx      List, create, delete courses
│   └── EnrollmentsPage.jsx  List, create, delete, grade enrollments
├── services/
│   ├── api.js               All Axios API calls
│   └── AuthContext.jsx      Global auth state
├── App.jsx                  Routes and layout
└── main.jsx                 Entry point
```

## API Routes Used

| Method | Endpoint | Used In |
|---|---|---|
| POST | `/api/auth/login` | Login page |
| GET | `/api/instructors` | Instructors page, Courses form |
| POST | `/api/instructors` | Instructors page |
| DELETE | `/api/instructors/:id` | Instructors page |
| GET | `/api/students` | Students page, Enrollments form |
| POST | `/api/students` | Students page |
| DELETE | `/api/students/:id` | Students page |
| GET | `/api/courses` | Courses page, Enrollments form |
| POST | `/api/courses` | Courses page |
| DELETE | `/api/courses/:id` | Courses page |
| GET | `/api/enrollments` | Enrollments page |
| POST | `/api/enrollments` | Enrollments page |
| PUT | `/api/enrollments/:id` | Grade update |
| DELETE | `/api/enrollments/:id` | Enrollments page |
