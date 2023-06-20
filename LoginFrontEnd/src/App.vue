<script setup>
import { ref } from 'vue'
import { useUserStore } from './stores/user'
import httpInstance from './utils/http'
import { useRouter } from 'vue-router'

const router = useRouter()
const userStore = useUserStore()


const logout = async () => {
  await httpInstance.request({
    method: 'POST',
    url: '/Login/Logout'
  })
  userStore.token = null
  router.push('/login')
}
</script>

<template>
  <div class="page">
    <div class="nav">
      <RouterLink class="link" to="/">Home</RouterLink>
      <!-- <RouterLink class="link" to="/signalR">SignalR</RouterLink> -->
      <RouterLink class="link" to="/login" v-if="!userStore.token">Login</RouterLink>
      <a class="link" @click="logout" v-if="userStore.token">Logout</a>
      <RouterLink class="link" to="/register">register</RouterLink>
    </div>
    <div class="content">
      <RouterView />
    </div>
  </div>
</template>

<style scoped>
.nav {
  width: auto;
  height: 50px;
  text-align: center;
  position: fixed;
  top: 0;
  padding: 10px;
}
.link {
  display: inline-block;
  width: 150px;
  height: 30px;
  text-align: center;
  line-height: 30px;
  cursor: pointer;
}
</style>
