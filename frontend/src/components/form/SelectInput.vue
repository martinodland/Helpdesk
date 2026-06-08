<script setup>

/**
 * Props.
 */

const props = defineProps({
    modelValue: {
        default: ''
    },
    label: {
        type: String,
        default: 'Ticket Prioritet'
    },
    required: {
        type: Boolean,
        default: true
    },
    values: {
        type: Object,
        default: {}
    },
});

/**
 * Define emits. 
 * 
 * Says to the parent that he needs to update the form with the modelvalue.
 */

const emit = defineEmits('update:modelValue');

</script>

<template>
    <div>
        <div class="flex flex-row gap-2" v-if="props.label">
            <p class="text-(--label-color) font-bold text-sm lg:text-md">{{ props.label }}</p>
            <p v-if="required === true" class="font-bold text-[#c23b30]">*</p>
        </div>
        <div class="flex flex-row gap-2">
            <div v-for="(value, key) in props.values">
                <div v-on:click="emit('update:modelValue', key);" :class="modelValue == key ? 'bg-(--main-theme-color)! text-white!' : ''" class="rounded-xl border-2 border-gray-300 bg-white p-2 text-(--secondary-text-color) font-medium px-4 cursor-pointer">
                    <p class="text-sm lg:text-md">{{ value }}</p>
                </div>
            </div>
        </div>
    </div>
</template>