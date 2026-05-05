import { useState, useEffect } from 'react'
import { getEnrollments, deleteEnrollment, createEnrollment, getStudents, getCourses, updateEnrollment } from '../services/api'

export default function EnrollmentsPage() {
  const [enrollments, setEnrollments] = useState([])
  const [students, setStudents] = useState([])
  const [courses, setCourses] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState({ studentId: '', courseId: '' })
  const [formError, setFormError] = useState('')
  const [formLoading, setFormLoading] = useState(false)

  const fetchData = async () => {
    try {
      const [eRes, sRes, cRes] = await Promise.all([getEnrollments(), getStudents(), getCourses()])
      setEnrollments(eRes.data)
      setStudents(sRes.data)
      setCourses(cRes.data)
      if (sRes.data.length > 0) setForm(f => ({ ...f, studentId: sRes.data[0].id }))
      if (cRes.data.length > 0) setForm(f => ({ ...f, courseId: cRes.data[0].id }))
    } catch {
      setError('Failed to load enrollments.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchData() }, [])

  const handleDelete = async (id) => {
    if (!confirm('Remove this enrollment?')) return
    try {
      await deleteEnrollment(id)
      setEnrollments(enrollments.filter(e => e.id !== id))
      setSuccess('Enrollment removed.')
      setTimeout(() => setSuccess(''), 3000)
    } catch {
      setError('Failed to delete.')
    }
  }

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const handleCreate = async (e) => {
    e.preventDefault()
    setFormLoading(true)
    setFormError('')
    try {
      await createEnrollment({ studentId: parseInt(form.studentId), courseId: parseInt(form.courseId) })
      setShowModal(false)
      setSuccess('Student enrolled successfully!')
      setTimeout(() => setSuccess(''), 3000)
      fetchData()
    } catch (err) {
      setFormError(err.response?.data?.message || 'Enrollment failed. Student may already be enrolled.')
    } finally {
      setFormLoading(false)
    }
  }

  const handleGradeUpdate = async (id, grade) => {
    try {
      await updateEnrollment(id, { grade })
      setEnrollments(enrollments.map(e => e.id === id ? { ...e, grade } : e))
      setSuccess('Grade updated!')
      setTimeout(() => setSuccess(''), 3000)
    } catch {
      setError('Failed to update grade.')
    }
  }

  return (
    <div style={{ padding: '40px 32px', maxWidth: '1100px', margin: '0 auto' }}>
      <div className="page-header">
        <h1 className="page-title">Enrollments</h1>
        <button className="btn btn-primary" onClick={() => setShowModal(true)}>+ Enroll Student</button>
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {loading ? (
        <div className="loading">Loading...</div>
      ) : enrollments.length === 0 ? (
        <div className="empty">No enrollments yet.</div>
      ) : (
        <div className="card" style={{ padding: 0, overflow: 'hidden' }}>
          <table>
            <thead>
              <tr>
                <th>Student</th>
                <th>Course</th>
                <th>Enrolled At</th>
                <th>Grade</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {enrollments.map(enrollment => (
                <tr key={enrollment.id}>
                  <td style={{ fontWeight: 500 }}>{enrollment.studentName}</td>
                  <td style={{ color: 'var(--text-muted)' }}>{enrollment.courseName}</td>
                  <td style={{ color: 'var(--text-muted)', fontSize: '13px' }}>
                    {new Date(enrollment.enrolledAt).toLocaleDateString()}
                  </td>
                  <td>
                    <select
                      value={enrollment.grade}
                      onChange={e => handleGradeUpdate(enrollment.id, e.target.value)}
                      style={{
                        background: 'var(--surface2)',
                        border: '1px solid var(--border)',
                        borderRadius: '6px',
                        padding: '4px 8px',
                        color: 'var(--text)',
                        fontSize: '13px'
                      }}
                    >
                      {['N/A','A','B','C','D','F'].map(g => <option key={g} value={g}>{g}</option>)}
                    </select>
                  </td>
                  <td>
                    <button
                      className="btn btn-danger"
                      style={{ padding: '5px 12px', fontSize: '12px' }}
                      onClick={() => handleDelete(enrollment.id)}
                    >
                      Remove
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2 className="modal-title">Enroll Student</h2>
            {formError && <div className="alert alert-error">{formError}</div>}
            <form onSubmit={handleCreate}>
              <div className="form-group">
                <label>Student</label>
                <select name="studentId" value={form.studentId} onChange={handleChange} required>
                  {students.map(s => <option key={s.id} value={s.id}>{s.fullName}</option>)}
                </select>
              </div>
              <div className="form-group">
                <label>Course</label>
                <select name="courseId" value={form.courseId} onChange={handleChange} required>
                  {courses.map(c => <option key={c.id} value={c.id}>{c.title}</option>)}
                </select>
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)}>Cancel</button>
                <button type="submit" className="btn btn-primary" disabled={formLoading}>
                  {formLoading ? 'Enrolling...' : 'Enroll'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
