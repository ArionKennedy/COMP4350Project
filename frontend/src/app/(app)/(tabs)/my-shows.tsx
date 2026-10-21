import { useCallback, useRef, useState } from 'react';
import { useFocusEffect } from 'expo-router';
import { Pressable, StyleSheet, View } from 'react-native';

import { useSession } from '@/auth-ctx';
import { ShowList } from '@/components/show-list';
import { ThemedText } from '@/components/themed-text';
import { DEMO_SHOWS, LOGGING_DEMO_ENABLED } from '@/constants/logging';
import { Spacing } from '@/constants/theme';
import { useTheme } from '@/hooks/use-theme';
import { getShowLogs, saveWatchedShow } from '@/services/show-logs';
import type { ShowLog, ShowToLog } from '@/types/show-log';

export default function MyShowsScreen() {
  const { session } = useSession();
  const theme = useTheme();
  const [shows, setShows] = useState<ShowLog[]>([]);
  const [loading, setLoading] = useState(false);
  const [savingId, setSavingId] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [message, setMessage] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const version = useRef(0);
  const focused = useRef(false);
  const saving = useRef(false);

  const load = useCallback(async (nextPage = 1) => {
    const currentVersion = ++version.current;
    setLoading(true);
    setError(null);
    try {
      const result = await getShowLogs(session, nextPage);
      if (currentVersion !== version.current) return;
      setShows(previous => nextPage === 1 ? result.items : [
        ...previous,
        ...result.items.filter(item => !previous.some(existing => existing.logId === item.logId)),
      ]);
      setPage(result.page);
      setHasMore(result.hasMore);
    } catch (problem) {
      if (currentVersion === version.current)
        setError(problem instanceof Error ? problem.message : 'Could not load your shows.');
    } finally {
      if (currentVersion === version.current) setLoading(false);
    }
  }, [session]);

  useFocusEffect(useCallback(() => {
    focused.current = true;
    setShows([]);
    setHasMore(false);
    setMessage(null);
    void load();
    return () => { focused.current = false; version.current += 1; };
  }, [load]));

  async function markWatched(show: ShowToLog) {
    if (saving.current) return;
    saving.current = true;
    setSavingId(show.tmdbTvId);
    setError(null);
    setMessage(null);
    // Ignore an older history request that might finish after this save.
    version.current += 1;
    try {
      await saveWatchedShow(session, show);
      if (!focused.current) return;
      setMessage(`${show.title} was saved to your history.`);
      await load();
    } catch (problem) {
      if (focused.current)
        setError(problem instanceof Error ? problem.message : 'Could not save this show. Try again.');
    } finally {
      saving.current = false;
      setSavingId(null);
      if (focused.current) setLoading(false);
    }
  }

  return (
    <ShowList shows={shows} loading={loading || savingId !== null} error={error}
      message={message} hasMore={hasMore}
      onRefresh={() => { setMessage(null); void load(); }}
      onLoadMore={() => void load(page + 1)}>
      {LOGGING_DEMO_ENABLED && (
        <View style={styles.picker}>
          <ThemedText themeColor="textSecondary">
            Local demo account · Choose a sample show to test saving. Your logs are stored in the database.
          </ThemedText>
          {DEMO_SHOWS.map(show => {
            const watched = shows.some(item => item.tmdbTvId === show.tmdbTvId && item.status === 'watched');
            return (
              <Pressable key={show.tmdbTvId} accessibilityRole="button"
                accessibilityLabel={`Mark ${show.title} as watched`}
                disabled={loading || savingId !== null || watched}
                onPress={() => void markWatched(show)}
                style={[styles.choice, { backgroundColor: theme.backgroundElement },
                  (savingId !== null || watched) && styles.disabled]}>
                <ThemedText>{show.title}</ThemedText>
                <ThemedText type="small" themeColor="textSecondary">
                  {savingId === show.tmdbTvId ? 'Saving…' : watched ? 'Watched' : 'Mark as watched'}
                </ThemedText>
              </Pressable>
            );
          })}
        </View>
      )}
    </ShowList>
  );
}

const styles = StyleSheet.create({
  picker: { gap: Spacing.two },
  choice: { padding: Spacing.three, borderRadius: Spacing.two },
  disabled: { opacity: 0.6 },
});
