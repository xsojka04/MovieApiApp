<template>
    <header>
        <nav>
            <div class="left-menu"><img id="logo" alt="logo" src=".././assets/logo.png" @click="homepage"><img id="web" alt="web" src=".././assets/web.png" @click="web"></div>
            <div class="search-bar">
                <div id="search-wrapper">
                    <input list="result_list" @input="fetchData" placeholder="Hledej filmy, seriály, herce...">
                    <div id="search_results" v-if="displayResults">
                        <h3 class="search-group-title">Filmy</h3>
                        <SearchResult v-for="item in moviesList" :key="item.id" :id="item.id" :image="item.image" :name="item.name" :type="'movie'" :description="item.description" :genres="item.genres" :date="item.date"/>
                        <h3 class="search-group-title">Seriály</h3>
                        <SearchResult v-for="item in tvList" :key="item.id" :id="item.id" :image="item.image" :name="item.name" :type="'tv'" :description="item.description" :genres="item.genres" :date="item.date"/>
                        <h3 class="search-group-title">Lidé</h3>
                        <SearchResult v-for="item in personList" :key="item.id" :id="item.id" :image="item.image" :name="item.name" :type="'person'"/>
                    </div>
                </div>
            </div>
            <div class="user-actions dropdown" v-if="isUserLoggedIn">
                <span type="button" class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false"><img
                        src="../assets/user-icon.svg">{{ username }}</span>
                <ul class="dropdown-menu">
                    <li><span class="dropdown-item" @click="doLogout">Odhlásit se</span></li>
                </ul>
            </div>
            <div v-else><button class="login-button" @click="login">Přihlásit</button></div>
        </nav>
    </header>
</template>
<script lang="js">
import { defineComponent } from 'vue';
import { setTokenEventId, isLoggedIn, logout } from '@/utils/auth'
import { parseItems } from '@/utils/searchItemParser'
import { mapState } from 'vuex'
import router from '../router'
import axios from "axios";
import SearchResult from "@/components/SearchResult.vue"

export default defineComponent({
    components: {
        SearchResult,
    },
    data() {
        return {
            isUserLoggedIn: isLoggedIn(),
            moviesList: [],
            tvList: [],
            personList: [],
            displayResults: false
        };
    },
    mounted() {

        window.addEventListener(setTokenEventId, () => {
            this.isUserLoggedIn = isLoggedIn()
        });
    },
    methods: {
        web() {
            this.displayResults = false
            router.push({ name: 'person' })

        },
        homepage() {
            this.displayResults = false
            router.push({ name: 'home' })

        },
        movieRedirect(id) {
            this.displayResults = false
            router.push({ name: 'movie', params: { id: id } })
        },

        tvRedirect(id) {
            this.displayResults = false
            router.push({ name: 'tv', params: { id: id } })
        },

        personRedirect(id) {
            this.displayResults = false
            router.push({ name: 'person', params: { id: id } })
        },
        displayResultsNone() {
            this.displayResults = false
        },
        fetchData(event) {
            const value = event.srcElement.value
            if (value.length < 3) {
                return
            }
            axios.get('/api/tmdb/get', { params: { url: "search%2Fmulti", query: value } }).then(response => {
                const data = parseItems(response.data)
                this.personList = data.people
                this.moviesList = data.movies
                this.tvList = data.tv
                this.displayResults = true
            }).catch(function () {
            })
        },
        doLogout() {
            logout()
        },
        login() {
            router.push({ path: '/login' })
        }
    },
    computed: mapState({
        username: state => state.username,
    }),
    watch: {
        $route() {
            this.displayResults = false
        }
    }
});
</script>