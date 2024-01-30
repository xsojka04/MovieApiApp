import axios from "axios";

export let genres = []

export function getGenres() {
    if (localStorage.genres) {
        genres = JSON.parse(localStorage.genres)
    } else {

        axios.get('/api/tmdb/get', { params: { url: "genre%2Fmovie%2Flist" } })
            .then(response => {
                genres = response.data.genres
                localStorage.genres = JSON.stringify(response.data.genres)
            }).catch(() => {
            })
    }
}

export function mapGenre(genreId) {
    for (const genre of genres) {

        if (genre.id == genreId) {
            return genre.name
        }
    }
    return null
}
