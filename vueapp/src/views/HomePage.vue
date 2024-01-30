<script>
import { getFavorites } from '@/utils/favorites'
import MovieCard from "@/components/MovieCard.vue"
import axios from "axios"
import { mapState } from 'vuex'
import { isLoggedIn, setTokenEventId } from '@/utils/auth'
export default {
    components: {
        MovieCard,
    },
    computed: mapState({
        username: state => state.username,
    }),
    data() {
        return {
            favoriteMovies: [],
            similarMovies: [],
            similarTvs: [],
            similarMoviesPick: "",
            similarTvsPick: "",
            favoriteTvs: [],
            favoritePersons: [],
            trendingMovies: [],
            trendingTvs: [],
            isUserLoggedIn: isLoggedIn(),
            favoritePersonCast: [],
            castPick: ""
        }
    },
    setup() {
        return {}
    },
    mounted() {
        window.addEventListener(setTokenEventId, () => {
            this.isUserLoggedIn = isLoggedIn();
        });
        getFavorites('movie').then((movies) => {
            this.favoriteMovies = movies
            if (this.favoriteMovies.length > 0) {
                const randomMovie = this.favoriteMovies[Math.floor(Math.random() * this.favoriteMovies.length)];

                axios.get('/api/tmdb/get', { params: { url: `movie%2F${randomMovie.id}%2Frecommendations` } }).then(response => {
                   for (const similarMovie of response.data.results) {

                        const item = { name: similarMovie.title, image: similarMovie.backdrop_path == undefined ? null : similarMovie.backdrop_path, id: similarMovie.id, mediaType: 'movie' }
                        this.similarMovies.push(item)

                    }
                    this.similarMoviesPick = randomMovie.name
                }).catch(function () {
                })
            }
        })
        getFavorites('tv').then((tvs) => {
            this.favoriteTvs = tvs
            if (this.favoriteTvs.length > 0) {
                const randomTv = this.favoriteTvs[Math.floor(Math.random() * this.favoriteTvs.length)];
                axios.get('/api/tmdb/get', { params: { url: `tv%2F${randomTv.id}%2Frecommendations` } }).then(response => {
                    for (const similarTv of response.data.results) {

                        const item = { name: similarTv.name, image: similarTv.backdrop_path == undefined ? null : similarTv.backdrop_path, id: similarTv.id, mediaType: 'tv' }
                        this.similarTvs.push(item)

                    }
                    this.similarTvsPick = randomTv.name
                }).catch(function () {
                })
            }
        })
        getFavorites('person').then((persons) => {
            this.favoritePersons = persons
            if (this.favoritePersons.length > 0) {
                const randomPerson = this.favoritePersons[Math.floor(Math.random() * this.favoritePersons.length)];

                axios.get('/api/tmdb/get', { params: { url: `person%2F${randomPerson.id}%2Fcombined_credits` } }).then(response => {
                 
                    for (const cast of response.data.cast) {

                        const text = cast.media_type == 'movie' ? cast.title : cast.name 
                        const item = { name: text, image: cast.backdrop_path == undefined ? null : cast.backdrop_path, id: cast.id, mediaType: cast.media_type }
                        this.favoritePersonCast.push(item)

                    }
                    this.castPick = randomPerson.name
                }).catch(function () {
                })
            }
        })
        axios.get('/api/tmdb/get', { params: { url: `trending%2Fmovie%2F%2Fweek` } }).then(response => {
            for (const trendingMovie of response.data.results) {

                const item = { name: trendingMovie.title, image: trendingMovie.backdrop_path == undefined ? null : trendingMovie.backdrop_path, id: trendingMovie.id, mediaType: 'movie' }
                this.trendingMovies.push(item)

            }
        }).catch(function () {
        })

        axios.get('/api/tmdb/get', { params: { url: `trending%2Ftv%2F%2Fweek` } }).then(response => {
            for (const trendingTv of response.data.results) {

                const item = { name: trendingTv.name, image: trendingTv.backdrop_path == undefined ? null : trendingTv.backdrop_path, id: trendingTv.id, mediaType: 'tv' }
                this.trendingTvs.push(item)

            }
        }).catch(function () {
        })
    },
    methods: {
    }
}
</script>
<template>
    <h1 class="welcome-message" v-if="username != null && isUserLoggedIn">Vítejte {{ username }}</h1>
    <section v-if="similarMovies.length > 0">
        <h2 class="section-title">Protože se Vám líbí <span class="title">{{ similarMoviesPick }}</span></h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in similarMovies" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="similarTvs.length > 0">
        <h2 class="section-title">Protože se Vám líbí <span class="title">{{ similarTvsPick }}</span></h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in similarTvs" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="favoritePersonCast.length > 0">
        <h2 class="section-title">Filmy a seriály, ve kterých hraje <span class="title">{{ castPick }}</span></h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in favoritePersonCast" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="favoriteMovies.length > 0">
        <h2 class="section-title">Vaše oblíbené filmy</h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in favoriteMovies" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="favoriteTvs.length > 0">
        <h2 class="section-title">Vaše oblíbené seriály</h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in favoriteTvs" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="trendingMovies.length > 0">
        <h2 class="section-title">Oblíbené filmy tento týden</h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in trendingMovies" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
    <section v-if="trendingTvs.length > 0">
        <h2 class="section-title">Oblíbené seriály tento týden</h2>
        <div class="movies-trail">
            <MovieCard v-for="credit in trendingTvs" :key="credit.id" :name="credit.name" :id="credit.id"
                :image="credit.image" :type="credit.mediaType" />
        </div>
    </section>
</template>