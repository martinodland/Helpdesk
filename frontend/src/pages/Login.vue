<script setup>

import AuthLayout from '@/layout/AuthLayout.vue';
import TextInput from '@/components/form/TextInput.vue';
import { RouterLink, useRouter } from 'vue-router';
import { reactive } from 'vue';
import { customFetch } from '@/router';

/**
 * Router
 */

const router = useRouter();

/**
 * Form
 */

const form = reactive({
    email: null,
    password: null
});

/**
 * Function to log in user.
 */

async function login() {
    try{
        const result = await customFetch('auth/login', 'POST', JSON.stringify(form));

        console.log("result: ", result);

        if(!result.ok){
            return;
        }
        
        router.push({ name: 'Dashboard' });
    }catch(error){
        console.log("error: ", error);
    }
}

</script>

<template>
    <AuthLayout>
        <div class="flex flex-col gap-2">
            <p class="text-xl font-bold text-(--main-text-color)">Logg inn</p>
            <p class="text-(--secondary-text-color)">Bruk ditt firma e-post og passord</p>
            <form class="flex flex-col gap-4" @submit.prevent="login">
                <TextInput v-model="form.email" label="E-post"  placeholder="martin.odland@hjelseth.com" type="email" :required="true" />
                <TextInput v-model="form.password" label="Passord"  placeholder="&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;" type="password" :required="true" />
                <button class="bg-(--main-theme-color) text-white font-bold w-full py-2.5 rounded-xl mt-1.5 cursor-pointer">Logg inn</button>
            </form>
            <span>Har du ikke en konto? <RouterLink class="text-(--main-theme-color)" to="/register">Registerer deg.</RouterLink></span>
        </div>
    </AuthLayout>
</template>