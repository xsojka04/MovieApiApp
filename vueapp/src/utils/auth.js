import jwt_decode from 'jwt-decode'
import axios from "axios";

export const setTokenEventId = "set-token-event-id";

export function setToken(token) {
    if (token == null) {
        localStorage.removeItem('token')
    } else {
        localStorage.setItem('token', token)
    }
    window.dispatchEvent(new CustomEvent(setTokenEventId));
    if (token == null) {
        return
    }
    automaticTokenRefresh(token)
}

function automaticTokenRefresh(token) {
    const expirationTime = jwt_decode(token).exp * 1000
    const timeUntilExpiration = expirationTime - new Date().getTime()

    setTimeout(() => {
        refreshToken()
    }, timeUntilExpiration - 60000)
}

function refreshToken() {
    axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`
    axios.get('/api/user/tokenrefresh')
        .then(response => {
            setToken(response.data)
        })
        .catch(() => {
            setToken(null)
        })
}

export function getToken() {
    return localStorage.getItem('token')
}

export function logout() {
    setToken(null)
}

export function isLoggedIn() {
    const token = getToken();
    if (token == null) {
        return false;
    }
    const expirationTime = jwt_decode(token).exp * 1000
    const timeUntilExpiration = expirationTime - new Date().getTime()
    return (Boolean) (timeUntilExpiration > 0);
}