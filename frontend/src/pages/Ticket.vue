<script setup>
import SelectInput from '@/components/form/SelectInput.vue';
import TextInput from '@/components/form/TextInput.vue';
import DashboardLayout from '@/layout/DashboardLayout.vue';
import { convertToReadable, customFetch, previousRouteName } from '@/router';
import { useUserStore } from '@/stores/useUserStore';
import { PencilIcon, UserIcon, XCircleIcon, CheckCircleIcon } from '@heroicons/vue/24/solid';
import { computed, onMounted, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';

let response;

/**
 * Refs.
 */

const ticket = ref([]);
const notes = ref({});
let editMode = ref(false);

/**
 * Store.
 */

const userStore = useUserStore();

/**
 * Form.
 * 
 * Creates the ticket. Update ticket function does not use form due to it being easier to do it manually with the custom component.
 * 
 */

const form = reactive({
    title: ticket.title,
    description: ticket.description,
    status: null,
    priority: null
});

/**
 * Route/Router.
 */

const route = useRoute();
const router = useRouter();

/**
 * Function that gets notes on ticket.
 */

async function getNotesOnTicket(){
    try {
        response = await customFetch(`tickets/${route.params.id}/notes`, 'GET');

        if(!response.ok){
            console.log("Failed retrieving notes on ticket: ", response);
        }

        const data = await response.json();

        notes.value = data.notes;

        console.log("Notes: ", notes.value)

    }catch(error){
        console.log("Failed retrieving notes: ", error)
    }
}

/**
 * Function that posts a note on the ticket.
 */

async function postNoteOnTicket(){
    try {
        response = await customFetch(`tickets/${route.params.id}/notes`, 'POST', JSON.stringify({ description: form.comment, onlyAdmin: form.onlyAdmin})
        )

        if(!response.ok){
            console.log("Failed posting note on ticket: ", response);
        }

        console.log("Successfully posted note on ticket.");
        form.comment = null;

        getNotesOnTicket();

    }catch(error){
        console.log("Failed posting comment: ", error)
    }
}

/**
 * Function that retrieves the ticket.
 */

async function retrieveTicket(){
  try{
    response = await customFetch(`tickets/${route.params.id}`, 'GET');

    if(!response.ok){
      console.log("Failed retrieving ticket: ", response);
    }

    const data = await response.json();

    ticket.value = data.ticket;

    form.title = ticket.value.title;
    form.description = ticket.value.description
    form.status = ticket.value.status;
    form.priority = ticket.value.priority;
  }catch(error){
    console.log("Failed retrieving ticket: ", error);
  }
}

/**
 * Function that updates the ticket.
 */

async function updateTicket(){
  try{
    console.log("udpatingticket!");

    response = await customFetch(`tickets/${route.params.id}`, 'PATCH', JSON.stringify(form));

    if(!response.ok){
      console.log("Failed updating ticket: ", response);
    }

    const data = await response.json();

    ticket.value = data.ticket;

    form.status = ticket.value.status;
    form.priority = ticket.value.priority;
    
    editMode.value = false;

    console.log("Ticket: ", ticket.value)

  }catch(error){
    console.log("Failed updating ticket: ", error);
  }
}

/**
 * Color for priority.
 */

const priorityColors = {
    High: "#bd5e0f",
    Normal: "#2f5fd0",
    Low: "#5b6678"
}

/**
 * Translation.
 */

const priorityLabels = {
    High: 'Høy',
    Normal: 'Normal',
    Low: 'Lav',
}

/**
 * Color for status.
 */

const statusColors = {
    Open: "#2f5fd0",
    InProgress: "#a86a0b",
    Closed: "#207a4d"
}

/**
 * Translation.
 */

const statusLabels = {
    Open: 'Åpen',
    InProgress: 'Påbegynt',
    Closed: 'Fullført',
}

/**
 * Run on load.
 */

onMounted(() => {
    retrieveTicket();
    getNotesOnTicket();
});

</script>

<template>
    <DashboardLayout>
        <div class="bg-(--main-background) h-fit min-h-full p-6 flex flex-col gap-2">
            <button v-on:click="previousRouteName && previousRouteName != 'Ticket' ? router.back() : router.push({ name: 'Tickets' })" class="text-(--secondary-text-color) text-left w-fit cursor-pointer"><p>Tilbake til {{ previousRouteName && previousRouteName != 'Ticket' ? previousRouteName.toLowerCase() : 'mine saker'}}</p></button>
            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3">
                <div class="flex flex-col gap-2 md:flex-row justify-between">
                    <div>
                        <p class="text-sm text-(--secondary-text-color)">SAK - {{ ticket.id }}</p>
                    </div>
                    <div class="flex flex-row gap-4 text-sm font-bold">
                        <div :style="{ backgroundColor: statusColors[ticket.status] + '80' }" class="rounded-xl px-4 py-1">
                            <p :style="{ color: statusColors[ticket.status] }">{{ statusLabels[ticket.status] }}</p>
                        </div>
                        <div :style="{ backgroundColor: priorityColors[ticket.priority] + '80' }" class="rounded-xl px-4 py-1">
                            <p :style="{ color: priorityColors[ticket.priority] }">{{ priorityLabels[ticket.priority] }}</p>
                        </div>
                    </div>
                </div>
                <form class="flex flex-col gap-2" @submit.prevent="updateTicket()">
                    <div class="flex flex-row gap-2 items-center">
                        <input v-model="form.title" class="px-2 py-2 rounded-lg" v-if="editMode" type="text" :placeholder="ticket.title" />
                        <p v-if="!editMode" class="font-bold text-xl">{{ ticket.title }}</p>
                        <PencilIcon v-if="!editMode" v-on:click="editMode = true" class="size-4 text-(--secondary-text-color) cursor-pointer" />
                        <div v-if="editMode" class="flex flex-row gap-2 items-center">
                            <button type="button" class="md:block hidden">
                                <XCircleIcon v-on:click="editMode = false" class="my-auto size-6 text-(--secondary-text-color) cursor-pointer" />
                            </button>
                            <button class="md:block hidden">
                                <CheckCircleIcon class="size-6 text-(--main-theme-color) cursor-pointer" />
                            </button>
                        </div>
                    </div>
                    <textarea v-model="form.description" v-if="editMode" class="px-2 py-2 rounded-lg" type="text" :placeholder="ticket.description" />
                    <p v-if="!editMode" class="flex lg:hidden teßxt-sm">{{ ticket.description}}</p>
                    <button type="button" v-if="editMode" v-on:click="editMode = false" class="bg-(--secondary-text-color) text-white font-bold rounded-lg p-2 md:hidden block cursor-pointer">Avbryt</button>
                    <button v-if="editMode" class="bg-(--main-theme-color) text-white font-bold rounded-lg p-2 md:hidden block cursor-pointer">Oppdater ticket</button>
                </form>
                <div class="border-b-2 border-(--secondary-background-border)"></div>
                <div class="grid grid-cols-2 md:grid-cols-3 gap-8">
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">OPPRETTET AV</p>
                        <p class="font-bold text-sm">{{ ticket.createdByUser?.name }}</p>
                    </div>
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">OPPRETTET</p>
                        <p class="font-bold text-sm">{{ convertToReadable(ticket.createdAt) }}</p>
                    </div>
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">SIST OPPDATERT</p>
                        <p class="font-bold text-sm">{{ convertToReadable(ticket.updatedAt)}}</p>
                    </div>
                </div>
            </div>
            <div class="flex flex-col lg:flex-row gap-4">
                <div :class="[userStore.user?.role === 'Admin' ? 'lg:w-1/2' : 'w-full', editMode === true ? 'hidden!' : '']" class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 hidden lg:flex flex-col gap-3">
                    <p class="font-bold">Beskrivelse</p>
                    <div class="border-b-2 border-(--secondary-background-border)"></div>
                    <p class="text-sm">{{ ticket.description }}</p>
                </div>
                <div v-if="userStore.user?.role === 'Admin' && editMode === false" class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3 lg:w-1/2">
                    <p class="font-bold">Oppdater ticket status</p>
                    <div class="border-b-2 border-(--secondary-background-border)"></div>
                    <form class="flex flex-col gap-3" @submit.prevent="updateTicket()">
                        <SelectInput v-model="form.status" :required="false" label="Ticket status" :values="{InProgress: 'Påbegynt', Closed: 'Ferdig'}" />
                        <SelectInput v-model="form.priority" :required="false" label="Ticket prioritet" :values="{Low: 'Lav', Normal: 'Normal', High: 'Høy'}"/>
                        <button class="bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium px-4 py-2 rounded-lg cursor-pointer">Oppdater ticket status</button>
                    </form>
                </div>
            </div>

            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex-1 flex flex-col gap-3 min-h-0">
                <p class="font-bold">Kommentarer</p>
                <div class="border-b-2 border-(--secondary-background-border)"></div>
                <div class="flex flex-col gap-2 overflow-y-auto flex-1 min-h-0">
                    <div v-for="note in notes">
                        <div class="flex flex-row gap-3">
                            <div class="my-auto">
                                <UserIcon class="size-8" />
                            </div>
                            <div class="flex flex-col">
                                <div class="flex flex-row items-center gap-3">
                                    <p class="font-bold">{{ note.writtenByUser}}</p>
                                    <p class="text-sm text-(--secondary-text-color)">{{ convertToReadable(note.createdAt) }}</p>
                                    <p class="text-sm text-(--secondary-text-color)" v-if="note.onlyAdmin">Admin</p>
                                </div>
                                <p class="text-sm">{{ note.description }}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="border-b-2 border-(--secondary-background-border) mt-auto"></div>
                <div class="flex flex-col gap-2">
                    <form @submit.prevent="postNoteOnTicket">
                        <textarea
                            v-model="form.comment"
                            rows="3"
                            placeholder="Skriv en kommentar..."
                            class="w-full border-2 border-(--secondary-background-border) rounded-lg p-3 text-sm resize-none focus:outline-none focus:border-blue-400"
                        ></textarea>
                        <div class="flex flex-row gap-4 justify-end">
                            <div v-if="userStore.user.role === 'Admin'" class="flex flex-row gap-2 items-center">
                                <p class="text-(--secondary-text-color) font-medium">Bare for admins</p>
                                <input type="checkbox" v-model="form.onlyAdmin">
                            </div>
                            <button class="bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium px-4 py-2 rounded-lg cursor-pointer">Send kommentar</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </DashboardLayout>
</template>