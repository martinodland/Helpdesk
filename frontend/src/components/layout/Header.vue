<script setup>

import { convertToReadable, customFetch } from '@/router';
import { usePageNameStore } from '@/stores/usePageNameStore';
import { useUserStore } from '@/stores/useUserStore';
import { ChevronDownIcon, BellIcon, UserIcon, Bars3Icon, XMarkIcon } from '@heroicons/vue/24/solid';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

/**
 * Refs.
 */

const showMenu = ref(false);
const openLogout = ref(false);

/**
 * Router.
 */

const router = useRouter();

/**
 * Stores
 */

const userStore = useUserStore();
const pageNameStore = usePageNameStore();

/**
 * Function that logs the user out.
 */

async function logout() {
    try{
        const response = await customFetch('auth/logout', 'DELETE');

        if(!response.ok){
            console.log("Could not log out the user!");
        }

        router.push({ name: Login });
    }catch(error){
        console.log("Could not log out the user!");
    }
}

/**{{ userStore.user?.name }} {{ pageNameStore.pageName }} */

// Object.keys(monthsNo).find(key => key.includes(new Date().))

</script>

<template> 
    <div v-if="showMenu" class="bg-(--secondary-theme-color) h-screen w-screen z-40">
        <div class="flex flex-col px-2 py-3">
            <div class="flex items-center">
                <XMarkIcon v-on:click="showMenu = false" class="ml-auto mt-3 size-7 text-white cursor-pointer" />
            </div>
            <div class="flex flex-col gap-4 text-white font-bold">
                <RouterLink v-on:click="showMenu = false" class="cursor-pointer" to="/overview">Oversikt</RouterLink>
                <RouterLink v-on:click="showMenu = false" class="cursor-pointer" to="/tickets">Mine saker</RouterLink>
                <RouterLink v-on:click="showMenu = false" class="cursor-pointer" to="/create">Ny sak</RouterLink>
                <form  @submit.prevent="logout()">
                    <button class="mr-auto">Logg ut</button>
                </form>
            </div>
        </div>
    </div>
    <div v-if="!showMenu" class="flex flex-row justify-between px-2 py-3 md:py-3 md:px-4">
        <div class="flex flex-col gap-1 w-1/2">
            <p class="font-bold text-md md:text-xl">{{ pageNameStore.pageName }}</p>
            
            <p class="text-sm md:text-md text-(--secondary-text-color)" v-if="pageNameStore.pageName == 'Oversikt'">{{ new Date().toLocaleDateString('nb-NO') + " " + new Date().toLocaleTimeString('nb-NO').slice(0, -3) }}</p>
        </div>
        <div class="hidden md:flex flex-row w-1/2 gap-7 justify-end">
            <div class="flex flex-row gap-4 items-center">
                <div class="bg-(--secondary-theme-color) rounded-3xl">
                    <UserIcon class="size-10 text-white" />
                </div>
                <div class="flex flex-col">
                    <p class="font-bold">{{ userStore.user?.name }}</p>
                    <p class="text-(--secondary-text-color) text-sm">{{ userStore.user?.role }}</p>
                </div>
            </div>
            <div class="relative my-auto">
                <button class="transition duration-200" :class="openLogout === false ? 'rotate-180' : ''" type="button" @click="openLogout = !openLogout">
                    <ChevronDownIcon class="size-4 text-(--secondary-text-color) cursor-pointer" />
                </button>
                <div v-if="openLogout" class="absolute right-0 top-[46px] mt-1 bg-white border border-t-0 rounded-tr-none rounded-tl-none border-gray-200 rounded-lg shadow-lg p-2 w-32">
                    <form @submit.prevent="logout()">
                        <button class="cursor-pointer hover:text-red-500" @click="logout()">Logg ut</button>
                    </form>
                </div>
            </div>
        </div>
        <div class="flex items-center md:hidden">
            <Bars3Icon v-on:click="showMenu = true" class="size-7 cursor-pointer" />
        </div>
    </div>
</template>