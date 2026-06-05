<script setup>
import { reactive, ref } from 'vue';
import DashboardLayout from '../layout/DashboardLayout.vue';
import TextInput from '@/components/form/TextInput.vue';
import TextAreaInput from '@/components/form/TextAreaInput.vue';
import SelectInput from '@/components/form/SelectInput.vue';
import { customFetch } from '@/router/index.js';

/**
 * Refs.
 */

const showMessage = ref(null);
let lastClicked = ref(null);

/**
 * Form
 */

const form = reactive({
  title: null,
  text: null,
  priority: null
});

/**
 * Function that creates the ticket.
 */

async function createTicket() {
  try{
    const response = await customFetch('tickets', 'POST', JSON.stringify(form));

    if(!response.ok){
      console.log("Could not create ticket!");
    }

    const data = await response.json();

    form.title = null;
    form.text = null;
    form.priority = null;

    showMessage.value = true;

  }catch(error){
    console.log("Could not create ticket!");
  }
}

</script>

<template>
  <DashboardLayout>
    <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
      <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-2 h-full">
        <p class="font-bold text-xl">Registrer ny sak</p>
        <p class="text-(--secondary-text-color)">Beskriv problemet så godt du kan, så hjelper brukerstøtte deg videre. Felter merket med * er obligatoriske.</p>
        <form @submit.prevent="createTicket()" class="flex flex-col gap-4.5">
          <TextInput v-model="form.title" placeholder="Får ikke til..." label="Tittel" :required="true" />
          <SelectInput :values="{Low: 'Lav', Normal: 'Normal', High: 'Høy'}" v-model:lastClicked="lastClicked" v-model="form.priority" />
          <TextAreaInput v-model="form.text" placeholder="Hva har skjedd? Hva har du forsøkt? Når startet problemet?" label="Beskrivelse" :required="true" />
          <button class="bg-(--main-theme-color) text-white font-bold lg:px-6 py-4 rounded-lg cursor-pointer">Registrer ny sak</button>
        </form>
        <p class="text-(--secondary-text-color)" v-if="showMessage">Ticket er opprettet</p>
      </div>
    </div>
  </DashboardLayout>
</template>
