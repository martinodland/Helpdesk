<script setup>
 
const props = defineProps({
    modelValue: {
        default: ''
    },
    label: {
        type: String,
        default: ''
    },
    placeholder: {
        type: String,
        default: ''
    },
    image: {
        type: String,
        default: ''
    },
    type: {
        type: String,
        default: ''
    },
    required: {
        type: Boolean,
        default: false
    }
});

const emit = defineEmits(['update:modelValue']);
 
</script>
 
<template>
    <div class="flex flex-col gap-2">
        <div class="flex flex-row gap-2">
            <p class="font-bold text-(--label-color)">{{ props.label }}</p>
            <p v-if="required === true" class="font-bold text-[#c23b30]">*</p>
        </div>
        <div class="rounded-xl border-2 border-gray-300 bg-white p-3">
            <input
                :class="type === 'password' ? 'placeholder:text-xs' : ''"
                class="h-fit w-full focus:border-white focus:ring-2 focus:ring-white focus:outline-none placeholder-(--placeholder-color)"
                :placeholder="props.placeholder"
                :type="props.type"
                :value="props.modelValue"
                @input="emit('update:modelValue', $event.target.value)"
            />
        </div>
    </div>
</template>