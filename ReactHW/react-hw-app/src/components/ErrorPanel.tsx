import './ErrorPanel.css'

interface ErrorPanelProps {
  error: string
}

function ErrorPanel({ error }: ErrorPanelProps) {
  return (
    <div className="error-panel">
      <h2>Ошибка</h2>
      <p>{error}</p>
    </div>
  )
}

export default ErrorPanel

