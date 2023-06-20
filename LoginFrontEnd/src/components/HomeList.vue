<script setup>
import { ref } from 'vue'
import httpInstance from '../utils/http'

const ContentText = ref()
const Content = () => {
  const res = httpInstance.request({
    url: '/Test/Content'
  })
  res
    .then((value) => {
      ContentText.value = value
    })
    .catch((error) => {
      vaild(error, ContentText)
    })
}

const ContentNeedMemberText = ref()
const ContentNeedMember = () => {
  const res = httpInstance.request({
    url: '/Test/ContentNeedMember'
  })
  res
    .then((value) => {
      ContentNeedMemberText.value = value
    })
    .catch((error) => {
      vaild(error, ContentNeedMemberText)
    })
}

const ContentNeedStaffText = ref()
const ContentNeedStaff = () => {
  const res = httpInstance.request({
    url: '/Test/ContentNeedStaff'
  })
  res
    .then((value) => {
      ContentNeedStaffText.value = value
    })
    .catch((error) => {
      vaild(error, ContentNeedStaffText)
    })
}

const ContentNeedAdminText = ref()
const ContentNeedAdmin = () => {
  const res = httpInstance.request({
    url: '/Test/ContentNeedAdmin'
  })
  res
    .then((value) => {
      ContentNeedAdminText.value = value
    })
    .catch((error) => {
      vaild(error, ContentNeedAdminText)
    })
}

const Clear = () => {
  ContentText.value = ''
  ContentNeedMemberText.value = ''
  ContentNeedStaffText.value = ''
  ContentNeedAdminText.value = ''
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

<template class="template">
  <a class="btn" @click="Content">Content</a>
  <span class="label">{{ ContentText }}</span>

  <a class="btn" @click="ContentNeedMember">ContentNeedMember</a>
  <span class="label">{{ ContentNeedMemberText }}</span>

  <a class="btn" @click="ContentNeedStaff">ContentNeedStaff</a>
  <span class="label">{{ ContentNeedStaffText }}</span>

  <a class="btn" @click="ContentNeedAdmin">ContentNeedAdmin</a>
  <span class="label">{{ ContentNeedAdminText }}</span>

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
