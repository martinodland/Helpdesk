<script setup>
import { onMounted } from 'vue';
import { useUserStore } from '@/stores/useUserStore';
import { usePageNameStore } from './stores/usePageNameStore';

/**
 * Stores.
 */

const userStore = useUserStore();
const pageNameStore = usePageNameStore();

/**
 * Run on load.
 */

onMounted(() => {
  userStore.fetchUser();
  pageNameStore.setPageName();
});

</script>

<template>
  <router-view />
</template>
