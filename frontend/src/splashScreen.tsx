import { SplashScreen } from 'expo-router';
import { useSession } from './auth-ctx';

SplashScreen.preventAutoHideAsync();

export function SplashScreenController() {
  const { isLoading } = useSession(); // wait until authentication finished to move away from splash screen

  if (!isLoading) {
    SplashScreen.hide();
  }

  return null;
}
