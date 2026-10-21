import type { ReactNode } from 'react';
import { ActivityIndicator, FlatList, Platform, Pressable, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';

import { ThemedText } from '@/components/themed-text';
import { BottomTabInset, MaxContentWidth, Spacing } from '@/constants/theme';
import { useTheme } from '@/hooks/use-theme';
import type { ShowLog } from '@/types/show-log';

type ShowListProps = {
  shows: ShowLog[];
  loading: boolean;
  error: string | null;
  message: string | null;
  hasMore: boolean;
  onRefresh: () => void;
  onLoadMore: () => void;
  children?: ReactNode;
};

export function ShowList({
  shows, loading, error, message, hasMore, onRefresh, onLoadMore, children,
}: ShowListProps) {
  const theme = useTheme();
  return (
    <SafeAreaView style={[styles.screen, { backgroundColor: theme.background }]}>
      <FlatList
        style={styles.list}
        contentContainerStyle={styles.content}
        data={shows}
        keyExtractor={(show) => show.logId}
        ListHeaderComponent={
          <View style={styles.header}>
            <ThemedText type="subtitle" accessibilityRole="header">My Shows</ThemedText>
            <ThemedText themeColor="textSecondary">Your saved viewing history.</ThemedText>
            {children}
            <Pressable accessibilityRole="button" disabled={loading} onPress={onRefresh}
              style={[styles.button, { backgroundColor: theme.backgroundSelected }]}>
              <ThemedText>{loading ? 'Loading…' : 'Refresh shows'}</ThemedText>
            </Pressable>
            {error && <ThemedText accessibilityRole="alert">{error}</ThemedText>}
            {message && <ThemedText accessibilityLiveRegion="polite">{message}</ThemedText>}
            <ThemedText accessibilityRole="header">Logged shows</ThemedText>
          </View>
        }
        renderItem={({ item }) => (
          <View style={[styles.row, { backgroundColor: theme.backgroundElement }]}>
            <ThemedText>{item.title}</ThemedText>
            <ThemedText type="small" themeColor="textSecondary">
              {item.status ?? 'Logged'} · {item.loggedDate}
            </ThemedText>
          </View>
        )}
        ListEmptyComponent={
          loading ? <ActivityIndicator accessibilityLabel="Loading shows" color={theme.text} />
            : !error ? <ThemedText>You haven&apos;t logged any shows yet.</ThemedText> : null
        }
        ListFooterComponent={hasMore ? (
          <Pressable accessibilityRole="button" disabled={loading} onPress={onLoadMore}
            style={[styles.button, { backgroundColor: theme.backgroundSelected }]}>
            <ThemedText>{loading ? 'Loading…' : 'Load more shows'}</ThemedText>
          </Pressable>
        ) : null}
      />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  screen: { flex: 1 },
  list: { flex: 1, width: '100%', maxWidth: MaxContentWidth, alignSelf: 'center' },
  content: {
    paddingHorizontal: Spacing.four,
    paddingTop: Platform.OS === 'web' ? Spacing.six + Spacing.five : Spacing.three,
    paddingBottom: BottomTabInset + Spacing.four,
  },
  header: { gap: Spacing.three, marginBottom: Spacing.three },
  row: { padding: Spacing.three, borderRadius: Spacing.two, marginBottom: Spacing.two },
  button: { padding: Spacing.three, borderRadius: Spacing.two, alignItems: 'center' },
});
