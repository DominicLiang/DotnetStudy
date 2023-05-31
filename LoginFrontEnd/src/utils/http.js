import axios from 'axios'

const httpInstance = axios.create({
  baseURL: 'https://localhost:7257/Test',
  timeout: 5000
})

httpInstance.interceptors.request.use(
  (config) => {
    return config
  },
  (e) => {
    return Promise.reject(e)
  }
)

httpInstance.interceptors.response.use(
  (res) => {
    return res.data
  },
  (e) => {
    return Promise.reject(e)
  }
)

export default httpInstance
