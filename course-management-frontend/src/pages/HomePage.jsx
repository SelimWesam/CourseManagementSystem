import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { getInstructors, getStudents, getCourses, getEnrollments } from '../services/api'
import { useAuth } from '../services/AuthContext'

export default function HomePage() {
  const { user } = useAuth()
  const [stats, setStats] = useState({ instructors: 0, students: 0, courses: 0, enrollments: 0 })
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const [i, s, c, e] = await Promise.all([
          getInstructors(), getStudents(), getCourses(), getEnrollments()
        ])
        setStats({
          instructors: i.data.length,
          students: s.data.length,
          courses: c.data.length,
          enrollments: e.data.length,
        })
      } catch {
        // ignore
      } finally {
        setLoading(false)
      }
    }
    fetchStats()
  }, [])

  const cards = [
    { label: 'Instructors', value: stats.instructors, path: '/instructors', color: '#6c63ff' },
    { label: 'Students', value: stats.students, path: '/students', color: '#ff6584' },
    { label: 'Courses', value: stats.courses, path: '/courses', color: '#4ade80' },
    { label: 'Enrollments', value: stats.enrollments, path: '/enrollments', color: '#facc15' },
  ]

  return (
    <div style={{ padding: '40px 32px', maxWidth: '1100px', margin: '0 auto' }}>
      <div style={{ marginBottom: '40px' }}>
        <h1 style={{ fontFamily: 'var(--font-heading)', fontSize: '32px', fontWeight: 800 }}>
          Welcome back, {user?.fullName} 👋
        </h1>
        <p style={{ color: 'var(--text-muted)', marginTop: '6px' }}>
          Here's an overview of your Course Management System.
        </p>
      </div>

      {loading ? (
        <div className="loading">Loading stats...</div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))', gap: '20px' }}>
          {cards.map(({ label, value, path, color }) => (
            <Link to={path} key={label} style={{ textDecoration: 'none' }}>
              <div className="card" style={{
                borderColor: 'var(--border)',
                transition: 'all 0.2s',
                cursor: 'pointer',
              }}
                onMouseEnter={e => e.currentTarget.style.borderColor = color}
                onMouseLeave={e => e.currentTarget.style.borderColor = 'var(--border)'}
              >
                <div style={{ fontSize: '13px', color: 'var(--text-muted)', fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.07em' }}>
                  {label}
                </div>
                <div style={{ fontSize: '42px', fontFamily: 'var(--font-heading)', fontWeight: 800, color, marginTop: '8px' }}>
                  {value}
                </div>
                <div style={{ fontSize: '13px', color: 'var(--text-muted)', marginTop: '4px' }}>
                  View all →
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}

      <div style={{ marginTop: '40px' }} className="card">
        <h2 style={{ fontFamily: 'var(--font-heading)', fontSize: '18px', fontWeight: 700, marginBottom: '16px' }}>
          Quick Actions
        </h2>
        <div style={{ display: 'flex', gap: '12px', flexWrap: 'wrap' }}>
          <Link to="/instructors/new" className="btn btn-primary">+ Add Instructor</Link>
          <Link to="/students/new" className="btn btn-ghost">+ Add Student</Link>
          <Link to="/courses/new" className="btn btn-ghost">+ Add Course</Link>
          <Link to="/enrollments/new" className="btn btn-ghost">+ Enroll Student</Link>
        </div>
      </div>
    </div>
  )
}
