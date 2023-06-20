import axios from 'axios'
import { useUserStore } from '../stores/user'

const httpInstance = axios.create({
  baseURL: 'https://localhost:7134/api',
  timeout: 5000
})

httpInstance.interceptors.request.use(
  (config) => {
    const userStore = useUserStore()
    const token = userStore.token
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

httpInstance.interceptors.response.use(
  (response) => {
    const token = response.headers['x-token']
    if (token) {
      const userStore = useUserStore()
      userStore.token = token
    }

    return response.data
  },
  (error) => {
    const userStore = useUserStore()
    userStore.token = null

    return Promise.reject(error)
  }
)

export default httpInstance
