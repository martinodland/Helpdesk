<script setup>
import { onMounted, ref } from 'vue';
import DashboardLayout from '../layout/DashboardLayout.vue';
import { customFetch, convertToReadable } from '@/router/index.js';
import SingleTicketCard from '@/components/cards/SingleTicketCard.vue';
import TicketStatusFilter from '@/components/TicketStatusFilter.vue';
import { useRoute } from 'vue-router';

/**
 * Refs.
 */

const tickets = ref([]);
const allTickets = ref([]);

/**
 * Route.
 */

const route = useRoute();

/**
 * Function that retrieves the tickets.
 */

async function retrieveTickets(){
  try{
    const query = route.query;

    const allResponse = await customFetch('tickets', 'GET');

    const allData = await allResponse.json();

    allTickets.value = allData.tickets;

    if(!query || Object.keys(query).length === 0){
      tickets.value = allData.tickets;
      return;
    }

    let param = buildParam(query);

    let response = await customFetch(`tickets/${param}`, 'GET');

    if(!response.ok){
      console.log("Failed retrieving tickets: ", response);
    }

    const data = await response.json();

    tickets.value = data.tickets;

  }catch(error){
    console.log("Failed retrieving tickets: ", error);
  }
}

/**
 * Function that builds query into string for api.
 */

function buildParam(params){
  let param = '?';
  const entries = Object.entries(params);

  for(let i = 0; i < entries.length; i++){
    if(i === 0){
      param += `${entries[i][0]}=${entries[i][1]}`;

      continue;
    }

    param += `&${entries[i][0]}=${entries[i][1]}`;
  }

  return param;
}

/**
 * Run on load.
 */

onMounted(() => {
  retrieveTickets();
});

</script>

<template>
  <DashboardLayout>
    <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
      <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3 h-full">
        <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 min-h-0 flex flex-col">
          <TicketStatusFilter v-model="tickets" :allTickets="allTickets" />
          <div class="overflow-y-auto flex-1 min-h-0">
            <div v-for="ticket in tickets">
              <SingleTicketCard :ticketId="ticket.id" :ticketStatus="ticket.status" :ticketTitle="ticket.title" :ticketCreated="convertToReadable(ticket.createdAt)" :ticketUpdated="ticket.updatedAt" :ticketPriority="ticket.priority" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </DashboardLayout>
</template>
