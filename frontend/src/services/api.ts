import { MediaSearchResult } from "@/types/mediaSearchResult";

const API_URL = process.env.EXPO_PUBLIC_API_URL;

export async function testConnection(): Promise<string> {
  if (!API_URL) {
    throw new Error('API URL is not configured');
  }

  const response = await fetch(`${API_URL}/api/test`);

  if (!response.ok) {
    throw new Error(`API request failed: ${response.status}`);
  }

  const data: { message: string } = await response.json();

  return data.message;
}

export async function movieSearch(query :string, signal?: AbortSignal,) : Promise<MediaSearchResult[]> {
  if (!API_URL) {
    throw new Error('API URL is not configured');
  }

  const url = `${API_URL}/api/search/${encodeURIComponent(query)}`
  const response = await fetch(url, {signal});

  if(!response.ok){
    const problem : {detail? : string } | null = await response.json().catch(() => null);
    throw new Error(problem?.detail ?? `API request failed: ${response.status}`);
  }

  const data: {resuslts : MediaSearchResult[] } = await response.json();
  return data.resuslts;
}
