import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/', component: () => import('../pages/Home.vue')
  },
];


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

  if(response.status !== 401 || url === "auth/refresh"){
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
    window.location.href = "/login";

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

export default router
