import { createRouter, createWebHistory } from 'vue-router'
import { ref } from 'vue'
/**
 * Refs.
 */

export const previousRouteName = ref(null)

/**
 * Routes.
 */

const routes = [
  {
    path: '/', redirect: { name: 'Login' },
  },
  {
    path: '/overview', component: () => import('../pages/Overview.vue'), name: 'Dashboard',
  },
  {
    path: '/login', component: () => import('../pages/Login.vue'), name: 'Login',
  },
  {
    path: '/register', component: () => import('../pages/Register.vue'), name: 'Register'
  },
  {
    path: '/create', component: () => import('../pages/Create.vue'), name: 'Create',
  },
  {
    path: '/tickets', component: () => import('../pages/Tickets.vue'), name: 'Tickets',
  },
  {
    path: '/tickets/:id', component: () => import('../pages/Ticket.vue'), name: 'Ticket'
  },
  {
    path: '/settings', component: () => import('../pages/Settings.vue'), name: 'Settings'
  }
];

/**
 * Router.
 */

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});

/**
 * Custom fetch fucnction that uses CSRF and also calls refresh endpoint.
 * 
 * @param {string} url The url for the api. 
 * @param {string} method The method to use for call. 
 * @param {object} body Data that you want to send.
 */

export async function customFetch(url, method, body = null){
  let response = await fetch(`http://localhost:5034/${url}`, {
    method: method,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      "X-CSRF": "1",
    },
    body: body 
  });

  if(response.status !== 401 || url === "auth/refresh" || url === "auth/login"){
    return response;
  }

  const refreshResponse = await fetch("http://localhost:5034/auth/refresh", {
    method: "POST",
    credentials: 'include',
    headers: {
      "X-CSRF": "1",
    },
  });

  if(!refreshResponse.ok){
    router.push({ name: 'Login'});

    return refreshResponse;
  }

  response = await fetch(`http://localhost:5034/${url}`, {
    method: method,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      "X-CSRF": "1",
    },
    body: body 
  });

  return response;
}

/**
 * Make time easily human readable. Z for local timezone.
 * 
 * @param {string} dateString The database time value.
 */

export function convertToReadable(dateString) {
  if (!dateString) return 'Aldri';

  const truncated = dateString.replace(/(\.\d{3})\d+/, '$1');
  const normalized = truncated.endsWith('Z') ? truncated : truncated + 'Z';
  const date = new Date(normalized);

  if (isNaN(date.getTime())) return 'Aldri';

  return new Intl.DateTimeFormat('nb-NO', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  }).format(date);
}

router.beforeEach((_to, from) => {
  previousRouteName.value = from.name ?? null
})

export default router
