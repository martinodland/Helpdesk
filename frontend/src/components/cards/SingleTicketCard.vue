<script setup>

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
        default: 'Høy'
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
    }
});

</script>

<template>
    <div class="border-t-2 border-(--secondary-background-border) py-5">
        <div class="flex flex-col lg:flex-row justify-between gap-2 cursor-pointer">
            <div class="flex flex-col gap-2">
                <div class="flex flex-row gap-2">
                    <p class="text-(--secondary-text-color)">SAK - {{ props.ticketId }}</p>
                    <div class="bg-red-500/30 rounded-xl px-3">
                        <p class="font-bold text-red-500">{{ props.ticketStatus }}</p>
                    </div>
                </div>
                <p class="font-bold text-md lg:text-lg">{{ props.ticketTitle }}</p>
                <p class="text-sm text-(--secondary-text-color)">Programvare - {{ props.ticketCreated }} - {{ props.ticketUpdated }}</p>
            </div>
            <div class="flex flex-col my-auto">
                <div class="bg-green-500/30 rounded-xl px-3">
                    <p class="font-bold text-green-500 text-center">Under arbeid</p>
                </div>
            </div>
        </div>
    </div>
</template>