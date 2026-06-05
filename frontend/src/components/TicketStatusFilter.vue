<script setup>
import { customFetch } from '@/router';
import { useRoute, useRouter } from 'vue-router';

/**
 * Props.
 */

const props = defineProps({
  tickets: {
    type: Array,
    required: true,
  },
});

/**
 * Route/router.
 */

const route = useRoute();
const router = useRouter();

/**
 * Function that counts how many types of 1 status.
 *
 * @param {array} tickets The array of tickets.
 * @param {string} status The status you want to count.
 *
 * @returns How many of that status is in ticket.
 */

function countStatus(tickets, status) {
  let amountOfStatus = 0;
  for (let i = 0; i < tickets.length; i++) {
    if (tickets[i].status === status) {
      amountOfStatus++;
    }
  }
  return amountOfStatus;
}

/**
 * Function that filters the tickets.
 * 
 * @param {string} status The status to filter to. 
 */

async function filterTickets(status = undefined){
  try {
    let response;

    if(status == undefined){
      response = await customFetch(`tickets`, 'GET');
    }else{
      response = await customFetch(`tickets/?status=${status}`, 'GET');
    }

    if(!response.ok){
      console.log("Could not filter tickets: ", error);
    }
    
    const data = await response.json();

    const tickets = data.tickets;

    console.log("FILTERTED TICKETS: ", tickets)
  }catch(error){
    console.log("Could not filter tickets: ", error);
  }
}

</script>

<template>
  <div class="flex flex-row bg-(--main-background) p-1 rounded-lg gap-1 mb-6 font-bold w-fit">
    <div v-on:click="router.push({ query: { ...route.query, status: undefined } }); filterTickets()" class="flex flex-row gap-2 bg-white px-4 py-2 rounded-lg items-center cursor-pointer">
      <p :class="route.query.status === undefined ? 'text-(--main-theme-color)' : ''">Alle</p>
      <div>
        <p :class="route.query.status === undefined ? 'text-(--main-theme-color) font-medium' : ''" class="bg-(--main-background) p-1 px-2 rounded-4xl text-sm">{{ tickets.length }}</p>
      </div>
    </div>

    <div v-on:click="router.push({ query: { ...route.query, status: 'open' } }); filterTickets('open')" class="flex flex-row gap-2 bg-white px-4 py-2 rounded-lg items-center cursor-pointer">
      <p :class="route.query.status === 'open' ? 'text-(--main-theme-color)' : ''">Åpen</p>
      <div>
        <p :class="route.query.status === 'open' ? 'text-(--main-theme-color) font-medium' : ''" class="bg-(--main-background) p-1 px-2 rounded-4xl text-sm">{{ countStatus(tickets, 'Åpen') }}</p>
      </div>
    </div>

    <div v-on:click="router.push({ query: { ...route.query, status: 'started' } }); filterTickets('started')" class="flex flex-row gap-2 bg-white px-4 py-2 rounded-lg items-center cursor-pointer">
      <p :class="route.query.status === 'started' ? 'text-(--main-theme-color)' : ''">Påbegynt</p>
      <div>
        <p :class="route.query.status === 'started' ? 'text-(--main-theme-color) font-medium' : ''" class="bg-(--main-background) p-1 px-2 rounded-4xl text-sm">{{ countStatus(tickets, 'Påbegynt') }}</p>
      </div>
    </div>

    <div v-on:click="router.push({ query: { ...route.query, status: 'finished' } }); filterTickets('finished')" class="flex flex-row gap-2 bg-white px-4 py-2 rounded-lg items-center cursor-pointer">
      <p :class="route.query.status === 'finished' ? 'text-(--main-theme-color)' : ''">Fullført</p>
      <div>
        <p :class="route.query.status === 'finished' ? 'text-(--main-theme-color) font-medium' : ''" class="bg-(--main-background) p-1 px-2 rounded-4xl text-sm">{{ countStatus(tickets, 'Fullført') }}</p>
      </div>
    </div>
  </div>
</template>
