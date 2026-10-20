import { Link } from 'expo-router';
import { Pressable, StyleSheet } from 'react-native';

import { ThemedText } from '@/components/themed-text';
import { ThemedView } from '@/components/themed-view';
import { Spacing } from '@/constants/theme';

export default function WelcomeScreen() {
  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.content}>
        <ThemedText type="title" style={styles.title}>
          Palette
        </ThemedText>
        <ThemedText type="subtitle" style={styles.subtitle}>
          Your media, your story.
        </ThemedText>
        <ThemedText themeColor="textSecondary" style={styles.description}>
          Keep track of what you watch, read, play, and listen to—all in one place.
        </ThemedText>
        <Link href="/(app)/(tabs)/home" asChild>
          <Pressable style={({ pressed }) => [styles.button, pressed && styles.buttonPressed]}>
            <ThemedText type="smallBold" style={styles.buttonLabel}>
              Get started
            </ThemedText>
          </Pressable>
        </Link>
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: Spacing.four,
  },
  content: {
    width: '100%',
    maxWidth: 480,
    alignItems: 'center',
    gap: Spacing.three,
  },
  title: {
    fontSize: 56,
    lineHeight: 64,
  },
  subtitle: {
    textAlign: 'center',
  },
  description: {
    textAlign: 'center',
    maxWidth: 360,
  },
  button: {
    marginTop: Spacing.two,
    paddingVertical: Spacing.three,
    paddingHorizontal: Spacing.five,
    borderRadius: 999,
    backgroundColor: '#6750A4',
  },
  buttonPressed: {
    opacity: 0.8,
  },
  buttonLabel: {
    color: '#ffffff',
  },
});
