<template>
    <div v-if="loading" class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
    </div>
    <div v-else>

        <div v-if="!isUserLoggedIn" class="container">
            <div v-if="isSigninAction" class="row justify-content-md-center">
                <div class="col-md-6">
                    <div class="card login-card">
                        <div class="card-body">
                            <div v-if="isBadReg" class="alert alert-danger" role="alert">
                                Uživatelské jméno je již obsazené.
                            </div>
                            <h1>Registrace</h1>
                            <form class="form-group" @submit.prevent="doRegister">
                                <div class="form-group">
                                    <input v-model="nameReg" type="text" maxlength="50" class="form-control"
                                           placeholder="Jméno" required>
                                </div>
                                <div class="form-group mt-4">
                                    <input v-model="passwordReg" type="password" maxlength="100" class="form-control"
                                           placeholder="Heslo" required>
                                    <div class="form-group mt-3">
                                        <input v-model="confirmPasswordReg" type="password" maxlength="100"
                                               class="form-control" placeholder="Potvrzení hesla" required>
                                    </div>
                                    <input type="submit" class="login-button mt-5 w-100" value="Registrovat">
                                </div>
                                <p>
                                    Už máte učet? Přihlašte se <a href="#" @click="swapActions">zde</a>
                                </p>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <div v-if="!isUserLoggedIn && isLoginAction" class="row justify-content-md-center">
                <div class="col-md-6">
                    <div class="card login-card">
                        <div class="card-body">
                            <div v-if="isBadLogin" class="alert alert-danger" role="alert">
                                Špatné heslo nebo jméno.
                            </div>
                            <h1>Přihlásit se</h1>
                            <form class="form-group" @submit.prevent="doLogin">
                                <div class="form-group">
                                    <input v-model="nameLog" type="text" maxlength="50" class="form-control"
                                           placeholder="Jméno" required>
                                </div>
                                <div class="form-group mt-3">
                                    <input v-model="passwordLog" type="password" maxlength="100" class="form-control"
                                           placeholder="Heslo" required>
                                </div>

                                <input type="submit" class="login-button mt-5 w-100" value="Přihlásit">
                            </form>
                            <p class="mt-3">
                                Nemáte učet? Zaregistrujte se <a href="#" @click="swapActions">zde</a>
                            </p>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <button class="btn btn-secondary" v-if="isUserLoggedIn" @click="me">Me</button>
</template>

<script lang="js">
import axios from "axios";
import { defineComponent } from 'vue';
import { setToken, getToken, setTokenEventId, isLoggedIn } from '@/utils/auth'
import router from '../router'

export default defineComponent({
    data() {
        return {
            nameReg: "",
            passwordReg: "",
            confirmPasswordReg: "",
            nameLog: "",
            passwordLog: "",
            isUserLoggedIn: isLoggedIn(),
            isLoginAction: true,
            isSigninAction: false,
            isBadLogin: false,
            isBadReg: false,
            loading: "",
        };
    },
    created() {
    },
    mounted() {
        window.addEventListener(setTokenEventId, () => {
            this.isUserLoggedIn = isLoggedIn();
        });
    },
    methods: {
        swapActions() {
            this.isLoginAction = !this.isLoginAction
            this.isSigninAction = !this.isSigninAction
        },

        showLogin() {
            this.isLoginAction = true
            this.isSigninAction = false
        },

        doRegister() {
            this.loading = true;

            if (this.nameReg === "" || this.passwordReg === "" || this.confirmPasswordReg === "") {
                return
            }

            if (this.passwordReg !== this.confirmPasswordReg) {
                return
            }

            axios.post('/api/user/signin', {
                name: this.nameReg,
                password: this.passwordReg,
            })
                .then(() => {
                    this.nameReg = ""
                    this.passwordReg = ""
                    this.confirmPasswordReg = ""
                    this.showLogin()
                })
                .catch(() => {
                    this.isBadReg = true
                })
                .finally(() => { this.loading = false })
        },

        doLogin() {
            this.loading = true;

            if (this.nameLog === "" || this.passwordLog === "") {
                return
            }

            axios.post('/api/user/token', {
                name: this.nameLog,
                password: this.passwordLog,
            })
                .then(response => {
                    setToken(response.data)
                    this.nameLog = ""
                    this.passwordLog = ""
                    this.me()
                    router.push({ path: '/' })
                })
                .catch(() => {
                    this.passwordLog = ""
                    this.isBadLogin = true
                })
                .finally(() => { this.loading = false })
        },

        me() {
            this.loading = true;
            axios.defaults.headers.common['Authorization'] = `Bearer ${getToken()}`
            axios.get('/api/user/me')
                .then(response => {
                    this.$store.commit("setUsername", response.data.name)
                })
                .catch(() => {
                })
                .finally(() => { this.loading = false })
        }
    },
});
</script>