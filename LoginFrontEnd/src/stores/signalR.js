import { defineStore } from 'pinia'
import { useUserStore } from '../stores/user'
import * as signalR from '@microsoft/signalr'
import { ref } from 'vue'

export const connectionStore = defineStore(
  'conn',
  () => {
    const connection = ref()

    const getConnection = async () => {
      let options = {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }
      options.accessTokenFactory = () => {
        return useUserStore().token
      }
      connection.value = new signalR.HubConnectionBuilder()
        .withUrl('https://localhost:7257/MyHub', options)
        .withAutomaticReconnect()
        .build()
      await connection.value.start()
    }

    return {
      connection,
      getConnection
    }
  }
)
