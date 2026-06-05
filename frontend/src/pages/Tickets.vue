<script setup>
import { onMounted, ref } from 'vue';
import DashboardLayout from '../layout/DashboardLayout.vue';
import { customFetch, convertToReadable } from '@/router/index.js';
import SingleTicketCard from '@/components/cards/SingleTicketCard.vue';
import TicketStatusFilter from '@/components/TicketStatusFilter.vue';

/**
 * Refs.
 */

const tickets = ref([]);

/**
 * Function that retrieves the tickets.
 */

async function retrieveTickets(){
  try{
    const response = await customFetch('tickets', 'GET');

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
 * Run on load.
 */

onMounted(() => {
  retrieveTickets();
});

</script>

<template>
  <DashboardLayout>
    <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
      <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3">
        <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 min-h-0 flex flex-col">
          <TicketStatusFilter :tickets="tickets" />
          <div class="overflow-y-auto flex-1 min-h-0">
            <div v-for="ticket in tickets">
              <SingleTicketCard :ticketId="ticket.id" :ticketStatus="ticket.status" :ticketTitle="ticket.title" :ticketCreated="convertToReadable(ticket.createdAt)" :ticketUpdated="ticket.updatedAt ?? 'Ikke oppdatert'" :ticketPriority="ticket.priority" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </DashboardLayout>
</template>
