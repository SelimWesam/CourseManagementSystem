import { useState, useEffect } from 'react'
import { getCourses, deleteCourse, createCourse, getInstructors } from '../services/api'

export default function CoursesPage() {
  const [courses, setCourses] = useState([])
  const [instructors, setInstructors] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState({ title: '', description: '', credits: 3, instructorId: '' })
  const [formError, setFormError] = useState('')
  const [formLoading, setFormLoading] = useState(false)

  const fetchData = async () => {
    try {
      const [cRes, iRes] = await Promise.all([getCourses(), getInstructors()])
      setCourses(cRes.data)
      setInstructors(iRes.data)
      if (iRes.data.length > 0) {
        setForm(f => ({ ...f, instructorId: iRes.data[0].id }))
      }
    } catch {
      setError('Failed to load courses.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchData() }, [])

  const handleDelete = async (id) => {
    if (!confirm('Delete this course?')) return
    try {
      await deleteCourse(id)
      setCourses(courses.filter(c => c.id !== id))
      setSuccess('Course deleted.')
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
      await createCourse({ ...form, credits: parseInt(form.credits), instructorId: parseInt(form.instructorId) })
      setShowModal(false)
      setForm({ title: '', description: '', credits: 3, instructorId: instructors[0]?.id || '' })
      setSuccess('Course created!')
      setTimeout(() => setSuccess(''), 3000)
      fetchData()
    } catch (err) {
      setFormError(err.response?.data?.title || 'Failed to create course.')
    } finally {
      setFormLoading(false)
    }
  }

  return (
    <div style={{ padding: '40px 32px', maxWidth: '1100px', margin: '0 auto' }}>
      <div className="page-header">
        <h1 className="page-title">Courses</h1>
        <button className="btn btn-primary" onClick={() => setShowModal(true)}>+ Add Course</button>
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {loading ? (
        <div className="loading">Loading...</div>
      ) : courses.length === 0 ? (
        <div className="empty">No courses yet.</div>
      ) : (
        <div className="card" style={{ padding: 0, overflow: 'hidden' }}>
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Instructor</th>
                <th>Credits</th>
                <th>Enrollments</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {courses.map(course => (
                <tr key={course.id}>
                  <td>
                    <div style={{ fontWeight: 500 }}>{course.title}</div>
                    <div style={{ fontSize: '12px', color: 'var(--text-muted)', marginTop: '2px' }}>
                      {course.description.slice(0, 60)}...
                    </div>
                  </td>
                  <td style={{ color: 'var(--text-muted)' }}>{course.instructorName}</td>
                  <td>
                    <span style={{ background: 'rgba(108,99,255,0.15)', color: 'var(--accent)', padding: '2px 10px', borderRadius: '20px', fontSize: '13px', fontWeight: 600 }}>
                      {course.credits} cr
                    </span>
                  </td>
                  <td style={{ color: 'var(--text-muted)' }}>{course.enrollmentCount} students</td>
                  <td>
                    <button
                      className="btn btn-danger"
                      style={{ padding: '5px 12px', fontSize: '12px' }}
                      onClick={() => handleDelete(course.id)}
                    >
                      Delete
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
            <h2 className="modal-title">Add Course</h2>
            {formError && <div className="alert alert-error">{formError}</div>}
            <form onSubmit={handleCreate}>
              <div className="form-group">
                <label>Title</label>
                <input name="title" value={form.title} onChange={handleChange} required placeholder="e.g. Introduction to Programming" />
              </div>
              <div className="form-group">
                <label>Description</label>
                <input name="description" value={form.description} onChange={handleChange} required placeholder="Course description" />
              </div>
              <div className="form-group">
                <label>Credits (1-6)</label>
                <input type="number" name="credits" value={form.credits} onChange={handleChange} min="1" max="6" required />
              </div>
              <div className="form-group">
                <label>Instructor</label>
                <select name="instructorId" value={form.instructorId} onChange={handleChange} required>
                  {instructors.map(i => (
                    <option key={i.id} value={i.id}>{i.fullName}</option>
                  ))}
                </select>
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-ghost" onClick={() => setShowModal(false)}>Cancel</button>
                <button type="submit" className="btn btn-primary" disabled={formLoading}>
                  {formLoading ? 'Creating...' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
