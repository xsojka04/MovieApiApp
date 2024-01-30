<template>
    <button class="basic-button back-button" @click="goBack">&lt; zpět</button>
    <div class="movie-show">
        <div>
            <h1 class="title">{{ movieTitle }}</h1>
            <p>{{ movieYear }}</p>
            <button v-if="isLoggedIn && !isFavorite && !favoriteAction" class="basic-button favorite-button"
                @click="changeFavorite(true)">
                Přidat do
                oblíbených
            </button>
            <button v-else-if="isLoggedIn && isFavorite && !favoriteAction" class="basic-button favorite-button-back"
                @click="changeFavorite(false)">
                Odebrat z oblíbených
            </button>
            <button v-else-if="favoriteAction" class="basic-button">
                <div class="spinner-border spinner-border-sm text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </button>
            <div class="genres m-1">
                <span v-for="genre in movieGenres" v-bind:key="genre.id">{{ genre.name }}</span>
            </div>
            <div class="ratings m-1">
                <span v-if="tmdbRating">TMDB: {{ tmdbRating }}</span>
                <span v-if="imdbRating">IMDb: {{ imdbRating }}</span>
                <span v-if="rottenTomatoesRating">RottenTomatoes: {{ rottenTomatoesRating }} %</span>
            </div>
            <p>{{ movieDescription }}</p>
        </div>
        <div class="actors">
            <PersonCard class="actor" v-for="actor in actors" :key="actor.id" :name="actor.name"
                :character="actor.character" :image="actor.image" :id="actor.id" />
        </div>
    </div>
</template>
<script>
import axios from "axios";
import router from '../router'
import PersonCard from "@/components/PersonCard.vue"
import { isLoggedIn, setTokenEventId } from '@/utils/auth'
import { removeFavorite, addFavorite, getSavedFavorites } from '@/utils/favorites'

export default {
    components: {
        PersonCard,
    },
    data() {
        return {
            movieId: "",
            imdbId: "",
            movieTitle: "",
            movieImage: "",
            movieYear: null,
            tmdbRating: null,
            imdbRating: null,
            rottenTomatoesRating: null,
            movieDescription: "",
            movieGenres: [],
            actors: [],
            isLoggedIn: isLoggedIn(),
            isFavorite: false,
            favoriteAction: false
        }
    },
    computed: {
    },
    methods: {
        changeFavorite(add) {
            this.favoriteAction = true
            if (add) {
                this.favoriteAction = true
                addFavorite(this.movieId, 'tv').then((result) => {
                    this.isFavorite = result
                    this.favoriteAction = false
                })
                return
            }
            removeFavorite(this.movieId, 'tv').then((result) => {
                this.isFavorite = !result
                this.favoriteAction = false
            })
        },
        goBack() {
            router.go(-1)
        },
        getMovieInfo() {

            axios.get('/api/tmdb/get', { params: { url: `movie%2F${this.$route.params.id}` } }).then(response => {
                this.movieTitle = response.data.title
                this.imdbId = response.data.imdb_id
                this.tmdbRating = response.data.vote_average
                this.movieDescription = response.data.overview
                this.movieGenres = response.data.genres
                this.movieId = response.data.id
                this.movieYear = new Date(response.data.release_date).getFullYear()
                document.body.style.backgroundImage = `url(https://image.tmdb.org/t/p/original/${response.data.backdrop_path})`
                document.body.style.backgroundPosition = "initial"
                document.body.style.backgroundPositionX = "center"
                this.getActors()
                this.getRatings(response.data.imdb_id)
            }).catch(function () {
            })
        },
        getActors() {
            axios.get('/api/tmdb/get', { params: { url: `movie%2F${this.$route.params.id}%2Fcredits` } }).then(response => {
                this.actors = []
                for (const cast of response.data.cast) {
                    this.actors.push({ character: cast.character, name: cast.name, id: cast.id, image: cast.profile_path })
                    if (this.actors.length > 7) {
                        break;
                    }
                }

            }).catch(function () {
            })
        },
        getRatings(imdbId) {
            axios.get('/api/imdb/ratings', { params: { imdbid: imdbId } })
                .then(response => {
                    this.imdbRating = response.data.imDb
                    this.rottenTomatoesRating = response.data.rottenTomatoes
                }).catch(function () {
                })
        }
    },
    mounted() {
        this.getMovieInfo()
        window.addEventListener(setTokenEventId, () => {
            this.isLoggedIn = isLoggedIn()
        });
        // resolve is favorite
        getSavedFavorites('movie').then((favoriteMoviesIds) => {
            if (favoriteMoviesIds.find(x => x['movieId'] == this.$route.params.id) != null) {
                this.isFavorite = true
            }
        })
    },
    watch: {
        $route() {
            this.getMovieInfo()
        }
    }
}
</script>
