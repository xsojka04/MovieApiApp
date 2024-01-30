<template>
    <div v-if="type == 'tv'" class="search-item" @click="tvRedirect(id)">

        <div class="search-item-image">
            <div class="search-item-image-wrapper">
                <img v-if="image !== null" v-bind:src="'https://image.tmdb.org/t/p/w500/' + image" v-bind:alt="name + 'image'">
            </div>
        </div>
        <div class="search-item-info">
            <h4 class="title">{{ name }}</h4>
            <p v-if="description.length < 100">{{ description }}</p>
            <p v-else>{{ description.substring(0, 97) + "..." }}</p>
            <span class="item-date">{{ date }}</span>
            <div class="genres">
                <span v-for="(genre, index) in genres" v-bind:key="index">{{ genre }}</span>
            </div>
        </div>
    </div>
    <div v-else-if="type == 'movie'" class="search-item" @click="movieRedirect(id)">

        <div class="search-item-image">
            <div class="search-item-image-wrapper">
                <img v-if="image !== null" v-bind:src="'https://image.tmdb.org/t/p/w500/' + image"
                    v-bind:alt="name + 'image'">
            </div>
        </div>
        <div class="search-item-info">
            <h4 class="title">{{ name }}</h4>
            <p v-if="description.length < 100">{{ description }}</p>
            <p v-else>{{ description.substring(0, 97) + "..." }}</p>
            <span v-if="date" class="item-date">{{ date }}</span>
            <div class="genres">
                <span v-for="(genre, index) in genres" v-bind:key="index">{{ genre }}</span>
            </div>
        </div>
    </div>
    <div v-else class="search-item" @click="personRedirect(id)">
        <div class="search-item-image">
            <div class="search-item-image-wrapper">
                <img v-if="image !== null" class="person-result-image" v-bind:src="'https://image.tmdb.org/t/p/w500/' + image"
                    v-bind:alt="name + 'image'">
            </div>
        </div>
        <div class="search-item-info">
            <h4 class="title">{{ name }}</h4>
        </div>
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
        description: { required: false },
        date: { required: false },
        genres: { required: false },
    },
    mounted() {
    },
    methods: {
        movieRedirect(id) {
            router.push({ name: 'movie', params: { id: id } })
        },

        tvRedirect(id) {
            router.push({ name: 'tv', params: { id: id } })
        },

        personRedirect(id) {
            router.push({ name: 'person', params: { id: id } })
        },

    }
};
</script>