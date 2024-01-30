<template>
    <button class="basic-button back-button" @click="goBack">&lt; zpět</button>
    <div class="movie-show">
        <div>
            <h1 class="title">{{ movieTitle }}</h1>
            <p>{{ date }}</p>
            <p v-if="episodeRuntime">{{ episodeRuntime }} min. epizoda</p>
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
            <p>{{ movieDescription }}</p>
            <div class="genres">
                <span v-for="genre in movieGenres" v-bind:key="genre.id">{{ genre.name }}</span>
            </div>
            <div class="seasons">
                <h3>Sezóny</h3>
                <ol>
                    <li v-for="season in seasons" :key="season.id">
                        {{ season.name }}, {{ season.episode_count }} epizod
                    </li>
                </ol>
            </div>
        </div>
        <div class="actors">
            <div class="actor" v-for="actor in actors" :key="actor.id">
                <div class="profile-image">
                    <img v-bind:src="'https://image.tmdb.org/t/p/w500/' + actor.image">
                </div>
                <h5 class="person-name" @click="addPerson(actor.id)">{{ actor.name }}</h5>
                <h6>{{ actor.character }}</h6>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import router from '../router'
import { isLoggedIn, setTokenEventId } from '@/utils/auth'
import { addFavorite, getSavedFavorites, removeFavorite } from '@/utils/favorites'

export default {
    data() {
        return {
            movieTitle: "",
            movieImage: "",
            movieDescription: "",
            movieGenres: [],
            actors: [],
            date: "",
            seasons: [],
            episodeRuntime: 0,
            isLoggedIn: isLoggedIn(),
            isFavorite: false,
            favoriteAction: false
        }
    },
    computed: {
        movieId() {
            return this.$route.params.id
        }
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
        addPerson(id) {
            router.push({ name: 'person', params: { id: id } })
        },
        getTvInfo() {
            axios.get('/api/tmdb/get', { params: { url: `tv%2F${this.$route.params.id}` } }).then(response => {
                // fetch tv info
                this.movieTitle = response.data.name
                this.movieDescription = response.data.overview
                this.movieGenres = response.data.genres
                this.date = response.data.first_air_date == "" ? null : (new Date(response.data.first_air_date)).getFullYear()
                this.seasons = response.data.seasons
                this.episodeRuntime = response.data.episode_run_time[0]
                // style the background to the tv theme
                document.body.style.backgroundImage = `url(https://image.tmdb.org/t/p/original/${response.data.backdrop_path})`
                document.body.style.backgroundPosition = "initial"
                document.body.style.backgroundPositionX = "center"
                this.getActors()
            }).catch(function () {
            })
        },
        getActors() {
            axios.get('/api/tmdb/get', { params: { url: `tv%2F${this.$route.params.id}%2Fcredits` } }).then(response => {
                this.actors = []

                for (const cast of response.data.cast) {
                    this.actors.push({ character: cast.character, name: cast.name, id: cast.id, image: cast.profile_path })
                    if (this.actors.length > 11) {
                        break;
                    }
                }

            }).catch(function () {
            })
        }
    },
    mounted() {
        // get info about the tw show
        this.getTvInfo()
        window.addEventListener(setTokenEventId, () => {
            this.isLoggedIn = isLoggedIn()
        });
        // resolve is favorite
        getSavedFavorites('tv').then((favoriteTvsIds) => {
            if (favoriteTvsIds.find(x => x['tvId'] == this.$route.params.id) != null) {
                this.isFavorite = true
            }
        })
    },
    watch: {
        $route() {
            this.getTvInfo()
        }
    }
}
</script>