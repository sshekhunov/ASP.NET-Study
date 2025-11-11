import { useState } from 'react'
import './App.css'
import FactsPanel from './components/FactsPanel'
import ErrorPanel from './components/ErrorPanel'

interface CatFact {
  fact: string
  length: number
}

interface CatFactsResponse {
  data: CatFact[]
}

function App() {
  const [facts, setFacts] = useState<CatFact[]>([])
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const [hasFetched, setHasFetched] = useState<boolean>(false)

  const handleFetch = async () => {
    try {
      setIsLoading(true)
      setError(null)
      setHasFetched(true)
      
      const response = await fetch('https://catfact.ninja/facts')
      
      if (!response.ok) {
        throw new Error(`Ошибка: ${response.status} ${response.statusText}`)
      }
      
      const data: CatFactsResponse = await response.json()
      setFacts(data.data || [])
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Произошла неизвестная ошибка')
      setFacts([])
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="app">
      <h1>Cat facts</h1>
      <button onClick={handleFetch} disabled={isLoading}>
        {isLoading ? 'Loading...' : 'Load'}
      </button>
      {hasFetched && !isLoading && error && <ErrorPanel error={error} />}
      {hasFetched && !isLoading && !error && facts.length > 0 && <FactsPanel facts={facts} />}
    </div>
  )
}

export default App
