<script setup>
import { onMounted } from 'vue';
import MainLayout from '../layout/MainLayout.vue';

/**
 * Register
 */

async function register(){
  try {
    const results = await fetch('http://localhost:5034/auth/register',{
      method: 'POST',
      headers: { 'Content-Type': 'application/json'},
      body: JSON.stringify({name: "test", email: "test@gmail.com", "password": "test"}),
      credentials: 'include'
    });
  
    if (results.ok){
      const data = await results.json();

      console.log("DATA: ", data);
    }
  }catch(error){
    console.log("error: ", error)
  }

  try {
    const results = await fetch('http://localhost:5034/auth/test',{
      method: 'GET',
      headers: { 'Content-Type': 'application/json'},
      credentials: 'include'
    });
  
    if (results.ok){
      const data = await results.json();

      console.log("DATA: ", data);

      console.log("AUTH WORKS");
    }
  }catch(error){
    console.log("error: ", error)
  }
}

/**
 * Run on load.
 */

onMounted(() => {
  register();
});

</script>

<template>
  <MainLayout>
    <div class="bg-red-500 text-white p-4">Tailwind test</div>
  </MainLayout>
</template>
