<script setup>
import { AcademicCapIcon } from '@heroicons/vue/24/outline';


</script>

<template>
    <div class="flex bg-(--main-background) w-screen h-screen">
        <div class="flex flex-col mx-4 sm:mx-auto my-auto bg-(--secondary-background) border border-(--secondary-background-border) h-145 w-full sm:w-115 rounded-xl p-4.5 sm:p-6">
            <div class="flex flex-row gap-3 items-center pb-4">
                <div class="bg-(--main-theme-color) rounded-xl p-2">
                    <AcademicCapIcon class="size-7 text-white" />
                </div>
                <p class="font-bold text-xl">Helpdesk</p>
            </div>
            <slot />
        </div>
    </div>
</template>