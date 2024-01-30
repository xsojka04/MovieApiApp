<template>
    <div @click="goToMovie()"  class="movie-card">
        <img v-if="image != null " :src="'https://image.tmdb.org/t/p/w500/' + image">
        <img v-else src=".././assets/movie-placeholder.png">
        <h2 class="title">{{ name }}</h2>
    </div>
</template>
<script>
import router from '../router'

export default {
    props: {
        name: { required: true },
        image: { required: true },
        id: { required: true },
        type: { required: true },
    },
    methods: {
        goToMovie() {
            if (this.type == 'movie') {
                router.push({ name: 'movie', params: { id: this.id } })
            } else {
                router.push({ name: 'tv', params: { id: this.id } })
            }
        }
    }
};
</script>