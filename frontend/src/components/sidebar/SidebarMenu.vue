<script setup>

import { PlusIcon, ListBulletIcon,RectangleGroupIcon, Cog6ToothIcon } from '@heroicons/vue/24/outline';
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';

/**
 * Route.
 */

const route = useRoute();

/**
 * Props.
 */

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

/**
 * Icon for menus.
 */

const icons = {
    RectangleGroupIcon: RectangleGroupIcon,
    PlusIcon: PlusIcon,
    ListBulletIcon: ListBulletIcon,
    Cog6ToothIcon: Cog6ToothIcon
}

/**
 * Refs.
 */

const selectedIcon = ref(icons[props.icon]);

/**
 * Set the active path.
 */

const isActive = route.path.includes(props.href);

</script>

<template>
    <RouterLink :to="props.href">
        <div class="text-white flex flex-row gap-4 p-4 cursor-pointer rounded-md" :class="isActive ? 'bg-(--main-theme-color)' : 'hover:bg-white/5 transition duration-100'">
            <component :is="selectedIcon" class="size-6 text-white" />
            <p>{{ props.label }}</p>
        </div>
    </RouterLink>
</template>