import { Stack } from 'expo-router';

export default function AppLayout() {
  // This renders the navigation stack for all authenticated app routes.
  return (
    <Stack>
        <Stack.Screen name="logout" options={{ headerShown: false }} />
        <Stack.Screen name="(tabs)" options={{ headerShown: false }} />
    </Stack>
  );
}