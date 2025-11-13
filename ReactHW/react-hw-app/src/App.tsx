import { useState } from 'react';
import './App.css';
import FactsPanel from './components/FactsPanel';
import ErrorPanel from './components/ErrorPanel';
import { fetchCatFacts, type CatFact } from './services/catFactsService';

function App() {
  const [facts, setFacts] = useState<CatFact[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleFetch = async () => {
    try {
      setIsLoading(true);
      setError(null);
      
      const data = await fetchCatFacts();
      setFacts(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unknown error');
      setFacts([]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="app">
      <h1>Cat facts</h1>
      <button onClick={handleFetch} disabled={isLoading}>
        {isLoading ? 'Loading...' : 'Load'}
      </button>
      {!isLoading && error && <ErrorPanel error={error} />}
      {!isLoading && !error && facts.length > 0 && <FactsPanel facts={facts} />}
    </div>
  )
}

export default App;
