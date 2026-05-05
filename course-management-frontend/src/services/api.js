import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' }
})

// Attach JWT token to every request automatically
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Redirect to login on 401
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

// ── Auth ─────────────────────────────────────────────────────────────────────
export const login = (data) => api.post('/auth/login', data)

// ── Instructors ───────────────────────────────────────────────────────────────
export const getInstructors = () => api.get('/instructors')
export const getInstructor = (id) => api.get(`/instructors/${id}`)
export const createInstructor = (data) => api.post('/instructors', data)
export const updateInstructor = (id, data) => api.put(`/instructors/${id}`, data)
export const deleteInstructor = (id) => api.delete(`/instructors/${id}`)

// ── Students ──────────────────────────────────────────────────────────────────
export const getStudents = () => api.get('/students')
export const getStudent = (id) => api.get(`/students/${id}`)
export const createStudent = (data) => api.post('/students', data)
export const updateStudent = (id, data) => api.put(`/students/${id}`, data)
export const deleteStudent = (id) => api.delete(`/students/${id}`)

// ── Courses ───────────────────────────────────────────────────────────────────
export const getCourses = () => api.get('/courses')
export const getCourse = (id) => api.get(`/courses/${id}`)
export const createCourse = (data) => api.post('/courses', data)
export const updateCourse = (id, data) => api.put(`/courses/${id}`, data)
export const deleteCourse = (id) => api.delete(`/courses/${id}`)

// ── Enrollments ───────────────────────────────────────────────────────────────
export const getEnrollments = () => api.get('/enrollments')
export const createEnrollment = (data) => api.post('/enrollments', data)
export const updateEnrollment = (id, data) => api.put(`/enrollments/${id}`, data)
export const deleteEnrollment = (id) => api.delete(`/enrollments/${id}`)

export default api
