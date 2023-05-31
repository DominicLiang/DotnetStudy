<script setup>
import { useUserStore } from './stores/user'
import httpInstance from './utils/http'
import { useRouter } from 'vue-router'

const router = useRouter()

const logout = async () => {
  await httpInstance.request({
    method: 'POST',
    url: '/Logout',
  })
  const userStore = useUserStore()
  userStore.token = null
  router.push('/login')
}
</script>

<template>
  <div class="page">
    <div class="nav">
      <RouterLink class="link" to="/">Home</RouterLink>
      <RouterLink class="link" to="/signalR">SignalR</RouterLink>
      <RouterLink class="link" to="/login">Login</RouterLink>
      <a class="link" @click="logout">Logout</a>
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
