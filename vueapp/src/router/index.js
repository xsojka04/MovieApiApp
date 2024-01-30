import { createWebHashHistory, createRouter } from "vue-router"
import Home from "@/views/HomePage.vue"
import SignIn from "@/views/SignIn.vue"
import Movie from "@/views/MoviePage.vue"
import Person from "@/views/ExploreWeb.vue"
import PersonDetail from "@/views/PersonPage.vue"
import Tv from "@/views/TvPage.vue"

const routes = [
    {
        path: "/movie/:id",
        name: "movie",
        component: Movie,
        meta: {
          title: "Film"
        }
    },
    {
        path: "/person/:id?",
        name: "person",
        component: Person,
        meta: {
          title: "Síť"
        }
    },
    {
        path: "/person-detail/:id",
        name: "personDetail",
        component: PersonDetail,
        meta: {
          title: "Osoba"
        }
    },
    {
        path: "/tv/:id",
        name: "tv",
        component: Tv,
        meta: {
          title: "Seriál"
        }
    },
    {
        path: "/",
        name: "home",
        component: Home,
        meta: {
          title: "Filmotéka"
        }
    },
    {
        path: "/login",
        name: "login",
        component: SignIn,
        meta: {
          title: "Přihlášení|Registrace"
        }
    },
    {
        path: '/:catchAll(.*)',
        redirect: '/'
    }
]
const router = createRouter({
    history: createWebHashHistory(),
    routes
  })
  router.beforeEach((to, from, next) => {
    // This goes through the matched routes from last to first, finding the closest route with a title.
    // e.g., if we have `/some/deep/nested/route` and `/some`, `/deep`, and `/nested` have titles,
    // `/nested`'s will be chosen.
    const nearestWithTitle = to.matched.slice().reverse().find(r => r.meta && r.meta.title);
  
    // Find the nearest route element with meta tags.
    const nearestWithMeta = to.matched.slice().reverse().find(r => r.meta && r.meta.metaTags);
  
    const previousNearestWithMeta = from.matched.slice().reverse().find(r => r.meta && r.meta.metaTags);
  
    // If a route with a title was found, set the document (page) title to that value.
    if(nearestWithTitle) {
      document.title = nearestWithTitle.meta.title;
    } else if(previousNearestWithMeta) {
      document.title = previousNearestWithMeta.meta.title;
    }
  
    // Remove any stale meta tags from the document using the key attribute we set below.
    Array.from(document.querySelectorAll('[data-vue-router-controlled]')).map(el => el.parentNode.removeChild(el));
  
    // Skip rendering meta tags if there are none.
    if(!nearestWithMeta) return next();
  
    // Turn the meta tag definitions into actual elements in the head.
    nearestWithMeta.meta.metaTags.map(tagDef => {
      const tag = document.createElement('meta');
  
      Object.keys(tagDef).forEach(key => {
        tag.setAttribute(key, tagDef[key]);
      });
  
      // We use this to track which meta tags we create so we don't interfere with other ones.
      tag.setAttribute('data-vue-router-controlled', '');
  
      return tag;
    })
    // Add the meta tags to the document head.
    .forEach(tag => document.head.appendChild(tag));
  
    next();
  });

export default router