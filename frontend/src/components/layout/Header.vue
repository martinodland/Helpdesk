<script setup>

import { usePageNameStore } from '@/stores/usePageNameStore';
import { useUserStore } from '@/stores/useUserStore';
import { ChevronDownIcon, BellIcon, UserIcon, Bars3Icon, XMarkIcon } from '@heroicons/vue/24/solid';
import { ref } from 'vue';

/**
 * Refs.
 */

const showMenu = ref(false);

/**
 * Stores
 */

const userStore = useUserStore();
const pageNameStore = usePageNameStore();

console.log("userStore: ", userStore)

/**{{ userStore.user?.name }} {{ pageNameStore.pageName }} */

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
            </div>
        </div>
    </div>
    <div v-if="!showMenu" class="flex flex-row justify-between px-2 py-3 md:py-3 md:px-4">
        <div class="flex flex-col gap-1 w-1/2">
            <p class="font-bold text-md md:text-xl">{{ pageNameStore.pageName }}</p>
            <p class="text-sm md:text-md text-(--secondary-text-color)" v-if="pageNameStore.pageName == 'Oversikt'">Søndag 1. juni 2026</p>
        </div>
        <div class="hidden md:flex flex-row w-1/2 gap-7 justify-end">
            <div class="flex items-center">
                <BellIcon class="size-7 text-(--secondary-text-color) cursor-pointer hover:text-(--secondary-theme-color) hover:scale-110 transition duration-200" />
            </div>
            <div class="flex flex-row gap-4 items-center">
                <div class="bg-(--secondary-theme-color) rounded-3xl">
                    <UserIcon class="size-10 text-white" />
                </div>
                <div class="flex flex-col">
                    <p class="font-bold">{{ userStore.user?.name }}</p>
                    <p class="text-(--secondary-text-color) text-sm">Bruker</p>
                </div>
            </div>
            <div class="flex items-center">
                <ChevronDownIcon class="size-4 text-(--secondary-text-color) cursor-pointer hover:text-(--secondary-theme-color) hover:scale-110 transition duration-200" />
            </div>
        </div>
        <div class="flex items-center md:hidden">
            <Bars3Icon v-on:click="showMenu = true" class="size-7 cursor-pointer" />
        </div>
    </div>
</template>