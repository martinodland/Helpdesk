<script setup>
import DashboardLayout from '@/layout/DashboardLayout.vue';
import { convertToReadable, customFetch, previousRouteName } from '@/router';
import { useUserStore } from '@/stores/useUserStore';
import { UserIcon } from '@heroicons/vue/24/solid';
import { onMounted, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';

let response;

/**
 * Refs.
 */

const ticket = ref([]);
const notes = ref({});

/**
 * Store.
 */

const userStore = useUserStore();

/**
 * Form.
 */

const form = reactive({
    ticketId: ticket.id,
    onlyAdmin: false,
    comment: null
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
        response = await customFetch(
            `tickets/${route.params.id}/notes`,
            'POST',
            JSON.stringify({ description: form.comment, onlyAdmin: form.onlyAdmin })
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

    console.log("Ticket: ", ticket.value)

  }catch(error){
    console.log("Failed retrieving ticket: ", error);
  }
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
        <div class="bg-(--main-background) h-full p-6 flex flex-col gap-2">
            <button v-on:click="previousRouteName && previousRouteName != 'Ticket' ? router.back() : router.push({ name: 'Tickets' })" class="text-(--secondary-text-color) text-left w-fit cursor-pointer"><p>Tilbake til {{ previousRouteName && previousRouteName != 'Ticket' ? previousRouteName.toLowerCase() : 'mine saker'}}</p></button>
            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3">
                <div class="flex flex-row justify-between">
                    <div>
                        <p class="text-sm text-(--secondary-text-color)">SAK - {{ ticket.id }}</p>
                    </div>
                    <div class="flex flex-row gap-4 text-sm">
                        <div>
                            <p>{{ ticket.status }}</p>
                        </div>
                        <div>
                            <p>{{ ticket.priority }}</p>
                        </div>
                    </div>
                </div>
                <p class="font-bold text-xl">{{ ticket.title }}</p>
                <div class="border-b-2 border-(--secondary-background-border)"></div>
                <div class="grid grid-cols-2 md:grid-cols-3 gap-8">
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">KATEGORI</p>
                        <p class="font-bold text-sm">Programvare</p>
                    </div>
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">OPPRETTET AV</p>
                        <p class="font-bold text-sm">{{ ticket.createdByUser?.name }}</p>
                    </div>
                    <div class="flex flex-col">
                        <p class="text-sm text-(--secondary-text-color) font-medium">OPPRETTET</p>
                        <p class="font-bold text-sm">{{ convertToReadable(ticket.createdAt) }}</p>
                    </div>
                </div>
            </div>
            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3">
                <p class="font-bold">Beskrivelse</p>
                <div class="border-b-2 border-(--secondary-background-border)"></div>
                <p class="text-sm">{{ ticket.description }}</p>
            </div>

            <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg p-6 mt-2 flex flex-col gap-3">
                <p class="font-bold">Kommentarer</p>
                <div class="border-b-2 border-(--secondary-background-border)"></div>
                <div class="flex flex-col gap-2">
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
                <div class="border-b-2 border-(--secondary-background-border)"></div>
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
                            <button class="bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium px-4 py-2 rounded-lg cursor-pointer">
                                Send kommentar
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </DashboardLayout>
</template>