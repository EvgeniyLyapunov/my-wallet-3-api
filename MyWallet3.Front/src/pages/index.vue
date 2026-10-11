<template>
  <q-page class="flex flex-center">
    <q-card style="width: 400px; max-width: 90vw;">
      <q-card-section>
        <div class="text-h6">Тест API: Авторизация</div>
      </q-card-section>

      <q-card-section class="q-gutter-md">
        <q-input 
          v-model="login" 
          label="Логин" 
          outlined 
          dense 
        />
        <q-input 
          v-model="password" 
          label="Пароль" 
          type="password" 
          outlined 
          dense 
        />
      </q-card-section>

      <q-card-actions :align="'right'">
        <q-btn 
          color="primary" 
          label="Войти" 
          :loading="isLoading"
          @click="testLogin" 
        />
      </q-card-actions>

      <!-- Блок для вывода ответа сервера -->
      <q-card-section v-if="response" class="bg-grey-2 q-mt-md">
        <div class="text-caption text-weight-bold">Ответ сервера:</div>
        <pre class="text-caption" style="white-space: pre-wrap; word-break: break-all;">
          {{ response }}
        </pre>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
// ПРОВЕРЬ ИМЯ ФУНКЦИИ И ПУТЬ!
import { getAuth } from '@/api/generated/auth/auth'; 

const login = ref('');
const password = ref('');
const isLoading = ref(false);
const response = ref<any>(null);

async function testLogin() {
  if (!login.value || !password.value) {
    response.value = 'Введите логин и пароль';
    return;
  }

  isLoading.value = true;
  response.value = null;

  try {
    // Вызываем API (TypeScript подскажет поля)
    const result = await getAuth().postApiAuthLogin({
      login: login.value,
      password: password.value
    });
    
    // В axios данные лежат в .data
    response.value = result.data; 
    console.log('Успех:', result.data);

  } catch (error: any) {
    // Ловим ошибки (например, неверный пароль)
    response.value = error.response?.data || error.message;
    console.error('Ошибка:', error);
  } finally {
    isLoading.value = false;
  }
}
</script>