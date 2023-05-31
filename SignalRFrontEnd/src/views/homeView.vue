<script setup>
import { onMounted, ref } from 'vue'
import * as signalR from '@microsoft/signalr'

let connection
const userMsg = ref()
const msg = ref([])

const onKeyPress = async (e) => {
  if (e.keyCode != 13) return
  await connection.invoke('SendPublicMsg', userMsg.value)
  //   对应服务器的SendPublicMsg方法
  userMsg.value = ''
}

onMounted(async () => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7285/MyHub', {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build()
  await connection.start()
  connection.on('PublicMsgReceived', (m) => {
    msg.value.push(m)
  })
})
</script>

<template>
  <!-- npm install @microsoft/signalr 需要安装singnalr包-->
  <input type="text" v-model="userMsg" @keypress="onKeyPress" />
  <div>
    <ul>
      <li v-for="(m, index) in msg" :key="index">
        {{ m }}
      </li>
    </ul>
  </div>
</template>

<style scoped></style>
