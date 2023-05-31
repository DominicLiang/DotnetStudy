<script setup>
import { onMounted, ref } from 'vue'
import { connectionStore } from '../stores/signalR'

const conn = connectionStore()

const userMsg = ref()
const privateMsg = ref()
const msg = ref([])
const msg2 = ref([])
const target = ref()

const onKeyPress = async (e) => {
  if (e.keyCode != 13) return
  await conn.connection.invoke('SendPublicMsg', userMsg.value)
  //   对应服务器的SendPublicMsg方法
  userMsg.value = ''
}

const onKeyPress2 = async (e) => {
  if (e.keyCode != 13) return
  await conn.connection.invoke('SendPrivateMsg', target.value, privateMsg.value)
  //   对应服务器的SendPublicMsg方法
  privateMsg.value = ''
}

onMounted(async () => {
  conn.connection.on('PublicMsgReceived', (m) => {
    console.log('PublicMsgReceived')
    msg.value.push(m)
  })

  conn.connection.on('PrivateMsgRecevied', (n, m) => {
    console.log(`PrivateMsgRecevied  ${n}: ${m}`)
    msg2.value.push(`${n}: ${m}`)
  })
})
</script>

<template>
<!-- npm install @microsoft/signalr 需要安装singnalr包-->
<template>
  <div>
    私聊 目标用户名:<input type="text" v-model="target" />
    <input type="text" v-model="privateMsg" @keypress="onKeyPress2" />
    <div>
      <ul>
        <li v-for="(m, index) in msg2" :key="index">
          {{ m }}
        </li>
      </ul>
    </div>
  </div>
  <div>
    公屏
    <input type="text" v-model="userMsg" @keypress="onKeyPress" />
    <div>
      <ul>
        <li v-for="(m, index) in msg" :key="index">
          {{ m }}
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped></style>
