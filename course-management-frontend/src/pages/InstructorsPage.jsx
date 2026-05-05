import { useState, useEffect } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { getInstructors, deleteInstructor, createInstructor } from '../services/api'

export default function InstructorsPage() {
  const [instructors, setInstructors] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState({ fullName: '', email: '', password: '' })
  const [formError, setFormError] = useState('')
  const [formLoading, setFormLoading] = useState(false)
  const [success, setSuccess] = useState('')

  const fetchInstructors = async () => {
    try {
      const res = await getInstructors()
      setInstructors(res.data)
    } catch {
      setError('Failed to load instructors.')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { fetchInstructors() }, [])

  const handleDelete = async (id) => {
    if (!confirm('Delete this instructor?')) return
    try {
      await deleteInstructor(id)
      setInstructors(instructors.filter(i => i.id !== id))
      setSuccess('Instructor deleted.')
      setTimeout(() => setSuccess(''), 3000)
    } catch {
      setError('Failed to delete instructor.')
    }
  }

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const handleCreate = async (e) => {
    e.preventDefault()
    setFormLoading(true)
    setFormError('')
    try {
      await createInstructor(form)
      setShowModal(false)
      setForm({ fullName: '', email: '', password: '' })
      setSuccess('Instructor created successfully!')
      setTimeout(() => setSuccess(''), 3000)
      fetchInstructors()
    } catch (err) {
      setFormError(err.response?.data?.title || 'Failed to create instructor.')
    } finally {
      setFormLoading(false)
    }
  }

  return (
    <div style={{ padding: '40px 32px', maxWidth: '1100px', margin: '0 auto' }}>
      <div className="page-header">
        <h1 className="page-title">Instructors</h1>
        <button className="btn btn-primary" onClick={() => setShowModal(true)}>
          + Add Instructor
        </button>
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {loading ? (
        <div className="loading">Loading...</div>
      ) : instructors.length === 0 ? (
        <div className="empty">No instructors yet. Add one!</div>
      ) : (
        <div className="card" style={{ padding: 0, overflow: 'hidden' }}>
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
                <th>Profile</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {instructors.map(instructor => (
                <tr key={instructor.id}>
                  <td style={{ fontWeight: 500 }}>{instructor.fullName}</td>
                  <td style={{ color: 'var(--text-muted)' }}>{instructor.email}</td>
                  <td><span className="badge badge-instructor">{instructor.role}</span></td>
                  <td style={{ color: 'var(--text-muted)', fontSize: '13px' }}>
                    {instructor.profile ? instructor.profile.department : '—'}
                  </td>
                  <td>
                    <button
                      className="btn btn-danger"
                      style={{ padding: '5px 12px', fontSize: '12px' }}
                      onClick={() => handleDelete(instructor.id)}
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
            <h2 className="modal-title">Add Instructor</h2>
            {formError && <div className="alert alert-error">{formError}</div>}
            <form onSubmit={handleCreate}>
              <div className="form-group">
                <label>Full Name</label>
                <input name="fullName" value={form.fullName} onChange={handleChange} required placeholder="e.g. Dr. Jane Smith" />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input type="email" name="email" value={form.email} onChange={handleChange} required placeholder="jane@university.com" />
              </div>
              <div className="form-group">
                <label>Password</label>
                <input type="password" name="password" value={form.password} onChange={handleChange} required placeholder="Min 6 characters" />
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
