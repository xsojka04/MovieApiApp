import { createApp } from 'vue'
import { createStore } from 'vuex'
import App from './App.vue'
import 'bootstrap'
import router from './router'
import VNetworkGraph from "v-network-graph"
import "v-network-graph/lib/style.css"

const store = createStore({
  state: {
    loggedIn: false,
    username: null,

  },
  mutations: {
    setUsername(state, username) {
      state.username = username
      state.loggedIn = true
    },
    initialiseStore(state) {
			// Check if the ID exists
			if(localStorage.getItem('vuex_store')) {
				// Replace the state object with the stored item
				this.replaceState(
					Object.assign(state, JSON.parse(localStorage.getItem('vuex_store')))
				);
			}
		}
  }
})
store.subscribe((_, state) => {
  // Store the state object as a JSON string
  localStorage.setItem('vuex_store', JSON.stringify(state));
});


const app = createApp({
  extends: App,
  beforeCreate() { this.$store.commit('initialiseStore') }
})
app.use(store)
app.use(router)
app.use(VNetworkGraph)
app.mount('#app')