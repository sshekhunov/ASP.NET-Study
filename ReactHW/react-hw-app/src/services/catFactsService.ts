export interface CatFact {
  fact: string
  length: number
}

interface CatFactsResponse {
  data: CatFact[]
}

const API_URL = 'https://catfact.ninja/facts'

export const fetchCatFacts = async (): Promise<CatFact[]> => {
  const response = await fetch(API_URL);
  
  if (!response.ok) {
    throw new Error(`Error: ${response.status} ${response.statusText}`)
  }
  
  const res: CatFactsResponse = await response.json()
  return res.data || []
}

