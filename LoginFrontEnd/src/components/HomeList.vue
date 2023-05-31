<script setup>
import { ref } from 'vue'
import httpInstance from '../utils/http'

const NoNeedLoginText = ref()
const NoNeedLogin = () => {
  const res = httpInstance.request({
    url: '/ContentNoNeedLogin'
  })
  res
    .then((value) => {
      NoNeedLoginText.value = value
    })
    .catch((error) => {
      vaild(error, NoNeedLoginText)
    })
}

const NeedLoginText = ref()
const NeedLogin = () => {
  const res = httpInstance.request({
    url: '/ContentNeedLogin'
  })
  res
    .then((value) => {
      NeedLoginText.value = value
    })
    .catch((error) => {
      vaild(error, NeedLoginText)
    })
}

const NeedAdminText = ref()
const NeedAdmin = () => {
  const res = httpInstance.request({
    url: '/ContentNeedAdmin'
  })
  res
    .then((value) => {
      NeedAdminText.value = value
    })
    .catch((error) => {
      vaild(error, NeedAdminText)
    })
}

const Clear = () => {
  NoNeedLoginText.value = ''
  NeedLoginText.value = ''
  NeedAdminText.value = ''
}

const vaild = (error, needChange) => {
  switch (error.response.status) {
    case 200:
      needChange.value = result
      break
    case 401:
      needChange.value = '请先登录'
      break
    case 403:
      needChange.value = '你没有权限'
      break
    default:
      needChange.value = '发生未知错误'
      break
  }
}
</script>

<template>
  <a class="btn" @click="NoNeedLogin">NoNeedLogin</a>
  <span class="label">{{ NoNeedLoginText }}</span>

  <a class="btn" @click="NeedLogin">NeedLogin</a>
  <span class="label">{{ NeedLoginText }}</span>

  <a class="btn" @click="NeedAdmin">NeedAdmin</a>
  <span class="label">{{ NeedAdminText }}</span>

  <a class="btn" @click="Clear">Clear</a>
</template>

<style scoped>
* {
  margin: 0px;
  padding: 0px;
}
.btn {
  display: block;
  width: 150px;
  height: 30px;
  text-align: center;
  line-height: 30px;
  cursor: pointer;
}
.label {
  display: block;
  width: 350px;
  height: 50px;
  text-align: left;
  line-height: 50px;
  padding: 0 0 0 30px;
}
</style>
