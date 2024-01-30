<script>
import axios from "axios";
import MovieCard from "@/components/MovieCard.vue"
import router from '../router'
import { removeFavorite, addFavorite, getSavedFavorites } from '@/utils/favorites'
import { isLoggedIn, setTokenEventId } from '@/utils/auth'


export default {
    components: {
        MovieCard,
    },
    data() {
        return {
            name: "",
            description: "",
            profile: "",
            birthday: "",
            palce: "",
            credits: {},
            maxCredits: 6,
            personId: "",
            favoriteAction: false,
            isFavorite: false,
            place: "",
            isLoggedIn: isLoggedIn(),
        }
    },
    mounted() {
        window.addEventListener(setTokenEventId, () => {
            this.isLoggedIn = isLoggedIn()
        });
        this.getPersonInfo()
        this.getMovies()
        // resolve is favorite
        getSavedFavorites('person').then((favoritePersonIds) => {
            if (favoritePersonIds.find(x => x['personId'] == this.$route.params.id) != null) {
                this.isFavorite = true
            }
        })
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
        getPersonInfo() {
            axios.get('/api/tmdb/get', { params: { url: `person%2F${this.$route.params.id}` } }).then(response => {
                this.name = response.data.name
                this.description = response.data.biography
                this.profile = response.data.profile_path
                this.birthday = (new Date(response.data.birthday)).getYear()
                this.place = response.data.place_of_birth
                this.personId = response.data.id
            }).catch(function () {
            })
        },
        getMovies() {
            axios.get('/api/tmdb/get', { params: { url: `person%2F${this.$route.params.id}%2Fcombined_credits` } }).then(response => {
                let count = 0
                for (const credit of response.data.cast) {
                    count++
                    if (count > this.maxCredits) {
                        break
                    }

                    const text = credit.media_type == 'tv' ? credit.name : credit.title
                    const creditId = `${credit.media_type}_${credit.id}`

                    this.credits[creditId] = { mediaType: credit.media_type, name: text, image: credit.backdrop_path == undefined ? null : credit.backdrop_path, id: credit.id }
                }
            }).catch(function () {
            })
        }
    }
}
</script>
<template>
    <button class="basic-button back-button" @click="goBack">&lt; zpět</button>
    <div class="person-show">
        <div class="person-info">
            <h1 class="title">{{ name }}</h1>
            <p><span>{{ birthday }}</span>, {{ place }}</p>
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
            <p class="description">{{ description }}</p>
        </div>
        <div>
            <img :src="'https://image.tmdb.org/t/p/original/' + profile">
        </div>
    </div>
    <div class="movies-trail">
        <MovieCard v-for="credit in credits" :key="credit.id" :name="credit.name" :id="credit.id" :image="credit.image"
            :type="credit.mediaType" />
    </div>
</template>
