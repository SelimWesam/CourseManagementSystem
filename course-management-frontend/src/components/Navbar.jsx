import { Link, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../services/AuthContext'

export default function Navbar() {
  const { user, logoutUser } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const handleLogout = () => {
    logoutUser()
    navigate('/login')
  }

  const isActive = (path) => location.pathname.startsWith(path)

  return (
    <nav style={{
      background: 'var(--surface)',
      borderBottom: '1px solid var(--border)',
      padding: '0 32px',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      height: '60px',
      position: 'sticky',
      top: 0,
      zIndex: 50
    }}>
      <div style={{ display: 'flex', alignItems: 'center', gap: '32px' }}>
        <Link to="/" style={{
          fontFamily: 'var(--font-heading)',
          fontWeight: 800,
          fontSize: '18px',
          color: 'var(--accent)',
          letterSpacing: '-0.02em'
        }}>
          CMS
        </Link>

        {user && (
          <div style={{ display: 'flex', gap: '4px' }}>
            {[
              { path: '/instructors', label: 'Instructors' },
              { path: '/students', label: 'Students' },
              { path: '/courses', label: 'Courses' },
              { path: '/enrollments', label: 'Enrollments' },
            ].map(({ path, label }) => (
              <Link key={path} to={path} style={{
                padding: '6px 14px',
                borderRadius: '6px',
                fontSize: '14px',
                fontWeight: 500,
                color: isActive(path) ? 'var(--accent)' : 'var(--text-muted)',
                background: isActive(path) ? 'rgba(108,99,255,0.1)' : 'transparent',
                transition: 'all 0.2s'
              }}>
                {label}
              </Link>
            ))}
          </div>
        )}
      </div>

      {user && (
        <div style={{ display: 'flex', alignItems: 'center', gap: '14px' }}>
          <span style={{ fontSize: '13px', color: 'var(--text-muted)' }}>
            {user.fullName}
          </span>
          <span className={`badge badge-${user.role.toLowerCase()}`}>{user.role}</span>
          <button className="btn btn-ghost" style={{ padding: '6px 14px', fontSize: '13px' }} onClick={handleLogout}>
            Logout
          </button>
        </div>
      )}
    </nav>
  )
}
