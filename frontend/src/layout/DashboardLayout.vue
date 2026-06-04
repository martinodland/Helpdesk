<script setup>
import Header from '@/components/layout/Header.vue';
import Sidebar from '@/components/layout/Sidebar.vue';



</script>

<template>
    <div class="flex flex-row">
        <Sidebar />
        <div class="w-full">
            <Header />
            <slot />
        </div>
    </div>
</template>