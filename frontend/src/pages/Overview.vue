<script setup>
import { useUserStore } from '@/stores/useUserStore.js';
import DashboardLayout from '../layout/DashboardLayout.vue';
import { PlusIcon } from '@heroicons/vue/24/solid';
import TicketStatusOverviewCard from '@/components/cards/TicketStatusOverviewCard.vue';
import SingleTicketCard from '@/components/cards/SingleTicketCard.vue';

/**
 * Stores
 */

const userStore = useUserStore();

</script>

<template>
  <DashboardLayout>
    <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
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
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-4 pt-1.5">
        <TicketStatusOverviewCard />
        <TicketStatusOverviewCard />
        <TicketStatusOverviewCard />
      </div>
      <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 min-h-0 flex flex-col">
        <div class="flex flex-row justify-between pb-6">
          <p class="font-bold">Mine nyeste saker</p>
          <RouterLink class="text-(--main-theme-color) font-medium" to="/tickets">Se alle (0)</RouterLink>
        </div>
        <div class="overflow-y-auto flex-1 min-h-0">
          <SingleTicketCard />
          <SingleTicketCard />
          <SingleTicketCard />
          <SingleTicketCard />
          <SingleTicketCard />
        </div>
      </div>
    </div>
  </DashboardLayout>
</template>
