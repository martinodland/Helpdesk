<script setup>
import SelectInput from '@/components/form/SelectInput.vue';
import DashboardLayout from '@/layout/DashboardLayout.vue';
import { customFetch } from '@/router';
import { useUserStore } from '@/stores/useUserStore';
import { onMounted, reactive, ref, computed } from 'vue';

/**
 * Refs.
 */

const showMessage = ref(null);
let settings = ref([]);

/**
 * Stores
 */

const userStore = useUserStore();

/**
 * Form.
 */

const form = reactive({
  statusOverview: settings.statusOverview,
  showNewestTickets: settings.ShowNewestTickets,
  showMyTickets: settings.ShowMyTickets
});

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

  form.statusOverview = String(data.settings.statusOverview);

  form.showNewestTickets = String(data.settings.showNewestTickets);

  form.showMyTickets = String(data.settings.showMyTickets);

 }catch(error){
  console.log("Could not retrieve settings: ", error)
 }
}

/**
 * Function that updates the settings.
 */

async function setSettings() {
  try{
    console.log("Form: ", form)

    const response = await customFetch('settings', 'PATCH', JSON.stringify(form));

    if(!response.ok){
      console.log("Could not update settings!");
    }

    const data = await response.json();

    retrieveSettings();

    showMessage.value = true;

  }catch(error){
    console.log("Could not change settings: ", error);
  }
}

/**
 * Run on load.
 */

onMounted(() => {
  retrieveSettings();
});

</script>

<template>
     <DashboardLayout>
        <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 max-h-fit">
                <div class="w-fit">
                    <p class="font-bold text-xl">Instillinger</p>
                    <p class="text-(--secondary-text-color)">Her kan du styre brukerinstillingene dine for Helpdesk.</p>
                    <form class="flex flex-col gap-2 pt-0.5" @submit.prevent="setSettings()">
                        <SelectInput v-model="form.statusOverview" :required="false" label="Vis sak status oversikt" :values="{false: 'Av', true: 'Aktivert'}" />
                        <SelectInput v-if="userStore.user?.role === 'Admin'" v-model="form.showNewestTickets" :required="false" label="Vis nyeste saker" :values="{false: 'Av', true: 'Aktivert'}"  />
                        <SelectInput v-if="userStore.user?.role === 'User'" v-model="form.showMyTickets" :required="false" label="Vis mine saker" :values="{false: 'Av', true: 'Aktivert'}"  />
                        <button class="bg-(--main-theme-color) text-white font-bold lg:px-6 py-4 rounded-lg cursor-pointer">Lagre innstillinger</button>
                    </form>
                    <p class="text-(--secondary-text-color) pt-2" v-if="showMessage">Lagret Instillinger</p>
                </div>
            </div>
        </div>
    </DashboardLayout>
</template>