<script setup>
import Header from '@/components/layout/Header.vue';
import Sidebar from '@/components/layout/Sidebar.vue';



</script>

<template>
    <div class="flex flex-row h-screen overflow-hidden">
        <Sidebar />
        <div class="flex flex-col flex-1 overflow-hidden">
            <Header />
            <div class="flex-1 overflow-auto">
                <slot />
            </div>
        </div>
    </div>
</template>