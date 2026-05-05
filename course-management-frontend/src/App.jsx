import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './services/AuthContext'
import Navbar from './components/Navbar'
import LoginPage from './pages/LoginPage'
import HomePage from './pages/HomePage'
import InstructorsPage from './pages/InstructorsPage'
import StudentsPage from './pages/StudentsPage'
import CoursesPage from './pages/CoursesPage'
import EnrollmentsPage from './pages/EnrollmentsPage'

function ProtectedRoute({ children }) {
  const { user } = useAuth()
  if (!user) return <Navigate to="/login" replace />
  return children
}

function AppRoutes() {
  const { user } = useAuth()

  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/login" element={user ? <Navigate to="/" replace /> : <LoginPage />} />
        <Route path="/" element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
        <Route path="/instructors" element={<ProtectedRoute><InstructorsPage /></ProtectedRoute>} />
        <Route path="/students" element={<ProtectedRoute><StudentsPage /></ProtectedRoute>} />
        <Route path="/courses" element={<ProtectedRoute><CoursesPage /></ProtectedRoute>} />
        <Route path="/enrollments" element={<ProtectedRoute><EnrollmentsPage /></ProtectedRoute>} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  )
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  )
}
