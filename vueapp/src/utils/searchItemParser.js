import { mapGenre } from './genres.js'

export function parseItems(searchresult) {

    const movies = []
    const people = []
    const tv = []
    for (const item of searchresult.results) {
        
        switch (item.media_type) {
            case 'tv':
                tv.push({id: item.id, name: item.name, genres: item.genre_ids.map((x) => mapGenre(x)), date: (new Date(item.first_air_date)).getFullYear(), image: item.backdrop_path, description: item.overview})
                break
            case 'movie':
                movies.push({id: item.id, name: item.title, genres: item.genre_ids.map((x) => mapGenre(x)), date: (new Date(item.release_date)).getFullYear(), image: item.backdrop_path, description: item.overview})
                break
            case 'person':
                people.push({id: item.id, name: item.name, image: item.profile_path})
                break
        }
    }
    return {
        movies: movies,
        people: people,
        tv: tv
    }
}
