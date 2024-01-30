import axios from "axios";
import { getToken, isLoggedIn } from '@/utils/auth'


export async function getFavorites(type) {
    return getSavedFavorites(type).then((favorites) => {
        let idKey

        switch (type) {
            case 'movie':
                idKey = 'movieId'
                break
            case 'tv':
                idKey = 'tvId'
                break
            case 'person':
                idKey = 'personId'
                break
        }

        const items = []
        const promises = []
        for (const favorite of favorites) {
            let id = favorite[idKey]

            promises.push(getDetail(id, type))
        }
        return Promise.all(promises).then((values) => {
            for (const value of values) {
                items.push(value)
            }
            return items
        });
    }).catch(() => {
        return []
    })
}

export async function getSavedFavorites(type) {
    if (!isLoggedIn()) {
        return []
    }
    const username = JSON.parse(localStorage.getItem('vuex_store')).username
    const key = `${username}:favorite:${type}`
    let favorites = JSON.parse(localStorage.getItem(key))
    if (favorites != null) {
        return favorites
    }

    axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`
    let url
    let idKey
    switch (type) {
        case 'movie':
            url = '/api/likedmovie/get'
            idKey = 'movieId'
            break
        case 'tv':
            url = '/api/likedtv/get'
            idKey = 'tvId'
            break
        case 'person':
            url = '/api/likedperson/get'
            idKey = 'personId'
            break
    }
    return axios.get(url)
        .then((response) => {
            const savedFavorites = []
            for (const savedFavorite of response.data) {
                if (savedFavorites.find(x => x[idKey] == savedFavorite[idKey]) == null) {
                    savedFavorites.push(savedFavorite)
                }
            }
            localStorage.setItem(key, JSON.stringify(savedFavorites))
            return savedFavorites
        })
        .catch(() => {
            return []
        })
}

export async function addFavorite(id, type) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`
    let url
    let json
    switch (type) {
        case 'movie':
            url = '/api/likedmovie/put'
            json = JSON.stringify({ likedMovieVM: {}, MovieId: id.toString() });
            break
        case 'tv':
            url = '/api/likedtv/put'
            json = JSON.stringify({ likedTvVM: {}, TvId: id.toString() });
            break
        case 'person':
            url = '/api/likedperson/put'
            json = JSON.stringify({ likedPersonVM: {}, PersonId: id.toString() });
            break
    }

    return axios.put(url, json, { headers: { 'Content-Type': 'application/json' } })
        .then(() => {
            const username = JSON.parse(localStorage.getItem('vuex_store')).username
            const key = `${username}:favorite:${type}`
            localStorage.removeItem(key)
            return true
        })
        .catch(() => {
            return false
        })
}

export async function removeFavorite(id, type) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`

    let url
    switch (type) {
        case 'movie':
            url = '/api/likedmovie/delete'
            break
        case 'tv':
            url = '/api/likedtv/delete'
            break
        case 'person':
            url = '/api/likedperson/delete'
            break
    }

    return axios.delete(url, { params: { id: id } }, {headers: { 'Content-Type': 'application/json' }})
        .then(() => {
            const username = JSON.parse(localStorage.getItem('vuex_store')).username
            const typeKey = `${username}:favorite:${type}`
            const itemKey = `favorite_${id}_${type}`
            localStorage.removeItem(itemKey)
            localStorage.removeItem(typeKey)
            return true
        })
        .catch(() => {
            return false
        })
}

async function getDetail(id, type) {

    const key = `favorite_${id}_${type}`
    let item = localStorage.getItem(key)
    if (item != null) {
        return JSON.parse(item)
    }
    const url = '/api/tmdb/get'
    let param
    switch (type) {
        case 'movie':
            param = `movie%2F${id}`
            break
        case 'tv':
            param = `tv%2F${id}`
            break
        case 'person':
            param = `person%2F${id}`
            break
    }

    return axios.get(url, { params: { url: param } })
        .then((response) => {
            switch (type) {
                case 'movie':
                    item = { name: response.data.title, image: response.data.backdrop_path == undefined ? null : response.data.backdrop_path, id: response.data.id, mediaType: 'movie' }
                    break;
                case 'tv':
                    item = { name: response.data.name, image: response.data.backdrop_path == undefined ? null : response.data.backdrop_path, id: response.data.id, mediaType: 'tv' }
                    break;
                case 'person':
                    item = { name: response.data.name, image: response.data.profile_path == undefined ? null : response.data.profile_path, id: response.data.id, mediaType: 'person' }
                    break;
            }
            localStorage.setItem(key, JSON.stringify(item))

            return item
        })
        .catch(() => {
            return {}
        })
}