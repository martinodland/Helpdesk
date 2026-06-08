<script setup>

import { EnvelopeIcon, EnvelopeOpenIcon, CheckCircleIcon } from '@heroicons/vue/24/outline';
import { computed } from 'vue';

/**
 * Props.
 */

const props = defineProps({
    amountOfTickets: {
        type: Number,
        default: 0
    },
    label: {
        type: String,
        default: "Åpne saker"
    },
    icon: {
        type: String,
        default: "EnvelopeIcon"
    }
});
/**
 * Icons.
 */

const icons = {
  EnvelopeIcon: EnvelopeIcon,
  EnvelopeOpenIcon: EnvelopeOpenIcon,
  CheckCircleIcon: CheckCircleIcon
}

/**
 * Get the icon that should be active.
 */

const selectedIcon = computed(() => icons[props.icon]);

</script>

<template>
    <div class="bg-white border-(--secondary-background-border) border-2 rounded-lg">
        <div class="flex flex-row gap-5">
            <div class="flex items-center m-4 p-2 bg-(--secondary-theme-color) text-white rounded-lg">
                <component :is="selectedIcon" class="size-8" />
            </div>
            <div class="flex flex-col my-auto">
                <p class="font-bold text-2xl">{{ props.amountOfTickets }}</p>
                <p class="text-sm text-(--secondary-text-color)">{{ props.label }}</p>
            </div>
        </div>
    </div>
</template>