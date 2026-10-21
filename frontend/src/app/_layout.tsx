import '../global.css';
import { Stack } from 'expo-router';

import { SessionProvider, useSession } from '@/auth-ctx';
import { SplashScreenController } from '@/splashScreen';
import { PortalHost } from '@rn-primitives/portal';

export default function Root() {
  return (
    <SessionProvider>
      <SplashScreenController />
      <RootNavigator />
      <PortalHost />
    </SessionProvider>
  );
}

function RootNavigator() {
  const { session } = useSession();

  return (
    <Stack screenOptions={{ headerShown: false }}>
      <Stack.Protected guard={!!session}>
        <Stack.Screen name="(app)"/>
      </Stack.Protected>

      <Stack.Protected guard={!session}>
        <Stack.Screen name="login"/>
        <Stack.Screen name="signup"/>
      </Stack.Protected>
    </Stack>
  );
}
