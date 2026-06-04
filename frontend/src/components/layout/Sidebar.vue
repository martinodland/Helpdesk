<script setup>
import { AcademicCapIcon } from '@heroicons/vue/24/outline';
import SidebarMenu from '../sidebar/SidebarMenu.vue';


</script>

<template>
    <div class="hidden md:flex md:flex-col md:gap-2 md:w-2/5 lg:w-1.5/5 xl:w-1/5 bg-(--secondary-theme-color) p-4 h-screen items-start">
        <div class="flex flex-row gap-3 items-center">
            <div class="bg-(--main-theme-color) rounded-xl p-2">
                <AcademicCapIcon class="size-5 text-white" />
            </div>
            <div class="flex flex-col">
                <p class="font-bold text-md text-white">Helpdesk</p>
                <p class="text-(--secondary-text-color) text-sm">Intern brukerstøtte</p>
            </div>
        </div>
        <div class="flex flex-col w-full gap-1">
            <SidebarMenu label="Ovesikt" href="/overview" />
            <SidebarMenu label="Mine saker" href="/tickets" icon="ListBulletIcon" />
            <SidebarMenu label="Ny sak" href="/create" icon="PlusIcon" />
        </div>
    </div>
</template>