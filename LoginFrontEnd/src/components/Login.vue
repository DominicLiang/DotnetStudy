<script setup>
import { ref } from 'vue'
import httpInstance from '../utils/http'
import { useRouter } from 'vue-router'
import { connectionStore } from '../stores/signalR'

const conn = connectionStore()
const router = useRouter()

const data = ref({
  username: '',
  password: ''
})

const isCanLogin = ref(false)

const Submit = () => {
  const res = httpInstance.request({
    url: '/Login/LoginByUsername',
    method: 'POST',
    data: {
      username: data.value.username,
      password: data.value.password,
      email: null,
      phoneNumber: null
    }
  })
  res
    .then((value) => {
      isCanLogin.value = false
      // conn.getConnection()
      router.replace({ path: '/' })
    })
    .catch((error) => {
      isCanLogin.value = true
    })
}
</script>

<template>
  <span v-show="isCanLogin">账号或密码错误！</span>
  <form>
    <div class="input">
      <div>
        <span class="label">用户名：</span>
        <input type="text" v-model="data.username" />
      </div>
      <div>
        <span class="label">密码：</span>
        <input type="text" v-model="data.password" />
      </div>
    </div>
    <a class="btn" @click="Submit">登录</a>
  </form>
</template>

<style scoped>
.label {
  display: inline-block;
  width: 70px;
}
.input {
  width: 300px;
  height: 90px;
  padding: 20px;
  background-color: rgb(37, 37, 37);
}
.btn {
  display: block;
  width: 300px;
  height: 30px;
  line-height: 30px;
  text-align: center;
  cursor: pointer;
}
</style>
