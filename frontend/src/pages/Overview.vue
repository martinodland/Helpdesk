<script setup>
import { useUserStore } from '@/stores/useUserStore.js';
import DashboardLayout from '../layout/DashboardLayout.vue';
import { PlusIcon, UsersIcon } from '@heroicons/vue/24/solid';
import TicketStatusOverviewCard from '@/components/cards/TicketStatusOverviewCard.vue';
import SingleTicketCard from '@/components/cards/SingleTicketCard.vue';
import { onMounted, ref } from 'vue';
import { convertToReadable, customFetch } from '@/router/index.js';

/**
 * Refs.
 */

const tickets = ref([]);
const totalTickets = ref(null);
const settings = ref(null);

/**
 * Stores
 */

const userStore = useUserStore();

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

    tickets.value = data.tickets.slice(-5);

    totalTickets.value = data.tickets.length;
  }catch(error){
    console.log("Failed retrieving tickets: ", error);
  }
}

/**
 * Function that retrieves the user settings.
 */

async function retrieveSettings(){
 try {
  let response = await customFetch('settings', 'GET');

  if(!response.ok){
    console.log("Could not retrieve settings");
  }

  const data = await response.json();

  settings.value = data.settings;

  console.log(settings.value  )

 }catch(error){
  console.log("Could not retrieve settings: ", error)
 }
}

/**
 * Function that counts how many times the find appears in the object.
 * 
 * @param {array} array The object to search.
 * @param {string} find The value to search for.
 * 
 * @returns Occurences
 */

function countOccurencesInArray(array, find){
  return array.filter((obj) => obj.status === find).length;
}

/**
 * Run on load.
 */

onMounted(() => {
  retrieveTickets();
  retrieveSettings();
});

</script>

<template>
  <DashboardLayout>
    <div class="bg-(--main-background) h-fit min-h-full  p-6 flex flex-col gap-2">
      <div class="flex flex-col gap-2 lg:flex-row justify-between">
        <div class="flex flex-col">
          <p class="text-xl font-bold">Hei, {{ userStore.user?.name }}</p>
          <p class="text-(--secondary-text-color)">Her er en oversikt over dine saker hos helpdesk</p>
        </div>
        <RouterLink to="/create" class="flex flex-row gap-4 items-center justify-center bg-(--main-theme-color) text-white font-bold lg:px-6 py-4 rounded-lg cursor-pointer">
          <PlusIcon class="size-5" />
          <p>Registrer ny sak</p>
        </RouterLink>
      </div>
      <div v-if="settings?.statusOverview == 'true'" class="grid grid-cols-1 lg:grid-cols-2 gap-4 pt-1.5">
        <TicketStatusOverviewCard :amountOfTickets="countOccurencesInArray(tickets, 'Open')" />
        <TicketStatusOverviewCard :amountOfTickets="countOccurencesInArray(tickets, 'InProgress')" icon="EnvelopeOpenIcon" label="Påbegynte saker"/>
        <TicketStatusOverviewCard :amountOfTickets="countOccurencesInArray(tickets, 'Closed')" icon="CheckCircleIcon" label="Fullførte saker" />
      </div>
      <div v-if="settings?.showMyTickets == 'true' && userStore.user?.role == 'User' || settings?.showNewestTickets == 'true' && userStore.user?.role == 'Admin'" class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 flex flex-col">
        <div class="flex flex-row justify-between pb-6">
          <p class="font-bold" v-if="userStore.user?.role !== 'Admin'">Mine nyeste saker</p>
          <p class="font-bold" v-else>Nyeste saker</p>
          <RouterLink class="text-(--main-theme-color) font-medium" to="/tickets">Se alle {{ totalTickets }}</RouterLink>
        </div>
        <div class="overflow-y-auto flex-1 min-h-120">
          <div v-for="ticket in tickets">
            <SingleTicketCard :ticketId="ticket.id" :ticketStatus="ticket.status" :ticketTitle="ticket.title" :ticketCreated="convertToReadable(ticket.createdAt)" :ticketUpdated="ticket.updatedAt ?? 'Ikke oppdatert'" :ticketPriority="ticket.priority" />
          </div>
        </div>
      </div>
    </div>
  </DashboardLayout>
</template>
