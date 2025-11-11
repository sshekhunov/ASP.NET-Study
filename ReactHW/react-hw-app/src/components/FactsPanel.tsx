import './FactsPanel.css'

interface FactsPanelProps {
  facts: Array<{
    fact: string
    length: number
  }>
}

function FactsPanel({ facts }: FactsPanelProps) {
  return (
    <div className="facts-panel">
      <h2>Cat Facts</h2>
      <ul>
        {facts.map((item, index) => (
          <li key={index}>{item.fact}</li>
        ))}
      </ul>
    </div>
  )
}

export default FactsPanel

