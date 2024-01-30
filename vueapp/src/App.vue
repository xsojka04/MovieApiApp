<template>
    <NavbarSearch />
    <main class="mt-5">
        <router-view />
    </main>
</template>

<script>
import NavbarSearch from './components/NavbarSearch.vue'
import { getGenres } from './utils/genres.js'

getGenres()
export default {
    name: 'App',
    components: {
        NavbarSearch,
    }
}

</script>

<style>
@import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
@import './assets/styles/main.css';

#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: white;
}
</style>
