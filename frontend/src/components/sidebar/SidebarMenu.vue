<script setup>

import { PlusIcon, ListBulletIcon,RectangleGroupIcon } from '@heroicons/vue/24/outline';
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const props = defineProps({
    label: {
        type: String,
        default: 'Overview'
    },
    href: {
        type: String,
        default: '/'
    },
    icon: {
        type: String,
        default: 'RectangleGroupIcon'
    }
});

const icons = {
    RectangleGroupIcon: RectangleGroupIcon,
    PlusIcon: PlusIcon,
    ListBulletIcon: ListBulletIcon,
}

const selectedIcon = computed(() => icons[props.icon]);

const route = useRoute();

const isActive = route.path === props.href;

</script>

<template>
    <RouterLink :to="props.href">
        <div class="text-white flex flex-row gap-4 p-4 cursor-pointer rounded-md"
            :class="isActive ? 'bg-(--main-theme-color)' : 'hover:bg-white/5 transition duration-100'"
            >
            <component :is="selectedIcon" class="size-6 text-white" />
            {{ props.label }}
        </div>
    </RouterLink>
</template>