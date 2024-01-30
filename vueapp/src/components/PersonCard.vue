<template>
    <div class="actor">
        <div class="profile-image">
            <img v-if="image != null" :src="'https://image.tmdb.org/t/p/w500/' + image">
            <img v-else src="@/assets/actor-placeholder.png">
        </div>
        <h4 class="person-name title" @click="addPerson()">{{ name }}</h4>
        <h6>{{ character }}</h6>
    </div>
</template>

<script>
import router from '../router'

export default {
    props: {
        name: { required: true },
        image: { required: true },
        id: { required: true },
        character: { required: true },
    },
    methods: {
        addPerson() {
            router.push({ name: 'person', params: { id: this.id } })
        },
    }
}
</script>
<style>
.person-name:hover {
    color: #aaaaaa;
    cursor: pointer;
}
</style>