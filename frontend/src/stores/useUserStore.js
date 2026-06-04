import { customFetch } from '@/router';
import { defineStore } from 'pinia';

// Define the userstore.

export const useUserStore = defineStore('user', {
    state: () => ({
        user: null
    }),
    actions: {
        async fetchUser(){
            const result = await customFetch('auth/me', 'GET');

            if(result.ok){
                const data = await result.json();

                this.user = data.user;
            }
        }
    }
});