<script setup>
import { AcademicCapIcon } from '@heroicons/vue/24/outline';
</script>

<template>
    <div class="flex flex-col items-center justify-center bg-(--main-background) w-screen h-screen gap-4">
        <div class="bg-(--main-theme-color) rounded-xl p-2">
            <AcademicCapIcon class="size-7 text-white" />
        </div>
        <p class="text-8xl font-bold text-(--main-text-color)">404</p>
        <p class="text-(--secondary-text-color)">Siden du leter etter finnes ikke</p>
        <RouterLink to="/" class="bg-(--main-theme-color) text-white font-bold px-6 py-2.5 rounded-xl cursor-pointer">Gå tilbake</RouterLink>
    </div>
</template>
