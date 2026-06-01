<script setup>
import Footer from '@/components/Footer.vue';
import Header from '@/components/Header.vue';

</script>

<template>
    <div>
        <Header />
            <slot />
        <Footer />
    </div>
</template>