<script setup>
import { computed } from 'vue';

/**
 * Props.
 */

const props = defineProps({
    ticketId: {
        type: Number,
        default: 1
    },
    ticketStatus: {
        type: String,
        default: 'Normal'
    },
    ticketTitle: {
        type: String,
        default: "Får ikke logget inn på e-post"
    },
    ticketCreated: {
        type: String,
        default: "Opprettet 29.05.2026"
    },
    ticketUpdated: {
        type: String,
        default: "Oppdatert 30.05.2026"
    },
    ticketPriority: {
        type: String,
        default: "Normal"
    }
});

/**
 * Color for priority.
 */

const priorityColors = {
    Høy: "#bd5e0f",
    Normal: "#2f5fd0",
    Lav: "#5b6678"
}

/**
 * Color for status.
 */

const statusColors = {
    Åpen: "#2f5fd0",
    Påbegynt: "#a86a0b",
    Fullført: "#207a4d"
}

/**
 * Get the colors that should be active.
 */

const activePriorityColor = computed(() => priorityColors[props.ticketPriority]);

const activeStatusColor = computed(() => statusColors[props.ticketStatus]);

</script>

<template>
    <RouterLink :to="`/tickets/${props.ticketId}`">
        <div class="border-t-2 border-(--secondary-background-border) py-5">
            <div class="flex flex-col lg:flex-row justify-between gap-2 cursor-pointer">
                <div class="flex flex-col gap-2">
                    <div class="flex flex-row gap-2">
                        <p class="text-(--secondary-text-color)">SAK - {{ props.ticketId }}</p>
                        <div :style="{ backgroundColor: activePriorityColor + '80' }" class="rounded-xl px-3">
                            <p :style="{ color: activePriorityColor }" class="font-bold">{{ props.ticketPriority }}</p>
                        </div>
                    </div>
                    <p class="font-bold text-md lg:text-lg">{{ props.ticketTitle }}</p>
                    <p class="text-sm text-(--secondary-text-color)">Programvare - {{ props.ticketCreated }} - {{ props.ticketUpdated }}</p>
                </div>
                <div class="flex flex-col my-auto">
                    <div :style="{ backgroundColor: activeStatusColor + '80' }" class="rounded-xl px-3">
                        <p :style="{ color: activeStatusColor }" class="font-bold text-center">{{ props.ticketStatus }}</p>
                    </div>
                </div>
            </div>
        </div>
    </RouterLink>
</template>