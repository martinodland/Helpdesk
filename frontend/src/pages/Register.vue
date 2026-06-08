<script setup>

import AuthLayout from '@/layout/AuthLayout.vue';
import TextInput from '@/components/form/TextInput.vue';
import { RouterLink, useRouter } from 'vue-router';
import { customFetch } from '@/router/index.js';
import { reactive } from 'vue';
import { useUserStore } from '@/stores/useUserStore';

/**
 * Router
 */

const router = useRouter();

/**
 * Stores
 */

const userStore = useUserStore();

/**
 * Form
 */

const form = reactive({
    name: null,
    email: null,
    password: null
});

/**
 * Function to register user.
 */

async function register() {
    try{
        const result = await customFetch('auth/register', 'POST', JSON.stringify(form));

        console.log("result: ", result);

        if(!result.ok){
            return;
        }
        
        await userStore.fetchUser();

        router.push({ name: 'Dashboard' });
    }catch(error){
        console.log("error: ", error);
    }
}

</script>

<template>
    <AuthLayout>
        <div class="flex flex-col gap-2">
            <p class="text-xl font-bold text-(--main-text-color)">Registrer deg</p>
            <p class="text-(--secondary-text-color)">Bruk ditt firma e-post og passord</p>
            <form class="flex flex-col gap-4" @submit.prevent="register">
                <TextInput v-model="form.name" label="Navn"  placeholder="Martin Odland" type="text" :required="true" />
                <TextInput v-model="form.email" label="E-post"  placeholder="martin.odland@hjelseth.com" type="email" :required="true" />
                <TextInput v-model="form.password" label="Passord"  placeholder="&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;" type="password" :required="true" />
                <button class="bg-(--main-theme-color) text-white font-bold w-full py-2.5 rounded-xl mt-1.5 cursor-pointer">Registrer</button>
            </form>
            <span>Har du allerede en konto? <RouterLink class="text-(--main-theme-color)" to="/login">Logg inn.</RouterLink></span>
        </div>
    </AuthLayout>
</template>