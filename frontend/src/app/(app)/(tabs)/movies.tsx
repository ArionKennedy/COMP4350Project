import { FlatList, Platform, StyleSheet, View, type ListRenderItem } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useState } from 'react';
import { useTheme } from '@/hooks/use-theme';
import { BottomTabInset, MaxContentWidth, Spacing } from '@/lib/theme';
import { movieSearch } from '@/services/api';
import SearchBar from '@/components/searchBar';
import Movie from '@/components/movie';
import { MediaSearchResult } from "@/types/mediaSearchResult";

export default function MoviesPage() {
  const theme = useTheme();
  const [searchResults, setSearchResults] = useState<MediaSearchResult[]>([]);
  const searchBarTop = Platform.OS === 'web' ? 80 : Spacing.three;

  async function searchMovie(searchQuery:string){
    const data:MediaSearchResult[] = await movieSearch(searchQuery);
    console.log(data);
    if(data?.length){
      setSearchResults(data);
    }
  }

  const renderMovieItem: ListRenderItem<MediaSearchResult> = ({ item }) => {
    return <Movie movie={item} />;
  };

  return (
    <SafeAreaView style={[styles.safeArea, { backgroundColor: theme.background}]}>
      <View
        pointerEvents="box-none"
        style={[
          styles.searchBar,
          {
            top: searchBarTop,
          },
        ]}>
        <SearchBar onSubmitSearch={searchMovie} />
      </View>
      <FlatList 
        data={searchResults}
        renderItem={renderMovieItem}
        keyExtractor={(item) => item.externalId}
        showsVerticalScrollIndicator={false}
        ListHeaderComponent={null}
        ListFooterComponent={null}
        contentContainerStyle={[
          styles.listContent,
          {
            paddingTop: searchBarTop + 40 + Spacing.three,
            paddingBottom: Platform.OS === 'web' ? Spacing.three : BottomTabInset + Spacing.three,
          },
        ]}
        style={styles.list} />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  list: {
    flex: 1,
  },
  listContent: {
    width: '100%',
    alignItems: 'stretch',
    gap: Spacing.three,
  },
  searchBar: {
    position: 'absolute',
    left: 0,
    right: 0,
    zIndex: 1,
    alignItems: 'center',
    paddingHorizontal: Spacing.four,
  },
  safeArea: {
    flex: 1,
    width: '100%',
    height: '100%',
    alignSelf: 'center',
    position: 'relative',
    maxWidth: MaxContentWidth,
  },
});
